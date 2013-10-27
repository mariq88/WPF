using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Spammer.ViewModels
{
    class NewEmailViewModel : BaseViewModel
    {
        private ICommand sendCommand;
        public event EventHandler<GoToHistory> NavigateToHistory;
        private ICommand cancelCommand;
        public string Emails { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public ICommand Cancel
        {
            get
            {
                if (this.cancelCommand == null)
                {
                    this.cancelCommand = new RelayCommand(this.HandleCancelCommand);
                }
                return this.cancelCommand;
            }
        }

        private void HandleCancelCommand(object obj)
        {
            this.NavigateToHistory(this, new GoToHistory());
        }
        public ICommand Send
        {
            get
            {
                if (this.sendCommand == null)
                {
                    this.sendCommand = new RelayCommand(this.HandleSendCommand);
                }
                return this.sendCommand;
            }
        }

        private void HandleSendCommand(object obj)
        {
            List<string> emails = this.Emails.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            SendEmailData newEmails = new SendEmailData()
            {
                Content = this.Content,
                Subject = this.Subject,
                Emails = emails
            };
            try
            {
                DataPersister.SendEmails(newEmails);
                MessageBox.Show("Message Sent!");

                this.Emails = string.Empty;
                this.Subject = string.Empty;
                this.Content = string.Empty;

                this.OnPropertyChanged("Emails");
                this.OnPropertyChanged("Subject");
                this.OnPropertyChanged("Content");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


    }
}
