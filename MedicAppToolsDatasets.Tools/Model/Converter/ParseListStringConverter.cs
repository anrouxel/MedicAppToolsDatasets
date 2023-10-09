using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class ParseListStringConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<string>();
        }
        return text.Split(';').Select(s => s.Trim().ToUpperInvariant()).ToList();
    }
}