using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class BoolConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == null)
        {
            return null;
        }
        return text.Trim().Equals("Oui", StringComparison.OrdinalIgnoreCase);
    }
}