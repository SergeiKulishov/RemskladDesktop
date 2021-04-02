﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RemskladDesktop.Reporter
{
    class Reporter
    {

        public static string CreateReportCSV()
        {
            var accums = Repository.FetchItemsWhichNumberLessThanFour("accums");
            string msg = "                  Аккумуляторы:\n";
            foreach (var item in accums)
            {
                msg += $"{item.title};{item.residue}\n";
            }
            msg += "\n";

            var disporig = Repository.FetchItemsWhichNumberLessThanFour("disp-orig");
            msg += "                  Дисплейныйе модули(Оригиналы):\n";
            foreach (var item in disporig)
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

        public static void DeleteCSVReportAfterSentAsync()
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

        public static string CreateReportHTML()
        {
            string htmlHead = "<!DOCTYPE html><html>< meta charset=\"utf-8\"><style>.table_dark {font-family: \"Lucida Sans Unicode\", \"Lucida Grande\", Sans-Serif;" +
            "font-size: 14px;width: 640px;text-align: left;border-collapse: collapse;background: #252F48;margin: 10px;}.table_dark th{color: #EDB749;border-bottom: 1px solid #37B5A5;padding: 12px 17px;}" +
            ".table_dark td{color: #CAD4D6;border-bottom: 1px solid #37B5A5;border-right:1px solid #37B5A5;padding: 7px 17px;}.table_dark tr:last-child td{border-bottom: none;}" +
            ".table_dark td:last-child {border-right: none;}.table_dark tr:hover td{text-decoration: underline;}</ style >< body >< table class= \"table_dark\" > ";
            
            var accums = Repository.FetchItemsWhichNumberLessThanFour("accums");
            string msg = "                  Аккумуляторы:\n";
            foreach (var item in accums)
            {
                msg += $"{item.title};{item.residue}\n";
            }
            msg += "\n";

            var disporig = Repository.FetchItemsWhichNumberLessThanFour("disp-orig");
            msg += "                  Дисплейныйе модули(Оригиналы):\n";
            foreach (var item in disporig)
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
    }
}