using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IncubeAdmin
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private User selectedUser;
        private SystemCassandra selectedSystemCassandra;
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
        public ObservableCollection<SystemCassandra> Cassandras { get; set; }

        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }
        public SystemCassandra SelectedSystemCassandra
        {
            get { return selectedSystemCassandra; }
            set
            {
                selectedSystemCassandra = value;
                OnPropertyChanged("SelectedSystemCassandra");
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
            /*Cassandras = new ObservableCollection<SystemCassandra>();
            foreach(SystemCassandra systemCassandra in global.systemCassandras)
            {
                Cassandras.Add(systemCassandra);
            }*/

        }








        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
