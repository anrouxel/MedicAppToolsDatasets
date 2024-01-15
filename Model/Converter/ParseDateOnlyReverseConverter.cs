using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

/// <summary>
/// Classe pour convertir les chaînes de caractères en objets DateOnly.
/// </summary>
public class ParseDateOnlyReverseConverter : DefaultTypeConverter
{
    /// <summary>
    /// Convertit une chaîne de caractères en DateOnly.
    /// </summary>
    /// <param name="text">La chaîne de caractères à convertir.</param>
    /// <param name="row">La ligne de données actuellement lue.</param>
    /// <param name="memberMapData">Les données de mappage de membre pour le membre actuellement en cours de lecture.</param>
    /// <returns>Retourne un objet DateOnly si la conversion est réussie, sinon null.</returns>
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        DateOnly date;
        if (DateOnly.TryParseExact(text, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date))
        {
            return date;
        }
        else
        {
            return null;
        }
    }
}