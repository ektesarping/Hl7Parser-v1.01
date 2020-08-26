using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7MappingSegmentBase
    {
        // -----------------------------------------------------------------------------------------------------
        // Fields are taken from 
        // https://corepointhealth.com/resource-center/hl7-resources/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-pid-segment/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-pv1-patient-visit-information-segment/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-msh-message-header/
        // https://corepointhealth.com/resource-center/hl7-resources/hl7-obr-segment/
        // -----------------------------------------------------------------------------------------------------


        

        public int Index { get; set; }

        /// <summary>
        /// Subindex. Subnoder har index verdi > 0
        /// </summary>
        public int SubIndex { get; set; }

        public int Level { get; set; }

        //[Obsolete]
        //public int Length { get; set; }

        //[Obsolete]
        //public string Dt { get; set; }

        //[Obsolete]
        //public string Opt { get; set; }

        //[Obsolete]
        //public string Rp { get; set; }

        /// <summary>
        /// Collapsed default ved innlesing.
        /// </summary>
        public bool CollapsedDefault { get; set; }

        public string SegmentName { get; set; }

        public string SectionName { get; set; }

        public Hl7MappingSection MappingSection { get; set; }

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

        public HL7MappingSegmentBase ParentSegment { get; set; }

        public HL7MappingSegments SubSegments { get; set; } = new HL7MappingSegments();



        public HL7MappingSegmentBase()
        { }

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

        public override string ToString()
        {
            string str = this.SectionName + " / " + this.Index + " / " + this.SegmentName;
            return str;
        }
    }
}
