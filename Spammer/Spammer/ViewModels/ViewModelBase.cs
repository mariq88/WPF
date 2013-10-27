using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spammer
{
    public abstract class ViewModelBase:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var propertyChangedEventArgs = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, propertyChangedEventArgs);
            }
        }
    }
}
