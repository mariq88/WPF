using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace Spammer.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public event EventHandler<GoToRegister> NavigateToRegister;
        public event EventHandler<GoToHistory> NavigateToHistory;

        public string Username { get; set; }
        public ICommand LoginButton { get; set; }
        public ICommand RegisterButton { get; set; }

        public LoginViewModel()
        {
            this.LoginButton = new RelayCommand(this.LoginButtonCommand);
            this.RegisterButton = new RelayCommand(this.RegisterButtonCommand);
        }

        private void LoginButtonCommand(object passwordBox)
        {
            var passBox = (passwordBox as PasswordBox);
            var password = passBox.Password.ToString();

            Validator.ValidateUsername(this.Username);
            Validator.ValidatePassword(password);

            try
            {
                DataPersister.LoginUser(this.Username, password);
                this.NavigateToHistory(this, new GoToHistory());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Username = string.Empty;
            this.OnPropertyChanged("Username");

            passBox.Password = string.Empty;
        }

        public void RegisterButtonCommand(object obj)
        {
            this.NavigateToRegister(this, new GoToRegister());
        }
    }
}
