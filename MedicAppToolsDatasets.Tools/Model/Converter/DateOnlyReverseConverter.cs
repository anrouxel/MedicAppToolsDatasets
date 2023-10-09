using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class DateOnlyReverseConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        DateOnly date;
        if (DateOnly.TryParseExact(text, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date))
        {
            return date;
        }
        else
        {
            return null;
        }
    }
}