using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class ParseListPourcentConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<decimal>();
        }
        return text.Split(';').Select(s => decimal.Parse(s.Trim().TrimEnd('%').Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture)).ToList();
    }
}