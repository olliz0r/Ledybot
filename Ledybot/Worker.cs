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
    public class Worker
    {
        private uint addr_RequestedPokemon_en = 0x30784ef4;
        private uint addr_RequestedPokemon_es = 0x307856F4;

        private uint addr_RequestedLevel_en = 0x307879B4;
        private uint addr_RequestedLevel_es = 0x307881B4;

        private uint addr_DepositedPokemonNickname_en = 0x3077c514;
        private uint addr_DepositedPokemonNickname_es = 0x3077cd14;

        private uint addr_TrainerName_en = 0x305F1864;
        private uint addr_TrainerName_es = 0x305F2064;

        private uint addr_TrainerCountry_en = 0x305F2A14;
        private uint addr_TrainerCountry_es = 0x305F3214;

        private uint addr_TrainerSubCountry_en = 0x305F76A4;
        private uint addr_TrainerSubCountry_es = 0x305F7EA4;

        private uint addr_RequestedPokemon;
        private uint addr_RequestedLevel;
        private uint addr_DepositedPokemonNickname;
        private uint addr_TrainerName;
        private uint addr_TrainerCountry;
        private uint addr_TrainerSubCountry;
        

        private volatile string szPokemonToFind = "";
        private volatile string szPokemonToGive = "";
        private volatile string szDefaultPk7 = "";
        private volatile string szPk7Folder = "";
        private volatile string szLevel = "";
        private volatile string szPID = "";
        private volatile int iPID = 0;
        private volatile bool bSpanish = false;

        private volatile bool _shouldStop = false;
        public void DoWork()
        {
            bool bPokemonFound = false;
            ScriptHelper h = Program.scriptHelper;
            
            while (!_shouldStop)
            {
                Thread.Sleep(2000);
                bool bSent = false;
                if (!bPokemonFound)
                {
                    h.press(h.Abtn);
                    Thread.Sleep(1000);
                    h.press(h.Abtn);
                    Thread.Sleep(1000);
                    h.press(h.up);
                    Thread.Sleep(1000);
                    //What Pokemon?
                    h.press(h.Abtn);
                    //black screen
                    Thread.Sleep(3000);

                    //"type" the name of the pokemon we want to find
                    byte[] name = new byte[this.szPokemonToFind.Length * 2];
                    for (int i = 0; i < szPokemonToFind.Length; i++)
                    {
                        name[i * 2] = (byte)szPokemonToFind[i];
                        name[i * 2 + 1] = 0x00;
                    }
                    //I like this solution so much
                    Program.scriptHelper.write(0x301118D4, name, iPID);
                    Thread.Sleep(1000);
                    h.press(h.start);
                    Thread.Sleep(1000);
                    h.press(h.Abtn);
                    //black screen
                    Thread.Sleep(4000);
                    bPokemonFound = true;
                }
                else
                {
                    h.press(h.Abtn);
                    Thread.Sleep(2000);
                }

                h.touch(h.searchBtn);
                //finding pokemon
                Thread.Sleep(4000);

                for(int i = 0; i < 25; i++)
                {
                    string szReqPokemon = h.readSafe(addr_RequestedPokemon, 20, iPID);
                    if(szReqPokemon == this.szPokemonToGive)
                    {
                        string szLevel = h.readSafe(addr_RequestedLevel, 12, iPID);
                        szLevel = szLevel.ToLower();
                        if (szLevel.Contains(this.szLevel) || (!this.bSpanish && szLevel.Contains("any")) || (this.bSpanish && szLevel.Contains("cual")))
                        {
                            string szNickname = h.readSafe(addr_DepositedPokemonNickname, 20, iPID);

                            string szPath = this.szDefaultPk7;
                            string szFileToFind = this.szPk7Folder + szNickname + ".pk7";
                            if (File.Exists(szFileToFind))
                            {
                                szPath = szFileToFind;
                            }

                            byte[] pkmEncrypted = System.IO.File.ReadAllBytes(szPath);
                            byte[] cloneshort = PKHeX.encryptArray(pkmEncrypted.Take(232).ToArray());
                            string ek7 = BitConverter.ToString(cloneshort).Replace("-", ", 0x");

                            //optional: grab some trainer data
                            string szTrainerName = h.readSafe(addr_TrainerName, 20, iPID);
                            string szCountry = h.readSafe(addr_TrainerCountry, 20, iPID);
                            string szSubCountry = h.readSafe(addr_TrainerSubCountry, 20, iPID);

                            Program.f1.AppendListViewItem(szTrainerName, szNickname, szCountry, szSubCountry);
                            //Inject the Pokemon to box1slot1
                            Program.scriptHelper.write(0x330d9838, cloneshort, iPID);
                            Thread.Sleep(1000);
                            h.press(h.Abtn);
                            Thread.Sleep(3000);
                            h.press(h.Abtn);
                            Thread.Sleep(1000);
                            h.press(h.Abtn);
                            Thread.Sleep(1000);
                            h.press(h.Abtn);
                            //trade starts here
                            Thread.Sleep(10000);
                            //if the pokemon has been traded we have 35 seconds to get back to the starting spot for the bot by spamming b a bit
                            h.press(h.Abtn);
                            Thread.Sleep(1000);
                            h.press(h.Bbtn);
                            Thread.Sleep(1000);
                            h.press(h.Bbtn);
                            Thread.Sleep(1000);
                            h.press(h.Bbtn);
                            Thread.Sleep(1000);
                            h.press(h.Bbtn);
                            Thread.Sleep(1000);
                            h.press(h.Bbtn);
                            //if the trade is still going on wait for it to finish
                            Thread.Sleep(30000);
                            bSent = true;
                            break;
                        }
                    }
                    h.press(h.right);
                    Thread.Sleep(500);
                }
                if (!bSent)
                {
                    //no tradable pokemon in the last X pokemon, start from the front of the list again
                    h.press(h.Bbtn);
                    Thread.Sleep(1000);
                    h.press(h.Bbtn);
                    Thread.Sleep(1000);
                }
            }
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }

        public void setValues(string szPtF, string szPtG, string szD, string szF, string szL, string szP, bool bSpanish)
        {
            this.szPokemonToFind = szPtF;
            this.szPokemonToGive = szPtG;
            this.szDefaultPk7 = szD;
            this.szPk7Folder = szF;
            this.szLevel = szL;
            this.szPID = szP;
            this.bSpanish = bSpanish;


            if (bSpanish)
            {
                addr_RequestedPokemon = addr_RequestedPokemon_es;
                addr_RequestedLevel = addr_RequestedLevel_es;
                addr_DepositedPokemonNickname = addr_DepositedPokemonNickname_es;
                addr_TrainerName = addr_TrainerName_es;
                addr_TrainerCountry = addr_TrainerCountry_es;
                addr_TrainerSubCountry = addr_TrainerSubCountry_es;
            }
            else
            {
                addr_RequestedPokemon = addr_RequestedPokemon_en;
                addr_RequestedLevel = addr_RequestedLevel_en;
                addr_DepositedPokemonNickname = addr_DepositedPokemonNickname_en;
                addr_TrainerName = addr_TrainerName_en;
                addr_TrainerCountry = addr_TrainerCountry_en;
                addr_TrainerSubCountry = addr_TrainerSubCountry_en;
            }

            this.iPID = int.Parse(szPID, NumberStyles.HexNumber);
        }

    }
}
