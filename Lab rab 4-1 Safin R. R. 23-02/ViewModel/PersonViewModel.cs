using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Lab_rab_4_1_Safin_R._R._23_02.Helper;
using Lab_rab_4_1_Safin_R._R._23_02.Model;
using Lab_rab_4_1_Safin_R._R._23_02.View;
using Lab_rab_4_1_Safin_R._R._23_02.ViewModel;
using Newtonsoft.Json;

public class PersonViewModel : INotifyPropertyChanged
{
    // Реализация синглтона
    private static PersonViewModel _instance;
    public static PersonViewModel Instance => _instance ??= new PersonViewModel();

    // Укажите правильный путь к вашему файлу
    readonly string path = System.IO.Path.Combine(
        System.AppDomain.CurrentDomain.BaseDirectory,
        @"D:\games\education\USPTU\Visual Programming\Repos\6\Lab 1\Lab\Lab rab 4-1 Safin R. R. 23-02\DataModels\PersonData.json");

    private PersonDpo _selectedPersonDpo;
    private RoleViewModel _roleViewModel;
    private string _jsonPersons = string.Empty;

    public string Error { get; set; }
    public string Message { get; set; }

    public PersonDpo SelectedPersonDpo
    {
        get { return _selectedPersonDpo; }
        set
        {
            if (_selectedPersonDpo != value)
            {
                _selectedPersonDpo = value;
                OnPropertyChanged(nameof(SelectedPersonDpo));

                // Принудительно обновляем состояние команд
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    public ObservableCollection<Person> ListPerson { get; set; }
    public ObservableCollection<PersonDpo> ListPersonDpo { get; set; }

    // Приватный конструктор для синглтона
    private PersonViewModel()
    {
        _roleViewModel = new RoleViewModel();
        ListPerson = new ObservableCollection<Person>();
        ListPersonDpo = new ObservableCollection<PersonDpo>();

        ListPerson = LoadPerson() ?? new ObservableCollection<Person>();
        ListPersonDpo = GetListPersonDpo();
    }

    // Публичный конструктор для совместимости
    public PersonViewModel(bool useExisting = true)
    {
        if (useExisting)
        {
            // Используем синглтон
            var instance = Instance;
            _roleViewModel = instance._roleViewModel;
            ListPerson = instance.ListPerson;
            ListPersonDpo = instance.ListPersonDpo;
        }
        else
        {
            _roleViewModel = new RoleViewModel();
            ListPerson = new ObservableCollection<Person>();
            ListPersonDpo = new ObservableCollection<PersonDpo>();

            ListPerson = LoadPerson() ?? new ObservableCollection<Person>();
            ListPersonDpo = GetListPersonDpo();
        }
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
                        Birthday = DateTime.Now.ToString("dd.MM.yyyy")
                    };

                    wnPerson.DataContext = per;
                    wnPerson.LoadRoles(_roleViewModel.ListRole);

                    if (wnPerson.ShowDialog() == true)
                    {
                        if (wnPerson.CbRole.SelectedItem is Role selectedRole)
                        {
                            per.RoleName = selectedRole.NameRole;
                            per.Birthday = PersonDpo.GetStringBirthday(per.Birthday);

                            ListPersonDpo.Add(per);

                            var newPerson = new Person();
                            newPerson = newPerson.CopyFromPersonDpo(per);
                            ListPerson.Add(newPerson);

                            try
                            {
                                SaveChanges(ListPerson);
                            }
                            catch (Exception e)
                            {
                                Error = "Ошибка добавления данных в json файл\n" + e.Message;
                                MessageBox.Show(Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
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
                try
                {
                    if (SelectedPersonDpo == null)
                    {
                        MessageBox.Show("Выберите сотрудника для редактирования!",
                            "Предупреждение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

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
                            SelectedPersonDpo.RoleName = selectedRole.NameRole;
                            SelectedPersonDpo.FirstName = tempPerson.FirstName;
                            SelectedPersonDpo.LastName = tempPerson.LastName;
                            SelectedPersonDpo.Birthday = PersonDpo.GetStringBirthday(tempPerson.Birthday);

                            var personToUpdate = ListPerson.FirstOrDefault(p => p.Id == SelectedPersonDpo.Id);
                            if (personToUpdate != null)
                            {
                                personToUpdate.FirstName = SelectedPersonDpo.FirstName;
                                personToUpdate.LastName = SelectedPersonDpo.LastName;
                                personToUpdate.Birthday = SelectedPersonDpo.Birthday;
                                personToUpdate.RoleId = selectedRole.Id;
                            }

                            try
                            {
                                SaveChanges(ListPerson);
                                MessageBox.Show("Данные успешно обновлены!",
                                    "Информация",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }
                            catch (Exception e)
                            {
                                Error = "Ошибка редактирования данных в json файл\n" + e.Message;
                                MessageBox.Show(Error, "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            Message = "Необходимо выбрать должность сотрудника.";
                            MessageBox.Show(Message, "Предупреждение",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }, obj => ListPersonDpo != null && ListPersonDpo.Count > 0); // Убираем проверку на SelectedPersonDpo
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
                try
                {
                    // Добавляем дополнительную проверку
                    if (SelectedPersonDpo == null)
                    {
                        MessageBox.Show("Выберите сотрудника для удаления!",
                            "Предупреждение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

                    var result = MessageBox.Show(
                        $"Удалить данные по сотруднику: \n{SelectedPersonDpo.LastName} {SelectedPersonDpo.FirstName}?",
                        "Подтверждение удаления",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Сохраняем ссылку на удаляемого сотрудника
                            var personToDelete = SelectedPersonDpo;

                            // Удаляем из ListPersonDpo
                            ListPersonDpo.Remove(personToDelete);

                            // Находим и удаляем из ListPerson
                            var personToRemove = ListPerson.FirstOrDefault(p => p.Id == personToDelete.Id);
                            if (personToRemove != null)
                            {
                                ListPerson.Remove(personToRemove);
                            }

                            // Сохраняем изменения в JSON
                            SaveChanges(ListPerson);

                            // Сбрасываем выбранный элемент
                            SelectedPersonDpo = null;

                            MessageBox.Show("Сотрудник успешно удален!",
                                "Информация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            Error = "Ошибка удаления данных\n" + ex.Message;
                            MessageBox.Show(Error, "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }, obj => ListPersonDpo != null && ListPersonDpo.Count > 0); // Убираем проверку на SelectedPersonDpo
        }
    }
    #endregion

    #region Methods
    public ObservableCollection<Person> LoadPerson()
        {
            try
            {
                string fullPath = System.IO.Path.GetFullPath(path);
                if (File.Exists(fullPath))
                {
                    _jsonPersons = File.ReadAllText(fullPath);
                    if (!string.IsNullOrEmpty(_jsonPersons))
                    {
                        return JsonConvert.DeserializeObject<ObservableCollection<Person>>(_jsonPersons);
                    }
                }
                else
                {
                    Error = $"Файл не найден: {fullPath}";
                    // Создаем тестовые данные, если файл не найден
                    return CreateTestData();
                }
            }
            catch (Exception ex)
            {
                Error = $"Ошибка загрузки JSON: {ex.Message}";
            }
            return null;
        }

        private ObservableCollection<Person> CreateTestData()
        {
            var testData = new ObservableCollection<Person>
            {
                new Person { Id = 1, RoleId = 1, FirstName = "Иван", LastName = "Иванов", Birthday = "28.02.1980" },
                new Person { Id = 2, RoleId = 2, FirstName = "Петр", LastName = "Петров", Birthday = "20.03.1981" },
                new Person { Id = 3, RoleId = 3, FirstName = "Виктор", LastName = "Викторов", Birthday = "16.04.1982" },
                new Person { Id = 4, RoleId = 3, FirstName = "Сидор", LastName = "Сидоров", Birthday = "10.05.1983" }
            };

            SaveChanges(testData);
            return testData;
        }

        public ObservableCollection<PersonDpo> GetListPersonDpo()
        {
            var tempList = new ObservableCollection<PersonDpo>();
            foreach (var person in ListPerson)
            {
                var p = new PersonDpo().CopyFromPerson(person, _roleViewModel);
                tempList.Add(p);
            }
            return tempList;
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

        private void SaveChanges(ObservableCollection<Person> listPersons)
        {
            try
            {
                // Явно указываем Newtonsoft.Json.Formatting
                var jsonPerson = JsonConvert.SerializeObject(listPersons, Newtonsoft.Json.Formatting.Indented);
                string fullPath = System.IO.Path.GetFullPath(path);

                string directory = System.IO.Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (StreamWriter writer = File.CreateText(fullPath))
                {
                    writer.Write(jsonPerson);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла \n" + e.Message;
                MessageBox.Show(Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
}