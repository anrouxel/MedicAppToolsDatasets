using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class ListPourcentConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == null)
        {
            return new List<decimal>();
        }
        // retourne une liste de pourcentage en decimal. La liste est séparer par un ;
        return text.Split(';').Select(s => decimal.TryParse(s.Trim().TrimEnd('%'), out var d) ? d / 100M : 0).ToList();
    }
}