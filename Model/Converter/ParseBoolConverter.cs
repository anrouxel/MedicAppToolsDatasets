using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

/// <summary>
/// Classe pour convertir les chaînes de caractères "Oui" et "Non" en booléens.
/// </summary>
public class ParseBoolConverter : DefaultTypeConverter
{
    /// <summary>
    /// Convertit une chaîne de caractères en booléen.
    /// </summary>
    /// <param name="text">La chaîne de caractères à convertir.</param>
    /// <param name="row">La ligne de données actuellement lue.</param>
    /// <param name="memberMapData">Les données de mappage de membre pour le membre actuellement en cours de lecture.</param>
    /// <returns>Retourne true si la chaîne est "Oui", false si la chaîne est "Non", et null dans les autres cas.</returns>
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }
        
        if (text.Trim().Equals("Oui", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        else if (text.Trim().Equals("Non", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        else
        {
            return null;
        }
    }
}