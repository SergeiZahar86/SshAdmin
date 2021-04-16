using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin.main
{
    public class Host
    {
        public string Ip { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public Host(string ip, string login, string pass)
        {
            Ip = ip;
            Login = login;
            Pass = pass;
        }
    }
}
