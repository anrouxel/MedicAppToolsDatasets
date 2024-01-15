using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

/// <summary>
/// Classe pour convertir les chaînes de caractères en objets GenericType.
/// </summary>
public class ParseGenericTypeConverter : DefaultTypeConverter
{
    /// <summary>
    /// Convertit une chaîne de caractères en GenericType.
    /// </summary>
    /// <param name="text">La chaîne de caractères à convertir.</param>
    /// <param name="row">La ligne de données actuellement lue.</param>
    /// <param name="memberMapData">Les données de mappage de membre pour le membre actuellement en cours de lecture.</param>
    /// <returns>Retourne un objet GenericType si la conversion est réussie, sinon null.</returns>
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return GenericType.FindByGenericTypeId(int.Parse(text ?? string.Empty));
    }
}