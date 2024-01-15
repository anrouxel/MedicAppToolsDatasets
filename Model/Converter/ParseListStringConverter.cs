using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Classe pour convertir les chaînes de caractères en listes de chaînes de caractères (List<string>).
/// </summary>
public class ParseListStringConverter : DefaultTypeConverter
{
    /// <summary>
    /// Convertit une chaîne de caractères en une liste de chaînes de caractères.
    /// </summary>
    /// <param name="text">La chaîne de caractères à convertir.</param>
    /// <param name="row">La ligne de données actuellement lue.</param>
    /// <param name="memberMapData">Les données de mappage de membre pour le membre actuellement en cours de lecture.</param>
    /// <returns>Retourne une liste de chaînes de caractères si la conversion est réussie, sinon une liste vide.</returns>
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<string>();
        }
        return text.Split(';').Select(s => s.Trim().ToUpperInvariant()).ToList();
    }
}