using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Lab_rab_4_1_Safin_R._R._23_02.Model;

namespace Lab_rab_4_1_Safin_R._R._23_02.View
{
    public partial class WindowNewEmployee : Window
    {
        public WindowNewEmployee()
        {
            InitializeComponent();
        }

        public void LoadRoles(ObservableCollection<Role> roles)
        {
            CbRole.ItemsSource = roles;
            CbRole.DisplayMemberPath = "NameRole";
            CbRole.SelectedValuePath = "Id";

            if (roles != null && roles.Count > 0)
            {
                CbRole.SelectedIndex = 0;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем заполнение полей через прямые ссылки на элементы
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Введите фамилию!", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLastName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Введите имя!", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFirstName.Focus();
                return;
            }

            if (CbRole.SelectedItem == null)
            {
                MessageBox.Show("Выберите должность!", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                CbRole.Focus();
                return;
            }

            if (dpBirthday.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату рождения!", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                dpBirthday.Focus();
                return;
            }

            // Если все поля заполнены, сохраняем
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}