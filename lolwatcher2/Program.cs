using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;

namespace lolwatcher2
{
    class Program
    {
        static List<string> LOCALES = new List<string>{
            "cs_CZ",
            "el_GR",
            "pl_PL",
            "ro_RO",
            "hu_HU",
            "en_GB",
            "de_DE",
            "es_ES",
            "it_IT",
            "fr_FR",
            "ja_JP",
            "ko_KR",
            "es_MX",
            "es_AR",
            "pt_BR",
            "en_US",
            "en_AU",
            "ru_RU",
            "tr_TR",
            "ms_MY",
            "en_PH",
            "en_SG",
            "th_TH",
            "vn_VN",
            "id_ID",
            "zh_MY",
            "zh_CN",
            "zh_TW"
        }.OrderBy(l => l).ToList();

        static void Main(string[] args)
        {
            string lang = "";

            if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {

                while (string.IsNullOrWhiteSpace(lang) || !LOCALES.Where(l => l == lang).Any())
                {
                    int counter = 0;
                    Console.WriteLine("Enter target language:");
                    LOCALES.ForEach(l => Console.WriteLine(++counter + ". " + l));

                    lang = Console.ReadLine();

                    int i;
                    if (int.TryParse(lang, out i) && i <= LOCALES.Count)
                    {
                        lang = LOCALES[i - 1];
                    }
                }
            }
            else
            {
                lang = args[0];
            }
            Console.Clear();
            Console.WriteLine("Locale: " + lang);
            Console.WriteLine("Language changer active.");

            while (true)
            {
                Process[] t = Process.GetProcessesByName("League of Legends");
                while (t.Length == 0)
                {
                    t = Process.GetProcessesByName("League of Legends");
                    Thread.Sleep(500);
                }

                string cmd;

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + t[0].Id))
                using (ManagementObjectCollection objects = searcher.Get())
                {
                    cmd = objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
                }

                if (cmd.Contains(lang))
                {
                    Thread.Sleep(5000);
                    continue;
                }

                LOCALES.ForEach(l => cmd = cmd.Replace("-Locale=" + l, "-Locale=" + lang));

                ProcessStartInfo lol = new ProcessStartInfo(
                    cmd
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

                Console.WriteLine("In-game language changed.");
            }
        }
    }
}
