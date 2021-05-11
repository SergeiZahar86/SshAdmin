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
            PieceDisk = PieceDisk;
        }
        private string piecedisk;
        public string name { get; set; }
        public string mount_point { get; set; }
        public string total { get; set; }
        public string used { get; set; }
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
                double.TryParse(total, out md);
                double.TryParse(used, out ud);
                pd = Math.Round((ud / (md / 100)), 1);
                piecedisk = pd.ToString();
            }
        }
    }
}
