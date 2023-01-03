using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Influx.Write
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
        private Action? execute;
        private Action<object?>? ExecuteObject;
        private readonly Predicate<object?>? _canExecute;
        public bool CanExecute(object? parameter) { return _canExecute == null ? true : _canExecute(parameter); }
        public void Execute(object? parameter) { if (execute != null) execute(); if (ExecuteObject != null) ExecuteObject(parameter); }

        public RelayCommand(Action? execute) { this.execute = execute; }
        public RelayCommand(Action? execute, Predicate<object?> _CanExecute) { this.execute = execute; this._canExecute = _CanExecute; }
        public RelayCommand(Action<object?>? execute) { this.ExecuteObject = execute; }
        public RelayCommand(Action<object?>? execute, Predicate<object?> _CanExecute) { this.ExecuteObject = execute; this._canExecute = _CanExecute; }

    }
}
