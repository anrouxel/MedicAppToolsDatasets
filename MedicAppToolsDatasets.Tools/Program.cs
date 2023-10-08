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
        Console.WriteLine(result);
    }
}
