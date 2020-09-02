using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7MappingSegments : List<HL7MappingSegmentString>
    {
        public HL7MappingSegments()
        { }

        public HL7MappingSegmentString GetSegment(string sectionName, int index_L1, int index_L2)
        {
            foreach (HL7MappingSegmentString segment in this)
            {
                if ((segment.SectionName == sectionName) && (segment.Index_L1 == index_L1) && (segment.Index_L1 == index_L2))
                {
                    return segment;
                }
                foreach (HL7MappingSegmentString subsegment in segment.SubSegments)
                {
                    if ((subsegment.SectionName == sectionName) && (subsegment.Index_L1 == index_L1) && (subsegment.Index_L1 == index_L2))
                    {
                        return subsegment;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Find segment with index/subindex. 
        /// NB! sectionname is 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public HL7MappingSegmentString GetOrCreateSegmentLevel1(string segmentName, string value, int index_L1, int index_L2)
        {
            // Segment på 1. nivå
            foreach (HL7MappingSegmentString segment in this)
            {
                if ((segment.Index_L1 == index_L1) && (segment.Index_L1 == index_L2))
                {
                    return segment;
                }
            }
            // Opprette nytt segment
            HL7MappingSegmentString newSegment = new HL7MappingSegmentString("!!" + segmentName + "!!", value, null, index_L2);
            this.Add(newSegment);
            return newSegment;
        }

        public HL7MappingSegmentString GetOrCreateSubSegment(HL7MappingSegmentString parentSegment, string segmentName, string value, int index, int subindex)
        {
            foreach (HL7MappingSegmentString subsegment in parentSegment.SubSegments)
            {
                if ((subsegment.Index_L1 == index) && (subsegment.Index_L1 == subindex))
                {
                    return subsegment;
                }
            }

            // Subsegment ikke funnet. Må opprette nytt.
            HL7MappingSegmentString segment = new HL7MappingSegmentString(segmentName, value, parentSegment, subindex);
            return segment;
        }


        public string ToReport()
        {
            string sep = "\t";
            string str = "Section name" + sep + "Index" + sep + "SubIndex" + sep + "Segment name" + sep + "Value" + "\r\n";
            foreach (HL7MappingSegmentString element in this)
            {
                str += element.ToReport() + "\r\n";
            }
            return str;
        }

        public override string ToString()
        {
            string str = String.Empty;
            foreach (HL7MappingSegmentString element in this)
            {
                str += element.ToString() + "\r\n";
            }
            return str;
        }
    }
}
