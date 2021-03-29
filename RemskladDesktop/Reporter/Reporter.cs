using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RemskladDesktop.Reporter
{
    class Reporter
    {

        public static string CreateReport()
        {
            var accums = Repository.FetchItemsWhichNumberLessThanFour("accums");
            string msg = "                  Аккумуляторы:\n";
            foreach(var item in accums)
            {
                msg += $"{item.title};{item.residue}\n";
            }
            msg += "\n";

            var disporig = Repository.FetchItemsWhichNumberLessThanFour("disp-orig");
            msg += "                  Дисплейныйе модули(Оригиналы):\n";
            foreach(var item in disporig)
            {
                msg += $"{item.title};{item.residue}\n";
            }
            msg += "\n";

            var dispcopy = Repository.FetchItemsWhichNumberLessThanFour("disp-copy");
            msg += "                  Дисплейныйе модули(Копии):\n";
            foreach (var item in dispcopy)
            {
                msg += $"{item.title};{item.residue}\n";
            }
            msg += "\n";

            var maincameras = Repository.FetchItemsWhichNumberLessThanFour("main-cameras");
            msg += "                   Основные камеры:\n";
            foreach (var item in maincameras)
            {
                msg += $"{item.title};{item.residue}\n";
            }
            msg += "\n";

            return msg;
        }

        public static async Task WriteReportInCSVAsync(string InnerText = "Empty")
        {
            string writePath = @"C:\RemskladReports\Report.csv";


            string text = InnerText;
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.UTF8))
                {
                    await sw.WriteLineAsync(text);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DeleteReportAfterSentAsync()
        {
            try
            {
                string path = @"C:\RemskladReports\Report.csv";
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                {
                    fileInf.Delete();
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}