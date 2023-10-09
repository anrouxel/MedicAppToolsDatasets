using CsvHelper.Configuration.Attributes;

public class HasSmrOpinion
{
    [Index(0)]
    [TypeConverter(typeof(StringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(StringConverter))]
    public string HasDossierCode { get; set; } = string.Empty;

    [Index(2)]
    [TypeConverter(typeof(StringConverter))]
    public string EvaluationReason { get; set; } = string.Empty;

    [Index(3)]
    [TypeConverter(typeof(DateOnlyReverseConverter))]
    public DateOnly TransparencyCommissionOpinionDate { get; set; }

    [Index(4)]
    [TypeConverter(typeof(StringConverter))]
    public string SmrValue { get; set; } = string.Empty;

    [Index(5)]
    [TypeConverter(typeof(StringConverter))]
    public string SmrLabel { get; set; } = string.Empty;
}