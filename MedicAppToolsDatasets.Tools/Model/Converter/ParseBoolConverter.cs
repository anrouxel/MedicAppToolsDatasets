using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class ParseBoolConverter : DefaultTypeConverter
{
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