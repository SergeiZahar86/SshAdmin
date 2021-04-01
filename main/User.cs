using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    class User
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private string Pass { get; set; }


        public User(int id, string name, string pass)
        {
            Id = id;
            Name = name;
            Pass = pass;
        }
        public User() { }
    }
}
