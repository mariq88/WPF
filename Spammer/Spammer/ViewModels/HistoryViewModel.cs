using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Spammer.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public List<FullHistoryData> MailsHistory { get; set; }

        public ICommand LogoutButton { get; set; }
        public ICommand SendNewEmailButton { get; set; }
        public ICommand ChartButton { get; set; }
        
        public event EventHandler<GoToLogin> NavigateToLogin;
        public event EventHandler<GoToNewEmail> NavigateToNewEmail;
        public event EventHandler<GoToChart> NavigateToChart;

        public HistoryViewModel()
        {
            this.LogoutButton = new RelayCommand(this.LogoutButtonHandler);
            this.SendNewEmailButton = new RelayCommand(this.SendNewEmailButtonHandler);
            this.ChartButton = new RelayCommand(this.ChartButtonHandler);
            this.MailsHistory = new List<FullHistoryData>();
        }

        private void LogoutButtonHandler(object obj)
        {
            try
            {
                DataPersister.Logout();
                this.NavigateToLogin(this, new GoToLogin());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendNewEmailButtonHandler(object obj)
        {
            this.NavigateToNewEmail(this, new GoToNewEmail());
        }

        private void ChartButtonHandler(object obj)
        {
            this.NavigateToChart(this, new GoToChart());
        }

        public void LoadHistory()
        {
            this.MailsHistory = DataPersister.GetFullHistory();
        }
    }
}
