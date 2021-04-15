using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin.main
{
    class Host
    {
        public string Ip { get; set; }
        public string Login { get; set; }
        public Host(string ip, string login)
        {
            Ip = ip;
            Login = login;
        }
    }
}
