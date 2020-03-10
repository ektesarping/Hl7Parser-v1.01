using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7SegmentCategories : List<HL7SegmentCategory>
    {
        public HL7SegmentCategory Get(string sectionName)
        {
            foreach (HL7SegmentCategory cat in this)
            {
                if (cat.CategoryName == sectionName)
                {
                    return cat;
                }
            }
            return null;
        }

        //public HL7SegmentString Get(string sectionName, int index, int subindex)
        //{
        //    foreach (HL7SegmentCategory cat in this)
        //    {
        //        return cat._HL7Segments.GetSegment(sectionName, index, subindex);
        //    }
        //    return null;
        //}


        public string ToReport()
        {
            string str = String.Empty;
            foreach (HL7SegmentCategory cat in this)
            {
                str += cat._HL7Segments.ToReport();
            }
            return str;
        }
    }
}
