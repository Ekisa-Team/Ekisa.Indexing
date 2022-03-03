namespace Ekisa.Indexing.Watcher.Utils;

public class EnumUtils
{
    public static List<string> EnumNamedValues<EnumType>() where EnumType : struct, Enum
    {
        return Enum
            .GetValues(typeof(EnumType))
            .Cast<EnumType>()
            .Select(x => x.ToString())
            .ToList();
    }
}