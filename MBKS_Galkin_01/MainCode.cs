using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MBKS_Galkin_01
{
    class MainCode
    {
        static void Main(string[] args)
        {
            Thread Sec = new Thread(new ThreadStart(Securitier.Chmod));
            try
            {
                Console.Clear();
                Console.WriteLine("1. Указание пути к файлу");
                Console.WriteLine("2. Установка времени");
                Console.WriteLine("3. Удаление времени");
                Console.WriteLine("4. Вкл");
                Console.WriteLine("5. Выкл");
                Console.WriteLine();
                while (true)
                {
                    switch (Console.ReadKey(true).KeyChar)
                    {
                        case '1':
                            Console.Clear();
                            Console.WriteLine("1. Указание пути к файлу");
                            Securitier.Pather();
                            Console.WriteLine("    '0' для выхода в меню");
                            break;
                        case '2':
                            Console.Clear();
                            Console.WriteLine("2. Установка времени");
                            Securitier.TimeAdder();
                            Console.WriteLine("    '0' для выхода в меню");
                            break;

                        case '3':
                            Console.Clear();
                            Console.WriteLine("3. Удаление времени");
                            Securitier.TimeDeleter();
                            Console.WriteLine("    '0' для выхода в меню");
                            break;

                        case '4':
                            Console.Clear();
                            Console.WriteLine("4. Вкл");
                            if (Sec.IsAlive)
                                Sec.Resume();
                            else
                            {
                                Sec.Start();
                                Sec.IsBackground = true;
                            }
                            Console.WriteLine("   Монитор активен");

                            Console.WriteLine("    '0' для выхода в меню");
                            break;
                        case '5':
                            Console.Clear();
                            Console.WriteLine("5. Выкл");
                            
                            if(Sec.IsAlive)
                            Sec.Suspend();
                            
                            Console.WriteLine("   Монитор не активен");

                            Console.WriteLine("    '0' для выхода в меню");
                            break;
                        case '0':
                            Console.Clear();
                            Console.WriteLine("1. Указание пути к файлу");
                            Console.WriteLine("2. Установка времени");
                            Console.WriteLine("3. Удаление времени");
                            Console.WriteLine("4. Вкл");
                            Console.WriteLine("5. Выкл");
                            Console.WriteLine();
                            break;
                    }
                }
            }
            catch (ArgumentException)
            {
                Thread.CurrentThread.Abort();
            };
        }
    }
}
