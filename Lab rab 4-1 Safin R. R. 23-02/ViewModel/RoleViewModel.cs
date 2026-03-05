using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Lab_rab_4_1_Safin_R._R._23_02.Model;
using Lab_rab_4_1_Safin_R._R._23_02.View;
using Lab_rab_4_1_Safin_R._R._23_02.Helper;

namespace Lab_rab_4_1_Safin_R._R._23_02.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        // Реализация синглтона
        private static RoleViewModel _instance;
        public static RoleViewModel Instance => _instance ??= new RoleViewModel();

        private Role selectedRole;

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
            ListRole = new ObservableCollection<Role>();
            LoadTestData();
        }

        // Публичный конструктор для совместимости
        public RoleViewModel(bool createNew = false)
        {
            if (!createNew)
            {
                // Если вызывается не для создания нового, возвращаем синглтон
                var instance = Instance;
                ListRole = instance.ListRole;
            }
            else
            {
                ListRole = new ObservableCollection<Role>();
                LoadTestData();
            }
        }

        private void LoadTestData()
        {
            // Добавляем тестовые данные только если список пуст
            if (ListRole.Count == 0)
            {
                ListRole.Add(new Role
                {
                    Id = 1,
                    NameRole = "Директор"
                });
                ListRole.Add(new Role
                {
                    Id = 2,
                    NameRole = "Бухгалтер"
                });
                ListRole.Add(new Role
                {
                    Id = 3,
                    NameRole = "Менеджер"
                });
            }
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in ListRole)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                }
            }
            return max;
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
                        SelectedRole = role;
                    }
                });
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
                        OnPropertyChanged(nameof(ListRole));
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
                    }
                }, obj => SelectedRole != null && ListRole.Count > 0);
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