using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class Hl7MappingSection
    {
        // public HL7MappingSegment Segment { get; set; } = new HL7MappingSegment();

        public string  Name { get; set; }

        public HL7MappingSegments Segments { get; set; } = new HL7MappingSegments();


        //public string SectionName
        //{
        //    get
        //    {
        //        if (Segment != null)
        //        {
        //            return Segment.SegmentName;
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //}

        public Hl7MappingSection(string name)
        {
            this.Name = name;
        }

        public string ToReport()
        {
            string str = String.Empty;
            foreach (HL7MappingSegment segment in this.Segments)
            {
                if (!String.IsNullOrWhiteSpace(str))
                {
                    str += "\r\n";
                }
                str += segment.ToReport();
            }
            return str;
        }

    }
}
