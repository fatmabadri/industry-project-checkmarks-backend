using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckmarksWebApi.Models
{
    public class NICEClass
    {
        [Key]
        public int Id { get; set; }  //Class ID

        public string Description { get; set; }

        public string ShortName { get; set; }

        public int Category { get; set; }

        public NICEClass()
        {

        }
    }
}
