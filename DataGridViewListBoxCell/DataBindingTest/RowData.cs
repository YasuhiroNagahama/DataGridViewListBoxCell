using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CustomDataGridViewControls.Test2
{
    internal class RowData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string userName;
        private string age;
        private string description;
        private string dispData;

        public RowData()
        {
            this.userName = "";
            this.age = "";
            this.description = "";
            this.dispData = "";
        }

        public string UserName
        {
            get => this.userName;
            set
            {
                Debug.WriteLine(value);
                this.userName = value;

                this.NotifyPropertyChanged();
            }
        }

        public string Age
        {
            get => this.age;
            set
            {
                Debug.WriteLine(value);
                this.age = value;

                this.NotifyPropertyChanged();
            }
        }

        public string Description
        {
            get => this.description;
            set
            {
                Debug.WriteLine(value);

                this.description = value;

                this.NotifyPropertyChanged();
            }
        }

        public string DispData
        {
            get => this.dispData;
            set
            {
                Debug.WriteLine(value);
                this.dispData = value;
                this.NotifyPropertyChanged();
            }
        }
    }
}
