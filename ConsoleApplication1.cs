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

            Random r = new Random();
            StreamWriter sw;

            using (sw = new StreamWriter("C:\\numbers.txt"))
            {
                for (long i = 0; i <= loopCount; i++)
                {
                    sw.WriteLine(r.Next(0, 2).ToString());
                }
            }

            sw.Close();
        }
    }
}
