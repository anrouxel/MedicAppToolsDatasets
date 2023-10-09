using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;

string CIS_bdpm = "CIS_bdpm";
string CIS_COMPO_bdpm = "CIS_COMPO_bdpm";
string CIS_CPD_bdpm = "CIS_CPD_bdpm";
string CIS_HAS_SMR = "CIS_HAS_SMR";
string HAS_LiensPageCT_bdpm = "HAS_LiensPageCT_bdpm";
string CIS_HAS_ASMR = "CIS_HAS_ASMR";
string CIS_GENER_bdpm = "CIS_GENER_bdpm";
string CIS_InfoImportantes_AAAAMMJJhhmiss_bdpm = "CIS_InfoImportantes_AAAAMMJJhhmiss_bdpm";
string CIS_CIP_bdpm = "CIS_CIP_bdpm";

Uri url = new Uri($"https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=");

using (HttpClient client = new HttpClient())
{
    HttpResponseMessage response = await client.GetAsync(url + CIS_COMPO_bdpm + ".txt");
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
                var records = csv.GetRecords<MedicationComposition>().ToList();

                foreach (var record in records)
                {
                    Console.WriteLine($"CISCode : {record.CISCode}");
                }

                string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("CIS_COMPO_bdpm.json", json);
            }
        }
    }
}
