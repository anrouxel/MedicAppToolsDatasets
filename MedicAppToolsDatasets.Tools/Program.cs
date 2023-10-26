using System.Diagnostics;
using System.Text.Json;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

public class Program
{
    public static async Task Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        List<Medication> medications = await DownloadAndConvertToCsvRecordsAsync<Medication>("CIS_bdpm");
        List<MedicationComposition> medicationCompositions = await DownloadAndConvertToCsvRecordsAsync<MedicationComposition>("CIS_COMPO_bdpm");
        List<MedicationPresentation> medicationPresentations = await DownloadAndConvertToCsvRecordsAsync<MedicationPresentation>("CIS_CIP_bdpm");
        List<GenericGroup> genericGroups = await DownloadAndConvertToCsvRecordsAsync<GenericGroup>("CIS_GENER_bdpm");
        List<HasSmrOpinion> hasSmrOpinions = await DownloadAndConvertToCsvRecordsAsync<HasSmrOpinion>("CIS_HAS_SMR_bdpm");
        List<HasAsmrOpinion> hasAsmrOpinions = await DownloadAndConvertToCsvRecordsAsync<HasAsmrOpinion>("CIS_HAS_ASMR_bdpm");
        List<ImportantInformation> importantInformations = await DownloadAndConvertToCsvRecordsAsync<ImportantInformation>("CIS_InfoImportantes");
        List<PrescriptionDispensingConditions> prescriptionDispensingConditions = await DownloadAndConvertToCsvRecordsAsync<PrescriptionDispensingConditions>("CIS_CPD_bdpm");
        List<TransparencyCommissionOpinionLinks> transparencyCommissionOpinionLinks = await DownloadAndConvertToCsvRecordsAsync<TransparencyCommissionOpinionLinks>("HAS_LiensPageCT_bdpm");

        var mergedMedications = medications
            .Join(medicationCompositions,
                medication => medication.CISCode,
                medicationComposition => medicationComposition.CISCode,
                (medication, medicationComposition) =>
                {
                    medication.MedicationComposition = medicationComposition;
                    return medication;
                })
            .Join(medicationPresentations,
                medication => medication.CISCode,
                medicationPresentation => medicationPresentation.CISCode,
                (medication, medicationPresentation) =>
                {
                    medication.MedicationPresentation = medicationPresentation;
                    return medication;
                })
            .Join(genericGroups,
                medication => medication.CISCode,
                genericGroup => genericGroup.CISCode,
                (medication, genericGroup) =>
                {
                    medication.GenericGroup = genericGroup;
                    return medication;
                })
            .Join(hasSmrOpinions,
                medication => medication.CISCode,
                hasSmrOpinion => hasSmrOpinion.CISCode,
                (medication, hasSmrOpinion) =>
                {
                    medication.HasSmrOpinion = hasSmrOpinion;
                    return medication;
                })
            .Join(hasAsmrOpinions,
                medication => medication.CISCode,
                hasAsmrOpinion => hasAsmrOpinion.CISCode,
                (medication, hasAsmrOpinion) =>
                {
                    medication.HasAsmrOpinion = hasAsmrOpinion;
                    return medication;
                })
            .Join(importantInformations,
                medication => medication.CISCode,
                importantInformation => importantInformation.CISCode,
                (medication, importantInformation) =>
                {
                    medication.ImportantInformation = importantInformation;
                    return medication;
                })
            .Join(prescriptionDispensingConditions,
                medication => medication.CISCode,
                prescriptionDispensingCondition => prescriptionDispensingCondition.CISCode,
                (medication, prescriptionDispensingCondition) =>
                {
                    medication.PrescriptionDispensingConditions = prescriptionDispensingCondition;
                    return medication;
                })
            .ToList();

        string json = JsonSerializer.Serialize(mergedMedications, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync("medications.json", json);

        stopwatch.Stop();
        Console.WriteLine($"Temps d'exécution: {stopwatch.ElapsedMilliseconds} ms");
    }

    public static async Task<string> DownloadAsync(string file)
    {
        using (HttpClient client = new HttpClient())
        {
            Uri baseApiUri = new Uri("https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=");
            var fileUriBuilder = new UriBuilder(baseApiUri);
            fileUriBuilder.Query += file + ".txt";
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

    public static async Task<List<T>> DownloadAndConvertToCsvRecordsAsync<T>(string file)
    {
        string csvData = await DownloadAsync(file);
        List<T> records = ConvertToCsvRecords<T>(csvData);
        return records;
    }
}