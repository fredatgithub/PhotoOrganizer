using System.Linq;
using Handyman.Wpf;

namespace PhotoOrganizer
{
   public class WorkInProgressWindowViewModel
   {
      public WorkInProgressWindowViewModel()
      {
         Info = new Observable<string>();
         Progress = new Observable<int>();
         TotalWork = new Observable<int>();
         IsIndeterminate = ReadOnlyObservable.Create(new[] { TotalWork }, items => items.Single().Value == 0);
      }

      public Observable<string> Info { get; private set; }
      public Observable<int> Progress { get; private set; }
      public Observable<int> TotalWork { get; private set; }
      public IReadOnlyObservable<bool> IsIndeterminate { get; private set; }
   }
}