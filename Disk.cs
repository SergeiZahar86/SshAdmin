using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    // класс для разбора json sysmon
    public class Disk
    {
        public Disk(string n, string m, string t, string u)
        {
            name = n;
            mount_point = m;
            total = t;
            used = u;
        }
        public string name { get; set; }
        public string mount_point { get; set; }
        public object total { get; set; }
        public object used { get; set; }
    }
}
