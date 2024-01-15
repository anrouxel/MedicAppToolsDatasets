using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

/// <summary>
/// Classe pour convertir les chaînes de caractères en objets Decimal.
/// </summary>
public class ParseDecimalConverter : DefaultTypeConverter
{
    /// <summary>
    /// Convertit une chaîne de caractères en Decimal.
    /// </summary>
    /// <param name="text">La chaîne de caractères à convertir.</param>
    /// <param name="row">La ligne de données actuellement lue.</param>
    /// <param name="memberMapData">Les données de mappage de membre pour le membre actuellement en cours de lecture.</param>
    /// <returns>Retourne un objet Decimal si la conversion est réussie, sinon null.</returns>
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        int lastCommaIndex = text.LastIndexOf(',');

        if (lastCommaIndex >= 0)
        {
            // Remplace le dernier "," par "."
            text = text.Substring(0, lastCommaIndex) + "." + text.Substring(lastCommaIndex + 1);
        }

        return decimal.Parse(text, CultureInfo.InvariantCulture);
    }
}