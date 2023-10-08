public class MedicationPresentation
{
    public string CISCode { get; set; }
    public string CIP7Code { get; set; }
    public string PresentationLabel { get; set; }
    public string PresentationStatus { get; set; }
    public string PresentationCommercializationStatus { get; set; }
    public DateOnly CommercializationDeclarationDate { get; set; }
    public string CIP13Code { get; set; }
    public string ApprovalForCommunities { get; set; }
    public List<decimal> ReimbursementRates { get; set; }
    public decimal PriceInEuros { get; set; }
    public string ReimbursementIndications { get; set; }
}