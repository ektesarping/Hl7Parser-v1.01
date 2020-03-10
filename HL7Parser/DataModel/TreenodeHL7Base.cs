using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Parser.DataModel
{
    public class TreenodeHL7Base : TreeNode
    {
        public HL7SegmentString _HL7Segment { get; set; }

        public TreenodeHL7Base(HL7SegmentString segment)
        {
            this._HL7Segment = segment;
        }

        public void UpdateNodeText()
        {
            this.Text = "[" + this._HL7Segment.SectionName + " " + this._HL7Segment.Index.ToString() + ": " + this._HL7Segment.SegmentName + "]" + " =  " + this._HL7Segment.Value;
        }

        public void UpdateSubNodeText()
        {
            this.Text = "[" + this._HL7Segment.SectionName + " " + this._HL7Segment.Index.ToString() + "." + this._HL7Segment.SubIndex.ToString() + ": " + this._HL7Segment.SegmentName + "]" + " =  " + this._HL7Segment.Value;
        }

    }
}
