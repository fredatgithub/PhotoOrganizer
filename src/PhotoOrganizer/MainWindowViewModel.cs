using Handyman.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows.Input;

namespace PhotoOrganizer
{
   public class MainWindowViewModel
   {
      public MainWindowViewModel()
      {
         SourceDirectory = new Observable<string>();
         TargetDirectory = new Observable<string>();
         RenamePattern = new Observable<string>();
         MarkTargetFilesAsReadOnly = new Observable<bool>();

         CopyCommand = new RelayCommand(Copy, CanExecute);
         MoveCommand = new RelayCommand(Move, CanExecute);
      }

      private void Copy()
      {
         Execute(new CopyWorker());
      }

      private void Move()
      {
         Execute(new MoveWorker());
      }

      private void Execute(WorkerBase worker)
      {
         SaveConfig();
         new Executor
         {
            SourceDirectory = SourceDirectory,
            TargetDirectory = TargetDirectory,
            RenamePattern = RenamePattern,
            MarkTargetFilesAsReadOnly = MarkTargetFilesAsReadOnly
         }.Execute(worker);
      }

      private bool CanExecute()
      {
         return Directory.Exists(SourceDirectory);
      }

      public Observable<string> SourceDirectory { get; private set; }
      public Observable<string> TargetDirectory { get; private set; }
      public Observable<string> RenamePattern { get; private set; }
      public Observable<bool> MarkTargetFilesAsReadOnly { get; private set; }

      public ICommand PreviewCommand { get; private set; }
      public ICommand CopyCommand { get; private set; }
      public ICommand MoveCommand { get; private set; }

      public void LoadConfig()
      {
         var path = GetSettingsPath();
         if (!File.Exists(path)) return;
         var json = File.ReadAllText(path);
         var settings = JsonConvert.DeserializeObject<JObject>(json);
         SourceDirectory.Value = settings.Value<string>("SourceDirectory");
         TargetDirectory.Value = settings.Value<string>("TargetDirectory");
         RenamePattern.Value = settings.Value<string>("RenamePattern");
         MarkTargetFilesAsReadOnly.Value = settings.Value<bool>("MarkTargetFilesAsReadOnly");
      }

      private static string GetSettingsPath()
      {
         var dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
         return Path.Combine(dir, "PhotoOrganizer", "settings.json");
      }

      private void SaveConfig()
      {
         var path = GetSettingsPath();
         FileSystemUtil.MakeSureDirectoryExists(Path.GetDirectoryName(path));
         var settings = new
         {
            SourceDirectory = SourceDirectory.Value,
            TargetDirectory = TargetDirectory.Value,
            RenamePattern = RenamePattern.Value,
            MarkTargetFilesAsReadOnly = MarkTargetFilesAsReadOnly.Value
         };
         var json = JsonConvert.SerializeObject(settings);
         File.WriteAllText(path, json);
      }
   }
}