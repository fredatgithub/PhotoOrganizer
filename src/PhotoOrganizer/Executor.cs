namespace PhotoOrganizer
{
    internal class Executor
    {
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public string RenamePattern { get; set; }
        public bool MarkTargetFilesAsReadOnly { get; set; }

        public void Execute(WorkerBase worker)
        {
            var sourceFiles = new ExifReader().Scan(SourceDirectory);
            foreach (var sourceFile in sourceFiles)
            {
                worker.Execute(new WorkerInput
                {
                    MarkTargetFilesAsReadOnly = MarkTargetFilesAsReadOnly,
                    RenamePattern = RenamePattern,
                    SourceFile = sourceFile,
                    TargetDirectory = TargetDirectory
                });
            }
        }
    }
}