using System;
using System.IO;

namespace PhotoOrganizer
{
    static class FileNameBuilder
    {
        public static string GetFileName(ExifReader.SourceFile sourceFile, string pattern)
        {
            while (true)
            {
                var start = pattern.IndexOf('<', 0);
                if (start == -1) break;
                var stop = pattern.IndexOf('>', start);
                if (stop == -1) throw new InvalidOperationException();
                var length = stop - start;
                var format = pattern.Substring(start + 1, length - 1);
                pattern = pattern.Remove(start, length + 1);
                var replacement = sourceFile.Timestamp.ToString(format);
                pattern = pattern.Insert(start, replacement);
            }
            return pattern + Path.GetExtension(sourceFile.Path);
        }
    }
}