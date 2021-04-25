using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckmarksWebApi.Models
{
    public class Image
    {
        public string FileName { get; set; }
        public byte[] Picture { get; set; }
    }
}
