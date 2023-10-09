using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class StringConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == null)
        {
            return string.Empty;
        }
        return text.Trim().ToUpperInvariant();
    }
}