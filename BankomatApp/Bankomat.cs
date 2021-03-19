using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatApp
{
    public class Bankomat
    {
        public Bankomat() { }

        public Bankomat(string path)
        {
            this.Path = path;
            
        }

        public string Path { get; set; }

        private SortedDictionary<decimal, int> _banknotes;
        public SortedDictionary<decimal, int> Banknotes
        {
            get
            {
                if (_banknotes == null)
                {
                    _banknotes = new SortedDictionary<decimal, int>();
                    if (!Path.EndsWith("\\")) Path += "\\";
                    string[] strings = File.ReadAllLines(this.Path + "atm.txt");
                    foreach (string str in strings)
                    {
                        int pos = str.IndexOf(":");
                        decimal nominal = Decimal.Parse(str.Substring(0, pos));
                        int count = Int32.Parse(str.Substring(pos + 1, str.Length - (pos + 1)));
                        _banknotes.Add(nominal, count);
                    }
                }
                return _banknotes;
            }
        }

        private SortedDictionary<decimal, int> GetBanknotesCopy()
        {
            SortedDictionary<decimal, int> result = new SortedDictionary<decimal, int>();
            foreach (decimal key in Banknotes.Keys)
                result.Add(key, Banknotes[key]);
            return result;
        }

        public Dictionary<decimal, int> TakeSumForClient(decimal sum)
        {
            int startPos = Banknotes.Count - 1;
            while (startPos >= 0)
            {
                Dictionary<decimal, int> result = TakeSumForClient(sum, GetBanknotesCopy(), startPos);
                if (result != null) return result;
                startPos--;
            }
            return null;
        }

        private Dictionary<decimal, int> TakeSumForClient(decimal sum, SortedDictionary<decimal, int> bankList, int startPos)
        {
            Dictionary<decimal, int> result = new Dictionary<decimal, int>();
            List<decimal> nominals = bankList.Keys.ToList();
            decimal totalRest = sum;
            while (totalRest > 0)
            {
                for (int i = startPos; i >= 0; i--)
                {
                    decimal nom = nominals[i];
                    if (nom <= totalRest && bankList[nom] > 0)
                    {
                        if (result.ContainsKey(nom))
                            result[nom]++;
                        else result.Add(nom, 1);
                        bankList[nom]--;
                        totalRest -= nom;
                        break;

                    }
                    else if (i == 0)
                    {
                        return null;
                    }
                }
            }
            return result;
        }
    }
}
