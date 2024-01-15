/// <summary>
/// Classe générique représentant une requête JSON.
/// </summary>
/// <typeparam name="T">Le type de données contenues dans la requête.</typeparam>
public class RequestJson<T>
{
    /// <summary>
    /// Obtient ou définit les données de la requête.
    /// </summary>
    public T? Data { get; set; }
}