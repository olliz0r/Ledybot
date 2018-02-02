using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Ledybot
{
    class GTSBot7
    {

        //private System.IO.StreamWriter file = new StreamWriter(@"C:\Temp\ledylog.txt");

        public enum gtsbotstates { botstart, startsearch, pressSeek, openpokemonwanted, openwhatpokemon, typepokemon, presssearch, startfind, findfromend, findfromstart, trade, research, botexit, updatecomments, quicksearch, panic };

        private string consoleName = "Ledybot";

        private TcpClient syncClient = new TcpClient();
        private IPEndPoint serverEndPointSync = null;
        private bool useLedySync = false;

        private TcpClient tvClient = new TcpClient();
        private IPEndPoint serverEndPointTV = null;
        private bool useLedybotTV = false;


        private const int SEARCHDIRECTION_FROMBACK = 0;
        private const int SEARCHDIRECTION_FROMBACKFIRSTPAGEONLY = 1;
        private const int SEARCHDIRECTION_FROMFRONT = 2;

        private uint addr_PageSize; //How many entries are on the current GTS page
        private uint addr_PageEndStartRecord; //This address holds the address to the last block in the entry-block-list
        private uint addr_PageStartStartRecord; //This address holds the address to the first block in the entry block-list
        private uint addr_PageCurrentView; //Current selected entry in the list
        private uint addr_PageStartingIndex; //To determine on which page we are, 0 = first page, 100 = second page, etc
        private uint addr_ListOfAllPageEntries; //Startingaddress of all up to 100 trade entries of the current page

        private uint addr_box1slot1; //To inject the pokemon into box1slot1

        //private uint addr_SearchPokemonNameField = 0x301118D4; //Holds the currently typed in name in the "search pokemon" window

        private uint addr_currentScreen; //Hopefully a address to tell us in what screen we are (roughly)

        private uint addr_pokemonToFind;
        private uint addr_pokemonToFindGender;
        private uint addr_pokemonToFindLevel;

        private int val_PlazaScreen;
        private int val_Quit_SeekScreen;
        private int val_SearchScreen; //also in the box during selecting etc
        private int val_WhatPkmnScreen;
        private int val_GTSListScreen;
        private int val_BoxScreen;
        private int val_system; //during error, saving, early sending
        private int val_duringTrade; //trade is split in several steps, sometimes even 0x00

        private int iPokemonToFind = 0;
        private int iPokemonToFindGender = 0;
        private int iPokemonToFindLevel = 0;
        private int iPID = 0;
        private bool bBlacklist = false;
        private bool bReddit = false;
        private int searchDirection = 0;
        private int dexnumber = 0;
        private string szFC = "";
        private byte[] principal = new byte[4];

        public bool botstop = false;
        private int botState = 0;
        public int botresult = 0;
        private int attempts = 0;
        Task<bool> waitTaskbool;
        private int commandtime = 250;
        private int delaytime = 150;
        private int o3dswaittime = 1000;

        private int listlength = 0;
        private int startIndex = 0;
        private byte[] block = new byte[256];
        private int tradeIndex = -1;
        private uint addr_PageEntry = 0;
        private bool foundLastPage = false;

        private Tuple<string, string, int, int, int, ArrayList> details;

        private async Task<bool> isCorrectWindow(int expectedScreen)
        {
            await Task.Delay(o3dswaittime);
            await Program.helper.waitNTRread(addr_currentScreen);
            int screenID = (int)Program.helper.lastRead;

            //file.WriteLine("Checkscreen: " + expectedScreen + " - " + screenID + " botstate:" + botState);
            //file.Flush();
            return expectedScreen == screenID;
        }

        private Boolean canThisTrade(byte[] principal, string consoleName, string trainerName, string country, string region, string pokemon, string szFC, string page, string index)
        {
            NetworkStream clientStream = syncClient.GetStream();
            byte[] buffer = new byte[4096];
            byte[] messageID = { 0x00 };
            string szmessage = consoleName + '\t' + trainerName + '\t' + country + '\t' + region + '\t' + pokemon + '\t' + page + "\t" + index + "\t";
            byte[] toSend = Encoding.UTF8.GetBytes(szmessage);

            buffer = messageID.Concat(principal).Concat(toSend).ToArray();
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
            byte[] message = new byte[4096];
            try
            {
                //blocks until a client sends a message
                int bytesRead = clientStream.Read(message, 0, 4096);
                if (message[0] == 0x02)
                {
                    Program.f1.banlist.Add(szFC);
                }
                return message[0] == 0x01;
            }
            catch
            {
                return false;
                //a socket error has occured
            }
        }

        private void didTradeTV(byte[] principal, string consoleName, string trainerName, string country, string region, string pokemon, string szFC, string page, string index)
        {
            NetworkStream clientStream = tvClient.GetStream();
            byte[] buffer = new byte[4096];
            byte[] messageID = { 0x00 };
            string szmessage = consoleName + '\t' + trainerName + '\t' + country + '\t' + region + '\t' + pokemon + '\t' + page + "\t" + index + "\t";
            byte[] toSend = Encoding.UTF8.GetBytes(szmessage);

            buffer = messageID.Concat(principal).Concat(toSend).ToArray();
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
            byte[] message = new byte[4096];
            try
            {
                int bytesRead = clientStream.Read(message, 0, 4096);
                if(message[0] == 0x01)
                {
                    Program.f1.ChangeStatus("LedybotTV message sent.");
                }
            }
            catch
            {
                return;
            }
        }

        public GTSBot7(int iP, int iPtF, int iPtFGender, int iPtFLevel, bool bBlacklist, bool bReddit, int iSearchDirection, string waittime, string consoleName, bool useLedySync, string ledySyncIp, string ledySyncPort, int game, bool useLedybotTV, string ledybotTVIp, string ledybotTVPort)
        {
            this.iPokemonToFind = iPtF;
            this.iPokemonToFindGender = iPtFGender;
            this.iPokemonToFindLevel = iPtFLevel;
            this.iPID = iP;
            this.bBlacklist = bBlacklist;
            this.bReddit = bReddit;
            this.searchDirection = iSearchDirection;
            this.o3dswaittime = Int32.Parse(waittime);
            if (useLedySync)
            {
                this.useLedySync = useLedySync;
                int iPort = Int32.Parse(ledySyncPort);
                this.serverEndPointSync = new IPEndPoint(IPAddress.Parse(ledySyncIp), iPort);
                syncClient.Connect(serverEndPointSync);
            }

            if(useLedybotTV)
            {
                this.useLedybotTV = useLedybotTV;
                int iPort = Int32.Parse(ledybotTVPort);
                this.serverEndPointTV = new IPEndPoint(IPAddress.Parse(ledybotTVIp), iPort);
                tvClient.Connect(serverEndPointTV);
            }
            this.consoleName = consoleName;

            if (game == 0)
            {
                addr_PageSize = 0x32A6A1A4; //How many entries are on the current GTS page
                addr_PageEndStartRecord = 0x32A6A68C; //This address holds the address to the last block in the entry-block-list
                addr_PageStartStartRecord = 0x32A6A690; //This address holds the address to the first block in the entry block-list
                addr_PageCurrentView = 0x305ea384; //Current selected entry in the list
                addr_PageStartingIndex = 0x32A6A190; //To determine on which page we are, 0 = first page, 100 = second page, etc
                addr_ListOfAllPageEntries = 0x32A6A7C4; //Startingaddress of all up to 100 trade entries of the current page

                addr_box1slot1 = 0x330D9838; //To inject the pokemon into box1slot1

                addr_currentScreen = 0x00674802; //Hopefully a address to tell us in what screen we are (roughly)

                addr_pokemonToFind = 0x32A6A180;
                addr_pokemonToFindGender = 0x32A6A184;
                addr_pokemonToFindLevel = 0x32A6A188;

                val_PlazaScreen = 0x00;
                val_Quit_SeekScreen = 0x3F2B;
                val_SearchScreen = 0x4128; //also in the box during selecting etc
                val_WhatPkmnScreen = 0x4160;
                val_GTSListScreen = 0x4180;
                val_BoxScreen = 0x4120;
                val_system = 0x41A8; //during error, saving, early sending
                val_duringTrade = 0x3FD5; //trade is split in several steps, sometimes even 0x00
            }
            else if (game == 1)
            {
                addr_PageSize = 0x329921A4;
                addr_PageEndStartRecord = 0x3299268C;
                addr_PageStartStartRecord = 0x32992690;
                addr_PageCurrentView = 0x305CD9F4;
                addr_PageStartingIndex = 0x32992190;
                addr_ListOfAllPageEntries = 0x329927C4;

                addr_box1slot1 = 0x33015AB0;

                addr_currentScreen = 0x006A610A;

                addr_pokemonToFind = 0x32992180;
                addr_pokemonToFindGender = 0x32992184;
                addr_pokemonToFindLevel = 0x32992188;

                val_PlazaScreen = 0x00;
                val_Quit_SeekScreen = 0x3F2B;
                val_SearchScreen = 0x412A;
                val_WhatPkmnScreen = 0x1040;
                val_GTSListScreen = 0x4180;
                val_BoxScreen = 0x4120;
                val_system = 0x1C848;
                val_duringTrade = 0x3FD5;
            }

        }

        public async Task<int> RunBot()
        {
            bool correctScreen = true;
            byte[] pokemonIndex = new byte[2];
            byte pokemonGender = 0x0;
            byte pokemonLevel = 0x0;
            byte[] full = BitConverter.GetBytes(iPokemonToFind);
            pokemonIndex[0] = full[0];
            pokemonIndex[1] = full[1];
            full = BitConverter.GetBytes(iPokemonToFindGender);
            pokemonGender = full[0];
            full = BitConverter.GetBytes(iPokemonToFindLevel);
            pokemonLevel = full[0];
            int panicAttempts = 0;
            while (!botstop)
            {
                if (botState != (int)gtsbotstates.panic)
                {
                    panicAttempts = 0;
                }
                switch (botState)
                {
                    case (int)gtsbotstates.botstart:
                        if (bReddit)
                            Program.f1.updateJSON();
                        botState = (int)gtsbotstates.startsearch;
                        break;
                    case (int)gtsbotstates.updatecomments:
                        Program.f1.updateJSON();
                        botState = (int)gtsbotstates.research;
                        break;
                    case (int)gtsbotstates.startsearch:
                        Program.f1.ChangeStatus("Setting Pokemon to find");
                        waitTaskbool = Program.helper.waitNTRwrite(addr_pokemonToFind, pokemonIndex, iPID);
                        waitTaskbool = Program.helper.waitNTRwrite(addr_pokemonToFindGender, pokemonGender, iPID);
                        waitTaskbool = Program.helper.waitNTRwrite(addr_pokemonToFindLevel, pokemonLevel, iPID);
                        botState = (int)gtsbotstates.pressSeek;
                        break;
                    case (int)gtsbotstates.pressSeek:
                        Program.f1.ChangeStatus("Pressing seek button");
                        //Seek/Deposite pokemon screen
                        correctScreen = await isCorrectWindow(val_Quit_SeekScreen);
                        if (!correctScreen)
                        {
                            botState = (int)gtsbotstates.panic;
                            break;
                        }
                        await Program.helper.waittouch(160, 80);
                        await Task.Delay(commandtime + delaytime);
                        botState = (int)gtsbotstates.presssearch;
                        break;
                    case (int)gtsbotstates.presssearch:
                        Program.f1.ChangeStatus("Press search button");
                        correctScreen = await isCorrectWindow(val_SearchScreen);
                        if (!correctScreen)
                        {
                            botState = (int)gtsbotstates.panic;
                            break;
                        }
                        //Pokemon wanted screen again, this time with filled out information
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
                        correctScreen = await isCorrectWindow(val_GTSListScreen);
                        if (!correctScreen)
                        {
                            botState = (int)gtsbotstates.panic;
                            break;
                        }
                        //GTS entry list screen, cursor at position 1
                        await Program.helper.waitNTRread(addr_PageSize);

                        attempts = 0;
                        listlength = (int)Program.helper.lastRead;

                        if (listlength == 100 && !foundLastPage && searchDirection == SEARCHDIRECTION_FROMBACK)
                        {
                            Program.f1.ChangeStatus("Moving to last page");
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
                                    //prevent potential loop by going left once more before the page is actually loaded
                                    Program.helper.quickbuton(Program.PKTable.DpadLEFT, commandtime);
                                    await Task.Delay(commandtime + delaytime);
                                    await Task.Delay(3000);
                                    Program.helper.quicktouch(10, 10, commandtime);
                                    await Task.Delay(commandtime + delaytime + 250);
                                    Program.helper.quicktouch(10, 10, commandtime);
                                    await Task.Delay(commandtime + delaytime + 250);
                                    Program.helper.quicktouch(10, 10, commandtime);
                                    await Task.Delay(commandtime + delaytime + 250);
                                    Program.helper.quicktouch(10, 10, commandtime);
                                    await Task.Delay(commandtime + delaytime + 250);
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
                            Program.f1.ChangeStatus("Looking for a pokemon to trade");
                            foundLastPage = true;
                            attempts = 0;
                            listlength = (int)Program.helper.lastRead;
                            dexnumber = 0;
                            if (searchDirection == SEARCHDIRECTION_FROMBACK || searchDirection == SEARCHDIRECTION_FROMBACKFIRSTPAGEONLY)
                            {
                                await Program.helper.waitNTRread(addr_PageEndStartRecord);
                            }
                            else
                            {
                                await Program.helper.waitNTRread(addr_PageStartStartRecord);
                            }
                            addr_PageEntry = Program.helper.lastRead;
                            await Program.helper.waitNTRread(addr_ListOfAllPageEntries, (uint)(256 * 100));
                            byte[] blockBytes = Program.helper.lastArray;
                            int iStartIndex, iEndIndex, iDirection, iNextPrevBlockOffest;
                            if (searchDirection == SEARCHDIRECTION_FROMBACK || searchDirection == SEARCHDIRECTION_FROMBACKFIRSTPAGEONLY)
                            {
                                iStartIndex = listlength;
                                iEndIndex = 0;
                                iDirection = -1;
                                iNextPrevBlockOffest = 0;
                            }
                            else
                            {
                                iStartIndex = 1;
                                iEndIndex = listlength + 1;
                                iDirection = 1;
                                iNextPrevBlockOffest = 4;
                            }
                            for (int i = iStartIndex; i * iDirection < iEndIndex; i += iDirection)
                            {
                                Array.Copy(blockBytes, addr_PageEntry - addr_ListOfAllPageEntries, block, 0, 256);
                                dexnumber = BitConverter.ToInt16(block, 0xC);
                                if (Program.f1.giveawayDetails.ContainsKey(dexnumber))
                                {

                                    Program.f1.giveawayDetails.TryGetValue(dexnumber, out details);
                                    if (details.Item1 == "")
                                    {
                                        string szNickname = Encoding.Unicode.GetString(block, 0x14, 24).Trim('\0'); //fix to prevent nickname clipping. Count should be 24, 2 bytes per letter, 2x12=24, not 20.
                                        string szFileToFind = details.Item2 + szNickname + ".pk7";
                                        if (!File.Exists(szFileToFind))
                                        {
                                            addr_PageEntry = BitConverter.ToUInt32(block, iNextPrevBlockOffest);
                                            continue;
                                        }
                                    }
                                    Array.Copy(block, 0x48, principal, 0, 4);
                                    byte checksum = Program.f1.calculateChecksum(principal);
                                    byte[] fc = new byte[8];
                                    Array.Copy(principal, 0, fc, 0, 4);
                                    fc[4] = checksum;
                                    long iFC = BitConverter.ToInt64(fc, 0);
                                    szFC = iFC.ToString().PadLeft(12, '0');

                                    int gender = block[0xE];
                                    int level = block[0xF];
                                    if ((gender == 0 || gender == details.Item3) && (level == 0 || level == details.Item4))
                                    {
                                        string szTrainerName = Encoding.Unicode.GetString(block, 0x4C, 24).Trim('\0');
                                        int countryIndex = BitConverter.ToInt16(block, 0x68);
                                        string country = "-";
                                        Program.f1.countries.TryGetValue(countryIndex, out country);
                                        Program.f1.getSubRegions(countryIndex);
                                        int subRegionIndex = BitConverter.ToInt16(block, 0x6A);
                                        string subregion = "-";
                                        Program.f1.regions.TryGetValue(subRegionIndex, out subregion);
                                        int ipage = Convert.ToInt32(Math.Floor(startIndex / 100.0)) + 1;
                                        if (useLedySync && !Program.f1.banlist.Contains(szFC) && canThisTrade(principal, consoleName, szTrainerName, country, subregion, Program.PKTable.Species7[dexnumber - 1], szFC, ipage + "", (i - 1) + ""))
                                        {
                                            Program.f1.ChangeStatus("Found a pokemon to trade");
                                            tradeIndex = i - 1;
                                            botState = (int)gtsbotstates.trade;
                                            break;
                                        }
                                        else if (!useLedySync)
                                        {
                                            if ((!bReddit || Program.f1.commented.Contains(szFC)) && !details.Item6.Contains(BitConverter.ToInt32(principal, 0)) && !Program.f1.banlist.Contains(szFC))
                                            {
                                                tradeIndex = i - 1;
                                                botState = (int)gtsbotstates.trade;
                                                break;
                                            }
                                        }
                                    }
                                }
                                addr_PageEntry = BitConverter.ToUInt32(block, iNextPrevBlockOffest);
                            }
                            if (tradeIndex == -1)
                            {
                                if (startIndex == 0)
                                {
                                    Program.f1.ChangeStatus("No pokemon to trade found");
                                    Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                    await Task.Delay(commandtime + delaytime + 500);
                                    Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                    await Task.Delay(commandtime + delaytime + 500);
                                    if (bReddit)
                                    {
                                        botState = (int)gtsbotstates.updatecomments;
                                    }
                                    else
                                    {
                                        botState = (int)gtsbotstates.research;
                                    }
                                }
                                else
                                {
                                    Program.f1.ChangeStatus("No pokemon to trade on this page, try previous page");
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
                        correctScreen = await isCorrectWindow(val_GTSListScreen);
                        if (!correctScreen)
                        {
                            botState = (int)gtsbotstates.panic;
                            break;
                        }
                        //also GTS entry list screen, but cursor is at the end of the list in this case
                        await Program.helper.waitNTRread(addr_PageSize);

                        attempts = 0;
                        listlength = (int)Program.helper.lastRead;
                        if (listlength == 100 && !foundLastPage)
                        {
                            Program.f1.ChangeStatus("Moving to last page");
                            startIndex += 100;
                            Program.helper.quickbuton(Program.PKTable.DpadRIGHT, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            await Task.Delay(3000);
                            Program.helper.quicktouch(10, 10, commandtime);
                            await Task.Delay(commandtime + delaytime + 250);
                            Program.helper.quicktouch(10, 10, commandtime);
                            await Task.Delay(commandtime + delaytime + 250);
                            Program.helper.quicktouch(10, 10, commandtime);
                            await Task.Delay(commandtime + delaytime + 250);
                            Program.helper.quicktouch(10, 10, commandtime);
                            await Task.Delay(commandtime + delaytime + 250);
                            await Program.helper.waitNTRread(addr_PageStartingIndex);
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
                            dexnumber = 0;
                            await Program.helper.waitNTRread(addr_PageEndStartRecord);
                            addr_PageEntry = Program.helper.lastRead;
                            await Program.helper.waitNTRread(addr_ListOfAllPageEntries, (uint)(256 * 100));
                            byte[] blockBytes = Program.helper.lastArray;
                            for (int i = listlength; i > 0; i--)
                            {
                                Program.f1.ChangeStatus("Looking for a pokemon to trade");
                                Array.Copy(blockBytes, addr_PageEntry - addr_ListOfAllPageEntries, block, 0, 256);
                                dexnumber = BitConverter.ToInt16(block, 0xC);
                                if (Program.f1.giveawayDetails.ContainsKey(dexnumber))
                                {
                                    Program.f1.giveawayDetails.TryGetValue(dexnumber, out details);
                                    if (details.Item1 == "")
                                    {
                                        string szNickname = Encoding.Unicode.GetString(block, 0x14, 24).Trim('\0'); //fix to prevent nickname clipping. Count should be 24, 2 bytes per letter, 2x12=24, not 20.
                                        string szFileToFind = details.Item2 + szNickname + ".pk7";
                                        if (!File.Exists(szFileToFind))
                                        {
                                            addr_PageEntry = BitConverter.ToUInt32(block, 0);
                                            continue;
                                        }
                                    }
                                    Array.Copy(block, 0x48, principal, 0, 4);
                                    byte checksum = Program.f1.calculateChecksum(principal);
                                    byte[] fc = new byte[8];
                                    Array.Copy(principal, 0, fc, 0, 4);
                                    fc[4] = checksum;
                                    long iFC = BitConverter.ToInt64(fc, 0);
                                    szFC = iFC.ToString().PadLeft(12, '0');
                                    int gender = block[0xE];
                                    int level = block[0xF];
                                    if ((gender == 0 || gender == details.Item3) && (level == 0 || level == details.Item4))
                                    {
                                        string szTrainerName = Encoding.Unicode.GetString(block, 0x4C, 24).Trim('\0');
                                        int countryIndex = BitConverter.ToInt16(block, 0x68);
                                        string country = "-";
                                        Program.f1.countries.TryGetValue(countryIndex, out country);
                                        Program.f1.getSubRegions(countryIndex);
                                        int subRegionIndex = BitConverter.ToInt16(block, 0x6A);
                                        string subregion = "-";
                                        Program.f1.regions.TryGetValue(subRegionIndex, out subregion);
                                        int ipage = Convert.ToInt32(Math.Floor(startIndex / 100.0)) + 1;
                                        if (useLedySync && !Program.f1.banlist.Contains(szFC) && canThisTrade(principal, consoleName, szTrainerName, country, subregion, Program.PKTable.Species7[dexnumber - 1], szFC, ipage + "", (i - 1) + ""))
                                        {
                                            Program.f1.ChangeStatus("Found a pokemon to trade");
                                            tradeIndex = i - 1;
                                            botState = (int)gtsbotstates.trade;
                                            break;
                                        }
                                        else if (!useLedySync)
                                        {
                                            if ((!bReddit || Program.f1.commented.Contains(szFC)) && !details.Item6.Contains(BitConverter.ToInt32(principal, 0)) && !Program.f1.banlist.Contains(szFC))
                                            {
                                                tradeIndex = i - 1;
                                                botState = (int)gtsbotstates.trade;
                                                break;
                                            }
                                        }
                                    }
                                }
                                addr_PageEntry = BitConverter.ToUInt32(block, 0);

                            }
                            if (tradeIndex == -1)
                            {
                                if (listlength < 100 && startIndex >= 200)
                                {
                                    Program.f1.ChangeStatus("No pokemon to trade on this page, try previous page");
                                    for (int i = 0; i < listlength; i++)
                                    {
                                        Program.helper.quickbuton(Program.PKTable.DpadLEFT, commandtime);
                                        await Task.Delay(commandtime + delaytime);
                                    }
                                    startIndex -= 100;
                                    await Task.Delay(2250);
                                    botState = (int)gtsbotstates.findfromend; //hope this is right
                                }
                                else if (startIndex >= 200)
                                {
                                    Program.f1.ChangeStatus("No pokemon to trade on this page, try previous page");
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
                                        await Task.Delay(3000);
                                        Program.helper.quicktouch(10, 10, commandtime);
                                        await Task.Delay(commandtime + delaytime + 250);
                                        Program.helper.quicktouch(10, 10, commandtime);
                                        await Task.Delay(commandtime + delaytime + 250);
                                        Program.helper.quicktouch(10, 10, commandtime);
                                        await Task.Delay(commandtime + delaytime + 250);
                                        Program.helper.quicktouch(10, 10, commandtime);
                                        await Task.Delay(commandtime + delaytime + 250);
                                        botState = (int)gtsbotstates.findfromstart;
                                    }
                                }
                                else if (startIndex == 0)
                                {
                                    Program.f1.ChangeStatus("No pokemon to trade found");
                                    Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                    await Task.Delay(commandtime + delaytime + 500);
                                    Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                    await Task.Delay(commandtime + delaytime + 500);
                                    if (bReddit)
                                    {
                                        botState = (int)gtsbotstates.updatecomments;
                                    }
                                    else
                                    {
                                        botState = (int)gtsbotstates.research;
                                    }
                                }
                                else if (startIndex < 200)
                                {
                                    Program.f1.ChangeStatus("No pokemon to trade found");
                                    botState = (int)gtsbotstates.quicksearch;
                                }
                            }
                        }

                        break;
                    case (int)gtsbotstates.trade:
                        //still in GTS list screen
                        //write index we want to trade
                        int page = Convert.ToInt32(Math.Floor(startIndex / 100.0)) + 1;
                        Program.f1.ChangeStatus("Trading pokemon on page " + page + " index " + tradeIndex + "");

                        waitTaskbool = Program.helper.waitNTRwrite(addr_PageCurrentView, BitConverter.GetBytes(tradeIndex), iPID);
                        if (await waitTaskbool)
                        {
                            string szNickname = Encoding.Unicode.GetString(block, 0x14, 24).Trim('\0'); //fix to prevent nickname clipping. Count should be 24, 2 bytes per letter, 2x12=24, not 20.


                            string szPath = details.Item1;
                            string szFileToFind = details.Item2 + szNickname + ".pk7";
                            if (File.Exists(szFileToFind))
                            {
                                szPath = szFileToFind;
                            }

                            byte[] pkmEncrypted = System.IO.File.ReadAllBytes(szPath);
                            byte[] cloneshort = PKHeX.encryptArray(pkmEncrypted.Take(232).ToArray());
                            string ek7 = BitConverter.ToString(cloneshort).Replace("-", ", 0x");

                            //optional: grab some trainer data
                            string szTrainerName = Encoding.Unicode.GetString(block, 0x4C, 24).Trim('\0');
                            int countryIndex = BitConverter.ToInt16(block, 0x68);
                            string country = "-";
                            Program.f1.countries.TryGetValue(countryIndex, out country);
                            Program.f1.getSubRegions(countryIndex);
                            int subRegionIndex = BitConverter.ToInt16(block, 0x6A);
                            string subregion = "-";
                            Program.f1.regions.TryGetValue(subRegionIndex, out subregion);
                            if (bBlacklist)
                            {
                                details.Item6.Add(BitConverter.ToInt32(principal, 0));
                            }
                            Program.f1.AppendListViewItem(szTrainerName, szNickname, country, subregion, Program.PKTable.Species7[dexnumber - 1], szFC, page + "", tradeIndex + "");
                            if (useLedybotTV)
                            {
                                didTradeTV(principal, consoleName, szTrainerName, country, subregion, Program.PKTable.Species7[dexnumber - 1], szFC, page + "", tradeIndex + "");
                            }
                            //Inject the Pokemon to box1slot1
                            Program.scriptHelper.write(addr_box1slot1, cloneshort, iPID);
                            //spam a to trade pokemon
                            Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                            await Task.Delay(commandtime + delaytime + 2500 + o3dswaittime);
                            Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            if (details.Item5 > 0)
                            {
                                Program.f1.giveawayDetails[dexnumber] = new Tuple<string, string, int, int, int, ArrayList>(details.Item1, details.Item2, details.Item3, details.Item4, details.Item5 - 1, details.Item6);
                                foreach (System.Data.DataRow row in Program.gd.details.Rows)
                                {
                                    if (row[0].ToString() == dexnumber.ToString())
                                    {
                                        int count = int.Parse(row[5].ToString()) - 1;
                                        row[5] = count;
                                        break;
                                    }
                                }
                            }

                            foreach (System.Data.DataRow row in Program.gd.details.Rows)
                            {
                                if (row[0].ToString() == dexnumber.ToString())
                                {
                                    int amount = int.Parse(row[6].ToString()) + 1;
                                    row[6] = amount;
                                    break;
                                }
                            }
                            //during the trade spam a/b to get back to the start screen in case of "this pokemon has been traded"
                            await Task.Delay(10250);
                            Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            await Task.Delay(1000);
                            Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            await Task.Delay(1000);
                            Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            await Task.Delay(1000);
                            Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                            await Task.Delay(commandtime + delaytime);
                            await Task.Delay(32000);
                            bool cont = false;
                            foreach (KeyValuePair<int, Tuple<string, string, int, int, int, ArrayList>> pair in Program.f1.giveawayDetails)
                            {
                                if (pair.Value.Item5 != 0)
                                {
                                    cont = true;
                                    break;
                                }
                            }
                            if (!cont)
                            {
                                botresult = 1;
                                botState = (int)gtsbotstates.botexit;
                                break;
                            }

                            startIndex = 0;
                            tradeIndex = -1;
                            listlength = 0;
                            addr_PageEntry = 0;
                            foundLastPage = false;

                            if (bReddit)
                            {
                                botState = (int)gtsbotstates.updatecomments;
                            }
                            else
                            {
                                botState = (int)gtsbotstates.research;
                            }
                        }
                        break;
                    case (int)gtsbotstates.quicksearch:
                        //end of list reach, press b and "search" again to reach GTS list again
                        Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                        await Task.Delay(commandtime + delaytime + 500);
                        await Program.helper.waittouch(160, 185);
                        await Task.Delay(2250);
                        botState = (int)gtsbotstates.findfromstart;
                        break;
                    case (int)gtsbotstates.research:
                        //press a and "search" again to reach GTS list again
                        Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                        await Task.Delay(commandtime + delaytime + 1000);
                        await Program.helper.waittouch(160, 185);
                        await Task.Delay(2250);
                        botState = (int)gtsbotstates.findfromstart;
                        break;
                    case (int)gtsbotstates.botexit:
                        Program.f1.ChangeStatus("Stopped");
                        botstop = true;
                        break;
                    case (int)gtsbotstates.panic:
                        Program.f1.ChangeStatus("Recovery mode!");
                        //recover from weird state here
                        await Program.helper.waitNTRread(addr_currentScreen);
                        int screenID = (int)Program.helper.lastRead;

                        if (screenID == val_PlazaScreen)
                        {
                            await Program.helper.waittouch(200, 120);
                            await Task.Delay(1000);
                            await Program.helper.waittouch(200, 120);
                            await Task.Delay(8000);
                            correctScreen = await isCorrectWindow(val_Quit_SeekScreen);
                            if (correctScreen)
                            {
                                botState = (int)gtsbotstates.startsearch;
                                break;
                            }
                            else
                            {
                                botState = (int)gtsbotstates.botexit;
                                break;
                            }
                        }
                        else if (screenID == val_Quit_SeekScreen)
                        {
                            //press b, press where seek button would be, press b again -> guaranteed seek screen
                            Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                            await Task.Delay(commandtime + delaytime + 500);
                            await Program.helper.waittouch(160, 80);
                            await Task.Delay(2250);
                            Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                            await Task.Delay(commandtime + delaytime + 500);
                            botState = (int)gtsbotstates.startsearch;
                            break;
                        }
                        else if (screenID == val_WhatPkmnScreen)
                        {
                            //can only exit this one by pressing the ok button
                            waitTaskbool = Program.helper.waitbutton(Program.PKTable.keySTART);
                            if (await waitTaskbool)
                            {
                                waitTaskbool = Program.helper.waitbutton(Program.PKTable.keyA);
                                if (await waitTaskbool)
                                {
                                    botState = (int)gtsbotstates.panic;
                                    break;
                                }
                            }
                        }
                        else // if(screenID == val_SearchScreen || screenID == val_BoxScreen || screenID == val_GTSListScreen)
                        {
                            //spam b a lot and hope we get to val_quit_seekscreen like this
                            for (int i = 0; i < 5; i++)
                            {
                                Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                await Task.Delay(commandtime + delaytime + 500);
                                await Task.Delay(1000);
                            }
                            correctScreen = await isCorrectWindow(val_Quit_SeekScreen);
                            if (correctScreen)
                            {
                                botState = (int)gtsbotstates.panic;
                                break;
                            }
                            else
                            {
                                Program.helper.quickbuton(Program.PKTable.keyA, commandtime);
                                await Task.Delay(commandtime + delaytime + 500);
                                for (int i = 0; i < 5; i++)
                                {
                                    Program.helper.quickbuton(Program.PKTable.keyB, commandtime);
                                    await Task.Delay(commandtime + delaytime + 500);
                                    await Task.Delay(1000);
                                }
                                correctScreen = await isCorrectWindow(val_Quit_SeekScreen);
                                if (correctScreen)
                                {
                                    botState = (int)gtsbotstates.panic;
                                    break;
                                }
                                else
                                {
                                    if (panicAttempts == 0)
                                    {
                                        panicAttempts++;
                                        botState = (int)gtsbotstates.panic;
                                        break;
                                    }
                                    botState = (int)gtsbotstates.botexit;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        botresult = -1;
                        botstop = true;
                        break;
                }
            }
            if (this.serverEndPointSync != null)
            {
                syncClient.Close();
            }
            if(this.serverEndPointTV != null)
            {
                tvClient.Close();
            }
            return botresult;
        }

        public void RequestStop()
        {
            botstop = true;
        }


    }
}
