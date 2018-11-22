using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedyLib
{
    public class EggBot
    {

        private volatile int iPID = 0;

        private uint eggOff = 0x3313EDD8;

        private bool botstop = false;

        private readonly RemoteControl _helper;

        public EggBot(int iP, int game, RemoteControl helper)
        {
            _helper = helper;
            iPID = iP;
            switch (game)
            {
                case 0:
                    eggOff = 0x3313EDD8;
                    break;
                case 1:
                    eggOff = 0x3307B1E8;
                    break;
            }
        }

        public async Task RunBot()
        {
            while (!botstop)
            {
                await Task.Delay(250);
                await _helper.waitNTRwrite(eggOff, 0x1, iPID);
            }
        }

        public void RequestStop()
        {
            botstop = true;
        }



    }
}
