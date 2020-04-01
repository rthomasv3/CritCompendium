using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CritCompendium
{
   public class NotifyPropertyChanged : INotifyPropertyChanged
   {
      public event PropertyChangedEventHandler PropertyChanged;

      protected void OnPropertyChanged([CallerMemberName] string callerName = "")
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
      }

      protected bool Set<T>(ref T obj, T value, [CallerMemberName] string callerName = "")
      {
         bool set = false;

         if ((obj != null && !obj.Equals(value)) || (obj == null && value != null))
         {
            obj = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
            set = true;
         }

         return set;
      }
   }
}
