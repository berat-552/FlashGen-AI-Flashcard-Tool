using System.IO;

namespace FlashGen;

public static class Extensions
{
    public static bool IsTextFile(this FileInfo self) =>
        self.Extension.Equals(
            ".txt", StringComparison.OrdinalIgnoreCase);
}
