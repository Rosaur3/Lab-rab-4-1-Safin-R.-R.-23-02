using Lab_rab_4_1_Safin_R._R._23_02.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_rab_4_1_Safin_R._R._23_02
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Employee_OnClick(object sender, RoutedEventArgs e)
        {
            var w = new WindowEmployee();
            w.Show();
        }

        private void Role_OnClick(object sender, RoutedEventArgs e)
        {
            var w = new WindowRole();
            w.Show();
        }
    }
}
