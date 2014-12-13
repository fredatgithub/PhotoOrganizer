using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoOrganizer
{
   public class ExifReader
   {
      public IEnumerable<ExifFile> Scan(string directory)
      {
         return from filePath in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories)
                let file = new FileInfo(filePath)
                select new ExifFile
                {
                   CapturedDate = file.LastWriteTime,
                   Path = filePath,
                   Size = (int)file.Length
                };
      }

      public class ExifFile
      {
         public string Path { get; set; }
         public DateTime CapturedDate { get; set; }
         public int Size { get; set; }
      }
   }
}