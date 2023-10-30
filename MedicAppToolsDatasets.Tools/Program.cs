using System.Diagnostics;
using System.Text.Json;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

public class Program
{
    public static async Task Main(string[] args)
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

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var medicationsTask = DownloadAndConvertToCsvRecordsAsync<Medication>(CIS_bdpm);
        var medicationCompositionsTask = DownloadAndConvertToCsvRecordsAsync<MedicationComposition>(CIS_COMPO_bdpm);
        var medicationPresentationsTask = DownloadAndConvertToCsvRecordsAsync<MedicationPresentation>(CIS_CIP_bdpm);
        var genericGroupsTask = DownloadAndConvertToCsvRecordsAsync<GenericGroup>(CIS_GENER_bdpm);
        var hasSmrOpinionsTask = DownloadAndConvertToCsvRecordsAsync<HasSmrOpinion>(CIS_HAS_SMR_bdpm);
        var hasAsmrOpinionsTask = DownloadAndConvertToCsvRecordsAsync<HasAsmrOpinion>(CIS_HAS_ASMR_bdpm);
        var importantInformationsTask = DownloadAndConvertToCsvRecordsAsync<ImportantInformation>(CIS_InfoImportantes);
        var prescriptionDispensingConditionsTask = DownloadAndConvertToCsvRecordsAsync<PrescriptionDispensingConditions>(CIS_CPD_bdpm);
        var transparencyCommissionOpinionLinksTask = DownloadAndConvertToCsvRecordsAsync<TransparencyCommissionOpinionLinks>(HAS_LiensPageCT_bdpm);

        await Task.WhenAll(medicationsTask, medicationCompositionsTask, medicationPresentationsTask, genericGroupsTask, hasSmrOpinionsTask, hasAsmrOpinionsTask, importantInformationsTask, prescriptionDispensingConditionsTask, transparencyCommissionOpinionLinksTask);

        List<Medication> medications = medicationsTask.Result;
        List<MedicationComposition> medicationCompositions = medicationCompositionsTask.Result;
        List<MedicationPresentation> medicationPresentations = medicationPresentationsTask.Result;
        List<GenericGroup> genericGroups = genericGroupsTask.Result;
        List<HasSmrOpinion> hasSmrOpinions = hasSmrOpinionsTask.Result;
        List<HasAsmrOpinion> hasAsmrOpinions = hasAsmrOpinionsTask.Result;
        List<ImportantInformation> importantInformations = importantInformationsTask.Result;
        List<PrescriptionDispensingConditions> prescriptionDispensingConditions = prescriptionDispensingConditionsTask.Result;
        List<TransparencyCommissionOpinionLinks> transparencyCommissionOpinionLinks = transparencyCommissionOpinionLinksTask.Result;

        var mergeHasSmrOpinions = hasSmrOpinions
            .GroupJoin(
                transparencyCommissionOpinionLinks,
                hasSmrOpinion => hasSmrOpinion.HasDossierCode,
                transparencyCommissionOpinionLink => transparencyCommissionOpinionLink.HasDossierCode,
                (hasAsmrOpinion, transparencyCommissionOpinionLink) => {
                    hasAsmrOpinion.TransparencyCommissionOpinionLinks = transparencyCommissionOpinionLink.ToList();
                    return hasAsmrOpinion;
                }
            );
        
        var mergeHasAsmrOpinions = hasAsmrOpinions
            .GroupJoin(
                transparencyCommissionOpinionLinks,
                hasAsmrOpinion => hasAsmrOpinion.HasDossierCode,
                transparencyCommissionOpinionLink => transparencyCommissionOpinionLink.HasDossierCode,
                (hasAsmrOpinion, transparencyCommissionOpinionLink) => {
                    hasAsmrOpinion.TransparencyCommissionOpinionLinks = transparencyCommissionOpinionLink.ToList();
                    return hasAsmrOpinion;
                }
            );

        var mergedMedications = medications
            .GroupJoin(
                medicationCompositions,
                medication => medication.CISCode,
                medicationComposition => medicationComposition.CISCode,
                (medication, medicationCompositions) =>
                {
                    medication.MedicationCompositions = medicationCompositions.ToList();
                    return medication;
                })
            .GroupJoin(
                medicationPresentations,
                medication => medication.CISCode,
                medicationPresentation => medicationPresentation.CISCode,
                (medication, medicationPresentations) =>
                {
                    medication.MedicationPresentations = medicationPresentations.ToList();
                    return medication;
                })
            .GroupJoin(
                genericGroups,
                medication => medication.CISCode,
                genericGroup => genericGroup.CISCode,
                (medication, genericGroups) =>
                {
                    medication.GenericGroups = genericGroups.ToList();
                    return medication;
                })
            .GroupJoin(
                mergeHasSmrOpinions,
                medication => medication.CISCode,
                hasSmrOpinion => hasSmrOpinion.CISCode,
                (medication, hasSmrOpinions) =>
                {
                    medication.HasSmrOpinions = hasSmrOpinions.ToList();
                    return medication;
                })
            .GroupJoin(
                mergeHasAsmrOpinions,
                medication => medication.CISCode,
                hasAsmrOpinion => hasAsmrOpinion.CISCode,
                (medication, hasAsmrOpinions) =>
                {
                    medication.HasAsmrOpinions = hasAsmrOpinions.ToList();
                    return medication;
                })
            .GroupJoin(
                importantInformations,
                medication => medication.CISCode,
                importantInformation => importantInformation.CISCode,
                (medication, importantInformations) =>
                {
                    medication.ImportantInformations = importantInformations.ToList();
                    return medication;
                })
            .GroupJoin(
                prescriptionDispensingConditions,
                medication => medication.CISCode,
                prescriptionDispensingCondition => prescriptionDispensingCondition.CISCode,
                (medication, prescriptionDispensingConditions) =>
                {
                    medication.PrescriptionDispensingConditions = prescriptionDispensingConditions.ToList();
                    return medication;
                })
            .ToList();

        var medicamentWithMultiple = mergedMedications.Where(
            m => m.MedicationCompositions.Count > 1
            && m.MedicationPresentations.Count > 1
            && m.ImportantInformations.Count > 1
            && m.PrescriptionDispensingConditions.Count > 1
            ).Take(1).ToList();

        await SaveAsJsonAsync(medicamentWithMultiple, "medicamentWithMultiple");

        await SaveAsJsonAsync(medications, CIS_bdpm);
        await SaveAsJsonAsync(medicationCompositions, CIS_COMPO_bdpm);
        await SaveAsJsonAsync(medicationPresentations, CIS_CIP_bdpm);
        await SaveAsJsonAsync(genericGroups, CIS_GENER_bdpm);
        await SaveAsJsonAsync(hasSmrOpinions, CIS_HAS_SMR_bdpm);
        await SaveAsJsonAsync(hasAsmrOpinions, CIS_HAS_ASMR_bdpm);
        await SaveAsJsonAsync(importantInformations, CIS_InfoImportantes);
        await SaveAsJsonAsync(prescriptionDispensingConditions, CIS_CPD_bdpm);
        await SaveAsJsonAsync(transparencyCommissionOpinionLinks, HAS_LiensPageCT_bdpm);
        await SaveAsJsonAsync(mergedMedications, "medications");

        stopwatch.Stop();
        Console.WriteLine($"Temps d'exécution: {stopwatch.ElapsedMilliseconds} ms");
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

    public static async Task SaveAsJsonAsync<T>(List<T> records, string fileName)
    {
        string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync("data/" + fileName + ".json", json);
    }
}