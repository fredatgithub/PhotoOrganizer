using System.IO;
using System.Text;
using System.Windows;
using Handyman;
using Handyman.Wpf;
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
         MakeReadOnly = new Observable<bool>();

         PreviewCommand = new RelayCommand(Preview, CanExecute);
         CopyCommand = new RelayCommand(Copy, CanExecute);
         MoveCommand = new RelayCommand(Move, CanExecute);
      }

      private void Preview()
      {
         var exifReader = new ExifReader();
         var exifFiles = exifReader.Scan(SourceDirectory);
         var builder = new StringBuilder();
         exifFiles.ForEach(x =>
         {
            builder.AppendFormat("{0} timestamp:{1}, size:{2}", x.Path, x.Timestamp.ToString("yyyyMMdd hhmmss"), x.Size);
            builder.AppendLine();
         });
         MessageBox.Show(builder.ToString());
      }

      private void Copy()
      {
         throw new System.NotImplementedException();
      }

      private void Move()
      {
         throw new System.NotImplementedException();
      }

      private bool CanExecute()
      {
         return Directory.Exists(SourceDirectory);
      }

      public Observable<string> SourceDirectory { get; private set; }
      public Observable<string> TargetDirectory { get; private set; }
      public Observable<string> RenamePattern { get; private set; }
      public Observable<bool> MakeReadOnly { get; private set; }

      public ICommand PreviewCommand { get; private set; }
      public ICommand CopyCommand { get; private set; }
      public ICommand MoveCommand { get; private set; }
   }
}