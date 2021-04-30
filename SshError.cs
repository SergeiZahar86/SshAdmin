using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncubeAdmin
{
    public class SshError
    {
        public SshError(string time, string text)
        {
            Time = time;
            Text = text;
        }
        public string Time { get; set; }
        public string Text { get; set; }
    }
}
