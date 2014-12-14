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
      public IReadOnlyCollection<SourceFile> Scan(string directory)
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
         return enumerable
            .Select(x => new SourceFile
            {
               Path = x.Value<string>("SourceFile"),
               Size = x.Value<int>("FileSize"),
               Timestamp = DateTime.ParseExact(x.Value<string>("DateTimeOriginal"), "yyyyMMddHHmmss", null)
            })
            .ToList();
      }

      public class SourceFile
      {
         public string Path { get; set; }
         public DateTime Timestamp { get; set; }
         public long Size { get; set; }
      }
   }
}