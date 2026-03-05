using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab_rab_4_1_Safin_R._R._23_02.Model
{
    public class Role : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string nameRole;
        public string NameRole
        {
            get { return nameRole; }
            set
            {
                nameRole = value;
                OnPropertyChanged(nameof(NameRole));
            }
        }

        public Role() { }

        public Role(int id, string nameRole)
        {
            this.Id = id;
            this.NameRole = nameRole;
        }

        public Role ShallowCopy()
        {
            return (Role)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}