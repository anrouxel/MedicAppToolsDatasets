using System.Diagnostics;
using System.Text.Json;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Runtime.InteropServices;
using System.CommandLine;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

public class Program
{
    const string CIS_bdpm = "CIS_bdpm";
    const string CIS_COMPO_bdpm = "CIS_COMPO_bdpm";
    const string CIS_CIP_bdpm = "CIS_CIP_bdpm";
    const string CIS_GENER_bdpm = "CIS_GENER_bdpm";
    const string CIS_HAS_SMR_bdpm = "CIS_HAS_SMR_bdpm";
    const string CIS_HAS_ASMR_bdpm = "CIS_HAS_ASMR_bdpm";
    const string CIS_InfoImportantes = "CIS_InfoImportantes";
    const string CIS_CPD_bdpm = "CIS_CPD_bdpm";
    const string HAS_LiensPageCT_bdpm = "HAS_LiensPageCT_bdpm";

    static List<Medication> medications = new List<Medication>();
    static List<MedicationComposition> medicationCompositions = new List<MedicationComposition>();
    static List<MedicationPresentation> medicationPresentations = new List<MedicationPresentation>();
    static List<GenericGroup> genericGroups = new List<GenericGroup>();
    static List<HasSmrOpinion> hasSmrOpinions = new List<HasSmrOpinion>();
    static List<HasAsmrOpinion> hasAsmrOpinions = new List<HasAsmrOpinion>();
    static List<ImportantInformation> importantInformations = new List<ImportantInformation>();
    static List<PrescriptionDispensingConditions> prescriptionDispensingConditions = new List<PrescriptionDispensingConditions>();
    static List<TransparencyCommissionOpinionLinks> transparencyCommissionOpinionLinks = new List<TransparencyCommissionOpinionLinks>();

    public static async Task<int> Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        var modelOption = new Option<string>(
            new string[] { "--model", "-m" },
            description: "Model to use : ai or dataset"
        );

        var datasetOption = new Option<string[]>(
            new string[] { "--dataset", "-d" },
            description: "Dataset to use : CIS_bdpm, CIS_COMPO_bdpm, CIS_CIP_bdpm, CIS_GENER_bdpm, CIS_HAS_SMR_bdpm, CIS_HAS_ASMR_bdpm, CIS_InfoImportantes, CIS_CPD_bdpm, HAS_LiensPageCT_bdpm, all"
        );

        var mergeOption = new Option<bool>(
            new string[] { "--merge" },
            description: "Merge datasets"
        );

        var outputDirOption = new Option<DirectoryInfo>(
            new string[] { "--outputDir", "-o" },
            description: "Output directory"
        );

        var outputUrlOption = new Option<Uri>(
            new string[] { "--outputUrl", "-u" },
            description: "Output url"
        );

        var sentenceCountOption = new Option<int>(
            new string[] { "--sentenceCount", "-s" },
            description: "Number of sentences to generate"
        );

        var rootCommand = new RootCommand("MedicAppToolsDatasets.Tools")
        {
            modelOption,
            datasetOption,
            mergeOption,
            outputDirOption,
            outputUrlOption,
            sentenceCountOption
        };


        rootCommand.SetHandler(async (model, dataset, merge, outputDir, outputUrl, sentenceCount) =>
        {
            try
            {
                // Si model ou dataset n'est pas spécifié, on utilise retourne une erreur.
                if (model == null || dataset == null || (outputDir == null && outputUrl == null))
                {
                    throw new Exception("Model, dataset and outputDir or outputUrl must be specified");
                }
                else if (model == "ai" && !dataset.Any(d => d == "all") && sentenceCount <= 0 && outputDir == null)
                {
                    throw new Exception("When model is 'ai', dataset must be 'all', sentenceCount must be greater than 0 and outputDir must be specified");
                }
                else if (model == "dataset" && !dataset.Any())
                {
                    throw new Exception("When model is 'dataset', dataset must be specified");
                }

                if (dataset.Any(d => d == "all"))
                {
                    dataset = new string[] {
                        CIS_bdpm,
                        CIS_COMPO_bdpm,
                        CIS_CIP_bdpm,
                        CIS_GENER_bdpm,
                        CIS_HAS_SMR_bdpm,
                        CIS_HAS_ASMR_bdpm,
                        CIS_InfoImportantes,
                        CIS_CPD_bdpm,
                        HAS_LiensPageCT_bdpm
                    };
                }

                await DownloadAndConvert(dataset.ToList());

                if (model == "ai" && outputDir != null)
                {
                    Merge();
                    await MakeIAModel(sentenceCount, outputDir, new FileInfo("ai_" + sentenceCount));
                }
                else if (model == "dataset" && !merge && outputDir != null)
                {
                    await SaveDatasets(dataset.ToList(), outputDir);
                }
                else if (model == "dataset" && !merge && outputUrl != null)
                {
                    await UploadJsonAsync(dataset.ToList(), outputUrl);
                }
                else if (model == "dataset" && merge && outputDir != null)
                {
                    Merge();
                    await SaveAsJsonAsync(medications, outputDir, new FileInfo("merged_" + CIS_bdpm));
                }
                else if (model == "dataset" && merge && outputUrl != null)
                {
                    Merge();
                    await UploadJsonAsync(medications, outputUrl);
                }
                else
                {
                    throw new Exception("Unknown model");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            stopwatch.Stop();
            Console.WriteLine($"Temps d'exécution: {stopwatch.ElapsedMilliseconds} ms");
        }, modelOption, datasetOption, mergeOption, outputDirOption, outputUrlOption, sentenceCountOption);
        return await rootCommand.InvokeAsync(args);
    }

    public static async Task DownloadAndConvert(List<string> datasets)
    {
        List<Task> tasks = new List<Task>();
        Task<List<Medication>> medicationsTask = Task.FromResult(new List<Medication>());
        Task<List<MedicationComposition>> medicationCompositionsTask = Task.FromResult(new List<MedicationComposition>());
        Task<List<MedicationPresentation>> medicationPresentationsTask = Task.FromResult(new List<MedicationPresentation>());
        Task<List<GenericGroup>> genericGroupsTask = Task.FromResult(new List<GenericGroup>());
        Task<List<HasSmrOpinion>> hasSmrOpinionsTask = Task.FromResult(new List<HasSmrOpinion>());
        Task<List<HasAsmrOpinion>> hasAsmrOpinionsTask = Task.FromResult(new List<HasAsmrOpinion>());
        Task<List<ImportantInformation>> importantInformationsTask = Task.FromResult(new List<ImportantInformation>());
        Task<List<PrescriptionDispensingConditions>> prescriptionDispensingConditionsTask = Task.FromResult(new List<PrescriptionDispensingConditions>());
        Task<List<TransparencyCommissionOpinionLinks>> transparencyCommissionOpinionLinksTask = Task.FromResult(new List<TransparencyCommissionOpinionLinks>());
        foreach (var dataset in datasets)
        {
            switch (dataset)
            {
                case CIS_bdpm:
                    medicationsTask = DownloadAndConvertToCsvRecordsAsync<Medication>(CIS_bdpm);
                    tasks.Add(medicationsTask);
                    break;
                case CIS_COMPO_bdpm:
                    medicationCompositionsTask = DownloadAndConvertToCsvRecordsAsync<MedicationComposition>(CIS_COMPO_bdpm);
                    tasks.Add(medicationCompositionsTask);
                    break;
                case CIS_CIP_bdpm:
                    medicationPresentationsTask = DownloadAndConvertToCsvRecordsAsync<MedicationPresentation>(CIS_CIP_bdpm);
                    tasks.Add(medicationPresentationsTask);
                    break;
                case CIS_GENER_bdpm:
                    genericGroupsTask = DownloadAndConvertToCsvRecordsAsync<GenericGroup>(CIS_GENER_bdpm);
                    tasks.Add(genericGroupsTask);
                    break;
                case CIS_HAS_SMR_bdpm:
                    hasSmrOpinionsTask = DownloadAndConvertToCsvRecordsAsync<HasSmrOpinion>(CIS_HAS_SMR_bdpm);
                    tasks.Add(hasSmrOpinionsTask);
                    break;
                case CIS_HAS_ASMR_bdpm:
                    hasAsmrOpinionsTask = DownloadAndConvertToCsvRecordsAsync<HasAsmrOpinion>(CIS_HAS_ASMR_bdpm);
                    tasks.Add(hasAsmrOpinionsTask);
                    break;
                case CIS_InfoImportantes:
                    importantInformationsTask = DownloadAndConvertToCsvRecordsAsync<ImportantInformation>(CIS_InfoImportantes);
                    tasks.Add(importantInformationsTask);
                    break;
                case CIS_CPD_bdpm:
                    prescriptionDispensingConditionsTask = DownloadAndConvertToCsvRecordsAsync<PrescriptionDispensingConditions>(CIS_CPD_bdpm);
                    tasks.Add(prescriptionDispensingConditionsTask);
                    break;
                case HAS_LiensPageCT_bdpm:
                    transparencyCommissionOpinionLinksTask = DownloadAndConvertToCsvRecordsAsync<TransparencyCommissionOpinionLinks>(HAS_LiensPageCT_bdpm);
                    tasks.Add(transparencyCommissionOpinionLinksTask);
                    break;
                default:
                    throw new Exception($"Unknown dataset: {dataset}");
            }
        }

        await Task.WhenAll(tasks);

        medications = medicationsTask.Result;
        medicationCompositions = medicationCompositionsTask.Result;
        medicationPresentations = medicationPresentationsTask.Result;
        genericGroups = genericGroupsTask.Result;
        hasSmrOpinions = hasSmrOpinionsTask.Result;
        hasAsmrOpinions = hasAsmrOpinionsTask.Result;
        importantInformations = importantInformationsTask.Result;
        prescriptionDispensingConditions = prescriptionDispensingConditionsTask.Result;
        transparencyCommissionOpinionLinks = transparencyCommissionOpinionLinksTask.Result;
    }

    public static async Task SaveDatasets(List<string> datasets, DirectoryInfo output)
    {
        List<Task> tasks = new List<Task>();
        foreach (var dataset in datasets)
        {
            switch (dataset)
            {
                case CIS_bdpm:
                    tasks.Add(SaveAsJsonAsync(medications, output, new FileInfo(CIS_bdpm)));
                    break;
                case CIS_COMPO_bdpm:
                    tasks.Add(SaveAsJsonAsync(medicationCompositions, output, new FileInfo(CIS_COMPO_bdpm)));
                    break;
                case CIS_CIP_bdpm:
                    tasks.Add(SaveAsJsonAsync(medicationPresentations, output, new FileInfo(CIS_CIP_bdpm)));
                    break;
                case CIS_GENER_bdpm:
                    tasks.Add(SaveAsJsonAsync(genericGroups, output, new FileInfo(CIS_GENER_bdpm)));
                    break;
                case CIS_HAS_SMR_bdpm:
                    tasks.Add(SaveAsJsonAsync(hasSmrOpinions, output, new FileInfo(CIS_HAS_SMR_bdpm)));
                    break;
                case CIS_HAS_ASMR_bdpm:
                    tasks.Add(SaveAsJsonAsync(hasAsmrOpinions, output, new FileInfo(CIS_HAS_ASMR_bdpm)));
                    break;
                case CIS_InfoImportantes:
                    tasks.Add(SaveAsJsonAsync(importantInformations, output, new FileInfo(CIS_InfoImportantes)));
                    break;
                case CIS_CPD_bdpm:
                    tasks.Add(SaveAsJsonAsync(prescriptionDispensingConditions, output, new FileInfo(CIS_CPD_bdpm)));
                    break;
                case HAS_LiensPageCT_bdpm:
                    tasks.Add(SaveAsJsonAsync(transparencyCommissionOpinionLinks, output, new FileInfo(HAS_LiensPageCT_bdpm)));
                    break;
                default:
                    throw new Exception($"Unknown dataset: {dataset}");
            }
        }

        await Task.WhenAll(tasks);
    }

    public static async Task UploadDatasets(List<string> datasets, Uri output)
    {
        List<Task> tasks = new List<Task>();
        foreach (var dataset in datasets)
        {
            switch (dataset)
            {
                case CIS_bdpm:
                    tasks.Add(UploadJsonAsync(medications, output));
                    break;
                case CIS_COMPO_bdpm:
                    tasks.Add(UploadJsonAsync(medicationCompositions, output));
                    break;
                case CIS_CIP_bdpm:
                    tasks.Add(UploadJsonAsync(medicationPresentations, output));
                    break;
                case CIS_GENER_bdpm:
                    tasks.Add(UploadJsonAsync(genericGroups, output));
                    break;
                case CIS_HAS_SMR_bdpm:
                    tasks.Add(UploadJsonAsync(hasSmrOpinions, output));
                    break;
                case CIS_HAS_ASMR_bdpm:
                    tasks.Add(UploadJsonAsync(hasAsmrOpinions, output));
                    break;
                case CIS_InfoImportantes:
                    tasks.Add(UploadJsonAsync(importantInformations, output));
                    break;
                case CIS_CPD_bdpm:
                    tasks.Add(UploadJsonAsync(prescriptionDispensingConditions, output));
                    break;
                case HAS_LiensPageCT_bdpm:
                    tasks.Add(UploadJsonAsync(transparencyCommissionOpinionLinks, output));
                    break;
                default:
                    throw new Exception($"Unknown dataset: {dataset}");
            }
        }

        await Task.WhenAll(tasks);
    }

    public static void Merge()
    {
        hasSmrOpinions
            .GroupJoin(
                transparencyCommissionOpinionLinks,
                hasSmrOpinion => hasSmrOpinion.HasDossierCode,
                transparencyCommissionOpinionLink => transparencyCommissionOpinionLink.HasDossierCode,
                (hasAsmrOpinion, transparencyCommissionOpinionLink) =>
                {
                    hasAsmrOpinion.TransparencyCommissionOpinionLinks = transparencyCommissionOpinionLink.ToList();
                    return hasAsmrOpinion;
                }
            )
            .ToList();

        hasAsmrOpinions
            .GroupJoin(
                transparencyCommissionOpinionLinks,
                hasAsmrOpinion => hasAsmrOpinion.HasDossierCode,
                transparencyCommissionOpinionLink => transparencyCommissionOpinionLink.HasDossierCode,
                (hasAsmrOpinion, transparencyCommissionOpinionLink) =>
                {
                    hasAsmrOpinion.TransparencyCommissionOpinionLinks = transparencyCommissionOpinionLink.ToList();
                    return hasAsmrOpinion;
                }
            )
            .ToList();

        medications
            .GroupJoin(
                medicationCompositions,
                medication => medication.CISCode,
                medicationComposition => medicationComposition.CISCode,
                (medication, medicationComposition) =>
                {
                    medication.MedicationCompositions = medicationComposition.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                medicationPresentations,
                medication => medication.CISCode,
                medicationPresentation => medicationPresentation.CISCode,
                (medication, medicationPresentation) =>
                {
                    medication.MedicationPresentations = medicationPresentation.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                genericGroups,
                medication => medication.CISCode,
                genericGroup => genericGroup.CISCode,
                (medication, genericGroup) =>
                {
                    medication.GenericGroups = genericGroup.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                hasSmrOpinions,
                medication => medication.CISCode,
                hasSmrOpinion => hasSmrOpinion.CISCode,
                (medication, hasSmrOpinion) =>
                {
                    medication.HasSmrOpinions = hasSmrOpinion.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                hasAsmrOpinions,
                medication => medication.CISCode,
                hasAsmrOpinion => hasAsmrOpinion.CISCode,
                (medication, hasAsmrOpinion) =>
                {
                    medication.HasAsmrOpinions = hasAsmrOpinion.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                importantInformations,
                medication => medication.CISCode,
                importantInformation => importantInformation.CISCode,
                (medication, importantInformation) =>
                {
                    medication.ImportantInformations = importantInformation.ToList();
                    return medication;
                }
            )
            .GroupJoin(
                prescriptionDispensingConditions,
                medication => medication.CISCode,
                prescriptionDispensingCondition => prescriptionDispensingCondition.CISCode,
                (medication, prescriptionDispensingCondition) =>
                {
                    medication.PrescriptionDispensingConditions = prescriptionDispensingCondition.ToList();
                    return medication;
                }
            )
            .ToList();
    }

    public static async Task<string> DownloadAsync(string fileName)
    {
        using (HttpClient client = new HttpClient())
        {
            Uri baseApiUri = new Uri("https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=");
            var fileUriBuilder = new UriBuilder(baseApiUri);
            fileUriBuilder.Query += fileName + ".txt";
            Uri fileUri = fileUriBuilder.Uri;

            HttpResponseMessage response = await client.GetAsync(fileUri);
            if (response.IsSuccessStatusCode)
            {
                if (response.Content.Headers.ContentType != null)
                {
                    response.Content.Headers.ContentType.CharSet = "latin1";
                }
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }

    public static List<T> ConvertToCsvRecords<T>(string csvData)
    {
        using (TextReader reader = new StringReader(csvData))
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t", // Définissez le délimiteur sur la tabulation.
                HasHeaderRecord = false, // Indiquez qu'il n'y a pas d'en-tête.
                TrimOptions = TrimOptions.Trim, // Indiquez que les valeurs doivent être nettoyées.
                IgnoreBlankLines = true, // Indiquez que les lignes vides doivent être ignorées.
                BadDataFound = null, // Indiquez que les données incorrectes doivent être ignorées.
            };

            using (var csv = new CsvReader(reader, csvConfig))
            {
                // Remplacez Medicament par le type correspondant à votre fichier CSV.
                var records = csv.GetRecords<T>().ToList();
                return records;
            }
        }
    }

    public static async Task<List<T>> DownloadAndConvertToCsvRecordsAsync<T>(string fileName)
    {
        string csvData = await DownloadAsync(fileName);
        List<T> records = ConvertToCsvRecords<T>(csvData);
        return records;
    }

    public static async Task SaveAsJsonAsync<T>(List<T> records, DirectoryInfo output, FileInfo fileName)
    {
        if (!output.Exists)
        {
            output.Create();
        }
        else if (output.Exists && output.GetFiles().Any(f => f.Name == fileName.Name + ".json"))
        {
            output.GetFiles().Where(f => f.Name == fileName.Name + ".json").ToList().ForEach(f => f.Delete());
        }
        string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(output.FullName + "/" + fileName.Name + ".json", json);
    }

    public static async Task UploadJsonAsync<T>(List<T> records, Uri output)
    {
        string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(output, new StringContent(json));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }

    public static async Task MakeIAModel(int sentenceCount, DirectoryInfo output, FileInfo fileName)
    {
        List<SentenceData> sentences = new List<SentenceData>();

        for (int i = 0; i < sentenceCount; i++)
        {
            string[] prescription = new Prescriptions().GetPrescription();

            List<string> prescriptionToken = tokenize(prescription, Labels.O, Labels.O);

            while (prescription.Contains(PatternPrescriptions.Drug))
            {
                Random random = new Random();
                int index = random.Next(medications.Count);
                Medication medication = medications[index];
                string[] medicationName = medication.Name.Split(' ');

                int medicationPosition = Array.IndexOf(prescription, PatternPrescriptions.Drug);

                var tmp_left_prescription = prescription.Take(medicationPosition).ToList();
                var tmp_right_prescription = prescription.Skip(medicationPosition + 1).ToList();

                prescription = tmp_left_prescription.Concat(medicationName).Concat(tmp_right_prescription).ToArray();

                var tmp_left_prescriptionToken = prescriptionToken.Take(medicationPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(medicationPosition + 1).ToList();

                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationName, Labels.B_Drug, Labels.I_Drug)).Concat(tmp_right_prescriptionToken).ToList();
            }

            while (prescription.Contains(PatternPrescriptions.DrugQuantity))
            {
                Random random = new Random();
                int quantity = random.Next(1, 10);
                string[] medicationQuantity = new string[] { quantity.ToString() };

                int quantityPosition = Array.IndexOf(prescription, PatternPrescriptions.DrugQuantity);

                var tmp_left_prescription = prescription.Take(quantityPosition).ToList();
                var tmp_right_prescription = prescription.Skip(quantityPosition + 1).ToList();

                prescription = tmp_left_prescription.Concat(medicationQuantity).Concat(tmp_right_prescription).ToArray();

                var tmp_left_prescriptionToken = prescriptionToken.Take(quantityPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(quantityPosition + 1).ToList();

                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationQuantity, Labels.B_DrugQuantity, Labels.I_DrugQuantity)).Concat(tmp_right_prescriptionToken).ToList();
            }

            while (prescription.Contains(PatternPrescriptions.DrugFrequency))
            {
                Random random = new Random();
                int index = random.Next(PatternPrescriptionsData.Frequency.Count);
                string[] medicationFrequency = PatternPrescriptionsData.Frequency[index].Split(' ');

                int frequencyPosition = Array.IndexOf(prescription, PatternPrescriptions.DrugFrequency);

                var tmp_left_prescription = prescription.Take(frequencyPosition).ToList();
                var tmp_right_prescription = prescription.Skip(frequencyPosition + 1).ToList();

                prescription = tmp_left_prescription.Concat(medicationFrequency).Concat(tmp_right_prescription).ToArray();

                var tmp_left_prescriptionToken = prescriptionToken.Take(frequencyPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(frequencyPosition + 1).ToList();

                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationFrequency, Labels.B_DrugFrequency, Labels.I_DrugFrequency)).Concat(tmp_right_prescriptionToken).ToList();
            }

            while (prescription.Contains(PatternPrescriptions.DrugDuration))
            {
                Random random = new Random();
                int index = random.Next(PatternPrescriptionsData.Duration.Count);

                int number = random.Next(1, 10);

                string[] medicationDuration = $"{number} {PatternPrescriptionsData.Duration[index]}".Split(' ');

                int durationPosition = Array.IndexOf(prescription, PatternPrescriptions.DrugDuration);

                var tmp_left_prescription = prescription.Take(durationPosition).ToList();
                var tmp_right_prescription = prescription.Skip(durationPosition + 1).ToList();

                prescription = tmp_left_prescription.Concat(medicationDuration).Concat(tmp_right_prescription).ToArray();

                var tmp_left_prescriptionToken = prescriptionToken.Take(durationPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(durationPosition + 1).ToList();

                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationDuration, Labels.B_DrugDuration, Labels.I_DrugDuration)).Concat(tmp_right_prescriptionToken).ToList();
            }

            while (prescription.Contains(PatternPrescriptions.DrugForm))
            {
                Random random = new Random();
                int index = random.Next(PatternPrescriptionsData.Form.Count);
                string[] medicationForm = PatternPrescriptionsData.Form[index].Split(' ');

                int formPosition = Array.IndexOf(prescription, PatternPrescriptions.DrugForm);

                var tmp_left_prescription = prescription.Take(formPosition).ToList();
                var tmp_right_prescription = prescription.Skip(formPosition + 1).ToList();

                prescription = tmp_left_prescription.Concat(medicationForm).Concat(tmp_right_prescription).ToArray();

                var tmp_left_prescriptionToken = prescriptionToken.Take(formPosition).ToList();
                var tmp_right_prescriptionToken = prescriptionToken.Skip(formPosition + 1).ToList();

                prescriptionToken = tmp_left_prescriptionToken.Concat(tokenize(medicationForm, Labels.B_DrugForm, Labels.I_DrugForm)).Concat(tmp_right_prescriptionToken).ToList();
            }

            SentenceData sentence = new SentenceData()
            {
                Sentence = string.Join(" ", prescription),
                Labels = prescriptionToken
            };

            sentences.Add(sentence);
        }

        await SaveAsJsonAsync(sentences, output, fileName);
    }

    public static List<string> tokenize(string[] sentence, string begin, string inside)
    {
        List<string> tokens = new List<string>();
        string[] words = sentence;
        for (int i = 0; i < words.Length; i++)
        {
            if (i == 0)
            {
                tokens.Add(begin);
            }
            else
            {
                tokens.Add(inside);
            }
        }
        return tokens;
    }
}