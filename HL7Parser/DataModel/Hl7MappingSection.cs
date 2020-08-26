using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class Hl7MappingSection
    {
        public HL7SegmentString Segment { get; set; } = new HL7SegmentString();

        public HL7MappingSegments Segments { get; set; } = new HL7MappingSegments();

        public string SegmentName
        {
            get
            {
                if (Segment != null)
                {
                    return Segment.SegmentName;
                }
                else
                {
                    return "";
                }
            }
        }

        public Hl7MappingSection(string name)
        {
            this.Segment.SegmentName = name;
        }
    }
}
