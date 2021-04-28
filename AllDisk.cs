using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    public class AllDisk
    {
        private string piecedisk;
        public string Ip { get; set; }
        public string Name { get; set; }
        public string MemoryDisk { get; set; }
        public string UsedDisk { get; set; }
        public string PieceDisk
        {
            get
            {
                return piecedisk;
            }
            set
            {
                double md;
                double ud;
                double pd;
                double.TryParse(MemoryDisk, out md);
                double.TryParse(UsedDisk, out ud);
                pd = Math.Round(( ud / (md / 100)),1);
                piecedisk = pd.ToString();
            }
        }
        public AllDisk(string ip, string name, string memory_disk, string used_disk)
        {
            Ip = ip;
            Name = name;
            MemoryDisk = memory_disk;
            UsedDisk = used_disk;
            PieceDisk = PieceDisk;
        }
    }
}
