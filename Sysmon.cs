using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    // класс для разбора json sysmon
    public class Sysmon
    {
        public Sysmon(string ho, string i, string o, string ve, int mem_t, int mem_u, List<Disk> di)
        {
            host = ho;
            ip = i;
            os = o;
            version = ve;
            mem_total = mem_t;
            mem_used = mem_u;
            disks = di;
        }
        public string host { get; set; }
        public string ip { get; set; }
        public string os { get; set; }
        public string version { get; set; }
        public int mem_total { get; set; }
        public int mem_used { get; set; }
        public List<Disk> disks { get; set; }
    }
}
