using MathNet.Symbolics;
using Expr = MathNet.Symbolics.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neville
{
    class Program
    {
        public class Dane
        {
            public Dictionary<double, double> tabela;
            public int iloscArg {get;}

            public Dane(int iloscArg)
            {
                tabela = new Dictionary<double, double>();
                this.iloscArg = iloscArg;
            }

            public void PobierzDane()
            {
                double x;
                double y;
                for(int i = 0; i < iloscArg; i++)
                {
                    while(true)
                    {
                        Console.WriteLine("Podaj x" + i + ":");
                        if(Double.TryParse(Console.ReadLine(), out x))
                        {
                            break;
                        }
                        Console.WriteLine("Nieprawidlowa wartosc x" + i);
                    }

                    while(true)
                    {
                        Console.WriteLine("Podaj y" + i + ":");
                        if (Double.TryParse(Console.ReadLine(), out y))
                        {
                            break;
                        }
                        Console.WriteLine("Nieprawidlowa wartosc y" + i);
                    }
                    tabela.Add(x, y);
                }
            }
        }

        class Obliczenia
        {
            static Dane danex;
            static string[,] P;

            public static string[,] AlgorytmNevillea(Dane dane)
            {
                danex = dane;
                int iloscArg = dane.iloscArg;
                P = new string[iloscArg,iloscArg];

                //Wypelnienie pierwszej kolumny tablcy P
                for(int i = 0; i < iloscArg; i++)
                {
                    P[i, 0] = dane.tabela.Values.ElementAt(i).ToString();
                }

                //Wypelnienie reszty kolumn tablicy P
                int iloscWierszy = iloscArg - 1;
                for (int k = 1; k < iloscArg; k++)
                {
                    for (int i = 0; i < iloscWierszy; i++)
                    {
                        P[i,k] = Oblicz("((x-"+DajXi(i)+")*("+P[i+1,k-1]+")-(x-"+DajXi(i+k)+")*("+P[i,k-1]+"))/("+DajXi(i+k)+"-"+DajXi(i)+")");
                    }
                    iloscWierszy--;
                }
                return P;
            }

            private static string DajXi(int i)
            {
                if(P != null && danex != null)
                {
                    return "("+danex.tabela.Keys.ElementAt(i).ToString()+")";
                }
                return "";
            }

            private static string Oblicz(string formula)
            {
                var x = Expr.Symbol("x");
                return Infix.Format(Algebraic.Expand(Infix.ParseOrUndefined(formula)));           
            }
        }

        static void Main(string[] args)
        {
            int iloscArg;

            while(true)
            {
                Console.WriteLine("Podaj ilosc argumentow:");
                if (Int32.TryParse(Console.ReadLine(), out iloscArg))
                {
                    break;
                }
                Console.WriteLine("Ilosc argumentow musi byc liczba calkowita");
            }

            Dane dane = new Dane(iloscArg);
            dane.PobierzDane();

            string[,] P = Obliczenia.AlgorytmNevillea(dane);
            
            int iloscWierszy = iloscArg;
            for (int k = 0; k < iloscArg; k++)
            {
                for (int i = 0; i < iloscWierszy; i++)
                {
                    Console.WriteLine("P[" + i + "," + k + "](x) = " + P[i,k]);
                }
                iloscWierszy--;
            }
            Console.ReadKey();
        }
    }
}
