using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

/// <summary>
/// Classe pour convertir les chaînes de caractères en chaînes de caractères traitées.
/// </summary>
public class ParseStringConverter : DefaultTypeConverter
{
    /// <summary>
    /// Convertit une chaîne de caractères en une chaîne de caractères traitée.
    /// </summary>
    /// <param name="text">La chaîne de caractères à convertir.</param>
    /// <param name="row">La ligne de données actuellement lue.</param>
    /// <param name="memberMapData">Les données de mappage de membre pour le membre actuellement en cours de lecture.</param>
    /// <returns>Retourne une chaîne de caractères traitée si la conversion est réussie, sinon une chaîne vide.</returns>
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == null)
        {
            return string.Empty;
        }
        return text.Trim().ToUpperInvariant();
    }
}