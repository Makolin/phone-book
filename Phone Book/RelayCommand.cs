using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Phone_Book
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
            this.execute = execute;
            this.canExecute = canExecute;
        }

        // Может ли команда выполняться
        public bool CanExecute(object parametr)
        {
            return this.canExecute == null || this.canExecute(parametr);
        }
        // Выполнение логики программы
        public void Execute(object parametr)
        {
            this.execute(parametr);
        }
    }
}
