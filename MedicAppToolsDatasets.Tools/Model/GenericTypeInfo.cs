public class GenericTypeInfo
{
    public int GenericTypeId { get; set; }

    private string genericTypeLabel = string.Empty;

    public string GenericTypeLabel
    {
        get { return genericTypeLabel; }
        set { genericTypeLabel = value.ToUpper(); }
    }
}