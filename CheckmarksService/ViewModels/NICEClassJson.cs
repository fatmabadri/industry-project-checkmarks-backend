using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CheckmarksService.ViewModels
{
    public class NICEClassJson
    {
        public IList<NICEClassResultJson> Result { get; set; }
    }

    public class NICEClassResultJson
    {
        public int ClassNumber { get; set; }
        public int NiceCategory { get; set; }
        public IList<NICEClassDescriptionJson> Descriptions { get; set; }
    }

    public class NICEClassDescriptionJson
    {
        //classDescription - result.descriptions[0].name
        public string Name { get; set; }
        //classShortName 
        public string ShortName { get; set; }
    }

    
}
