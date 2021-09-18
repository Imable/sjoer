using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DataManagement
{
    abstract class Connection
    {
        protected Connection()
        {
            this.connect();
        }

        protected abstract void connect();
        public abstract string get();
    }

    class GPSInfoConnection : Connection
    {
        protected override void connect()
        {

        }
        public override string get()
        {
            return "$GPRMC,071228.00,A,5402.6015,N,00025.9797,E,0.2,332.1,180921,0.2,W,A,S*50";
        }
    }
}
