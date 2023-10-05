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
        string[] lines = result.Split('\n');
        foreach (string line in lines)
        {
            string[] data = line.Split('\t');
            foreach (string item in data)
            {
                Console.Write(item + "|");
            }
            Console.WriteLine();
        }
    }
}
