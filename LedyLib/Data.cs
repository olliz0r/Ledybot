using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LedyLib
{
    public class Data
    {
        public Dictionary<int, Tuple<string, string, int, int, int, ArrayList>> giveawayDetails = new Dictionary<int, Tuple<string, string, int, int, int, ArrayList>>();
        public Dictionary<int, string> countries = new Dictionary<int, string>();
        public Dictionary<int, string> regions = new Dictionary<int, string>();
        public ArrayList commented = new ArrayList();
        public ArrayList banlist = new ArrayList();
        public DataTable gdetails = new DataTable();
        public DataTable bdetails = new DataTable();

        public List<Tuple<string, int, int>> tradeQueueRec = new List<Tuple<string, int, int>>();

        public static GTSBot7 GtsBot7;

        public string thread = "";
        public string subreddit = "PokemonPlaza";

        public Data()
        {
            loadDetails();
            getCountries();
        }

        public class DataJSON
        {

            [JsonProperty("data")]
            public MainData data { get; set; }
        }

        public class MainData
        {
            [JsonProperty("children")]
            public List<ChildrenData> children { get; set; }
        }

        public class ChildrenData
        {
            [JsonProperty("data")]
            public CommentData data { get; set; }
        }

        public class CommentData
        {
            [JsonProperty("author_flair_text")]
            public string flair { get; set; }
        }

        public void updateJSON()
        {
            if (thread == "")
            {
                return;
            }
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadStringCompleted += (sender, e) =>
                    {
                        List<DataJSON> data = JsonConvert.DeserializeObject<List<DataJSON>>(e.Result);
                        foreach (ChildrenData cd in data[1].data.children)
                        {
                            string fc = (cd.data.flair != null && cd.data.flair.Length >= 14 ? cd.data.flair.Substring(cd.data.flair.IndexOf('-', 4) - 4, 14) : "").Replace("-", "");
                            if (!commented.Contains(fc) && fc != "")
                            {
                                commented.Add(fc);
                            }
                        }
                    };

                    wc.DownloadStringAsync(new Uri("https://www.reddit.com/r/" + subreddit + "/comments/" + thread + ".json?limt=1000&showmore=true"));
                }
            }
            catch
            {

            }
        }

        public static string[] getStringList(string f)
        {
            object txt = Properties.Resources.ResourceManager.GetObject(f); // Fetch File, \n to list.
            if (txt == null) return new string[0];
            string[] rawlist = ((string)txt).Split('\n');
            for (int i = 0; i < rawlist.Length; i++)
                rawlist[i] = rawlist[i].Trim();
            return rawlist;
        }

        public void getCountries()
        {
            string[] inputCSV = getStringList("countries");
            // Gather our data from the input file
            for (int i = 1; i < inputCSV.Length; i++)
            {
                string[] countryData = inputCSV[i].Split(',');
                countries.Add(int.Parse(countryData[0]), countryData[2]);
            }
        }

        public void getSubRegions(int country)
        {
            regions.Clear();

            string[] inputCSV = getStringList("sr_" + country.ToString("000"));

            for (int i = 1; i < inputCSV.Length; i++)
            {
                string[] regionData = inputCSV[i].Split(',');
                regions.Add(int.Parse(regionData[0]), regionData[2]);
            }
        }

        public byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public byte calculateChecksum(byte[] principal)
        {
            byte[] newPrincipal = new byte[4];
            Array.Copy(principal, newPrincipal, 4);
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(newPrincipal);
                byte MyChecksum = (byte)(hash[0] >> 1);
                return MyChecksum;
            }
        }

        public string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        private void loadDetails()
        {
            gdetails.TableName = "Giveaway Details";
            gdetails.Columns.Add("Dex Number", typeof(int));
            gdetails.Columns.Add("Specific Path", typeof(string));
            gdetails.Columns.Add("Optional Path", typeof(string));
            gdetails.Columns.Add("Gender Index", typeof(int));
            gdetails.Columns.Add("Level Index", typeof(int));
            gdetails.Columns.Add("Count", typeof(int));
            gdetails.Columns.Add("Traded", typeof(int));

            if (File.Exists(Application.StartupPath + "\\giveawaydetails.xml"))
            {
                gdetails.ReadXml(Application.StartupPath + "\\giveawaydetails.xml");
            }

            foreach (DataRow row in gdetails.Rows)
            {
                giveawayDetails.Add((int)row[0], new Tuple<string, string, int, int, int, ArrayList>(row[1].ToString(), row[2].ToString(), (int)row[3], (int)row[4], (int)row[5], new ArrayList()));
            }

            bdetails.TableName = "Banlist";
            bdetails.Columns.Add("FC", typeof(string));

            if (File.Exists(Application.StartupPath + "\\banlistdetails.xml"))
            {
                bdetails.ReadXml(Application.StartupPath + "\\banlistdetails.xml");
            }

            foreach (DataRow row in bdetails.Rows)
            {
                banlist.Add(row[0]);
            }

        }

        public void refreshDetails(bool giveawayB = true, bool banB = true, string giveaway = "giveawaydetails.xml",
            string ban = "banlistdetails.xml")
        {
            if (giveawayB)
            {
                gdetails.Clear();
                if (File.Exists(Application.StartupPath + "\\" + giveaway))
                {
                    gdetails.ReadXml(Application.StartupPath + "\\" + giveaway);
                }

                foreach (DataRow row in gdetails.Rows)
                {
                    giveawayDetails.Add((int)row[0], new Tuple<string, string, int, int, int, ArrayList>(row[1].ToString(), row[2].ToString(), (int)row[3], (int)row[4], (int)row[5], new ArrayList()));
                }
            }

            if (banB)
            {
                bdetails.Clear();
                if (File.Exists(Application.StartupPath + "\\" + ban))
                {
                    bdetails.ReadXml(Application.StartupPath + "\\" + ban);
                }

                foreach (DataRow row in bdetails.Rows)
                {
                    banlist.Add(row[0]);
                }
            }

        }

        public void saveDetails()
        {
            gdetails.WriteXml(Application.StartupPath + "\\giveawaydetails.xml", true);
            bdetails.WriteXml(Application.StartupPath + "\\banlistdetails.xml", true);
        }

        public void AddToQueue(int dex, string fc)
        {
            tradeQueueRec.Add(new Tuple<string, int, int>(fc, dex, 0));
        }

        public void RemoveFromQueue(int index)
        {
            if (tradeQueueRec.Count > 0)
                tradeQueueRec.RemoveAt(index);
        }

        public string ViewQueue(int page)
        {
            string result = "";
            if (tradeQueueRec.Count == 0) return result;
            if (tradeQueueRec.Count < 5 * page - 4)
            {
                for (var i = (tradeQueueRec.Count - 6 < 0 ? 0 : tradeQueueRec.Count - 6); i < tradeQueueRec.Count - 1; i++)
                {
                    result += i + "|" + tradeQueueRec[i].Item1 + "|" + tradeQueueRec[i].Item2 + ",";
                }
            }
            else
            {
                for (var i = 5 * (page - 1); i < (5 * page > tradeQueueRec.Count ? tradeQueueRec.Count : 5 * page); i++)
                {
                    result += i + "|" + tradeQueueRec[i].Item1 + "|" + tradeQueueRec[i].Item2 + ",";
                }
            }

            return result.TrimEnd(',');
        }
    }
}
