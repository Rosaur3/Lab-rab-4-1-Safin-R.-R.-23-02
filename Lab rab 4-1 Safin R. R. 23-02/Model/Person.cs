using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lab_rab_4_1_Safin_R._R._23_02.ViewModel;

namespace Lab_rab_4_1_Safin_R._R._23_02.Model
{
    public class Person : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string birthday; // Изменено на string для JSON
        public string Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
        }

        public Person() { }

        public Person(int id, int roleId, string firstName, string lastName, string birthday)
        {
            this.Id = id;
            this.RoleId = roleId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }

        public Person ShallowCopy()
        {
            return (Person)this.MemberwiseClone();
        }

        public Person CopyFromPersonDpo(PersonDpo personDpo)
        {
            this.Id = personDpo.Id;
            this.FirstName = personDpo.FirstName;
            this.LastName = personDpo.LastName;
            this.Birthday = personDpo.Birthday;

            // Находим RoleId по названию должности
            var vmRole = new ViewModel.RoleViewModel();
            foreach (var role in vmRole.ListRole)
            {
                if (role.NameRole == personDpo.RoleName)
                {
                    this.RoleId = role.Id;
                    break;
                }
            }

            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}