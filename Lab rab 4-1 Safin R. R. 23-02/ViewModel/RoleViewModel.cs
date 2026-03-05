using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Lab_rab_4_1_Safin_R._R._23_02.Model;
using Lab_rab_4_1_Safin_R._R._23_02.View;
using Lab_rab_4_1_Safin_R._R._23_02.Helper;
using Newtonsoft.Json;

namespace Lab_rab_4_1_Safin_R._R._23_02.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        // Реализация синглтона
        private static RoleViewModel _instance;
        public static RoleViewModel Instance => _instance ??= new RoleViewModel();

        // Укажите правильный путь к вашему файлу
        readonly string path = System.IO.Path.Combine(
            System.AppDomain.CurrentDomain.BaseDirectory,
            @"D:\games\education\USPTU\Visual Programming\Repos\6\Lab 1\Lab\Lab rab 4-1 Safin R. R. 23-02\DataModels\RoleData.json");

        private Role selectedRole;
        private string _jsonRoles = string.Empty;

        public string Error { get; set; }

        public Role SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        public ObservableCollection<Role> ListRole { get; set; }

        // Приватный конструктор для синглтона
        private RoleViewModel()
        {
            ListRole = LoadRole() ?? new ObservableCollection<Role>();
        }

        // Публичный конструктор для совместимости
        public RoleViewModel(bool useExisting = true)
        {
            if (useExisting)
            {
                // Используем синглтон
                var instance = Instance;
                ListRole = instance.ListRole;
            }
            else
            {
                ListRole = LoadRole() ?? new ObservableCollection<Role>();
            }
        }

        #region AddRole
        private RelayCommand addRole;
        public RelayCommand AddRole
        {
            get
            {
                return addRole ??= new RelayCommand(obj =>
                {
                    var wnRole = new WindowNewRole
                    {
                        Title = "Новая должность",
                        Owner = Application.Current.MainWindow
                    };

                    int maxIdRole = MaxId() + 1;
                    var role = new Role { Id = maxIdRole };
                    wnRole.DataContext = role;

                    if (wnRole.ShowDialog() == true)
                    {
                        ListRole.Add(role);
                        SaveChanges(ListRole);
                    }
                    SelectedRole = role;
                }, obj => true);
            }
        }
        #endregion

        #region EditRole
        private RelayCommand editRole;
        public RelayCommand EditRole
        {
            get
            {
                return editRole ??= new RelayCommand(obj =>
                {
                    if (SelectedRole == null) return;

                    var wnRole = new WindowNewRole
                    {
                        Title = "Редактирование должности",
                        Owner = Application.Current.MainWindow
                    };

                    var tempRole = SelectedRole.ShallowCopy();
                    wnRole.DataContext = tempRole;

                    if (wnRole.ShowDialog() == true)
                    {
                        SelectedRole.NameRole = tempRole.NameRole;
                        SaveChanges(ListRole);
                    }
                }, obj => SelectedRole != null && ListRole.Count > 0);
            }
        }
        #endregion

        #region DeleteRole
        private RelayCommand deleteRole;
        public RelayCommand DeleteRole
        {
            get
            {
                return deleteRole ??= new RelayCommand(obj =>
                {
                    if (SelectedRole == null) return;

                    var result = MessageBox.Show(
                        $"Удалить данные по должности: {SelectedRole.NameRole}",
                        "Предупреждение",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.OK)
                    {
                        ListRole.Remove(SelectedRole);
                        SaveChanges(ListRole);
                    }
                }, obj => SelectedRole != null && ListRole.Count > 0);
            }
        }
        #endregion

        #region Methods
        public ObservableCollection<Role> LoadRole()
        {
            try
            {
                string fullPath = System.IO.Path.GetFullPath(path);
                if (File.Exists(fullPath))
                {
                    _jsonRoles = File.ReadAllText(fullPath);
                    if (!string.IsNullOrEmpty(_jsonRoles))
                    {
                        return JsonConvert.DeserializeObject<ObservableCollection<Role>>(_jsonRoles);
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

        private ObservableCollection<Role> CreateTestData()
        {
            var testData = new ObservableCollection<Role>
            {
                new Role { Id = 1, NameRole = "Директор" },
                new Role { Id = 2, NameRole = "Бухгалтер" },
                new Role { Id = 3, NameRole = "Менеджер" }
            };

            // Сохраняем тестовые данные в файл
            SaveChanges(testData);
            return testData;
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in ListRole)
            {
                if (max < r.Id)
                    max = r.Id;
            }
            return max;
        }

        private void SaveChanges(ObservableCollection<Role> listRole)
        {
            try
            {
                // Явно указываем Newtonsoft.Json.Formatting
                var jsonRole = JsonConvert.SerializeObject(listRole, Newtonsoft.Json.Formatting.Indented);
                string fullPath = System.IO.Path.GetFullPath(path);

                string directory = System.IO.Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (StreamWriter writer = File.CreateText(fullPath))
                {
                    writer.Write(jsonRole);
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
}