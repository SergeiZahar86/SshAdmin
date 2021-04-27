using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    public class AllDisk
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Disk { get; set; }
        public AllDisk(int id, string ip, string disk)
        {
            Id = id;
            Ip = Ip;
            Disk = disk;
        }
    }
}
