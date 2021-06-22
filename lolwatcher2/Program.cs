using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;

namespace lolwatcher2
{
    class Program
    {
        static void Main(string[] args)
        {
            string lang;

            if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {

                Console.WriteLine("Enter target language:");
                Console.WriteLine("cs_CZ el_GR pl_PL ro_RO hu_HU en_GB de_DE es_ES it_IT fr_FR ja_JP ko_KR es_MX es_AR pt_BR en_US en_AU ru_RU tr_TR ms_MY en_PH en_SG th_TH vn_VN id_ID zh_MY zh_CN zh_TW");

                lang = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(lang))
                {
                    Console.WriteLine("Enter target language:");
                    Console.WriteLine("cs_CZ el_GR pl_PL ro_RO hu_HU en_GB de_DE es_ES it_IT fr_FR ja_JP ko_KR es_MX es_AR pt_BR en_US en_AU ru_RU tr_TR ms_MY en_PH en_SG th_TH vn_VN id_ID zh_MY zh_CN zh_TW");

                    lang = Console.ReadLine();
                }
            }
            else
            {
                lang = args[0];
            }

            while (true)
            {
                Console.WriteLine("Waiting for game to start...");

                Process[] t = Process.GetProcessesByName("League of Legends");
                while (t.Length == 0)
                {
                    t = Process.GetProcessesByName("League of Legends");
                    Thread.Sleep(50);
                }

                string cmd;

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + t[0].Id))
                using (ManagementObjectCollection objects = searcher.Get())
                {
                    cmd = objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
                }

                ProcessStartInfo lol = new ProcessStartInfo(
                    cmd
                        .Replace("cs_CZ", lang)
                        .Replace("el_GR", lang)
                        .Replace("pl_PL", lang)
                        .Replace("ro_RO", lang)
                        .Replace("hu_HU", lang)
                        .Replace("en_GB", lang)
                        .Replace("de_DE", lang)
                        .Replace("es_ES", lang)
                        .Replace("it_IT", lang)
                        .Replace("fr_FR", lang)
                        .Replace("ja_JP", lang)
                        .Replace("ko_KR", lang)
                        .Replace("es_MX", lang)
                        .Replace("es_AR", lang)
                        .Replace("pt_BR", lang)
                        .Replace("en_US", lang)
                        .Replace("en_AU", lang)
                        .Replace("ru_RU", lang)
                        .Replace("tr_TR", lang)
                        .Replace("ms_MY", lang)
                        .Replace("en_PH", lang)
                        .Replace("en_SG", lang)
                        .Replace("th_TH", lang)
                        .Replace("vn_VN", lang)
                        .Replace("id_ID", lang)
                        .Replace("zh_MY", lang)
                        .Replace("zh_CN", lang)
                        .Replace("zh_TW", lang)
                );

                lol.UseShellExecute = false;

                try
                {
                    t[0].Kill();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Process.Start(lol);

                while (Console.KeyAvailable)
                    Console.ReadKey(false);

                Console.WriteLine("In-game language changed, press any key to start listening again.");
                Console.ReadKey(false);
            }
        }
    }
}
