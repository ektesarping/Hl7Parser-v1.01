using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7MappingSegments : List<HL7MappingSegment>
    {
        public HL7MappingSegments()
        { }

        public HL7MappingSegment GetSegment(string sectionName, int index_L1, int index_L2)
        {
            foreach (HL7MappingSegment segment in this)
            {
                if ((segment.SectionName == sectionName) && (segment.Index_L1 == index_L1) && (segment.Index_L2 == index_L2))
                {
                    return segment;
                }
                foreach (HL7MappingSegment subsegment in segment.SubSegments)
                {
                    if ((subsegment.SectionName == sectionName) && (subsegment.Index_L1 == index_L1) && (subsegment.Index_L2 == index_L2))
                    {
                        return subsegment;
                    }
                }
            }
            return null;
        }

        public bool Contains(string sectionName, int index_L1, int index_L2)
        {
            if (this.GetSegment(sectionName, index_L1, index_L2) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public void Add(string sectionName, string value, int index_L1, int index_L2)
        //{
        //    if (!this.Contains(sectionName, index_L1, index_L2))
        //    {
        //        HL7MappingSegmentBase mappingSegmentNew = new HL7MappingSegmentBase(sectionName, value, index_L1, index_L2);
        //        base.Add(mappingSegmentNew);
        //    }
        //}

        public new void Add(HL7MappingSegment newSegment)
        {
            if (!this.Contains(newSegment.SectionName, newSegment.Index_L1, newSegment.Index_L2))
            {
                base.Add(newSegment);
            }
        }



        /// <summary>
        /// Find segment with index/subindex. 
        /// NB! sectionname is 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public HL7MappingSegment GetOrCreateSegmentLevel1(string segmentName, string value, int index_L1, int index_L2)
        {
            // Segment på 1. nivå
            foreach (HL7MappingSegment segment in this)
            {
                if ((segment.Index_L1 == index_L1) && (segment.Index_L1 == index_L2))
                {
                    return segment;
                }
            }
            // Opprette nytt segment
            HL7MappingSegment newSegment = new HL7MappingSegment("!!" + segmentName + "!!", value, null, index_L2);
            this.Add(newSegment);
            return newSegment;
        }

        public HL7MappingSegment GetOrCreateSubSegment(HL7MappingSegment parentSegment, string segmentName, string value, int index, int subindex)
        {
            foreach (HL7MappingSegment subsegment in parentSegment.SubSegments)
            {
                if ((subsegment.Index_L1 == index) && (subsegment.Index_L1 == subindex))
                {
                    return subsegment;
                }
            }

            // Subsegment ikke funnet. Må opprette nytt.
            HL7MappingSegment segment = new HL7MappingSegment(segmentName, value, parentSegment, subindex);
            return segment;
        }


        public string ToReport()
        {
            string sep = "\t";
            string str = "Section name" + sep + "Index" + sep + "SubIndex" + sep + "Segment name" + sep + "Value" + "\r\n";
            foreach (HL7MappingSegment element in this)
            {
                str += element.ToReport() + "\r\n";
            }
            return str;
        }

        public override string ToString()
        {
            string str = String.Empty;
            foreach (HL7MappingSegment element in this)
            {
                str += element.ToString() + "\r\n";
            }
            return str;
        }
    }
}
