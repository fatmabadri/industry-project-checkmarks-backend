﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckmarksWebApi.Models
{
    public class TradeMark
    {
        public string Title { get; set; }
        // "NIKE"
        public DateTime FileDate { get; set; }
        // "2000-05-24T00:00:00"
        public DateTime RegDate { get; set; }
        // "2003-06-11T00:00:00"
        public DateTime IntrnlRenewDate { get; set; }
        // "2033-06-11T00:00:00"
        public string Owner { get; set; }
        // "NIKE INNOVATE C.V."
        public string StatusDescEn { get; set; }
        // "REGISTERED"
        public NICEClass[] NiceClasses { get; set; }
        // 0	9
        // 1	10
        // 2	16
        // 3	18
        // 4	20
        // 5	28
        public int[] TmType { get; set; }
        // 0	1
        public string[] ApplicationNumberL { get; set; }
        // 0	"1060300"
        // 1	"106030000"
        // 2	"1060300-00"
        public string[] MediaUrls { get; set; }
        // null
    }
}