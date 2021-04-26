using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckmarksWebApi.Models
{
    public class NICETerm
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("NICEClass")]
        public int ClassId { get; set; }

        public string Name { get; set; }

        public NICEClass NICEClass { get; set; }

        public NICETerm()
        {

        }
    }
}
