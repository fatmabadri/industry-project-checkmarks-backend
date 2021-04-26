using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CheckmarksService.ViewModels;

namespace CheckmarksWebApi.Models
{
    public class NICEClass
    {
        [Key]
        public int Id { get; set; }  //Class ID
        public string Description { get; set; }
        public string ShortName { get; set; }
        public int Category { get; set; }

        public NICEClass(NICEClassResultJson view)
        {
            this.Id = view.ClassNumber;
            this.Description = view.Descriptions[0].Name;
            this.ShortName = view.Descriptions[0].ShortName;
            this.Category = view.NiceCategory;
        }

        public NICEClass()
        {

        }

    }
}
