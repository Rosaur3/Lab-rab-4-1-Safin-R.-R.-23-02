using System;
using System.Collections.ObjectModel;
using System.Windows;
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

        private void tbBirthday_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbBirthday.Visibility == Visibility.Hidden)
            {
                ClBirthday.Visibility = Visibility.Visible;

                // Пытаемся распарсить дату из текстового поля
                if (DateTime.TryParse(tbBirthday.Text, out DateTime date))
                {
                    ClBirthday.SelectedDate = date;
                }
            }
            else
            {
                ClBirthday.Visibility = Visibility.Hidden;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем заполнение полей
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

            // Обновляем дату рождения из календаря, если он видим
            if (ClBirthday.Visibility == Visibility.Visible && ClBirthday.SelectedDate.HasValue)
            {
                tbBirthday.Text = ClBirthday.SelectedDate.Value.ToString("dd.MM.yyyy");
            }

            if (string.IsNullOrWhiteSpace(tbBirthday.Text))
            {
                MessageBox.Show("Введите дату рождения!", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                tbBirthday.Focus();
                return;
            }

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