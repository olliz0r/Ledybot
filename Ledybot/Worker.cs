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

        private uint addr_PageSize = 0x32A6A1A4;
        private uint addr_PageEndStartRecord = 0x32A6A68C;
        private uint addr_PageCurrentView = 0x305ea384;

        private uint addr_RequestedPokemon;
        private uint addr_RequestedLevel;
        private uint addr_DepositedPokemonNickname;
        private uint addr_TrainerName;
        private uint addr_TrainerCountry;
        private uint addr_TrainerSubCountry;


        private volatile string szPokemonToFind = "";
        private volatile string szDefaultPk7 = "";
        private volatile string szPk7Folder = "";
        private volatile string szPID = "";
        private volatile int iPID = 0;
        private volatile bool bSpanish = false;
        private volatile int dexNum = 0;
        private volatile int genderIndex = 0;
        private volatile int levelIndex = 0;

        private uint eggOff = 0x3313EDD8;

        private volatile int mode = 0;

        private volatile bool _shouldStop = false;
        //public void DoWork()
        //{

        //    bool bPokemonFound = false;
        //    ScriptHelper h = Program.scriptHelper;
        //    if (mode == 0)
        //    {
        //        while (!_shouldStop)
        //        {
        //            Thread.Sleep(2250);
        //            bool bSent = false;
        //            if (!bPokemonFound)
        //            {
        //                h.press(h.Abtn);
        //                Thread.Sleep(1000);
        //                h.press(h.Abtn);
        //                Thread.Sleep(1000);
        //                h.press(h.up);
        //                Thread.Sleep(1000);
        //                //What Pokemon?
        //                h.press(h.Abtn);
        //                //black screen
        //                Thread.Sleep(3250);

        //                //"type" the name of the pokemon we want to find
        //                byte[] name = new byte[this.szPokemonToFind.Length * 2];
        //                for (int i = 0; i < szPokemonToFind.Length; i++)
        //                {
        //                    name[i * 2] = (byte)szPokemonToFind[i];
        //                    name[i * 2 + 1] = 0x00;
        //                }
        //                //I like this solution so much
        //                Program.scriptHelper.write(0x301118D4, name, iPID);
        //                Thread.Sleep(1000);
        //                h.press(h.start);
        //                Thread.Sleep(1000);
        //                h.press(h.Abtn);
        //                //black screen
        //                Thread.Sleep(4250);
        //                bPokemonFound = true;
        //            }
        //            else
        //            {
        //                h.touch(h.searchBtn);
        //                Thread.Sleep(3000);
        //                h.press(h.Bbtn);
        //                Thread.Sleep(3000);
        //                h.press(h.Abtn);
        //                Thread.Sleep(3000);
        //            }

        //            h.touch(h.searchBtn);
        //            //finding pokemon
        //            Thread.Sleep(4250);

        //            bool lastPage = false;
        //            int pageCount = 1;
        //            while (!lastPage)
        //            {
        //                int szListCount = int.Parse(h.readSafe(addr_PageSize, 1, iPID), NumberStyles.HexNumber);
        //                if (szListCount == 100)
        //                {
        //                    uint addr_PageEntryAddress = uint.Parse(h.readSafe(addr_PageEndStartRecord, 4, iPID), NumberStyles.HexNumber);
        //                    string hex = h.readSafe(addr_PageEntryAddress + 0x48, 4, iPID);
        //                    int szTrainerID1 = int.Parse(hex, NumberStyles.HexNumber);

        //                    for (int i = 0; i < szListCount - 1; i++)
        //                    {
        //                        h.press(h.right);
        //                    }
        //                    Thread.Sleep(100);
        //                    h.press(h.right);
        //                    Thread.Sleep(3000);

        //                    addr_PageEntryAddress = uint.Parse(h.readSafe(addr_PageEndStartRecord, 4, iPID), NumberStyles.HexNumber);
        //                    hex = h.readSafe(addr_PageEntryAddress + 0x48, 4, iPID);
        //                    int szTrainerID2 = int.Parse(hex, NumberStyles.HexNumber);

        //                    if (szTrainerID1 == szTrainerID2)
        //                    {
        //                        lastPage = true;
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        pageCount += 1;
        //                    }

        //                }
        //                else
        //                {
        //                    lastPage = true;
        //                    for (int i = 0; i < szListCount; i++)
        //                    {
        //                        h.press(h.right);
        //                        Thread.Sleep(100);
        //                    }
        //                }

        //            }

        //            for (int i = pageCount; i > 0; i--)
        //            {
        //                int szListCount = int.Parse(h.readSafe(addr_PageSize, 1, iPID), NumberStyles.HexNumber);
        //                uint addr_PageEntryAddress = uint.Parse(h.readSafe(addr_PageEndStartRecord, 4, iPID), NumberStyles.HexNumber);
        //                int dexNumber = 0;
        //                for (int j = 0; j < szListCount; j++)
        //                {
        //                    string hex = h.readSafe(addr_PageEntryAddress + 0xC, 2, iPID);
        //                    dexNumber = int.Parse(hex, NumberStyles.HexNumber);
        //                    if (dexNumber == dexNum)
        //                    {
        //                        hex = h.readSafe(addr_PageEntryAddress + 0xE, 1, iPID);
        //                        int gender = (hex == "" ? 0 : int.Parse(hex, NumberStyles.HexNumber));
        //                        hex = h.readSafe(addr_PageEntryAddress + 0xF, 1, iPID);
        //                        int level = (hex == "" ? 0 : int.Parse(hex, NumberStyles.HexNumber));
        //                        if ((gender == 0 || gender == genderIndex) && (level == 0 || level == (levelIndex)))
        //                        {
        //                            for (int k = 0; k < j; k++)
        //                            {
        //                                h.press(h.left);
        //                                Thread.Sleep(150);
        //                            }

        //                            string szNickname = h.readSafe(addr_DepositedPokemonNickname, 20, iPID);

        //                            string szPath = this.szDefaultPk7;
        //                            string szFileToFind = this.szPk7Folder + szNickname + ".pk7";
        //                            if (File.Exists(szFileToFind))
        //                            {
        //                                szPath = szFileToFind;
        //                            }

        //                            byte[] pkmEncrypted = System.IO.File.ReadAllBytes(szPath);
        //                            byte[] cloneshort = PKHeX.encryptArray(pkmEncrypted.Take(232).ToArray());
        //                            string ek7 = BitConverter.ToString(cloneshort).Replace("-", ", 0x");

        //                            //optional: grab some trainer data
        //                            string szTrainerName = h.readSafe(addr_TrainerName, 20, iPID);
        //                            string szCountry = h.readSafe(addr_TrainerCountry, 20, iPID);
        //                            string szSubCountry = h.readSafe(addr_TrainerSubCountry, 20, iPID);
        //                            hex = h.readSafe(addr_PageEntryAddress + 0x48, 4, iPID);
        //                            byte[] principal = Program.f1.StringToByteArray(hex);
        //                            byte checksum = Program.f1.calculateChecksum(principal);
        //                            byte[] fc = new byte[5];
        //                            Array.Copy(principal, 0, fc, 1, 4);
        //                            fc[0] = checksum;
        //                            hex = Program.f1.ByteArrayToString(fc);
        //                            long iFC = long.Parse(hex, NumberStyles.HexNumber);

        //                            Program.f1.AppendListViewItem(szTrainerName, szNickname, szCountry, szSubCountry, iFC.ToString());
        //                            //Inject the Pokemon to box1slot1
        //                            Program.scriptHelper.write(0x330d9838, cloneshort, iPID);
        //                            Thread.Sleep(1000);
        //                            h.press(h.Abtn);
        //                            Thread.Sleep(3250);
        //                            h.press(h.Abtn);
        //                            Thread.Sleep(1000);
        //                            h.press(h.Abtn);
        //                            Thread.Sleep(1000);
        //                            h.press(h.Abtn);
        //                            //trade starts here
        //                            Thread.Sleep(10250);
        //                            //if the pokemon has been traded we have 35 seconds to get back to the starting spot for the bot by spamming b a bit
        //                            h.press(h.Abtn);
        //                            Thread.Sleep(1000);
        //                            h.press(h.Bbtn);
        //                            Thread.Sleep(1000);
        //                            h.press(h.Bbtn);
        //                            Thread.Sleep(1000);
        //                            h.press(h.Bbtn);
        //                            Thread.Sleep(1000);
        //                            h.press(h.Bbtn);
        //                            Thread.Sleep(1000);
        //                            h.press(h.Bbtn);
        //                            //if the trade is still going on wait for it to finish
        //                            Thread.Sleep(30250);
        //                            bSent = true;
        //                            break;
        //                        }
        //                    }
        //                    addr_PageEntryAddress = uint.Parse(h.readSafe(addr_PageEntryAddress, 4, iPID), NumberStyles.HexNumber);
        //                }

        //                if (bSent)
        //                {
        //                    break;
        //                }

        //                if (i != 1)
        //                {
        //                    for (int j = 0; j < szListCount - 1; j++)
        //                    {
        //                        h.press(h.left);
        //                        Thread.Sleep(100);
        //                    }
        //                    h.press(h.left);
        //                    Thread.Sleep(3000);
        //                }
        //            }

        //            if (!bSent)
        //            {
        //                //no tradable pokemon found
        //                h.press(h.Bbtn);
        //                Thread.Sleep(1250);
        //                h.press(h.Bbtn);
        //                Thread.Sleep(1250);
        //            }

        //        }
        //    }
        //    else if(mode == 1)
        //    {
        //        while(!_shouldStop)
        //        {
        //            int available = int.Parse(h.readSafe(eggOff, 1, iPID), NumberStyles.HexNumber);
        //            if(available == 0)
        //            {
        //                byte[] data = BitConverter.GetBytes(0x01);
        //                Program.scriptHelper.write(eggOff, data, iPID);
        //            }
        //            Thread.Sleep(3000);
        //        }
        //    }
        //}
        public void RequestStop()
        {
            _shouldStop = true;
        }

        public void setValues(int mode, int iP, string szPtF = "", string szD = "", string szF = "", bool bSpanish = false, int dex = 0, int gender = 0, int level = 0)
        {
            this.mode = mode;
            this.szPokemonToFind = szPtF;
            this.szDefaultPk7 = szD;
            this.szPk7Folder = szF;
            this.iPID = iP;
            this.bSpanish = bSpanish;
            this.dexNum = dex;
            this.genderIndex = gender + 1;
            this.levelIndex = level + 1;


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


        }

    }
}