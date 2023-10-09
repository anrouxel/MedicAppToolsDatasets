using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class DateOnlyReverseDashConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        DateOnly date;
        if (DateOnly.TryParseExact(text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
        {
            return date;
        }
        else
        {
            return null;
        }
    }
}