using System;
using System.Collections.Generic;
using System.Text;

namespace CheckmarksService.ViewModels
{
    public class NICETermsJson
    {
        public IList<NICETermsResultJson> Result { get; set; }
    }

    public class NICETermsResultJson 
    {
        public int TermNumber { get; set; }
        public string TermName { get; set; }
        public IList<NICETermsClassesJson> NiceClasses { get; set; }

        public override String ToString()
        {
            return $"Term Number: {TermNumber}, Term Name: {TermName}.";
        }
    }

    public class NICETermsClassesJson
    {
        public IList<NICETermsDescriptionsJson> Descriptions { get; set; }
    }

    public class NICETermsDescriptionsJson
    {
        public int Number { get; set; }  //this is class id
    }
}
