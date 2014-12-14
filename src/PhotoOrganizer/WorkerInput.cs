namespace PhotoOrganizer
{
    class WorkerInput
    {
        public ExifReader.SourceFile SourceFile { get; set; }
        public string TargetDirectory { get; set; }
        public string RenamePattern { get; set; }
        public bool MarkTargetFilesAsReadOnly { get; set; }
    }
}