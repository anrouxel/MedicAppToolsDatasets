using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class BoolConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == null)
        {
            return base.ConvertFromString(text, row, memberMapData);
        }
        return text.Trim().Equals("Oui", StringComparison.OrdinalIgnoreCase);
    }
}