using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Security.AccessControl;
using System.Timers;

namespace MBKS_Galkin_01
{
    public class Tim
    {
        public delegate void Ruller();
        public event Ruller TimeToAllow;
        public event Ruller TimeToDeny;
        public void Checker()
        {
            DateTime someTimeCur = DateTime.Now;
            TimeToAllow += delegate { Securitier.RulAllower(); };
            TimeToDeny += delegate { Securitier.RulDenier(); };

            while (true)
            {
                Thread.Sleep(1000);
                someTimeCur = DateTime.Now;
                if (Securitier.IntChecker(someTimeCur))
                    TimeToAllow();
                else
                    TimeToDeny();

            }
        }
    }
    public static class Securitier
    {
        public static string Path { get; set; }
        public static System.Security.Principal.WindowsIdentity wi = System.Security.Principal.WindowsIdentity.GetCurrent();
        public class TimeNode
        {
            public TimeNode(DateTime a, DateTime b)
            {
                FirstTime = a;
                SecondTime = b;
            }
            public DateTime FirstTime { get; set; }
            public DateTime SecondTime { get; set; }
        }
        public static List<TimeNode> Intervals = new List<TimeNode> { };

        public static bool IntChecker(DateTime time)
        {
            for (int i = 0; i < Intervals.Count; i++)
            {
                if (time >= Intervals[i].FirstTime && time <= Intervals[i].SecondTime)
                    return true;
            }
            return false;
        }

        public static void TimeAdder()
        {
            Console.WriteLine("   Введите временной промежуток в формате !!:!!:!! - !!:!!:!!");
            Console.Write(">> ");
            string str = "";
            str = Console.ReadLine();
            if (TimeChecker(str))
            {
                DateTime timeA = new DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, System.DateTime.Today.Day, Convert.ToInt32(str.Substring(0, 2)), Convert.ToInt32(str.Substring(3, 2)), Convert.ToInt32(str.Substring(6, 2)));
                DateTime timeB = new DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, System.DateTime.Today.Day, Convert.ToInt32(str.Substring(11, 2)), Convert.ToInt32(str.Substring(14, 2)), Convert.ToInt32(str.Substring(17, 2)));
                Intervals.Add(new TimeNode(timeA, timeB));
                Console.WriteLine("   Временной промежуток введен корректно");
            }
            else
            Console.WriteLine("   Временной промежуток введен некорректно");
        }

        public static void TimeDeleter()
        {
            if (Intervals.Count == 0)
            {
                Console.WriteLine("   Список временных промежутков пуст!");
                return;
            }
            Console.WriteLine("Внесеные временные промежутки:");
            for (int i = 0; i < Intervals.Count; i++)
            {
                Console.WriteLine("   "+i+") "+Intervals[i].FirstTime.ToShortTimeString()+" - "+ Intervals[i].SecondTime.ToShortTimeString());
            }

            int B;
            string c = "";
            
            do
            {
                Console.Write("Введите номер временного промежутка: ");
                c = Console.ReadLine();
            } while (!int.TryParse(c, out B) || (Convert.ToInt32(c) >= Intervals.Count));

            B = Convert.ToInt32(c);

            Intervals.RemoveAt(B);
            Console.WriteLine("   Временной промежуток удален!!!");

        }

        public static bool TimeChecker(string currenttime)
        {
            if (currenttime.Length != 19)
                return false;

            int hoursA = 0;
            int minsA =  0;
            int secsA =  0;

            int hoursB = 0;
            int minsB = 0;
            int secsB = 0;

            int B;

            if (!int.TryParse(currenttime.Substring(0, 2), out B))
                return false;
            if (!int.TryParse(currenttime.Substring(3, 2), out B))
                return false;
            if (!int.TryParse(currenttime.Substring(6, 2), out B))
                return false;

            if (!int.TryParse(currenttime.Substring(11, 2), out B))
                return false;
            if (!int.TryParse(currenttime.Substring(14, 2), out B))
                return false;
            if (!int.TryParse(currenttime.Substring(17, 2), out B))
                return false;

            hoursA = Convert.ToInt32(currenttime.Substring(0, 2));
            minsA =  Convert.ToInt32(currenttime.Substring(3, 2));
            secsA =  Convert.ToInt32(currenttime.Substring(6, 2));

            hoursB = Convert.ToInt32(currenttime.Substring(11, 2));
            minsB = Convert.ToInt32(currenttime.Substring(14, 2));
            secsB = Convert.ToInt32(currenttime.Substring(17, 2));

            if (hoursA > hoursB) return false;
            if (hoursA == hoursB && minsA > minsB) return false;
            if (hoursA == hoursB && minsA == minsB && secsA > secsB) return false;

            if ((hoursA <= 23 && hoursA >= 0) && (minsA <= 59 && minsA >= 0) && (secsA <= 59 && secsA >= 0) && currenttime[2] == ':' && currenttime[5] == ':')
                if ((hoursB <= 23 && hoursB >= 0) && (minsB <= 59 && minsB >= 0) && (secsB <= 59 && secsB >= 0) && currenttime[13] == ':' && currenttime[16] == ':')
                    if (currenttime[8] == ' ' && currenttime[9] == '-' && currenttime[10] == ' ')
                        return true;
                    else
                        return false;
            return false;
        }

        public static void Pather()
        {
            string buf = "";
            do
            {
                Console.Write("   Введите путь к файлу: ");
                buf = Console.ReadLine();

            } while (!File.Exists(buf));

            Path = buf;
        }
       
        public static void RulDenier()
        {
            RemoveFileSecurity(Path, @wi.Name, FileSystemRights.FullControl, AccessControlType.Allow);
            AddFileSecurity(Path, @wi.Name, FileSystemRights.FullControl, AccessControlType.Deny);
            
        }

        public static void RulAllower()
        {
            RemoveFileSecurity(Path, @wi.Name, FileSystemRights.FullControl, AccessControlType.Deny);
            AddFileSecurity(Path, @wi.Name, FileSystemRights.FullControl, AccessControlType.Allow);
        }

        public static FileSecurity fSecurity;

        public static void Chmod()
        {
            if (!File.Exists(Securitier.Path))
                Pather();

            Tim Timer = new Tim();
            Timer.Checker();
        }
        public static void AddFileSecurity(string fileName, string account,FileSystemRights rights, AccessControlType controlType)
        {
            fSecurity = File.GetAccessControl(fileName);
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,rights, controlType));
            File.SetAccessControl(fileName, fSecurity);
        }
        public static void RemoveFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            fSecurity = File.GetAccessControl(fileName);
            fSecurity.RemoveAccessRuleAll(new FileSystemAccessRule(account,rights, controlType));
            File.SetAccessControl(fileName, fSecurity);
        }
    }
}
