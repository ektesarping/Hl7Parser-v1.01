using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7Segments : List<HL7SegmentString>
    {

        public HL7Segments()
        { }

        public HL7SegmentString GetSegment(string sectionName, int index, int subindex)
        {
            foreach (HL7SegmentString segment in this)
            {
                if ((segment.SectionName == sectionName) && (segment.Index == index) && (segment.SubIndex == subindex))
                {
                    return segment;
                }
                foreach (HL7SegmentString subsegment in segment.SubSegments)
                {
                    if ((subsegment.SectionName == sectionName) && (subsegment.Index == index) && (subsegment.SubIndex == subindex))
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
        public HL7SegmentString GetOrCreateSegmentLevel1(string segmentName, string value, int index, int subindex)
        {
            // Segment på 1. nivå
            foreach (HL7SegmentString segment in this)
            {
                if ((segment.Index == index) && (segment.SubIndex == subindex))
                {
                    return segment;
                }
            }
            // Opprette nytt segment
            HL7SegmentString newSegment = new HL7SegmentString("!!" + segmentName + "!!", value, null, subindex);
            this.Add(newSegment);
            return newSegment;
        }

        public HL7SegmentString GetOrCreateSubSegment(HL7SegmentString parentSegment, string segmentName, string value, int index, int subindex)
        {
            foreach (HL7SegmentString subsegment in parentSegment.SubSegments)
            {
                if ((subsegment.Index == index) && (subsegment.SubIndex == subindex))
                {
                    return subsegment;
                }
            }

            // Subsegment ikke funnet. Må opprette nytt.
            HL7SegmentString segment = new HL7SegmentString(segmentName, value, parentSegment, subindex);
            return segment;
        }


        public string ToReport()
        {
            string sep = "\t";
            string str = "Section name" + sep + "Index" + sep + "SubIndex" + sep + "Segment name" + sep + "Value" + "\r\n";
            foreach (HL7SegmentString element in this)
            {
                str += element.ToReport() + "\r\n";
            }
            return str;
        }

        public override string ToString()
        {
            string str = String.Empty;
            foreach (HL7SegmentString element in this)
            {
                str += element.ToString() + "\r\n";
            }
            return str;
        }
    }
}
