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
                OnPropertyChanged(nameof(RoleName));
            }
        }

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

        private string birthday;
        public string Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
        }

        public PersonDpo() { }

        public PersonDpo(int id, string roleName, string firstName, string lastName, string birthday)
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

        public static string GetStringBirthday(string birthday)
        {
            if (DateTime.TryParse(birthday, out DateTime date))
            {
                return date.ToString("dd.MM.yyyy");
            }
            return birthday;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}