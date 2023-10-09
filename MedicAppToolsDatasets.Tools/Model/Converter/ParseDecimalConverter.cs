using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class ParseDecimalConverter : DefaultTypeConverter
{
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