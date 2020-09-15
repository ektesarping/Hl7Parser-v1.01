using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7MappingSegment
    {
        // -----------------------------------------------------------------------------------------------------
        // Fields are taken from 
        // https://corepointhealth.com/resource-center/hl7-resources/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-pid-segment/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-pv1-patient-visit-information-segment/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-msh-message-header/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-obr-segment/
        // -----------------------------------------------------------------------------------------------------


        public int Index_L1 { get; set; }

        public int Index_L2 { get; set; }

        public int Level { get; set; }

        /// <summary>
        /// Collapsed default ved innlesing.
        /// </summary>
        public bool CollapsedDefault { get; set; } = false;

        public string SegmentName { get; set; }

        public string SectionName { get; set; }

        public Hl7MappingSection MappingSection { get; set; }

        public string IndexComplete
        {
            get
            {
                if (this.ParentSegment == null)
                {
                    return this.Index_L1.ToString();
                }
                else
                {
                    // segment is subsegment
                    return this.ParentSegment.Index_L1.ToString() + "." + this.Index_L1.ToString();
                }
            }
        }

        public HL7MappingSegment ParentSegment { get; set; }

        public HL7MappingSegments SubSegments { get; set; } = new HL7MappingSegments();


        public HL7SegmentCategory _HL7SegmentCategory { get; set; }

        public HL7MappingSegment() { }

        public HL7MappingSegment(string sectionName, string segementName, int index, int subindex) : this()
        {
            this.SectionName = sectionName;
            this.SegmentName = segementName;
            this.Index_L1 = index;
            this.Index_L1 = subindex;
        }

        public HL7MappingSegment(string segmentName, string value, HL7MappingSegment parentSegment, int subindex) : this()
        {
            this.SegmentName = value;
            this.ParentSegment = parentSegment;
            this.ParentSegment.SubSegments.Add(this);
            this.Index_L1 = parentSegment.Index_L1;
            this.Index_L1 = subindex;
        }

        //public HL7MappingSegmentBase()
        //{ }

        //public HL7SegmentBase(string section, int index, int length, string elementName, string dt, string opt, string rp, bool collapsedDefault) : this()
        //{
        //    this.Index = index;
        //    //this.Length = length;
        //    this.SegmentName = elementName;
        //    //this.Dt = dt;
        //    //this.Opt = opt;
        //    //this.Rp = rp;
        //    this.CollapsedDefault = collapsedDefault;
        //}

        //public override string ToString()
        //{
        //    string str = this.SectionName + " / " + this.Index_L1 + " / " + this.SegmentName;
        //    return str;
        //}

        public string ToReport()
        {
            string str = this.SectionName + "\t" + this.Index_L1.ToString() + "\t" + this.Index_L1.ToString() + "\t" + this.SegmentName + "\t" + SegmentName;
            foreach (HL7MappingSegment child in this.SubSegments)
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
