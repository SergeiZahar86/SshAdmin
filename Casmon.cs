using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    public class Casmon
    {
        public Casmon(string n, string c)
        {
            node = n;
            check = c;
        }
        public string node { get; set; }
        public string check { get; set; }
    }
}
