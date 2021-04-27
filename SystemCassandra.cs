using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    public class SystemCassandra : INotifyPropertyChanged
    {
        public SystemCassandra() { }
        public SystemCassandra(int id, string name_server, string ip_server, string all_memory, string used_memory, 
            string cpu, string all_disk, string used_disk, string name_db, string ip_db, string status_db)
        {
            Id = id;
            NameServer = name_server;
            IpServer = ip_server;
            AllMemory = all_memory;
            UsedMemory = used_memory;
            Cpu = cpu;
            AllDisk = all_disk;
            UsedDisk = used_disk;
            NameDB = name_db;
            IpDB = ip_db;
            StatusDB = status_db;
        }


        private int id;
        private string nameServer;
        private string ipServer;
        private string allMemory;
        private string usedMemory;
        private string cpu;
        private string allDisk;
        private string usedDisk;
        private string nameDB;
        private string ipDB;
        private string statusDB;

        public int Id { get { return id; } set { id = value; OnPropertyChanged("Id"); }}
        public string NameServer { get => nameServer; set { nameServer = value; OnPropertyChanged("NameServer"); } }
        public string IpServer { get => ipServer; set { ipServer = value; OnPropertyChanged("IpServer"); } }
        public string AllMemory { get => allMemory; set { allMemory = value; OnPropertyChanged("AllMemory"); } }
        public string UsedMemory { get => usedMemory; set { usedMemory = value; OnPropertyChanged("UsedMemory"); } }
        public string Cpu { get => cpu; set { cpu = value; OnPropertyChanged("Cpu"); } }
        public string AllDisk { get => allDisk; set { allDisk = value; OnPropertyChanged("AllDisk"); } }
        public string UsedDisk { get => usedDisk; set { usedDisk = value; OnPropertyChanged("UsedDisk"); } }
        public string NameDB { get => nameDB; set { nameDB = value; OnPropertyChanged("NameDB"); } }
        public string IpDB { get => ipDB; set { ipDB = value; OnPropertyChanged("IpDB"); } }
        public string StatusDB { get => statusDB; set { statusDB = value; OnPropertyChanged("StatusDB"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
