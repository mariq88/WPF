using System;
using System.Windows.Input;
using Spammer.Views;

namespace Spammer.ViewModels
{
    public class ChartShowSeries : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            (ChartViewModel.GetInstance()).MainChart = ChartColumnView.getInstance();
        }

        public event EventHandler CanExecuteChanged;
    }
}
