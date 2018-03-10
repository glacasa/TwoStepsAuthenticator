using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace TwoStepsAuthenticatorTestApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                if (key != value)
                {
                    key = value;
                    RaisePropertyChanged("Key");
                }
            }
        }

        private string code;
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                if (code != value)
                {
                    code = value;
                    RaisePropertyChanged("Code");
                    GetCode();
                }
            }
        }

        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if (count != value)
                {
                    count = value;
                    if (count < 0)
                    {
                        count = 30;
                        GetCode();
                    }
                    RaisePropertyChanged("Count");
                }
            }
        }

        DispatcherTimer timer;

        public ViewModel()
        {
            var authenticator = new TwoStepsAuthenticator.TimeAuthenticator();
            this.Key = TwoStepsAuthenticator.Authenticator.GenerateKey();
            timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, timerCallback, App.Current.Dispatcher);
            timer.Start();
        }

        private void timerCallback(object sender, EventArgs e)
        {
            Count--;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void GetCode()
        {
            var auth = new TwoStepsAuthenticator.TimeAuthenticator();
            Code = auth.GetCode(this.Key);

            auth.CheckCode(key, Code, "user");
        }
    }
}
