using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;

/// <summary>
/// Classe pour convertir les chaînes de caractères en objets Uri.
/// </summary>
public class ParseUriConverter : DefaultTypeConverter
{
    /// <summary>
    /// Convertit une chaîne de caractères en Uri.
    /// </summary>
    /// <param name="text">La chaîne de caractères à convertir.</param>
    /// <param name="row">La ligne de données actuellement lue.</param>
    /// <param name="memberMapData">Les données de mappage de membre pour le membre actuellement en cours de lecture.</param>
    /// <returns>Retourne un objet Uri si la conversion est réussie, sinon null.</returns>
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == null)
        {
            return null;
        }
        return new Uri(text.Trim());
    }
}