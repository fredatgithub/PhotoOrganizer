using System.IO;

namespace PhotoOrganizer
{
    internal static class FileSystemUtil
    {
        public static void MakeSureDirectoryExists(string directory)
        {
            if (Directory.Exists(directory)) return;
            MakeSureDirectoryExists(Path.GetDirectoryName(directory));
            Directory.CreateDirectory(directory);
        }
    }
}