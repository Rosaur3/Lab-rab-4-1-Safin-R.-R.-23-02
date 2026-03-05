using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lab_rab_4_1_Safin_R._R._23_02.ViewModel;

namespace Lab_rab_4_1_Safin_R._R._23_02.Model
{
    public class PersonDpo : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string roleName;
        public string RoleName
        {
            get { return roleName; }
            set
            {
                roleName = value;
                OnPropertyChanged("RoleName");
            }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private DateTime birthday;
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public PersonDpo() { }

        public PersonDpo(int id, string roleName, string firstName, string lastName, DateTime birthday)
        {
            this.Id = id;
            this.RoleName = roleName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }

        public PersonDpo ShallowCopy()
        {
            return (PersonDpo)this.MemberwiseClone();
        }

        public PersonDpo CopyFromPerson(Person person, RoleViewModel vmRole)
        {
            var perDpo = new PersonDpo();

            foreach (var r in vmRole.ListRole)
            {
                if (r.Id == person.RoleId)
                {
                    perDpo.Id = person.Id;
                    perDpo.RoleName = r.NameRole;
                    perDpo.FirstName = person.FirstName;
                    perDpo.LastName = person.LastName;
                    perDpo.Birthday = person.Birthday;
                    break;
                }
            }

            return perDpo;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}