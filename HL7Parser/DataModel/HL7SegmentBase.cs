using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7SegmentBase
    {
        // -----------------------------------------------------------------------------------------------------
        // Fields are taken from 
        // https://corepointhealth.com/resource-center/hl7-resources/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-pid-segment/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-pv1-patient-visit-information-segment/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-msh-message-header/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-obr-segment/
        // -----------------------------------------------------------------------------------------------------


        public string SectionName { get; set; }

        public int Index { get; set; }
        public int SubIndex { get; set; }
        public int Length { get; set; }
        public string Dt { get; set; }
        public string Opt { get; set; }
        public string Rp { get; set; }

        public string SegmentName { get; set; }

        public string IndexComplete
        {
            get
            {
                if (this.ParentSegment == null)
                {
                    return this.Index.ToString();
                }
                else
                {
                    // segment is subsegment
                    return this.ParentSegment.Index.ToString() + "." + this.SubIndex.ToString();
                }
            }
        }

        public HL7SegmentBase ParentSegment { get; set; }

        public HL7Segments SubSegments { get; set; } = new HL7Segments();



        public HL7SegmentBase()
        { }

        public HL7SegmentBase(string section, int index, int length, string elementName, string dt, string opt, string rp) : this()
        {
            this.Index = index;
            this.Length = length;
            this.SegmentName = elementName;
            this.Dt = dt;
            this.Opt = opt;
            this.Rp = rp;
        }

        public override string ToString()
        {
            string str = this.SectionName + " / " + this.Index + " / " + this.SegmentName;
            return str;
        }
    }
}
