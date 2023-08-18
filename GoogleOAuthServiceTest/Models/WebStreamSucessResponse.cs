using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleOAuthServiceTest.Models
{
    public class WebStreamSucessResponse
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public WebStreamData WebStreamData { get; set; }
    }

    public class WebStreamData
    {
        public string MeasurementId { get; set; }
        public string StreamId { get; set; }
        public string DefaultUri { get; set; }
    }
}
