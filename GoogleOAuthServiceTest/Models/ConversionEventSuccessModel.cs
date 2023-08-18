using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleOAuthServiceTest.Models
{
    public class ConversionEventSuccessModel
    {
        public string Name { get; set; }
        public string EventName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Deletable { get; set; }
        public bool Custom { get; set; }

    }
}
