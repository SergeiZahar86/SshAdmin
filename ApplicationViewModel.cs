using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private User selectedUser;
        private static ApplicationViewModel instance;
        private Global global;

        public static ApplicationViewModel getInstance()
        {
            if (instance == null)
            {
                instance = new ApplicationViewModel();
                return instance;
            }
            else
            {
                return instance;
            }
        }
        public ObservableCollection<User> Users { get; set; }


        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public ApplicationViewModel()
        {
            global = Global.getInstance();
            Users = new ObservableCollection<User>();
            foreach(User user in global.UsersGlobal)
            {
                Users.Add(user);
            }

        }








        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
