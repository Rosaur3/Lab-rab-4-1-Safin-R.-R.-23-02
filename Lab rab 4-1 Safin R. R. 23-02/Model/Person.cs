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

        public Person() { }

        public Person(int id, int roleId, string firstName, string lastName, DateTime birthday)
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

        public Person CopyFromPersonDPO(PersonDpo personDpo)
        {
            Person person = new Person();
            RoleViewModel vmRole = new RoleViewModel();

            foreach (var r in vmRole.ListRole)
            {
                if (r.NameRole == personDpo.RoleName)
                {
                    person.RoleId = r.Id;
                    break;
                }
            }

            person.Id = personDpo.Id;
            person.FirstName = personDpo.FirstName;
            person.LastName = personDpo.LastName;
            person.Birthday = personDpo.Birthday;

            return person;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}