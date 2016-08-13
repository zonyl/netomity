using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Utility
{
    public static class SystemTime
    {
        static public Func<DateTime> s_dt = () => DateTime.Now;
        static public DateTime Now
        {
            get
            {
                return s_dt();
            }
            set
            {
                s_dt = () => value;
            }
        }

        static public void Reset()
        {
            s_dt = () => DateTime.Now;
        }
    }
}
