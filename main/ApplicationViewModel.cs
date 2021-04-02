using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin.main
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private User selectedUser;
        public ObservableCollection<User> Users { get; set; }

        // команда добавления нового объекта
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      User user = new User();
                      Users.Insert(0, user);
                      SelectedPhone = user;
                  }));
            }
        }

        public User SelectedPhone
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        public ApplicationViewModel()
        {
            Users = new ObservableCollection<User>
            {
                new User { Id=3, Name="Apple", Pass = "fff" },
                new User {Id=4, Name="jjjjjj", Pass = "678" },
                new User {Id=5, Name="kkkkk", Pass = "jhk" },
                new User {Id=6, Name="llllll", Pass = "44444"}
            };
        }


















        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
