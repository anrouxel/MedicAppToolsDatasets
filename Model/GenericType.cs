/// <summary>
/// Classe statique pour gérer les types génériques.
/// </summary>
public static class GenericType
{
    /// <summary>
    /// Dictionnaire pour mapper les identifiants de type générique aux informations de type.
    /// </summary>
    private static readonly Dictionary<int, string> GenericTypeInfoMap = new Dictionary<int, string>
    {
        [0] = "princeps",
        [1] = "générique",
        [2] = "génériques par complémentarité posologique",
        [4] = "générique substituable"
    };

    /// <summary>
    /// Trouve les informations de type générique par identifiant de type.
    /// </summary>
    /// <param name="typeId">L'identifiant du type générique.</param>
    /// <returns>Les informations de type générique si l'identifiant est trouvé, sinon null.</returns>
    public static string? FindByGenericTypeId(int typeId)
    {
        if (GenericTypeInfoMap.TryGetValue(typeId, out var typeInfo))
        {
            return typeInfo;
        }
        return null;
    }
}