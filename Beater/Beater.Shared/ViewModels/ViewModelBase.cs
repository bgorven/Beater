using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Beater.ViewModels
{

    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string[] changedProperties)
        {
            if (changedProperties != null)
            {
                foreach (var name in changedProperties)
                {
                    RaisePropertyChanged(name);
                }
            }
        }

        internal void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
