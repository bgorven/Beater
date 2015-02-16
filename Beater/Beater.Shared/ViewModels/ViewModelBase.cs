using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Beater.ViewModels
{

    class ViewModelBase : INotifyPropertyChanged
    {
        protected static readonly Task nullTask = Task.Delay(0);

        public virtual event PropertyChangedEventHandler PropertyChanged;

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

        internal void PropagatePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null && args != null)
            {
                PropertyChanged(this, args);
            }
        }
    }
}
