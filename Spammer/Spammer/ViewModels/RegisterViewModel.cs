using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Spammer.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public event EventHandler<GoToLogin> NavigateToLogin;
        public event EventHandler<GoToHistory> NavigateToHistory;

        public string Username { get; set; }
        public string Email { get; set; }
        public ICommand RegisterButton { get; set; }
        public ICommand LoginButton { get; set; }

        public RegisterViewModel()
        {
            this.RegisterButton = new RelayCommand(this.RegisterButtonCommand);
            this.LoginButton = new RelayCommand(this.LoginButtonCommand);
        }

        private void RegisterButtonCommand(object passwordBox)
        {
            var passBox = (passwordBox as PasswordBox);
            var password = passBox.Password.ToString();

            Validator.ValidateUsername(this.Username);
            Validator.ValidatePassword(password);
            Validator.ValidateEmail(this.Email);

            try
            {
                DataPersister.RegisterUser(this.Username, password, this.Email);
                this.NavigateToHistory(this, new GoToHistory());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Username = string.Empty;
            this.Email = string.Empty;
            this.OnPropertyChanged("Username");
            this.OnPropertyChanged("Email");
            passBox.Password = string.Empty;
        }

        private void LoginButtonCommand(object obj)
        {
            this.NavigateToLogin(this, new GoToLogin());
        }
    }
}
