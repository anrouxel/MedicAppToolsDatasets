using CsvHelper.Configuration.Attributes;

public class TransparencyCommissionOpinionLinks
{
    [Index(0)]
    [TypeConverter(typeof(StringConverter))]
    public string HasDossierCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(StringConverter))]
    public string CommissionOpinionLink { get; set; } = string.Empty;
}