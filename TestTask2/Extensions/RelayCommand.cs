using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestTask2
{
    public class RelayCommand : ICommand
    {        private Action<object> _action;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Base constructor, create a command 
        /// </summary>
        /// <param name="execute">function which need execute</param>
        /// <param name="canExecute">Optional parameter, function whatever will check </param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this._action = execute;
            this._canExecute = canExecute;
        }
        /// <summary
        /// Check, can function execute with/without parameters
        /// </summary>
        /// <param name="parameter">Parameters, which may need in function</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        /// <summary>        /// Execution function which initialize action. 
        /// </summary>
        /// <param name="parameter">Parameters, which may need in function</param>
        public void Execute(object parameter)
        {
            this._action(parameter);
        }
    }
}
