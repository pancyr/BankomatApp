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
                        Banknotes.Add(nominal, count);
                    }
                }
                return _banknotes;
            }
        }

        public Dictionary<decimal, int> TakeSumForClient(decimal sum)
        {
            Dictionary<decimal, int> result = new Dictionary<decimal, int>();
            List<decimal> nominals = Banknotes.Keys.ToList();
            decimal totalRest = sum;
            while (totalRest > 0)
            {
                int lastIndex = Banknotes.Count - 1;
                for (int i = lastIndex; i>=0; i--)
                {
                    decimal nom = nominals[i];
                    if (nom <= totalRest)
                    {
                        if (result.ContainsKey(nom))
                            result[nom]++;
                        else result.Add(nom, 1);
                        Banknotes[nom]--;
                        if (Banknotes[nom] == 0)
                        {
                            Banknotes.Remove(nom);
                            nominals.Remove(nom);
                        }
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
