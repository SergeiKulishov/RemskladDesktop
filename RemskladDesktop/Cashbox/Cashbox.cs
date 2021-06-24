﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemskladDesktop.Cashbox
{
    public class Cashbox
    {
        public int type { get; set; }
        public string currency { get; set; }
        public double balance { get; set; }
        public int id { get; set; }
        public bool is_global { get; set; }
        public string title { get; set; }
    }

    public class CashboxesFromRemOnline
    {
        public List<Cashbox> data { get; set; }
        public int count { get; set; }
        public bool success { get; set; }
    }
}
