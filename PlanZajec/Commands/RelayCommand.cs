using System;
using System.Windows.Input;

namespace PlanZajec
{
    public class RelayCommand : ICommand
    {
        private Action mAction;

        private Func<bool> mCanExecute;

        public event EventHandler CanExecuteChanged = (obj, e) => { };

        public RelayCommand(Action action)
        {
            mAction = action;
            mCanExecute = () => true;
        }

        public RelayCommand(Action action, Func<bool> canexecute)
        {
            mAction = action;
            mCanExecute = canexecute;
        }

        public bool CanExecute(object parameter)
        {
            return mCanExecute();
        }

        public void Execute(object parameter)
        {
            mAction();
        }
    }
}
