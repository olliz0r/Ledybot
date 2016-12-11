using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ledybot
{
    class GTSBot7
    {

        public enum gtsbotstates { botstart, startsearch, openpokemonwanted, openwhatpokemon, typepokemon, presssearch, startfind, findfromend, findfromstart, trade, research, botexit };

        private uint addr_RequestedPokemon = 0x30784ef4;
        private uint addr_RequestedLevel = 0x307879B4;
        private uint addr_DepositedPokemonNickname = 0x3077c514;
        private uint addr_TrainerName = 0x305F1864;
        private uint addr_TrainerCountry = 0x305F2A14;
        private uint addr_TrainerSubCountry = 0x305F76A4;

        private uint addr_PageSize = 0x32A6A1A4;
        private uint addr_PageEndStartRecord = 0x32A6A68C;
        private uint addr_PageCurrentView = 0x305ea384;
        private uint addr_PageStartingIndex = 0x32A6A190;

        private string szPokemonToFind = "";
        private string szDefaultPk7 = "";
        private string szPk7Folder = "";
        private string szPID = "";
        private int iPID = 0;
        private bool bSpanish = false;
        private int dexNum = 0;
        private int genderIndex = 0;
        private int levelIndex = 0;

        private uint eggOff = 0x3313EDD8;
        private uint currentIndex = 0x305ea384;
        private uint pageStartingIndex = 0x32A6A190;

        public bool botstop = false;
        public string finishmessage;
        private int botState = 0;
        public int botresult = 0;
        private int attempts = 0;
        long dataready;
        private bool taskresultbool;
        Task<bool> waitTaskbool;
        Task<long> waitTaskint;
        private int commandtime = 250;
        private int delaytime = 150;
        private int maxreconnect = 10;

        private int listlength = 0;
        private int startIndex = 0;
        private byte[] temp;
        private int tradeIndex = -1;
        private uint addr_PageEntry = 0;
        private bool foundLastPage = false;

        public GTSBot7(int iP, string szPtF = "", string szD = "", string szF = "", bool bSpanish = false, int dex = 0, int gender = 0, int level = 0)
        {
            this.szPokemonToFind = szPtF;
            this.szDefaultPk7 = szD;
            this.szPk7Folder = szF;
            this.iPID = iP;
            this.bSpanish = bSpanish;
            this.dexNum = dex;
            this.genderIndex = gender + 1;
            this.levelIndex = level + 1;
        }

        public async Task<int> RunBot()
        {
            while (!botstop)
            {
                switch (botState)
                {
                    case (int)gtsbotstates.botstart:
                        botState = (int)gtsbotstates.startsearch;
                        break;
                    case (int)gtsbotstates.startsearch:
                        Program.helper.quicktouch(128, 64, commandtime);
                        await Task.Delay(commandtime + delaytime);
                        botState = (int)gtsbotstates.openpokemonwanted;
                        break;
                    case (int)gtsbotstates.openpokemonwanted:
                        await Task.Delay(500);
                        Program.helper.quicktouch(128, 50, commandtime);
                        await Task.Delay(commandtime + delaytime);
                        botState = (int)gtsbotstates.openwhatpokemon;
                        break;
                    case (int)gtsbotstates.openwhatpokemon:
                        await Task.Delay(500);
                        Program.helper.quickbuton(Program.PKTable.DpadUP, commandtime);
                        await Task.Delay(commandtime + delaytime);
                        Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                        await Task.Delay(commandtime + delaytime);
                        botState = (int)gtsbotstates.typepokemon;
                        break;
                    case (int)gtsbotstates.typepokemon:
                        await Task.Delay(3000);
                        byte[] name = new byte[this.szPokemonToFind.Length * 2];
                        for (int i = 0; i < szPokemonToFind.Length; i++)
                        {
                            name[i * 2] = (byte)szPokemonToFind[i];
                            name[i * 2 + 1] = 0x00;
                        }
                        //I like this solution so much
                        waitTaskbool = Program.helper.waitNTRwrite(0x301118D4, name, iPID);
                        if (await waitTaskbool)
                        {
                            attempts = 0;
                            waitTaskbool = Program.helper.waitbutton(Program.PKTable.keySTART);
                            if (await waitTaskbool)
                            {
                                waitTaskbool = Program.helper.waitbutton(Program.PKTable.keyA);
                                if (await waitTaskbool)
                                {
                                    botState = (int)gtsbotstates.presssearch;
                                }
                            }
                        }
                        else
                        {
                            attempts = 11;
                            botresult = -1;
                            botState = (int)gtsbotstates.botexit;
                            break;
                        }
                        break;
                    case (int)gtsbotstates.presssearch:
                        await Task.Delay(2000);
                        waitTaskbool = Program.helper.waittouch(160, 185);
                        if (await waitTaskbool)
                        {
                            botState = (int)gtsbotstates.findfromstart;
                            await Task.Delay(2250);
                        }
                        else
                        {
                            attempts++;
                            botresult = 6;
                            botState = (int)gtsbotstates.startsearch;
                        }
                        break;
                    case (int)gtsbotstates.findfromstart:
                        await Program.helper.waitNTRread(addr_PageSize);

                        attempts = 0;
                        listlength = (int)Program.helper.lastRead;

                        if (listlength == 100 && !foundLastPage)
                        {
                            waitTaskbool = Program.helper.waitNTRread(addr_PageStartingIndex);
                            if (await waitTaskbool)
                            {
                                startIndex = (int)Program.helper.lastRead;
                                waitTaskbool = Program.helper.waitNTRwrite(addr_PageStartingIndex, (uint)(startIndex + 200), iPID);
                                if (await waitTaskbool)
                                {
                                    startIndex += 100;
                                    Program.helper.quickbuton(Program.PKTable.DpadRIGHT, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    Program.helper.quickbuton(Program.PKTable.DpadLEFT, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    Program.helper.quickbuton(Program.PKTable.DpadLEFT, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    await Task.Delay(2250);
                                    Program.helper.quicktouch(0, 0, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    Program.helper.quicktouch(0, 0, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    Program.helper.quicktouch(0, 0, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    Program.helper.quicktouch(0, 0, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    await Program.helper.waitNTRread(addr_PageStartingIndex);
                                    if (Program.helper.lastRead == 0)
                                    {
                                        foundLastPage = true;
                                    }
                                    else
                                    {
                                        botState = (int)gtsbotstates.findfromend;
                                    }

                                }
                            }
                        }
                        else
                        {
                            foundLastPage = true;
                            attempts = 0;
                            listlength = (int)Program.helper.lastRead;
                            int dexnumber = 0;
                            await Program.helper.waitNTRread(addr_PageEndStartRecord);
                            addr_PageEntry = Program.helper.lastRead;
                            await Program.helper.waitNTRread(0x32A6A7C4, (uint)(256 * 100));
                            byte[] blockBytes = Program.helper.lastArray;
                            byte[] block = new byte[256];
                            for(int i = listlength; i > 0; i--)
                            {
                                Array.Copy(blockBytes, addr_PageEntry - 0x32A6A7C4, block, 0, 256);
                                dexnumber = BitConverter.ToInt16(block, 0xC);
                                if(dexnumber == dexNum)
                                {
                                    int gender = block[0xE];
                                    int level = block[0xF];
                                    if ((gender == 0 || gender == genderIndex) && (level == 0 || level == (levelIndex)))
                                    {
                                        tradeIndex = i - 1;
                                        botState = (int)gtsbotstates.trade;
                                        break;
                                    }
                                }

                                addr_PageEntry = BitConverter.ToUInt32(block, 0);

                            }
                            //for (int i = listlength; i > 0; i--)
                            //{
                            //    await Program.helper.waitNTRread(addr_PageEntry + 0xC, 2);
                            //    dexnumber = BitConverter.ToInt16(Program.helper.lastArray, 0);
                            //    if (dexnumber == dexNum)
                            //    {
                            //        await Program.helper.waitNTRread(addr_PageEntry + 0xE, 1);
                            //        int gender = Program.helper.lastArray[0];
                            //        await Program.helper.waitNTRread(addr_PageEntry + 0xF, 1);
                            //        int level = Program.helper.lastArray[0];
                            //        if ((gender == 0 || gender == genderIndex) && (level == 0 || level == (levelIndex)))
                            //        {
                            //            tradeIndex = i - 1;
                            //            botState = (int)gtsbotstates.trade;
                            //            break;
                            //        }
                            //    }
                            //    await Program.helper.waitNTRread(addr_PageEntry);
                            //    addr_PageEntry = Program.helper.lastRead;
                            //}
                            if (tradeIndex == -1)
                            {
                                if (startIndex == 0)
                                {
                                    Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                    await Task.Delay(commandtime + delaytime + 500);
                                    Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    botState = (int)gtsbotstates.research;
                                }
                                else
                                {
                                    startIndex -= 100;
                                    Program.helper.quickbuton(Program.PKTable.DpadLEFT, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    await Task.Delay(2250);
                                    botState = (int)gtsbotstates.findfromend;
                                }
                            }
                        }

                        break;
                    case (int)gtsbotstates.findfromend:
                        await Program.helper.waitNTRread(addr_PageSize);

                        attempts = 0;
                        listlength = (int)Program.helper.lastRead;
                        if (listlength == 100 && !foundLastPage)
                        {
                            startIndex += 100;
                            Program.helper.quickbuton(Program.PKTable.DpadRIGHT, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            await Task.Delay(2250);
                            Program.helper.quicktouch(0, 0, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            Program.helper.quicktouch(0, 0, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            Program.helper.quicktouch(0, 0, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            Program.helper.quicktouch(0, 0, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            await Program.helper.waitNTRread(addr_PageCurrentView);
                            if (Program.helper.lastRead == 0)
                            {
                                foundLastPage = true;
                            }
                            botState = (int)gtsbotstates.findfromstart;
                        }
                        else
                        {
                            foundLastPage = true;
                            attempts = 0;
                            listlength = (int)Program.helper.lastRead;
                            int dexnumber = 0;
                            await Program.helper.waitNTRread(addr_PageEndStartRecord);
                            addr_PageEntry = Program.helper.lastRead;
                            await Program.helper.waitNTRread(0x32A6A7C4, (uint)(256 * 100));
                            byte[] blockBytes = Program.helper.lastArray;
                            byte[] block = new byte[256];
                            for (int i = listlength; i > 0; i--)
                            {
                                Array.Copy(blockBytes, addr_PageEntry - 0x32A6A7C4, block, 0, 256);
                                dexnumber = BitConverter.ToInt16(block, 0xC);
                                if (dexnumber == dexNum)
                                {
                                    int gender = block[0xE];
                                    int level = block[0xF];
                                    if ((gender == 0 || gender == genderIndex) && (level == 0 || level == (levelIndex)))
                                    {
                                        tradeIndex = i - 1;
                                        botState = (int)gtsbotstates.trade;
                                        break;
                                    }
                                }

                                addr_PageEntry = BitConverter.ToUInt32(block, 0);

                            }
                            //for (int i = listlength; i > 0; i--)
                            //{
                            //    await Program.helper.waitNTRread(addr_PageEntry + 0xC, 2);
                            //    dexnumber = BitConverter.ToInt16(Program.helper.lastArray, 0);
                            //    if (dexnumber == dexNum)
                            //    {
                            //        await Program.helper.waitNTRread(addr_PageEntry + 0xE, 1);
                            //        int gender = Program.helper.lastArray[0];
                            //        await Program.helper.waitNTRread(addr_PageEntry + 0xF, 1);
                            //        int level = Program.helper.lastArray[0];
                            //        if ((gender == 0 || gender == genderIndex) && (level == 0 || level == (levelIndex)))
                            //        {
                            //            tradeIndex = i - 1;
                            //            botState = (int)gtsbotstates.trade;
                            //            break;
                            //        }
                            //    }
                            //    await Program.helper.waitNTRread(addr_PageEntry);
                            //    addr_PageEntry = Program.helper.lastRead;
                            //}
                            if (tradeIndex == -1)
                            {
                                if (listlength < 100)
                                {
                                    for (int i = 0; i < listlength; i++)
                                    {
                                        Program.helper.quickbuton(Program.PKTable.DpadLEFT, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                    }
                                    await Task.Delay(2250);
                                }
                                else if (startIndex >= 200)
                                {
                                    waitTaskbool = Program.helper.waitNTRwrite(addr_PageStartingIndex, (uint)(startIndex - 200), iPID);
                                    if (await waitTaskbool)
                                    {
                                        await Program.helper.waitNTRwrite(addr_PageSize, 0x64, iPID);
                                        startIndex -= 100;
                                        Program.helper.quickbuton(Program.PKTable.DpadLEFT, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quickbuton(Program.PKTable.DpadRIGHT, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quickbuton(Program.PKTable.DpadRIGHT, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        await Task.Delay(2250);
                                        Program.helper.quicktouch(0, 0, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quicktouch(0, 0, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quicktouch(0, 0, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quicktouch(0, 0, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        botState = (int)gtsbotstates.findfromstart;
                                    }
                                }
                                else if (startIndex == 0)
                                {
                                    Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                    await Task.Delay(commandtime + delaytime + 500);
                                    Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    botState = (int)gtsbotstates.research;
                                }
                                else
                                {
                                    waitTaskbool = Program.helper.waitNTRwrite(addr_PageStartingIndex, 100000, iPID);
                                    if (await waitTaskbool)
                                    {
                                        startIndex -= 100;
                                        Program.helper.quickbuton(Program.PKTable.DpadLEFT, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quickbuton(Program.PKTable.DpadRIGHT, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quickbuton(Program.PKTable.DpadRIGHT, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        await Task.Delay(2250);
                                        Program.helper.quicktouch(0, 0, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quicktouch(0, 0, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quicktouch(0, 0, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        Program.helper.quicktouch(0, 0, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                        botState = (int)gtsbotstates.findfromstart;
                                    }
                                }
                            }
                        }

                        break;
                    case (int)gtsbotstates.trade:
                        waitTaskbool = Program.helper.waitNTRwrite(addr_PageCurrentView, BitConverter.GetBytes(tradeIndex), iPID);
                        if (await waitTaskbool)
                        {
                            await Program.helper.waitNTRread(addr_PageEntry + 0x14, 20);
                            string szNickname = Encoding.Unicode.GetString(Program.helper.lastArray).Trim('\0');

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
                            await Program.helper.waitNTRread(addr_PageEntry + 0x4C, 20);
                            string szTrainerName = Encoding.Unicode.GetString(Program.helper.lastArray).Trim('\0');
                            await Program.helper.waitNTRread(addr_TrainerCountry, 20);
                            string szCountry = Encoding.Unicode.GetString(Program.helper.lastArray).Trim('\0');
                            await Program.helper.waitNTRread(addr_TrainerSubCountry, 20);
                            string szSubCountry = Encoding.Unicode.GetString(Program.helper.lastArray).Trim('\0');
                            await Program.helper.waitNTRread(addr_PageEntry + 0x48);
                            byte[] principal = Program.helper.lastArray;
                            byte checksum = Program.f1.calculateChecksum(principal);
                            byte[] fc = new byte[5];
                            Array.Copy(principal, 0, fc, 1, 4);
                            fc[0] = checksum;
                            string hex = Program.f1.ByteArrayToString(fc);
                            long iFC = long.Parse(hex, NumberStyles.HexNumber);

                            Program.f1.AppendListViewItem(szTrainerName, szNickname, szCountry, szSubCountry, iFC.ToString());
                            //Inject the Pokemon to box1slot1
                            Program.scriptHelper.write(0x330d9838, cloneshort, iPID);
                            Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                            await Task.Delay(commandtime + delaytime + 2500);
                            Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            await Task.Delay(47000);
                            startIndex = 0;
                            tradeIndex = -1;
                            listlength = 0;
                            addr_PageEntry = 0;
                            foundLastPage = false;
                            botState = (int)gtsbotstates.research;
                        }
                        break;
                    case (int)gtsbotstates.research:
                        Program.helper.quicktouch(128, 64, commandtime);
                        await Task.Delay(commandtime + delaytime + 500);
                        await Program.helper.waittouch(160, 190);
                        await Task.Delay(2250);
                        botState = (int)gtsbotstates.findfromstart;
                        break;
                    case (int)gtsbotstates.botexit:
                    default:
                        botresult = -1;
                        botstop = true;
                        break;
                }
            }
            return botresult;
        }

        public void RequestStop()
        {
            botstop = true;
        }


    }
}
