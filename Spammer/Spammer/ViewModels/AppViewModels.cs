using System;

namespace Spammer.ViewModels
{
    public class AppViewModels : BaseViewModel
    {
        private LoginViewModel loginVM;
        private RegisterViewModel registerVm;
        private HistoryViewModel historyVM;
        private NewEmailViewModel newEmailVm;
        private ChartViewModel chartVm;

        public BaseViewModel CurrentViewModel { get; set; }

        public AppViewModels()
        {
            this.loginVM = new LoginViewModel();
            this.loginVM.NavigateToRegister += this.NavigateToRegister;
            this.loginVM.NavigateToHistory += this.NavigateToHistory;

            this.registerVm = new RegisterViewModel();
            this.registerVm.NavigateToLogin += this.NavigateToLogin;
            this.registerVm.NavigateToHistory += this.NavigateToHistory;

            this.newEmailVm = new NewEmailViewModel();
            this.newEmailVm.NavigateToHistory += this.NavigateToHistory;

            this.historyVM = new HistoryViewModel();
            this.historyVM.NavigateToLogin += this.NavigateToLogin;
            this.historyVM.NavigateToNewEmail += this.NavigateToNewEmail;
            this.historyVM.NavigateToChart += this.NavigateToChart;

            this.chartVm = new ChartViewModel();
            this.chartVm.NavigateToHistory += this.NavigateToHistory;

            this.CurrentViewModel = this.loginVM;
        }

        public void NavigateToRegister(object sender, GoToRegister e)
        {
            this.CurrentViewModel = this.registerVm;
            this.OnPropertyChanged("CurrentViewModel");
        }

        public void NavigateToLogin(object sender, GoToLogin e)
        {
            this.CurrentViewModel = this.loginVM;
            this.OnPropertyChanged("CurrentViewModel");
        }

        public void NavigateToNewEmail(object sender, GoToNewEmail e)
        {
            this.CurrentViewModel = this.newEmailVm;
            this.OnPropertyChanged("CurrentViewModel");
        }

        public void NavigateToHistory(object sender, GoToHistory e)
        {
            this.historyVM.LoadHistory();
            this.CurrentViewModel = this.historyVM;
            this.OnPropertyChanged("CurrentViewModel");
        }

        public void NavigateToChart(object sender, GoToChart e)
        {
            this.chartVm.LoadChartHistory();
            this.CurrentViewModel = this.chartVm;
            this.OnPropertyChanged("CurrentViewModel");
        }
    }
}
