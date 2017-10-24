using SugaarSoft.MVVM.Helpers;
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SugaarSoft.MVVM.Base
{
    public class VisualCommand : BasicCommand, INotifyPropertyChanged
    {

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    OnPropertyChanged(this.GetPropertyName(() => ImageSource));
                }
            }
        }

        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption != value)
                {
                    _caption = value;
                    OnPropertyChanged(this.GetPropertyName(() => Caption));
                }
            }
        }

        #region CTOR

        public VisualCommand(string caption, string imagepath, Action<object> executeMethod)
            : this(caption, new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute)), executeMethod)
        {

        }

        public VisualCommand(string caption, string imagepath, Action<object> executeMethod, Func<object, bool> canExecuteMethod)
            : this(caption, new BitmapImage(new Uri(imagepath, UriKind.RelativeOrAbsolute)), executeMethod, canExecuteMethod)
        {

        }

        public VisualCommand(string caption, ImageSource imageSource, Action<object> executeMethod)
            : this(caption, imageSource, executeMethod, (param) => true)
        {

        }
        public VisualCommand(string caption, ImageSource imageSource, Action<object> executeMethod, Func<object, bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {
            Caption = caption;
            ImageSource = imageSource;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null && PropertyChanged != null)
            {
                var eventArgs = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, eventArgs);
            }
        }

    }
}
