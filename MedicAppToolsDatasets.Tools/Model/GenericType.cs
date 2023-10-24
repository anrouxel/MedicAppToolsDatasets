public static class GenericType
{
    private static readonly Dictionary<int, GenericTypeInfo> GenericTypeInfoMap = new Dictionary<int, GenericTypeInfo>
    {
        [0] = new GenericTypeInfo { GenericTypeId = 0, GenericTypeLabel = "princeps" },
        [1] = new GenericTypeInfo { GenericTypeId = 1, GenericTypeLabel = "générique" },
        [2] = new GenericTypeInfo { GenericTypeId = 2, GenericTypeLabel = "génériques par complémentarité posologique" },
        [4] = new GenericTypeInfo { GenericTypeId = 4, GenericTypeLabel = "générique substituable" }
    };

    public static GenericTypeInfo? FindByGenericTypeId(int typeId)
    {
        if (GenericTypeInfoMap.TryGetValue(typeId, out var typeInfo))
        {
            return typeInfo;
        }
        return null;
    }
}