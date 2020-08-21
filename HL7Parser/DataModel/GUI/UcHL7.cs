using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HL7Viewer.DataModel.GUI
{

    public partial class UcHL7 : UserControl
    {
        public HL7 _HL7 { get; set; } = new HL7();


        //public HL7Segments _HL7Segments { get; set; }
        public HL7SegmentCategories _HL7SegmentCategories { get; set; } // = new HL7SegmentCategories();

        public string RootnodeText { get; set; }

        public UcHL7()
        {
            InitializeComponent();
        }


        #region -- Populate --
        public void Populate(HL7SegmentCategories _hL7SegmentCategories)
        {
            this._HL7SegmentCategories = _hL7SegmentCategories;
            Repopulate();
        }

        private void Repopulate()
        {
            this.tvHL7.Nodes.Clear();
            TreeNode root = new TreeNode();
            root.Text = RootnodeText;

            this.tvHL7.Nodes.Add(root);

            foreach (HL7SegmentCategory cat in _HL7SegmentCategories)
            {
                TreeNode nodeCategory = new TreeNode(cat.CategoryName);
                cat.Treenode = nodeCategory;
                nodeCategory.Text = cat.CategoryName;
                root.Nodes.Add(nodeCategory);
                nodeCategory.Expand();

                foreach (HL7SegmentString segment in cat._HL7Segments)
                {
                    if (!((this.chkHideEmptyFields.Checked) && (string.IsNullOrEmpty(segment.Value))))
                    {
                        TreenodeHL7Base node = new TreenodeHL7Base(segment);
                        nodeCategory.Nodes.Add(node);
                        node.UpdateNodeText();
                        PopulateRecursively(node);
                    }
                }
            }

            // Ekspanderer root node og neste level
            root.Expand();
            foreach (HL7SegmentCategory cat in _HL7SegmentCategories)
            {
                cat.Treenode.Expand();
            }
        }

        private void PopulateRecursively(TreenodeHL7Base node)
        {
            foreach (HL7SegmentString subsegment in node._HL7Segment.SubSegments)
            {
                if (!((this.chkHideEmptyFields.Checked) && (string.IsNullOrEmpty(subsegment.Value))))
                {
                    TreenodeHL7Base childnode = new TreenodeHL7Base(subsegment);
                    node.Nodes.Add(childnode);
                    childnode.UpdateSubNodeText();
                    PopulateRecursively(childnode);
                }
            }
        }


        #endregion -- Populate --

        #region  -- File methods --
        public void OpenMessageFile(FileInfo fi)
        {
            this.RootnodeText = fi.FullName;

            _HL7.MsgFile = fi;
            _HL7.ImportHL7MsgFile(_HL7.MsgFile);
            this._HL7SegmentCategories = this._HL7._HL7SegmentCategories;
            this.Populate(this._HL7._HL7SegmentCategories);
        }

        /// <summary>
        /// Open message file with dialog.
        /// </summary>
        public void OpenMessageFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "HL7";
            dlg.Filter = "HL7 filer (*.HL7)|*.HL7|Alle filer (*.*)|*.*";

            FileInfo fi = null;
            try
            {
                fi = new FileInfo(Properties.Settings.Default.Filename);

                if (fi.Directory.Exists)
                {
                    dlg.InitialDirectory = fi.DirectoryName;
                }

                if (_HL7.MsgFile != null)
                {
                    if (_HL7.MsgFile.Exists)
                    {
                        dlg.InitialDirectory = _HL7.MsgFile.DirectoryName;
                    }
                }
            }
            catch (Exception ex)
            { }


            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                _HL7.MsgFile = new FileInfo(dlg.FileName);
                Properties.Settings.Default.Filename = _HL7.MsgFile.FullName;
                Properties.Settings.Default.Save();
                OpenMessageFile(_HL7.MsgFile);
            }
        }
        #endregion  -- File methods --


        #region -- Menu methods --
        private void åpneHL7FilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMessageFile();
        }

        private void kopierSegmenterTilClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this._HL7._HL7SegmentCategories.ToReport());
        }

        private void limInnHL7FilFraClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = Clipboard.GetText();
            _HL7.ImportHL7MsgFile(str);
            Populate(this._HL7._HL7SegmentCategories);
        }

        private void chkShowEmptyFields_CheckedChanged(object sender, EventArgs e)
        {
            Repopulate();
        }
        #endregion -- Menu methods --




        private void UcHL7_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(File)))
            {
                MessageBox.Show("File");
            }
            else
            {
                MessageBox.Show("Not File");
            }
        }

        private void tvHL7_DragEnter(object sender, DragEventArgs e)
        {
            // https://www.youtube.com/watch?v=OyKv1Xcod6c

            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void tvHL7_DragDrop(object sender, DragEventArgs e)
        {
            string fileName = String.Empty;
            try
            {
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                fileName = droppedFiles[0];
                FileInfo fi = new FileInfo(fileName);
                OpenMessageFile(fi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunne ikke åpne filen" + fileName + "\r\n" + ex.Message, "Drag drop file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreenodeHL7Base selectedNode = (TreenodeHL7Base)tvHL7.SelectedNode;

            if (selectedNode != null)
            {
                HL7SegmentString segment = selectedNode._HL7Segment;
                if (segment != null)
                {
                    string value = segment.Value;
                    if (value != null)
                    {
                        Clipboard.SetText(value);
                    }
                }
            }
        }

        private void copyNavnOgVerdiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreenodeHL7Base selectedNode = (TreenodeHL7Base)tvHL7.SelectedNode;

                if (selectedNode != null)
                {
                    HL7SegmentString segment = selectedNode._HL7Segment;
                    if (segment != null)
                    {
                        string value = segment.Value;
                        if (value != null)
                        {
                            Clipboard.SetText(segment.SectionName + "\t" + segment.SegmentName + "\t" + value);
                        }
                    }
                }
            }
            catch (InvalidCastException ec )
            {
                MessageBox.Show("Ingen gyldig felt valgt. Prøv igjen.", "Kopiere Felt/Verdi til utklippstavlen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
