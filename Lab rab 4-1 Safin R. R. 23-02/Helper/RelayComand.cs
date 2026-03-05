using System;
using System.Windows.Input;

namespace Lab_rab_4_1_Safin_R._R._23_02.Helper
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return canExecute == null || canExecute(parameter);
            }
            catch
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        // Метод для принудительного обновления команды
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}