public class Medication
{
    public string CISCode { get; set; }
    public string Name { get; set; }
    public string PharmaceuticalForm { get; set; }
    public List<string> AdministrationRoutes { get; set; }
    public string MarketingAuthorizationStatus { get; set; }
    public string MarketingAuthorizationProcedureType { get; set; }
    public string CommercializationStatus { get; set; }
    public DateOnly MarketingAuthorizationDate { get; set; }
    public string BdmStatus { get; set; }
    public string EuropeanAuthorizationNumber { get; set; }
    public List<string> Holders { get; set; }
    public bool EnhancedMonitoring { get; set; }
}