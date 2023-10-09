using CsvHelper.Configuration.Attributes;

public class HasAsmrOpinion
{
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string HasDossierCode { get; set; } = string.Empty;

    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string EvaluationReason { get; set; } = string.Empty;

    [Index(3)]
    [TypeConverter(typeof(ParseDateOnlyReverseConverter))]
    public DateOnly? TransparencyCommissionOpinionDate { get; set; }

    [Index(4)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string AsmrValue { get; set; } = string.Empty;

    [Index(5)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string AsmrLabel { get; set; } = string.Empty;
}