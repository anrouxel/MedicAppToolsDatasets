/// <summary>
/// Classe représentant les données d'une phrase.
/// </summary>
public class SentenceData
{
    /// <summary>
    /// Obtient ou définit la phrase.
    /// </summary>
    public string? Sentence { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des étiquettes associées à la phrase.
    /// </summary>
    public List<string>? Labels { get; set; }
}