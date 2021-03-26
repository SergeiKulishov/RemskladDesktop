using System;
using System.Collections.Generic;
using System.Text;

namespace RemskladDesktop.Reporter
{
    class Reporter
    {

        public static string CreateReport()
        {
            var s = Repository.FetchData();
            string msg = "";
            foreach (var item in s)
            {
                msg += $"<b>{item.title} - Остаток :{item.residue}</b>\n";
            }
            return msg;
        }
    }
}