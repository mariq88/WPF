using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Spammer.ViewModels
{
    public class ChartViewModel : BaseViewModel
    {
        private static ChartViewModel chartViewModel;
        private string currentChartType;
        private ContentControl mainChart;
        private ChartModel chartModel;

        public event EventHandler<GoToHistory> NavigateToHistory;
        public ICommand BackToHistoryButton { get; set; }
        public ICommand SelectNewValueButton { get; set; }

        public static ChartViewModel GetInstance()
        {
            return chartViewModel;
        }

        public ChartViewModel()
        {
            chartViewModel = this;
            chartModel = new ChartModel();
            OnPropertyChanged("data");
            CurrentChartType = "ColumnSeries";
            this.BackToHistoryButton = new RelayCommand(this.BackToHistoryButtonCommand);
            this.SelectNewValueButton = new RelayCommand(this.SelectNewValueButtonCommand);
        }

        public ObservableCollection<KeyValuePair<string, int>> Data
        {
            get
            {
                return chartModel;
            }
        }

        public ContentControl MainChart
        {
            get
            {
                return mainChart;
            }
            set
            {
                mainChart = value;
                OnPropertyChanged("CurrentChartType");
            }
        }

        public string CurrentChartType
        {
            get
            {
                return currentChartType;
            }
            set
            {
                currentChartType = value;
                OnPropertyChanged("CurrentChartType");
            }
        }

        public void LoadChartHistory(int daysCount = 15)
        {
            this.chartModel.Init(daysCount);
        }

        private void BackToHistoryButtonCommand(object obj)
        {
            this.NavigateToHistory(this, new GoToHistory());
        }

        private void SelectNewValueButtonCommand(object obj)
        {
            var textBox = obj as TextBox;
            int daysCount = int.Parse(textBox.Text);
            if (daysCount > 50)
            {
                daysCount = 50;
            }
            this.LoadChartHistory(daysCount);
        }
    }
}
