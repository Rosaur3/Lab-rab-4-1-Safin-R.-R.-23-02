using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lab_rab_4_1_Safin_R._R._23_02.Helper;
using Lab_rab_4_1_Safin_R._R._23_02.Model;
using Lab_rab_4_1_Safin_R._R._23_02.ViewModel;

namespace Lab_rab_4_1_Safin_R._R._23_02.View
{
    public partial class WindowEmployee : Window
    {
        public WindowEmployee()
        {
            InitializeComponent();
            DataContext = PersonViewModel.Instance;
        }

        private void WindowEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as PersonViewModel;
            if (vm != null)
            {
                System.Diagnostics.Debug.WriteLine($"Loaded: ListPersonDpo count = {vm.ListPersonDpo.Count}");
                System.Diagnostics.Debug.WriteLine($"Loaded: SelectedPersonDpo = {vm.SelectedPersonDpo?.LastName ?? "null"}");
            }
        }
    }
}