using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledybot
{
    public class EggBot
    {

        private volatile int iPID = 0;

        private uint eggOff = 0x3313EDD8;

        private bool botstop = false;

        public EggBot(int iP, int game)
        {
            iPID = iP;
            if(game == 0)
            {
                eggOff = 0x3313EDD8;
            } 
            else if(game == 1)
            {
                eggOff = 0x3307B1E8;
            }
        }

        public async Task RunBot()
        {
            while(!botstop)
            {
                await Task.Delay(250);
                await Program.helper.waitNTRwrite(eggOff, 0x1, iPID);
            }
        }
        
        public void RequestStop()
        {
            botstop = true;
        }



    }
}