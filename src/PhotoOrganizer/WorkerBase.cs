using System.IO;
using System.Linq;
using Handyman;

namespace PhotoOrganizer
{
    abstract class WorkerBase
    {
        public void Execute(WorkerInput input)
        {
            var targetPath = GetTargetPath(input);
            var targetDirectory = Path.GetDirectoryName(targetPath);
            var targetFile = Path.GetFileNameWithoutExtension(targetPath);
            var targetExtension = Path.GetExtension(targetPath);

            FileSystemUtil.MakeSureDirectoryExists(targetDirectory);

            var pattern = string.Format("{0}*{1}", targetFile, targetExtension);
            var files = new DirectoryInfo(targetDirectory).GetFiles(pattern, SearchOption.TopDirectoryOnly);
            if (files.Any(x => x.Length == input.SourceFile.Size)) return;
            if (!files.Any())
            {
                Execute(input.SourceFile.Path, targetPath, input.MarkTargetFilesAsReadOnly);
                return;
            }

            var nextIndex = files.Select(x => Path.GetFileNameWithoutExtension(x.Name))
                                 .Select(x => x.SubstringSafe(0, targetFile.Length + 1))
                                 .Select(int.Parse)
                                 .Max() + 1;
            targetPath = Path.Combine(targetDirectory, string.Format("{0}-{1}{2}", targetFile, nextIndex, targetExtension));
            Execute(input.SourceFile.Path, targetPath);
        }

        private void Execute(string sourcePath, string targetPath, bool @readonly)
        {
            Execute(sourcePath, targetPath);
            if (@readonly) File.SetAttributes(targetPath, File.GetAttributes(targetPath) | FileAttributes.ReadOnly);
        }

        private string GetTargetPath(WorkerInput input)
        {
            var dir = input.TargetDirectory;
            var relativePath = FileNameBuilder.GetFileName(input.SourceFile, input.RenamePattern);
            return Path.Combine(dir, relativePath);
        }

        protected abstract void Execute(string sourcePath, string targetPath);
        protected abstract void TargetAlreadyExists(string sourcePath);
    }
}