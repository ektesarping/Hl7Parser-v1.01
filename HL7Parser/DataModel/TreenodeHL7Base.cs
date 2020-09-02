using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Viewer.DataModel
{
    public class TreenodeHL7Base : TreeNode
    {
        public HL7MappingSegmentString _HL7Segment { get; set; }

        public TreenodeHL7Base(HL7MappingSegmentString segment)
        {
            this._HL7Segment = segment;
        }

        private Color FORECOLOR_DEFAULT = Color.Black;
        private Color FORECOLOR_IMPORTED_FROM_MESSAGE_FILE = Color.Blue;



        public void UpdateNodeText()
        {
            // Marker med farge at segmentdefinisjonen ikke ble funnet i mappingen.
            if (this._HL7Segment._HL7SegmentCategory.AddedFromMessageFile)
            {
                this.ForeColor = FORECOLOR_IMPORTED_FROM_MESSAGE_FILE;
            }
            else
            {
                this.ForeColor = FORECOLOR_DEFAULT;
            }

            this.Text = "[" + this._HL7Segment.SectionName + " " + this._HL7Segment.Index_L1.ToString() + ": " + this._HL7Segment.SegmentName + "]" + " =  " + this._HL7Segment.Value;
        }

        public void UpdateSubNodeText()
        {
            this.Text = "[" + this._HL7Segment.SectionName + " " + this._HL7Segment.Index_L1.ToString() + "." + this._HL7Segment.Index_L1.ToString() + ": " + this._HL7Segment.SegmentName + "]" + " =  " + this._HL7Segment.Value;
        }

    }
}
