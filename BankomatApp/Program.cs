using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Bankomat bankomat = new Bankomat("c:\\temp");
            Console.WriteLine("Добрый день!");
            Console.WriteLine("В банкомате имеются купюры");

            foreach (decimal note in bankomat.Banknotes.Keys)
                Console.WriteLine(note + " : " + bankomat.Banknotes[note]);

            Console.WriteLine("Какая сумма Вам нужна?");

            string sumString = Console.ReadLine();

            try
            {
                decimal sumForClient = Decimal.Parse(sumString);
                Dictionary<decimal, int> result = bankomat.TakeSumForClient(sumForClient);

                if (result == null)
                    Console.WriteLine("Мы не сможем выдать Вам эту сумму, извините!");
                else
                {
                    Console.WriteLine("Заберите Ваши деньги!");
                    foreach (decimal note in result.Keys)
                        Console.WriteLine(note + " : " + result[note]);
                }
            }
            catch
            {
                Console.WriteLine("Вы ввели некорректное значение!");
            }


            Console.ReadKey();
        }
    }
}
