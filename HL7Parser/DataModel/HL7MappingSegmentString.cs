﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7MappingSegmentString : HL7MappingSegmentBase
    {
        public string Value { get; set; }

        public HL7SegmentCategory _HL7SegmentCategory { get; set; }

        public HL7MappingSegmentString() { }

        public HL7MappingSegmentString(string segmentName, string value, int index, int subindex) : this()
        {
            this.SegmentName = segmentName;
            this.Value = value;
            this.Index = index;
            this.SubIndex = subindex;
        }

        public HL7MappingSegmentString(string segmentName, string value, HL7MappingSegmentString parentSegment, int subindex) : this()
        {
            this.SegmentName = segmentName;
            this.Value = value;
            this.ParentSegment = parentSegment;
            this.ParentSegment.SubSegments.Add(this);
            this.Index = parentSegment.Index;
            this.SubIndex = subindex;
        }

        public string ToReport()
        {
            string str = this.SectionName + "\t" + this.Index.ToString() + "\t" + this.SubIndex.ToString() + "\t" + this.SegmentName + "\t" + Value;
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

            string str = this.SectionName + " / " + this.Index + " / " + this.SubIndex + " / " + this.SegmentName + " = " + Value + strSubsegments;
            return str;
        }

    }
}