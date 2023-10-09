using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class ListStringConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == null)
        {
            return new List<string>();
        }
        return text.Split(';').Select(s => s.Trim().ToUpperInvariant()).ToList();
    }
}