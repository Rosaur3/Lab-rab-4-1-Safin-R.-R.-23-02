using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Lab_rab_4_1_Safin_R._R._23_02.Model;
using Lab_rab_4_1_Safin_R._R._23_02.View;
using Lab_rab_4_1_Safin_R._R._23_02.Helper;

namespace Lab_rab_4_1_Safin_R._R._23_02.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        // Реализация синглтона
        private static PersonViewModel _instance;
        public static PersonViewModel Instance => _instance ??= new PersonViewModel();

        private PersonDpo selectedPersonDpo;
        private RoleViewModel _roleViewModel;

        public PersonDpo SelectedPersonDpo
        {
            get { return selectedPersonDpo; }
            set
            {
                selectedPersonDpo = value;
                OnPropertyChanged(nameof(SelectedPersonDpo));
            }
        }

        public ObservableCollection<Person> ListPerson { get; set; }
        public ObservableCollection<PersonDpo> ListPersonDpo { get; set; }

        // Приватный конструктор для синглтона
        private PersonViewModel()
        {
            ListPerson = new ObservableCollection<Person>();
            ListPersonDpo = new ObservableCollection<PersonDpo>();
            _roleViewModel = RoleViewModel.Instance;

            LoadTestData();
        }

        // Публичный конструктор для совместимости (создает новый экземпляр)
        public PersonViewModel(bool createNew = false)
        {
            if (!createNew)
            {
                // Если вызывается не для создания нового, возвращаем синглтон
                var instance = Instance;
                ListPerson = instance.ListPerson;
                ListPersonDpo = instance.ListPersonDpo;
                _roleViewModel = RoleViewModel.Instance;
            }
            else
            {
                ListPerson = new ObservableCollection<Person>();
                ListPersonDpo = new ObservableCollection<PersonDpo>();
                _roleViewModel = RoleViewModel.Instance;
                LoadTestData();
            }
        }

        private void LoadTestData()
        {
            // Добавляем тестовые данные только если список пуст
            if (ListPerson.Count == 0)
            {
                ListPerson.Add(new Person
                {
                    Id = 1,
                    RoleId = 1,
                    FirstName = "Иван",
                    LastName = "Иванов",
                    Birthday = new DateTime(1980, 2, 28)
                });
                ListPerson.Add(new Person
                {
                    Id = 2,
                    RoleId = 2,
                    FirstName = "Петр",
                    LastName = "Петров",
                    Birthday = new DateTime(1981, 3, 20)
                });
                ListPerson.Add(new Person
                {
                    Id = 3,
                    RoleId = 3,
                    FirstName = "Виктор",
                    LastName = "Викторов",
                    Birthday = new DateTime(1982, 4, 15)
                });
                ListPerson.Add(new Person
                {
                    Id = 4,
                    RoleId = 3,
                    FirstName = "Сидор",
                    LastName = "Сидоров",
                    Birthday = new DateTime(1983, 5, 10)
                });
            }

            // Обновляем PersonDpo список
            RefreshPersonDpoList();
        }

        private void RefreshPersonDpoList()
        {
            ListPersonDpo.Clear();
            foreach (var person in ListPerson)
            {
                var personDpo = new PersonDpo().CopyFromPerson(person, _roleViewModel);
                ListPersonDpo.Add(personDpo);
            }
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var p in ListPerson)
            {
                if (max < p.Id)
                    max = p.Id;
            }
            return max;
        }

        #region AddPerson
        private RelayCommand addPerson;
        public RelayCommand AddPerson
        {
            get
            {
                return addPerson ??= new RelayCommand(obj =>
                {
                    var wnPerson = new WindowNewEmployee
                    {
                        Title = "Новый сотрудник",
                        Owner = Application.Current.MainWindow
                    };

                    int maxIdPerson = MaxId() + 1;
                    var per = new PersonDpo
                    {
                        Id = maxIdPerson,
                        Birthday = DateTime.Now
                    };

                    wnPerson.DataContext = per;
                    wnPerson.LoadRoles(_roleViewModel.ListRole);

                    if (wnPerson.ShowDialog() == true)
                    {
                        if (wnPerson.CbRole.SelectedItem is Role selectedRole)
                        {
                            // Добавляем в Person
                            var newPerson = new Person
                            {
                                Id = per.Id,
                                FirstName = per.FirstName,
                                LastName = per.LastName,
                                Birthday = per.Birthday,
                                RoleId = selectedRole.Id
                            };
                            ListPerson.Add(newPerson);

                            // Обновляем PersonDpo для отображения
                            var newPersonDpo = new PersonDpo().CopyFromPerson(newPerson, _roleViewModel);
                            ListPersonDpo.Add(newPersonDpo);
                        }
                    }
                }, obj => true);
            }
        }
        #endregion

        #region EditPerson
        private RelayCommand editPerson;
        public RelayCommand EditPerson
        {
            get
            {
                return editPerson ??= new RelayCommand(obj =>
                {
                    if (SelectedPersonDpo == null) return;

                    var wnPerson = new WindowNewEmployee
                    {
                        Title = "Редактирование данных сотрудника",
                        Owner = Application.Current.MainWindow
                    };

                    var tempPerson = SelectedPersonDpo.ShallowCopy();
                    wnPerson.DataContext = tempPerson;
                    wnPerson.LoadRoles(_roleViewModel.ListRole);

                    // Устанавливаем выбранную роль
                    foreach (Role role in _roleViewModel.ListRole)
                    {
                        if (role.NameRole == SelectedPersonDpo.RoleName)
                        {
                            wnPerson.CbRole.SelectedItem = role;
                            break;
                        }
                    }

                    if (wnPerson.ShowDialog() == true)
                    {
                        if (wnPerson.CbRole.SelectedItem is Role selectedRole)
                        {
                            // Обновляем Person
                            var personToUpdate = ListPerson.FirstOrDefault(p => p.Id == SelectedPersonDpo.Id);
                            if (personToUpdate != null)
                            {
                                personToUpdate.FirstName = tempPerson.FirstName;
                                personToUpdate.LastName = tempPerson.LastName;
                                personToUpdate.Birthday = tempPerson.Birthday;
                                personToUpdate.RoleId = selectedRole.Id;
                            }

                            // Обновляем PersonDpo
                            SelectedPersonDpo.FirstName = tempPerson.FirstName;
                            SelectedPersonDpo.LastName = tempPerson.LastName;
                            SelectedPersonDpo.Birthday = tempPerson.Birthday;
                            SelectedPersonDpo.RoleName = selectedRole.NameRole;

                            // Обновляем отображение
                            OnPropertyChanged(nameof(ListPersonDpo));
                        }
                    }
                }, obj => SelectedPersonDpo != null && ListPersonDpo.Count > 0);
            }
        }
        #endregion

        #region DeletePerson
        private RelayCommand deletePerson;
        public RelayCommand DeletePerson
        {
            get
            {
                return deletePerson ??= new RelayCommand(obj =>
                {
                    if (SelectedPersonDpo == null) return;

                    var result = MessageBox.Show(
                        $"Удалить данные по сотруднику: \n{SelectedPersonDpo.LastName} {SelectedPersonDpo.FirstName}",
                        "Предупреждение",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.OK)
                    {
                        // Удаляем из Person
                        var personToRemove = ListPerson.FirstOrDefault(p => p.Id == SelectedPersonDpo.Id);
                        if (personToRemove != null)
                        {
                            ListPerson.Remove(personToRemove);
                        }

                        // Удаляем из PersonDpo
                        ListPersonDpo.Remove(SelectedPersonDpo);
                    }
                }, obj => SelectedPersonDpo != null && ListPersonDpo.Count > 0);
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}