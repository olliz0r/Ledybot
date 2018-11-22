/// I do not own the code in this class. 
/// All rights and credits for the code in this class belong to Kaphotics.
/// All code within this class is taken from PKHeX https://github.com/kwsch/PKHeX
/// with minor modifications

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedyLib
{
    public class PKHeX
    {
        public bool Gen7 => Version >= 30 && Version <= 31;
        public bool Gen6 => Version >= 24 && Version <= 29;
        public bool Gen5 => Version >= 20 && Version <= 23;
        public bool Gen4 => Version >= 7 && Version <= 12 && Version != 9;
        public bool Gen3 => Version >= 1 && Version <= 5 || Version == 15;

        public int GenNumber
        {
            get
            {
                if (Gen7) return 7;
                if (Gen6) return 6;
                if (Gen5) return 5;
                if (Gen4) return 4;
                if (Gen3) return 3;
                return -1;
            }
        }

        public static uint LCRNG(uint seed)
        {
            const uint a = 0x41C64E6D;
            const uint c = 0x00006073;

            return seed * a + c;
        }

        public static uint LCRNG(ref uint seed)
        {
            const uint a = 0x41C64E6D;
            const uint c = 0x00006073;

            return seed = seed * a + c;
        }

        public static readonly byte[][] blockPosition =
{
            new byte[] {0, 0, 0, 0, 0, 0, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3},
            new byte[] {1, 1, 2, 3, 2, 3, 0, 0, 0, 0, 0, 0, 2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2},
            new byte[] {2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2, 0, 0, 0, 0, 0, 0, 3, 2, 3, 2, 1, 1},
            new byte[] {3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 0, 0, 0, 0, 0, 0},
        };

        public static readonly byte[] blockPositionInvert =
        {
            0, 1, 2, 4, 3, 5, 6, 7, 12, 18, 13, 19, 8, 10, 14, 20, 16, 22, 9, 11, 15, 21, 17, 23
        };

        public static byte[] shuffleArray(byte[] data, uint sv)
        {
            byte[] sdata = new byte[data.Length];
            Array.Copy(data, sdata, 8); // Copy unshuffled bytes

            // Shuffle Away!
            for (int block = 0; block < 4; block++)
                Array.Copy(data, 8 + 56 * blockPosition[block][sv], sdata, 8 + 56 * block, 56);

            // Fill the Battle Stats back
            if (data.Length > 232)
                Array.Copy(data, 232, sdata, 232, 28);

            return sdata;
        }

        public static byte[] decryptArray(byte[] ekx)
        {
            byte[] pkx = (byte[])ekx.Clone();

            uint pv = BitConverter.ToUInt32(pkx, 0);
            uint sv = (pv >> 0xD & 0x1F) % 24;

            uint seed = pv;

            // Decrypt Blocks with RNG Seed
            for (int i = 8; i < 232; i += 2)
                BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(pkx, i) ^ LCRNG(ref seed) >> 16)).CopyTo(pkx, i);

            // Deshuffle
            pkx = shuffleArray(pkx, sv);

            // Decrypt the Party Stats
            seed = pv;
            if (pkx.Length <= 232) return pkx;
            for (int i = 232; i < 260; i += 2)
                BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(pkx, i) ^ LCRNG(ref seed) >> 16)).CopyTo(pkx, i);

            return pkx;
        }
        public static byte[] encryptArray(byte[] pkx)
        {
            // Shuffle
            uint pv = BitConverter.ToUInt32(pkx, 0);
            uint sv = (pv >> 0xD & 0x1F) % 24;

            byte[] ekx = (byte[])pkx.Clone();

            ekx = shuffleArray(ekx, blockPositionInvert[sv]);

            uint seed = pv;

            // Encrypt Blocks with RNG Seed
            for (int i = 8; i < 232; i += 2)
                BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(ekx, i) ^ LCRNG(ref seed) >> 16)).CopyTo(ekx, i);

            // If no party stats, return.
            if (ekx.Length <= 232) return ekx;

            // Encrypt the Party Stats
            seed = pv;
            for (int i = 232; i < 260; i += 2)
                BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(ekx, i) ^ LCRNG(ref seed) >> 16)).CopyTo(ekx, i);

            // Done
            return ekx;
        }

        public static ushort getCHK(byte[] data)
        {
            ushort chk = 0;
            for (int i = 8; i < 232; i += 2) // Loop through the entire PKX
                chk += BitConverter.ToUInt16(data, i);

            return chk;
        }

        public static readonly int[,] hpivs =
        {
            { 1, 1, 0, 0, 0, 0 }, // Fighting
            { 0, 0, 0, 0, 0, 1 }, // Flying
            { 1, 1, 0, 0, 0, 1 }, // Poison
            { 1, 1, 1, 0, 0, 1 }, // Ground
            { 1, 1, 0, 1, 0, 0 }, // Rock
            { 1, 0, 0, 1, 0, 1 }, // Bug
            { 1, 0, 1, 1, 0, 1 }, // Ghost
            { 1, 1, 1, 1, 0, 1 }, // Steel
            { 1, 0, 1, 0, 1, 0 }, // Fire
            { 1, 0, 0, 0, 1, 1 }, // Water
            { 1, 0, 1, 0, 1, 1 }, // Grass
            { 1, 1, 1, 0, 1, 1 }, // Electric
            { 1, 0, 1, 1, 1, 0 }, // Psychic
            { 1, 0, 0, 1, 1, 1 }, // Ice
            { 1, 0, 1, 1, 1, 1 }, // Dragon
            { 1, 1, 1, 1, 1, 1 }, // Dark
        };

        public byte[] Data { get; set; }

        ////////////////////////////
        ///Begin PKHeX PK6 Layout///
        ////////////////////////////
        #region Block A
        public uint EncryptionConstant
        {
            get { return BitConverter.ToUInt32(Data, 0x00); }
            set { BitConverter.GetBytes(value).CopyTo(Data, 0x00); }
        }
        public ushort Sanity
        {
            get { return BitConverter.ToUInt16(Data, 0x04); }
            set { BitConverter.GetBytes(value).CopyTo(Data, 0x04); } // Should always be zero...
        }
        public ushort Checksum
        {
            get { return BitConverter.ToUInt16(Data, 0x06); }
            set { BitConverter.GetBytes(value).CopyTo(Data, 0x06); }
        }
        public int Species
        {
            get { return BitConverter.ToUInt16(Data, 0x08); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08); }
        }
        public int HeldItem
        {
            get { return BitConverter.ToUInt16(Data, 0x0A); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0A); }
        }
        public int TID
        {
            get { return BitConverter.ToUInt16(Data, 0x0C); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0C); }
        }
        public int SID
        {
            get { return BitConverter.ToUInt16(Data, 0x0E); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0E); }
        }
        public uint EXP
        {
            get { return BitConverter.ToUInt32(Data, 0x10); }
            set { BitConverter.GetBytes(value).CopyTo(Data, 0x10); }
        }
        public int Ability { get { return Data[0x14]; } set { Data[0x14] = (byte)value; } }
        public int AbilityNumber { get { return Data[0x15]; } set { Data[0x15] = (byte)value; } }
        public int TrainingBagHits { get { return Data[0x16]; } set { Data[0x16] = (byte)value; } }
        public int TrainingBag { get { return Data[0x17]; } set { Data[0x17] = (byte)value; } }
        public uint PID
        {
            get { return BitConverter.ToUInt32(Data, 0x18); }
            set { BitConverter.GetBytes(value).CopyTo(Data, 0x18); }
        }
        public int Nature { get { return Data[0x1C]; } set { Data[0x1C] = (byte)value; } }
        public bool FatefulEncounter { get { return (Data[0x1D] & 1) == 1; } set { Data[0x1D] = (byte)(Data[0x1D] & ~0x01 | (value ? 1 : 0)); } }
        public int Gender { get { return (Data[0x1D] >> 1) & 0x3; } set { Data[0x1D] = (byte)(Data[0x1D] & ~0x06 | (value << 1)); } }
        public int AltForm { get { return Data[0x1D] >> 3; } set { Data[0x1D] = (byte)(Data[0x1D] & 0x07 | (value << 3)); } }
        public int EV_HP { get { return Data[0x1E]; } set { Data[0x1E] = (byte)value; } }
        public int EV_ATK { get { return Data[0x1F]; } set { Data[0x1F] = (byte)value; } }
        public int EV_DEF { get { return Data[0x20]; } set { Data[0x20] = (byte)value; } }
        public int EV_SPE { get { return Data[0x21]; } set { Data[0x21] = (byte)value; } }
        public int EV_SPA { get { return Data[0x22]; } set { Data[0x22] = (byte)value; } }
        public int EV_SPD { get { return Data[0x23]; } set { Data[0x23] = (byte)value; } }
        public int CNT_Cool { get { return Data[0x24]; } set { Data[0x24] = (byte)value; } }
        public int CNT_Beauty { get { return Data[0x25]; } set { Data[0x25] = (byte)value; } }
        public int CNT_Cute { get { return Data[0x26]; } set { Data[0x26] = (byte)value; } }
        public int CNT_Smart { get { return Data[0x27]; } set { Data[0x27] = (byte)value; } }
        public int CNT_Tough { get { return Data[0x28]; } set { Data[0x28] = (byte)value; } }
        public int CNT_Sheen { get { return Data[0x29]; } set { Data[0x29] = (byte)value; } }
        public byte MarkByte { get { return Data[0x2A]; } protected set { Data[0x2A] = value; } }
        private byte PKRS { get { return Data[0x2B]; } set { Data[0x2B] = value; } }
        public int PKRS_Days { get { return PKRS & 0xF; } set { PKRS = (byte)(PKRS & ~0xF | value); } }
        public int PKRS_Strain { get { return PKRS >> 4; } set { PKRS = (byte)(PKRS & 0xF | value << 4); } }
        private byte ST1 { get { return Data[0x2C]; } set { Data[0x2C] = value; } }
        public bool Unused0 { get { return (ST1 & (1 << 0)) == 1 << 0; } set { ST1 = (byte)(ST1 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool Unused1 { get { return (ST1 & (1 << 1)) == 1 << 1; } set { ST1 = (byte)(ST1 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool SuperTrain1_SPA { get { return (ST1 & (1 << 2)) == 1 << 2; } set { ST1 = (byte)(ST1 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool SuperTrain1_HP { get { return (ST1 & (1 << 3)) == 1 << 3; } set { ST1 = (byte)(ST1 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool SuperTrain1_ATK { get { return (ST1 & (1 << 4)) == 1 << 4; } set { ST1 = (byte)(ST1 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool SuperTrain1_SPD { get { return (ST1 & (1 << 5)) == 1 << 5; } set { ST1 = (byte)(ST1 & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool SuperTrain1_SPE { get { return (ST1 & (1 << 6)) == 1 << 6; } set { ST1 = (byte)(ST1 & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
        public bool SuperTrain1_DEF { get { return (ST1 & (1 << 7)) == 1 << 7; } set { ST1 = (byte)(ST1 & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        private byte ST2 { get { return Data[0x2D]; } set { Data[0x2D] = value; } }
        public bool SuperTrain2_SPA { get { return (ST2 & (1 << 0)) == 1 << 0; } set { ST2 = (byte)(ST2 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool SuperTrain2_HP { get { return (ST2 & (1 << 1)) == 1 << 1; } set { ST2 = (byte)(ST2 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool SuperTrain2_ATK { get { return (ST2 & (1 << 2)) == 1 << 2; } set { ST2 = (byte)(ST2 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool SuperTrain2_SPD { get { return (ST2 & (1 << 3)) == 1 << 3; } set { ST2 = (byte)(ST2 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool SuperTrain2_SPE { get { return (ST2 & (1 << 4)) == 1 << 4; } set { ST2 = (byte)(ST2 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool SuperTrain2_DEF { get { return (ST2 & (1 << 5)) == 1 << 5; } set { ST2 = (byte)(ST2 & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool SuperTrain3_SPA { get { return (ST2 & (1 << 6)) == 1 << 6; } set { ST2 = (byte)(ST2 & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
        public bool SuperTrain3_HP { get { return (ST2 & (1 << 7)) == 1 << 7; } set { ST2 = (byte)(ST2 & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        private byte ST3 { get { return Data[0x2E]; } set { Data[0x2E] = value; } }
        public bool SuperTrain3_ATK { get { return (ST3 & (1 << 0)) == 1 << 0; } set { ST3 = (byte)(ST3 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool SuperTrain3_SPD { get { return (ST3 & (1 << 1)) == 1 << 1; } set { ST3 = (byte)(ST3 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool SuperTrain3_SPE { get { return (ST3 & (1 << 2)) == 1 << 2; } set { ST3 = (byte)(ST3 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool SuperTrain3_DEF { get { return (ST3 & (1 << 3)) == 1 << 3; } set { ST3 = (byte)(ST3 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool SuperTrain4_1 { get { return (ST3 & (1 << 4)) == 1 << 4; } set { ST3 = (byte)(ST3 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool SuperTrain5_1 { get { return (ST3 & (1 << 5)) == 1 << 5; } set { ST3 = (byte)(ST3 & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool SuperTrain5_2 { get { return (ST3 & (1 << 6)) == 1 << 6; } set { ST3 = (byte)(ST3 & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
        public bool SuperTrain5_3 { get { return (ST3 & (1 << 7)) == 1 << 7; } set { ST3 = (byte)(ST3 & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        private byte ST4 { get { return Data[0x2F]; } set { Data[0x2F] = value; } }
        public bool SuperTrain5_4 { get { return (ST4 & (1 << 0)) == 1 << 0; } set { ST4 = (byte)(ST4 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool SuperTrain6_1 { get { return (ST4 & (1 << 1)) == 1 << 1; } set { ST4 = (byte)(ST4 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool SuperTrain6_2 { get { return (ST4 & (1 << 2)) == 1 << 2; } set { ST4 = (byte)(ST4 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool SuperTrain6_3 { get { return (ST4 & (1 << 3)) == 1 << 3; } set { ST4 = (byte)(ST4 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool SuperTrain7_1 { get { return (ST4 & (1 << 4)) == 1 << 4; } set { ST4 = (byte)(ST4 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool SuperTrain7_2 { get { return (ST4 & (1 << 5)) == 1 << 5; } set { ST4 = (byte)(ST4 & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool SuperTrain7_3 { get { return (ST4 & (1 << 6)) == 1 << 6; } set { ST4 = (byte)(ST4 & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
        public bool SuperTrain8_1 { get { return (ST4 & (1 << 7)) == 1 << 7; } set { ST4 = (byte)(ST4 & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        private byte RIB0 { get { return Data[0x30]; } set { Data[0x30] = value; } }
        private byte RIB1 { get { return Data[0x31]; } set { Data[0x31] = value; } }
        private byte RIB2 { get { return Data[0x32]; } set { Data[0x32] = value; } }
        private byte RIB3 { get { return Data[0x33]; } set { Data[0x33] = value; } }
        private byte RIB4 { get { return Data[0x34]; } set { Data[0x34] = value; } }
        private byte RIB5 { get { return Data[0x35]; } set { Data[0x35] = value; } }
        public bool RibbonChampionKalos { get { return (RIB0 & (1 << 0)) == 1 << 0; } set { RIB0 = (byte)(RIB0 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool RibbonChampionG3Hoenn { get { return (RIB0 & (1 << 1)) == 1 << 1; } set { RIB0 = (byte)(RIB0 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool RibbonChampionSinnoh { get { return (RIB0 & (1 << 2)) == 1 << 2; } set { RIB0 = (byte)(RIB0 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool RibbonBestFriends { get { return (RIB0 & (1 << 3)) == 1 << 3; } set { RIB0 = (byte)(RIB0 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool RibbonTraining { get { return (RIB0 & (1 << 4)) == 1 << 4; } set { RIB0 = (byte)(RIB0 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool RibbonBattlerSkillful { get { return (RIB0 & (1 << 5)) == 1 << 5; } set { RIB0 = (byte)(RIB0 & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool RibbonBattlerExpert { get { return (RIB0 & (1 << 6)) == 1 << 6; } set { RIB0 = (byte)(RIB0 & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
        public bool RibbonEffort { get { return (RIB0 & (1 << 7)) == 1 << 7; } set { RIB0 = (byte)(RIB0 & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        public bool RibbonAlert { get { return (RIB1 & (1 << 0)) == 1 << 0; } set { RIB1 = (byte)(RIB1 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool RibbonShock { get { return (RIB1 & (1 << 1)) == 1 << 1; } set { RIB1 = (byte)(RIB1 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool RibbonDowncast { get { return (RIB1 & (1 << 2)) == 1 << 2; } set { RIB1 = (byte)(RIB1 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool RibbonCareless { get { return (RIB1 & (1 << 3)) == 1 << 3; } set { RIB1 = (byte)(RIB1 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool RibbonRelax { get { return (RIB1 & (1 << 4)) == 1 << 4; } set { RIB1 = (byte)(RIB1 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool RibbonSnooze { get { return (RIB1 & (1 << 5)) == 1 << 5; } set { RIB1 = (byte)(RIB1 & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool RibbonSmile { get { return (RIB1 & (1 << 6)) == 1 << 6; } set { RIB1 = (byte)(RIB1 & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
        public bool RibbonGorgeous { get { return (RIB1 & (1 << 7)) == 1 << 7; } set { RIB1 = (byte)(RIB1 & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        public bool RibbonRoyal { get { return (RIB2 & (1 << 0)) == 1 << 0; } set { RIB2 = (byte)(RIB2 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool RibbonGorgeousRoyal { get { return (RIB2 & (1 << 1)) == 1 << 1; } set { RIB2 = (byte)(RIB2 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool RibbonArtist { get { return (RIB2 & (1 << 2)) == 1 << 2; } set { RIB2 = (byte)(RIB2 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool RibbonFootprint { get { return (RIB2 & (1 << 3)) == 1 << 3; } set { RIB2 = (byte)(RIB2 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool RibbonRecord { get { return (RIB2 & (1 << 4)) == 1 << 4; } set { RIB2 = (byte)(RIB2 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool RibbonLegend { get { return (RIB2 & (1 << 5)) == 1 << 5; } set { RIB2 = (byte)(RIB2 & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool RibbonCountry { get { return (RIB2 & (1 << 6)) == 1 << 6; } set { RIB2 = (byte)(RIB2 & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
        public bool RibbonNational { get { return (RIB2 & (1 << 7)) == 1 << 7; } set { RIB2 = (byte)(RIB2 & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        public bool RibbonEarth { get { return (RIB3 & (1 << 0)) == 1 << 0; } set { RIB3 = (byte)(RIB3 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool RibbonWorld { get { return (RIB3 & (1 << 1)) == 1 << 1; } set { RIB3 = (byte)(RIB3 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool RibbonClassic { get { return (RIB3 & (1 << 2)) == 1 << 2; } set { RIB3 = (byte)(RIB3 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool RibbonPremier { get { return (RIB3 & (1 << 3)) == 1 << 3; } set { RIB3 = (byte)(RIB3 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool RibbonEvent { get { return (RIB3 & (1 << 4)) == 1 << 4; } set { RIB3 = (byte)(RIB3 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool RibbonBirthday { get { return (RIB3 & (1 << 5)) == 1 << 5; } set { RIB3 = (byte)(RIB3 & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool RibbonSpecial { get { return (RIB3 & (1 << 6)) == 1 << 6; } set { RIB3 = (byte)(RIB3 & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
        public bool RibbonSouvenir { get { return (RIB3 & (1 << 7)) == 1 << 7; } set { RIB3 = (byte)(RIB3 & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        public bool RibbonWishing { get { return (RIB4 & (1 << 0)) == 1 << 0; } set { RIB4 = (byte)(RIB4 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool RibbonChampionBattle { get { return (RIB4 & (1 << 1)) == 1 << 1; } set { RIB4 = (byte)(RIB4 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool RibbonChampionRegional { get { return (RIB4 & (1 << 2)) == 1 << 2; } set { RIB4 = (byte)(RIB4 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool RibbonChampionNational { get { return (RIB4 & (1 << 3)) == 1 << 3; } set { RIB4 = (byte)(RIB4 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool RibbonChampionWorld { get { return (RIB4 & (1 << 4)) == 1 << 4; } set { RIB4 = (byte)(RIB4 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool RIB4_5 { get { return (RIB4 & (1 << 5)) == 1 << 5; } set { RIB4 = (byte)(RIB4 & ~(1 << 5) | (value ? 1 << 5 : 0)); } } // Unused
        public bool RIB4_6 { get { return (RIB4 & (1 << 6)) == 1 << 6; } set { RIB4 = (byte)(RIB4 & ~(1 << 6) | (value ? 1 << 6 : 0)); } } // Unused
        public bool RibbonChampionG6Hoenn { get { return (RIB4 & (1 << 7)) == 1 << 7; } set { RIB4 = (byte)(RIB4 & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        public bool RibbonContestStar { get { return (RIB5 & (1 << 0)) == 1 << 0; } set { RIB5 = (byte)(RIB5 & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool RibbonMasterCoolness { get { return (RIB5 & (1 << 1)) == 1 << 1; } set { RIB5 = (byte)(RIB5 & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool RibbonMasterBeauty { get { return (RIB5 & (1 << 2)) == 1 << 2; } set { RIB5 = (byte)(RIB5 & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool RibbonMasterCuteness { get { return (RIB5 & (1 << 3)) == 1 << 3; } set { RIB5 = (byte)(RIB5 & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool RibbonMasterCleverness { get { return (RIB5 & (1 << 4)) == 1 << 4; } set { RIB5 = (byte)(RIB5 & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool RibbonMasterToughness { get { return (RIB5 & (1 << 5)) == 1 << 5; } set { RIB5 = (byte)(RIB5 & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool RIB5_6 { get { return (RIB5 & (1 << 6)) == 1 << 6; } set { RIB5 = (byte)(RIB5 & ~(1 << 6) | (value ? 1 << 6 : 0)); } } // Unused
        public bool RIB5_7 { get { return (RIB5 & (1 << 7)) == 1 << 7; } set { RIB5 = (byte)(RIB5 & ~(1 << 7) | (value ? 1 << 7 : 0)); } } // Unused
        public byte _0x36 { get { return Data[0x36]; } set { Data[0x36] = value; } }
        public byte _0x37 { get { return Data[0x37]; } set { Data[0x37] = value; } }
        public int RibbonCountMemoryContest { get { return Data[0x38]; } set { Data[0x38] = (byte)value; } }
        public int RibbonCountMemoryBattle { get { return Data[0x39]; } set { Data[0x39] = (byte)value; } }
        private byte DistByte { get { return Data[0x3A]; } set { Data[0x3A] = value; } }
        public bool DistSuperTrain1 { get { return (DistByte & (1 << 0)) == 1 << 0; } set { DistByte = (byte)(DistByte & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
        public bool DistSuperTrain2 { get { return (DistByte & (1 << 1)) == 1 << 1; } set { DistByte = (byte)(DistByte & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
        public bool DistSuperTrain3 { get { return (DistByte & (1 << 2)) == 1 << 2; } set { DistByte = (byte)(DistByte & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
        public bool DistSuperTrain4 { get { return (DistByte & (1 << 3)) == 1 << 3; } set { DistByte = (byte)(DistByte & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
        public bool DistSuperTrain5 { get { return (DistByte & (1 << 4)) == 1 << 4; } set { DistByte = (byte)(DistByte & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
        public bool DistSuperTrain6 { get { return (DistByte & (1 << 5)) == 1 << 5; } set { DistByte = (byte)(DistByte & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
        public bool Dist7 { get { return (DistByte & (1 << 6)) == 1 << 6; } set { DistByte = (byte)(DistByte & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
        public bool Dist8 { get { return (DistByte & (1 << 7)) == 1 << 7; } set { DistByte = (byte)(DistByte & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
        public byte _0x3B { get { return Data[0x3B]; } set { Data[0x3B] = value; } }
        public byte _0x3C { get { return Data[0x3C]; } set { Data[0x3C] = value; } }
        public byte _0x3D { get { return Data[0x3D]; } set { Data[0x3D] = value; } }
        public byte _0x3E { get { return Data[0x3E]; } set { Data[0x3E] = value; } }
        public byte _0x3F { get { return Data[0x3F]; } set { Data[0x3F] = value; } }
        #endregion
        #region Block B
        public string Nickname
        {
            get
            {
                return Encoding.Unicode.GetString(Data, 0x40, 24)
                    .Replace("\uE08F", "\u2640") // nidoran
                    .Replace("\uE08E", "\u2642") // nidoran
                    .Replace("\u2019", "\u0027"); // farfetch'd
            }
            set
            {
                if (value.Length > 12)
                    value = value.Substring(0, 12); // Hard cap
                string TempNick = value // Replace Special Characters and add Terminator
                    .Replace("\u2640", "\uE08F") // nidoran
                    .Replace("\u2642", "\uE08E") // nidoran
                    .Replace("\u0027", "\u2019") // farfetch'd
                    .PadRight(value.Length + 1, '\0'); // Null Terminator
                Encoding.Unicode.GetBytes(TempNick).CopyTo(Data, 0x40);
            }
        }
        public int Move1
        {
            get { return BitConverter.ToUInt16(Data, 0x5A); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x5A); }
        }
        public int Move2
        {
            get { return BitConverter.ToUInt16(Data, 0x5C); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x5C); }
        }
        public int Move3
        {
            get { return BitConverter.ToUInt16(Data, 0x5E); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x5E); }
        }
        public int Move4
        {
            get { return BitConverter.ToUInt16(Data, 0x60); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x60); }
        }
        public int Move1_PP { get { return Data[0x62]; } set { Data[0x62] = (byte)value; } }
        public int Move2_PP { get { return Data[0x63]; } set { Data[0x63] = (byte)value; } }
        public int Move3_PP { get { return Data[0x64]; } set { Data[0x64] = (byte)value; } }
        public int Move4_PP { get { return Data[0x65]; } set { Data[0x65] = (byte)value; } }
        public int Move1_PPUps { get { return Data[0x66]; } set { Data[0x66] = (byte)value; } }
        public int Move2_PPUps { get { return Data[0x67]; } set { Data[0x67] = (byte)value; } }
        public int Move3_PPUps { get { return Data[0x68]; } set { Data[0x68] = (byte)value; } }
        public int Move4_PPUps { get { return Data[0x69]; } set { Data[0x69] = (byte)value; } }
        public int RelearnMove1
        {
            get { return BitConverter.ToUInt16(Data, 0x6A); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x6A); }
        }
        public int RelearnMove2
        {
            get { return BitConverter.ToUInt16(Data, 0x6C); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x6C); }
        }
        public int RelearnMove3
        {
            get { return BitConverter.ToUInt16(Data, 0x6E); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x6E); }
        }
        public int RelearnMove4
        {
            get { return BitConverter.ToUInt16(Data, 0x70); }
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x70); }
        }
        public bool SecretSuperTrainingUnlocked { get { return (Data[0x72] & 1) == 1; } set { Data[0x72] = (byte)((Data[0x72] & ~1) | (value ? 1 : 0)); } }
        public bool SecretSuperTrainingComplete { get { return (Data[0x72] & 2) == 2; } set { Data[0x72] = (byte)((Data[0x72] & ~2) | (value ? 2 : 0)); } }
        public byte _0x73 { get { return Data[0x73]; } set { Data[0x73] = value; } }
        public uint IV32 { get { return BitConverter.ToUInt32(Data, 0x74); } set { BitConverter.GetBytes(value).CopyTo(Data, 0x74); } }
        public int IV_HP { get { return (int)(IV32 >> 00) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 00)) | (uint)((value > 31 ? 31 : value) << 00)); } }
        public int IV_ATK { get { return (int)(IV32 >> 05) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 05)) | (uint)((value > 31 ? 31 : value) << 05)); } }
        public int IV_DEF { get { return (int)(IV32 >> 10) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 10)) | (uint)((value > 31 ? 31 : value) << 10)); } }
        public int IV_SPE { get { return (int)(IV32 >> 15) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 15)) | (uint)((value > 31 ? 31 : value) << 15)); } }
        public int IV_SPA { get { return (int)(IV32 >> 20) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 20)) | (uint)((value > 31 ? 31 : value) << 20)); } }
        public int IV_SPD { get { return (int)(IV32 >> 25) & 0x1F; } set { IV32 = (uint)((IV32 & ~(0x1F << 25)) | (uint)((value > 31 ? 31 : value) << 25)); } }
        public bool IsEgg { get { return ((IV32 >> 30) & 1) == 1; } set { IV32 = (uint)((IV32 & ~0x40000000) | (uint)(value ? 0x40000000 : 0)); } }
        public bool IsNicknamed { get { return ((IV32 >> 31) & 1) == 1; } set { IV32 = (IV32 & 0x7FFFFFFF) | (value ? 0x80000000 : 0); } }
        #endregion
        #region Block C
        public string HT_Name
        {
            get
            {
                return Encoding.Unicode.GetString(Data, 0x78, 24)
                    .Replace("\uE08F", "\u2640") // nidoran
                    .Replace("\uE08E", "\u2642") // nidoran
                    .Replace("\u2019", "\u0027"); // farfetch'd
            }
            set
            {
                if (value.Length > 12)
                    value = value.Substring(0, 12); // Hard cap
                string TempNick = value // Replace Special Characters and add Terminator
                    .Replace("\u2640", "\uE08F") // nidoran
                    .Replace("\u2642", "\uE08E") // nidoran
                    .Replace("\u0027", "\u2019") // farfetch'd
                    .PadRight(value.Length + 1, '\0'); // Null Terminator
                Encoding.Unicode.GetBytes(TempNick).CopyTo(Data, 0x78);
            }
        }
        public int HT_Gender { get { return Data[0x92]; } set { Data[0x92] = (byte)value; } }
        public int CurrentHandler { get { return Data[0x93]; } set { Data[0x93] = (byte)value; } }
        public int Geo1_Region { get { return Data[0x94]; } set { Data[0x94] = (byte)value; } }
        public int Geo1_Country { get { return Data[0x95]; } set { Data[0x95] = (byte)value; } }
        public int Geo2_Region { get { return Data[0x96]; } set { Data[0x96] = (byte)value; } }
        public int Geo2_Country { get { return Data[0x97]; } set { Data[0x97] = (byte)value; } }
        public int Geo3_Region { get { return Data[0x98]; } set { Data[0x98] = (byte)value; } }
        public int Geo3_Country { get { return Data[0x99]; } set { Data[0x99] = (byte)value; } }
        public int Geo4_Region { get { return Data[0x9A]; } set { Data[0x9A] = (byte)value; } }
        public int Geo4_Country { get { return Data[0x9B]; } set { Data[0x9B] = (byte)value; } }
        public int Geo5_Region { get { return Data[0x9C]; } set { Data[0x9C] = (byte)value; } }
        public int Geo5_Country { get { return Data[0x9D]; } set { Data[0x9D] = (byte)value; } }
        public byte _0x9E { get { return Data[0x9E]; } set { Data[0x9E] = value; } }
        public byte _0x9F { get { return Data[0x9F]; } set { Data[0x9F] = value; } }
        public byte _0xA0 { get { return Data[0xA0]; } set { Data[0xA0] = value; } }
        public byte _0xA1 { get { return Data[0xA1]; } set { Data[0xA1] = value; } }
        public int HT_Friendship { get { return Data[0xA2]; } set { Data[0xA2] = (byte)value; } }
        public int HT_Affection { get { return Data[0xA3]; } set { Data[0xA3] = (byte)value; } }
        public int HT_Intensity { get { return Data[0xA4]; } set { Data[0xA4] = (byte)value; } }
        public int HT_Memory { get { return Data[0xA5]; } set { Data[0xA5] = (byte)value; } }
        public int HT_Feeling { get { return Data[0xA6]; } set { Data[0xA6] = (byte)value; } }
        public byte _0xA7 { get { return Data[0xA7]; } set { Data[0xA7] = value; } }
        public int HT_TextVar { get { return BitConverter.ToUInt16(Data, 0xA8); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xA8); } }
        public byte _0xAA { get { return Data[0xAA]; } set { Data[0xAA] = value; } }
        public byte _0xAB { get { return Data[0xAB]; } set { Data[0xAB] = value; } }
        public byte _0xAC { get { return Data[0xAC]; } set { Data[0xAC] = value; } }
        public byte _0xAD { get { return Data[0xAD]; } set { Data[0xAD] = value; } }
        public byte Fullness { get { return Data[0xAE]; } set { Data[0xAE] = value; } }
        public byte Enjoyment { get { return Data[0xAF]; } set { Data[0xAF] = value; } }
        #endregion
        #region Block D
        public string OT_Name
        {
            get
            {
                return Encoding.Unicode.GetString(Data, 0xB0, 24)
                    .Replace("\uE08F", "\u2640") // Nidoran ♂
                    .Replace("\uE08E", "\u2642") // Nidoran ♀
                    .Replace("\u2019", "\u0027"); // farfetch'd
            }
            set
            {
                if (value.Length > 12)
                    value = value.Substring(0, 12); // Hard cap
                string TempNick = value // Replace Special Characters and add Terminator
                .Replace("\u2640", "\uE08F") // Nidoran ♂
                .Replace("\u2642", "\uE08E") // Nidoran ♀
                .Replace("\u0027", "\u2019") // Farfetch'd
                .PadRight(value.Length + 1, '\0'); // Null Terminator
                Encoding.Unicode.GetBytes(TempNick).CopyTo(Data, 0xB0);
            }
        }
        public int OT_Friendship { get { return Data[0xCA]; } set { Data[0xCA] = (byte)value; } }
        public int OT_Affection { get { return Data[0xCB]; } set { Data[0xCB] = (byte)value; } }
        public int OT_Intensity { get { return Data[0xCC]; } set { Data[0xCC] = (byte)value; } }
        public int OT_Memory { get { return Data[0xCD]; } set { Data[0xCD] = (byte)value; } }
        public int OT_TextVar { get { return BitConverter.ToUInt16(Data, 0xCE); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xCE); } }
        public int OT_Feeling { get { return Data[0xD0]; } set { Data[0xD0] = (byte)value; } }
        protected int Egg_Year { get { return Data[0xD1]; } set { Data[0xD1] = (byte)value; } }
        protected int Egg_Month { get { return Data[0xD2]; } set { Data[0xD2] = (byte)value; } }
        protected int Egg_Day { get { return Data[0xD3]; } set { Data[0xD3] = (byte)value; } }
        protected int Met_Year { get { return Data[0xD4]; } set { Data[0xD4] = (byte)value; } }
        protected int Met_Month { get { return Data[0xD5]; } set { Data[0xD5] = (byte)value; } }
        protected int Met_Day { get { return Data[0xD6]; } set { Data[0xD6] = (byte)value; } }
        public byte _0xD7 { get { return Data[0xD7]; } set { Data[0xD7] = value; } }
        public int Egg_Location { get { return BitConverter.ToUInt16(Data, 0xD8); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xD8); } }
        public int Met_Location { get { return BitConverter.ToUInt16(Data, 0xDA); } set { BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xDA); } }
        public int Ball { get { return Data[0xDC]; } set { Data[0xDC] = (byte)value; } }
        public int Met_Level { get { return Data[0xDD] & ~0x80; } set { Data[0xDD] = (byte)((Data[0xDD] & 0x80) | value); } }
        public int OT_Gender { get { return Data[0xDD] >> 7; } set { Data[0xDD] = (byte)((Data[0xDD] & ~0x80) | (value << 7)); } }
        public int EncounterType { get { return Data[0xDE]; } set { Data[0xDE] = (byte)value; } }
        public int HyperTrainFlags { get { return Data[0xDE]; } set { Data[0xDE] = (byte)value; } }
        public bool HT_HP { get { return ((HyperTrainFlags >> 0) & 1) == 1; } set { HyperTrainFlags = (HyperTrainFlags & ~(1 << 0)) | ((value ? 1 : 0) << 0); } }
        public bool HT_ATK { get { return ((HyperTrainFlags >> 1) & 1) == 1; } set { HyperTrainFlags = (HyperTrainFlags & ~(1 << 1)) | ((value ? 1 : 0) << 1); } }
        public bool HT_DEF { get { return ((HyperTrainFlags >> 2) & 1) == 1; } set { HyperTrainFlags = (HyperTrainFlags & ~(1 << 2)) | ((value ? 1 : 0) << 2); } }
        public bool HT_SPA { get { return ((HyperTrainFlags >> 3) & 1) == 1; } set { HyperTrainFlags = (HyperTrainFlags & ~(1 << 3)) | ((value ? 1 : 0) << 3); } }
        public bool HT_SPD { get { return ((HyperTrainFlags >> 4) & 1) == 1; } set { HyperTrainFlags = (HyperTrainFlags & ~(1 << 4)) | ((value ? 1 : 0) << 4); } }
        public bool HT_SPE { get { return ((HyperTrainFlags >> 5) & 1) == 1; } set { HyperTrainFlags = (HyperTrainFlags & ~(1 << 5)) | ((value ? 1 : 0) << 5); } }
        public int Version { get { return Data[0xDF]; } set { Data[0xDF] = (byte)value; } }
        public int Country { get { return Data[0xE0]; } set { Data[0xE0] = (byte)value; } }
        public int Region { get { return Data[0xE1]; } set { Data[0xE1] = (byte)value; } }
        public int ConsoleRegion { get { return Data[0xE2]; } set { Data[0xE2] = (byte)value; } }
        public int Language { get { return Data[0xE3]; } set { Data[0xE3] = (byte)value; } }
        #endregion
        //////////////////////////
        ///End PKHeX PK6 Layout///
        //////////////////////////

        internal static readonly Random rand = new Random();
        internal static uint rnd32()
        {
            return (uint)rand.Next(1 << 30) << 2 | (uint)rand.Next(1 << 2);
        }

        public static uint getRandomPID(int species, int cg, int origin, int nature, int form, uint OLDPID)
        {
            if (origin >= 24)
                return rnd32();

            uint ABILITY_MASK = 0x00010001;
            uint GENDER_MASK = 0x000000FF;
            uint bits = OLDPID & ABILITY_MASK;
            uint gv = OLDPID & GENDER_MASK;

            bool g3unown = origin <= 5 && species == 201;
            while (true) // Loop until we find a suitable PID
            {
                uint pid = rnd32();
                // Gen 3/4/5: Gender derived from PID

                //Keep old gender value
                pid = (pid & ~GENDER_MASK) | gv;

                //Keep ability bits
                pid = (pid & ~ABILITY_MASK) | bits;

                // Gen 3/4: Nature derived from PID
                if (origin <= 15 && pid % 25 != nature)
                    continue;

                // Gen 3 Unown: Letter/form derived from PID
                if (g3unown)
                {
                    uint pidLetter = ((pid & 0x3000000) >> 18 | (pid & 0x30000) >> 12 | (pid & 0x300) >> 6 | pid & 0x3) % 28;
                    if (pidLetter != form)
                        continue;
                }

                return pid;
            }
        }

        internal static uint getHEXval(string s)
        {
            string str = getOnlyHex(s);
            return string.IsNullOrWhiteSpace(str) ? 0 : Convert.ToUInt32(str, 16);
        }

        internal static string getOnlyHex(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? "0" : s.Select(char.ToUpper).Where("0123456789ABCDEF".Contains).Aggregate("", (str, c) => str + c);
        }


        public virtual bool isShiny => ((TID ^ SID) >> 4) == ((int)((PID >> 16 ^ PID & 0xFFFF) >> 4));

        public void setRandomPID()
        {
            PID = getRandomPID(Species, Gender, Version, Nature, AltForm, PID);
            if (GenNumber < 6)
                EncryptionConstant = PID;
        }

        public void setShinyPID()
        {
            do
            {
                PID = getRandomPID(Species, Gender, Version, Nature, AltForm, PID);
            }
            while (!isShiny);
            if (GenNumber < 6)
                EncryptionConstant = PID;
        }

    }
}
