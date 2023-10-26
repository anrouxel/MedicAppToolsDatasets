public static class GenericType
{
    private static readonly Dictionary<int, string> GenericTypeInfoMap = new Dictionary<int, string>
    {
        [0] = "princeps",
        [1] = "générique",
        [2] = "génériques par complémentarité posologique",
        [4] = "générique substituable"
    };

    public static string? FindByGenericTypeId(int typeId)
    {
        if (GenericTypeInfoMap.TryGetValue(typeId, out var typeInfo))
        {
            return typeInfo;
        }
        return null;
    }
}