using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhotoOrganizer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PhotoOrganizer
{
   public class ExifReader
   {
      public IEnumerable<ExifFile> Scan(string directory)
      {
         var tempDir = Path.GetTempPath();
         var exiftoolPath = Path.Combine(tempDir, "exiftool.exe");
         if (!File.Exists(exiftoolPath))
            File.WriteAllBytes(exiftoolPath, Resources.exiftool);

         var process = new Process
         {
            EnableRaisingEvents = false,
            StartInfo =
            {
               CreateNoWindow = true,
               LoadUserProfile = false,
               RedirectStandardOutput = true,
               RedirectStandardInput = true,
               StandardOutputEncoding = Encoding.UTF8,
               UseShellExecute = false,
               WindowStyle = ProcessWindowStyle.Hidden,
               FileName = string.Format("\"{0}\"", exiftoolPath),
               Arguments = string.Format("\"{0}\" -json  -FileSize# -DateTimeOriginal -d \"%Y%m%d%H%M%S\"", directory)
            }
         };
         process.Start();
         process.WaitForExit();
         var enumerable = JsonConvert.DeserializeObject<IEnumerable<JObject>>(process.StandardOutput.ReadToEnd());
         return from d in enumerable
                select new ExifFile
                {
                   Path = d.Value<string>("SourceFile"),
                   Size = d.Value<int>("FileSize"),
                   Timestamp = DateTime.ParseExact(d.Value<string>("DateTimeOriginal"), "yyyyMMddHHmmss", null)
                };
      }

      public class ExifFile
      {
         public string Path { get; set; }
         public DateTime Timestamp { get; set; }
         public int Size { get; set; }
      }
   }
}