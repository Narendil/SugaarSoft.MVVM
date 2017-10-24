using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SugaarSoft.MVVM.Base
{
    public class AsyncCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            bool rslt = true;
            if (_task != null)
                rslt = _task.Status == TaskStatus.RanToCompletion;

            if (rslt && _canExecute != null)
                rslt = _canExecute(parameter);

            return rslt;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        Task _task;
        public void Execute(object parameter)
        {
            //_task = new Task(ExecuteInternal, parameter);
            //_task.Start();
            // ver http://blogs.msdn.com/b/pfxteam/archive/2012/03/25/10287435.aspx
            //if (_task != null)
            //    _task.Dispose();

            Application.Current.Dispatcher.BeginInvoke((Action<bool>)SetEnabled, false);
            _task = Task.Factory.StartNew(ExecuteInternal, parameter);
        }

        private void SetEnabled(bool isEnabled)
        {
            if (SetIsEnabledAction != null)
                SetIsEnabledAction(isEnabled);
            RaiseCanExecuteChanged();
        }

        protected virtual void ExecuteInternal(object parameter)
        {
            try
            {
                if (_action != null)
                    _action();
                else if (_actionOfObject != null)
                    _actionOfObject(parameter);
                LaunchCallback();
            }
            catch (Exception ex)
            {
                LaunchErrorCallback(ex);
            }
            //_callback();

            Application.Current.Dispatcher.BeginInvoke((Action<bool>)SetEnabled, true);
        }

        protected virtual void LaunchErrorCallback(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("RegisterException {0}", new object[] { ex.Message });
            if (ErrorCallback != null)
                ErrorCallback(ex);
        }

        protected virtual void LaunchCallback()
        {
            if (Callback != null)
                Application.Current.Dispatcher.BeginInvoke(Callback);
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        readonly Action _action;
        readonly Action<object> _actionOfObject;
        readonly Func<object, bool> _canExecute;
        public AsyncCommand(Action action, Func<object, bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public AsyncCommand(Action<object> action, Func<object, bool> canExecute)
        {
            _actionOfObject = action;
            _canExecute = canExecute;
        }

        public Action Callback { get; set; }
        public Action<Exception> ErrorCallback { get; set; }
        public Action<bool> SetIsEnabledAction { get; set; }

    }
}
