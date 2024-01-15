/// <summary>
/// Classe représentant les informations sur un type générique.
/// </summary>
public class GenericTypeInfo
{
    /// <summary>
    /// Obtient ou définit l'identifiant du type générique.
    /// </summary>
    public int GenericTypeId { get; set; }

    private string genericTypeLabel = string.Empty;

    /// <summary>
    /// Obtient ou définit le libellé du type générique.
    /// La valeur est toujours convertie en majuscules.
    /// </summary>
    public string GenericTypeLabel
    {
        get { return genericTypeLabel; }
        set { genericTypeLabel = value.ToUpper(); }
    }
}