using System.Windows;

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
            // Проверка заполнения поля
            if (string.IsNullOrWhiteSpace(NameRoleTextBox?.Text))
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