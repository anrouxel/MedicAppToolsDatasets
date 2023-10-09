using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class DateOnlySlashConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        DateOnly date;
        if (DateOnly.TryParseExact(text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
        {
            return date;
        }
        else
        {
            return null;
        }
    }
}