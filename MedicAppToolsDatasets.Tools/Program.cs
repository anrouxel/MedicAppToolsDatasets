using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;

string url = "https://base-donnees-publique.medicaments.gouv.fr/telechargement.php?fichier=CIS_bdpm.txt";

using (HttpClient client = new HttpClient())
{
    HttpResponseMessage response = await client.GetAsync(url);
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
                    Console.WriteLine($"CISCode : {record.CISCode} - Name : {record.Name}");
                }

                string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("medications.json", json);
            }
        }
    }
}
