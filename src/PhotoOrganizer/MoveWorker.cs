using System.IO;

namespace PhotoOrganizer
{
    class MoveWorker : WorkerBase
    {
        protected override void Execute(string sourcePath, string targetPath)
        {
            File.Move(sourcePath, targetPath);
        }

        protected override void TargetAlreadyExists(string sourcePath)
        {
            File.Delete(sourcePath);
        }
    }
}