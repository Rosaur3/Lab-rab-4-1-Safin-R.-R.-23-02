using System.Windows;
using Lab_rab_4_1_Safin_R._R._23_02.Model;

namespace Lab_rab_4_1_Safin_R._R._23_02.View
{
    public partial class WindowNewRole : Window
    {
        public WindowNewRole()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем через DataContext
            if (DataContext is Role role && string.IsNullOrWhiteSpace(role.NameRole))
            {
                MessageBox.Show("Введите название должности!", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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