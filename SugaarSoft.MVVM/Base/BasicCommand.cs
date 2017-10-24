using System;
using System.Windows.Input;

namespace SugaarSoft.MVVM.Base
{
    public class BasicCommand : ICommand
    {
        public BasicCommand(Action<object> executeMethod)
            : this(executeMethod, null)
        {
        }

        public BasicCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            ExecuteMethod = executeMethod;
            CanExecuteMethod = canExecuteMethod;
        }

        //Properties
        public Func<object, bool> CanExecuteMethod { get; set; }

        public Action<object> ExecuteMethod { get; set; }

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteMethod == null || CanExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            if (ExecuteMethod == null)
                return;
            ExecuteMethod(parameter);
        }

        #endregion ICommand Members
    }

    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(object parameter)
        {
            CommandParameter = parameter;
        }

        public object CommandParameter { get; set; }
    }
}