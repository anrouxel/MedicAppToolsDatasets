using System.Globalization;
using System.Collections;
using System.Diagnostics;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;

public class Program
{
    public static async Task Main(string[] args)
    {
        Uri baseApiUri = new Uri("https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=");

        Dictionary<string, Type> fileMappings = new Dictionary<string, Type>
        {
            {"CIS_bdpm", typeof(Medication)},
            {"CIS_COMPO_bdpm", typeof(MedicationComposition)},
            {"CIS_CPD_bdpm", typeof(PrescriptionDispensingConditions)},
            {"CIS_HAS_SMR_bdpm", typeof(HasSmrOpinion)},
            {"HAS_LiensPageCT_bdpm", typeof(TransparencyCommissionOpinionLinks)},
            {"CIS_HAS_ASMR_bdpm", typeof(HasAsmrOpinion)},
            {"CIS_GENER_bdpm", typeof(GenericGroup)},
            {"CIS_InfoImportantes", typeof(ImportantInformation)},
            {"CIS_CIP_bdpm", typeof(MedicationPresentation)},
        };

        List<Task> tasks = new List<Task>();
        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (KeyValuePair<string, Type> fileMapping in fileMappings)
        {
            UriBuilder fileUriBuilder = new UriBuilder(baseApiUri);
            fileUriBuilder.Query += fileMapping.Key + ".txt";
            Uri fileUri = fileUriBuilder.Uri;
            tasks.Add(DownloadConvertAndSaveAsync(fileUri, fileMapping.Key, fileMapping.Value));
        }
        await Task.WhenAll(tasks);

        stopwatch.Stop();
        Console.WriteLine($"Total time taken: {stopwatch.ElapsedMilliseconds} ms");
    }

    public static async Task DownloadConvertAndSaveAsync(Uri fileUri, string fileName, Type type)
    {
        using (HttpClient client = new HttpClient())
        {
            // define le .txt à la fin de l'url
            HttpResponseMessage response = await client.GetAsync(fileUri);
            if (response.IsSuccessStatusCode)
            {
                if (response.Content.Headers.ContentType != null)
                {
                    response.Content.Headers.ContentType.CharSet = "latin1";
                }
                string result = await response.Content.ReadAsStringAsync();

                using (TextReader reader = new StringReader(result))
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
                        var records = csv.GetRecords(type).ToList();

                        string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText($"{fileName}.json", json);
                    }
                }
            }
            Console.WriteLine($"Status Code: {(int)response.StatusCode} - {response.ReasonPhrase}");
        }
    }
}