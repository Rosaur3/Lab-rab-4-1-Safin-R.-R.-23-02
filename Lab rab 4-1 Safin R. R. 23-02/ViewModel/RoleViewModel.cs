using Lab_rab_4_1_Safin_R._R._23_02.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_rab_4_1_Safin_R._R._23_02.ViewModel
{
    public class RoleViewModel
    {
        public ObservableCollection<Role> ListRole { get; set; } = new ObservableCollection<Role>();
        public RoleViewModel()
        {
            this.ListRole.Add(
            new Role
            {
                Id = 1,
                NameRole = "Директор"
            }
            );
            this.ListRole.Add(
            new Role
            {
                Id = 2,
                NameRole = "Бухгалтер"
            }
            );
            this.ListRole.Add(new Role
            {
                Id = 3,
                NameRole = "Менеджер"
            });
        }
    }
}