using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;

string CIS_bdpm = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_bdpm.txt";
string CIS_COMPO_bdpm = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_COMPO_bdpm.txt";
string CIS_CPD_bdpm = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_CPD_bdpm.txt";
string CIS_HAS_SMR = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_HAS_SMR_bdpm.txt";
string HAS_LiensPageCT_bdpm = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=HAS_LiensPageCT_bdpm.txt";
string CIS_HAS_ASMR = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_HAS_ASMR_bdpm.txt";
string CIS_GENER_bdpm = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_GENER_bdpm.txt";
string CIS_InfoImportantes_AAAAMMJJhhmiss_bdpm = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_InfoImportantes.txt";
string CIS_CIP_bdpm = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_CIP_bdpm.txt";


using (HttpClient client = new HttpClient())
{
    HttpResponseMessage response = await client.GetAsync(CIS_bdpm);
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
                var records = csv.GetRecords<Medication>().ToList();

                foreach (var record in records)
                {
                    Console.WriteLine($"CISCode : {record.CISCode}");
                }

                string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("CIS_bdpm.json", json);
            }
        }
    }
}