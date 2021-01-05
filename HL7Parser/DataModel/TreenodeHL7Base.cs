using HL7Viewer.DataModel.Msg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HL7Viewer.DataModel.GUI.UcHL7;

namespace HL7Viewer.DataModel
{
    public class TreenodeHL7Base : TreeNode
    {
        public HL7MappingSegment _HL7Segment { get; set; }

        public MsgNode _MsgNode { get; set; }

        public Font _Font { get; set; }

        public bool _NodeIsHidden { get; set; }

        public TreenodeHL7Base(bool isRoot, Font font)
        {
            this._Font = font;
        }

        public TreenodeHL7Base(MsgNode msgNode, Font font) 
        {
            this._Font = font;
            this._HL7Segment = msgNode.MappingSegment;
            this.Text = msgNode.Value;
            this._MsgNode = msgNode;

            if (msgNode.ShowAsBoldFont)
            {
                this.NodeFont = new Font(this._Font.FontFamily, _Font.Size, FontStyle.Bold);
            }
            else
            {
                this.NodeFont = this._Font;
            }
            msgNode.Treenode = this;
        }

        public TreenodeHL7Base(MsgNode msgNode, bool skjulTomme, bool normalvisning, Font font) : this(msgNode, font)
        {
            // -- Opprett noden hvis den ikke skal skjules -- 
            if (!((skjulTomme) && (String.IsNullOrEmpty(msgNode.Value))))
            {
                //TreeNode treenode = new TreeNode();
                msgNode.Treenode = this;
                this._MsgNode = _MsgNode;

                this.Text = msgNode.TreeNodeText;

                // -- Collapse treenode hvis den er satt som default collapsed i mappingen --
                this.Expand();
                if (msgNode.MappingSegment != null)
                {
                    if ((normalvisning) && (msgNode.MappingSegment.CollapsedDefault))
                    {
                        this.Collapse();
                    }
                }
            }
            else
            {
                if (msgNode.ExtraLevelforRepeatingNodes == true)
                {
                    _NodeIsHidden = false; // Vis samlenode for repeterende noder uansett
                }
                else
                {
                    _NodeIsHidden = true;
                }
            }

            if ((msgNode.Level >= 1) && (msgNode.MappingSegment == null))
            {
                this.ForeColor = FORECOLOR_IMPORTED_FROM_MESSAGE_FILE;
            }

            if (!String.IsNullOrEmpty(_MsgNode.ErrorMsg))
            {
                this.ForeColor = FORECOLOR_VALIDATION_ERROR;
            }
        }


        private Color FORECOLOR_DEFAULT = Color.Black;
        private Color FORECOLOR_IMPORTED_FROM_MESSAGE_FILE = Color.Blue;
        private Color FORECOLOR_VALIDATION_ERROR = Color.Red;



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

            this.Text = "[" + this._HL7Segment.SectionName + " " + this._HL7Segment.Index_L1.ToString() + ": " + this._HL7Segment.SegmentName + "]" + " =  " + this._HL7Segment.SegmentName;
        }

        public void UpdateSubNodeText()
        {
            this.Text = "[" + this._HL7Segment.SectionName + " " + this._HL7Segment.Index_L1.ToString() + "." + this._HL7Segment.Index_L1.ToString() + ": " + this._HL7Segment.SegmentName + "]" + " =  " + this._HL7Segment.SegmentName;
        }

    }
}
