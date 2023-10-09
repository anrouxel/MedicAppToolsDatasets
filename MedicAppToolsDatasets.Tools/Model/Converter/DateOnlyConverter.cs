using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class DateOnlyConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        DateOnly date;
        if (DateOnly.TryParseExact(text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
        {
            return date;
        }
        else if (DateOnly.TryParseExact(text, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date))
        {
            return date;
        }
        else
        {
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}