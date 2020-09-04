using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7MappingSegmentString : HL7MappingSegmentBase
    {
     //   [Obsolete]
     //   public string Value { get; set; }

        public HL7SegmentCategory _HL7SegmentCategory { get; set; }

        public HL7MappingSegmentString() { }

        public HL7MappingSegmentString(string sectionName, string segementName, int index, int subindex) : this()
        {
            this.SectionName= sectionName;
            this.SegmentName = segementName;
            this.Index_L1 = index;
            this.Index_L1 = subindex;
        }

        public HL7MappingSegmentString(string segmentName, string value, HL7MappingSegmentString parentSegment, int subindex) : this()
        {
            this.SegmentName = value;
            this.ParentSegment = parentSegment;
            this.ParentSegment.SubSegments.Add(this);
            this.Index_L1 = parentSegment.Index_L1;
            this.Index_L1 = subindex;
        }

        public string ToReport()
        {
            string str = this.SectionName + "\t" + this.Index_L1.ToString() + "\t" + this.Index_L1.ToString() + "\t" + this.SegmentName + "\t" + SegmentName;
            foreach (HL7MappingSegmentString child in this.SubSegments)
            {
                str += "\r\n" + child.ToReport();
            }
            return str;
        }

        public override string ToString()
        {
            string strSubsegments = string.Empty;
            if (this.SubSegments.Count > 0)
            {
                strSubsegments = " / " + this.SubSegments.Count + " subsegments";
            }

            string str = this.SectionName + " / " + this.Index_L1 + " / " + this.Index_L2 + " / " + this.SegmentName + strSubsegments;
            return str;
        }

    }
}
