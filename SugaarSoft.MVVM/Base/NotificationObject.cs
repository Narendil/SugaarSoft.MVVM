using SugaarSoft.MVVM.Helpers;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace SugaarSoft.MVVM.Base
{
    public class NotificationObject : INotifyPropertyChanged
    {
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        protected void OnPropertyChanged<TModel>(Expression<Func<TModel, Object>> propertyExpression)
        {
            OnPropertyChanged(PropertyNameHelper.GetPropertyName(propertyExpression));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertyNameHelper.GetPropertyName(propertyExpression);
            if (propertyName != null)
                OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members
    }
}
