using System.IO;

namespace PhotoOrganizer
{
    class CopyWorker : WorkerBase
    {
        protected override void Execute(string sourcePath, string targetPath)
        {
            File.Copy(sourcePath, targetPath);
        }

        protected override void TargetAlreadyExists(string sourcePath)
        {
        }
    }
}