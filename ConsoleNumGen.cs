using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static long loopCount;

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if(loopCount <= 0)
                {
                    Console.WriteLine("");
                }
                loopCount = long.Parse(args[0]);
            }
            else
            {
                Console.WriteLine("");
                Environment.Exit(1);
            }

            Random r = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);
            StreamWriter sw;

            using (sw = new StreamWriter("C:\\asdf.csv"))
            {
                long summation = 0;

                for (long i = 0; i <= loopCount; i++)
                {
                    int numberGen = r.Next(0, 2);

                    if (numberGen == 1)
                    {
                        summation++;
                    }
                    else
                    {
                        summation--;
                    }

                    sw.WriteLine(summation.ToString());
                }
            }

            sw.Close();
        }
    }
}
