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
using HL7Viewer.DataModel.Msg;

namespace HL7Viewer.DataModel.GUI
{

    public partial class UcHL7 : UserControl
    {
        public HL7 _HL7 { get; set; } = new HL7();


        //public HL7Segments _HL7Segments { get; set; }
        public HL7SegmentCategories _HL7SegmentCategories { get; set; } // = new HL7SegmentCategories();

        public string RootnodeText { get; set; }


        public bool SkjulTomme
        {
            get
            {
                return Properties.Settings.Default.SkjulTommeNoder;
            }
            set
            {
                Properties.Settings.Default.SkjulTommeNoder = value;
                Properties.Settings.Default.Save();
                if (chkSkjulTomme.Checked != value)
                {
                    chkSkjulTomme.Checked = value;
                }
            }
        }


        /// <summary>
        /// Normalvisning. Collapser noder som er merket i mppingfilen.
        /// </summary>
        public bool Normalvisning
        {
            get
            {
                return Properties.Settings.Default.Normalvisning;
            }
            set
            {
                Properties.Settings.Default.Normalvisning = value;
                Properties.Settings.Default.Save();
                if (chkNormalVisning.Checked != value)
                {
                    chkNormalVisning.Checked = value;
                }
            }
        }


        public UcHL7()
        {
            InitializeComponent();
            //VisningsModusPropertyRead();
            SkjulTomme = false; // initiell verdi
        }

        #region -- Manage radiobuttons for visning av tomme noder --
        //public enum Visningsmodus
        //{
        //    VisAlle = 0,
        //    Normalvisning = 1,
        //    SkjulTomme = 2
        //}


        //public Visningsmodus _Visningsmodus { get; set; }


        //private void VisningsModusPropertyRead()
        //{
        //    if (Properties.Settings.Default.Visningsmodus == (int)Visningsmodus.Normalvisning)
        //    {
        //        _Visningsmodus = Visningsmodus.Normalvisning;
        //        if (!rbNormalvisning.Checked)
        //        {
        //            rbNormalvisning.Checked = true;
        //        }
        //    }
        //    else if (Properties.Settings.Default.Visningsmodus == (int)Visningsmodus.SkjulTomme)
        //    {
        //        _Visningsmodus = Visningsmodus.SkjulTomme;
        //        if (!rbSkjulTommeNoder.Checked)
        //        {
        //            rbSkjulTommeNoder.Checked = true;
        //        }
        //    }
        //    else
        //    {
        //        _Visningsmodus = Visningsmodus.VisAlle;
        //        if (!rbVisAlle.Checked)
        //        {
        //            rbVisAlle.Checked = true;
        //        }
        //    }
        //}

        //private void VisningsModusPropertyWrite()
        //{
        //    Properties.Settings.Default.Visningsmodus = (int)_Visningsmodus;
        //}


        #endregion -- Manage radiobuttons for visning av tomme noder --


        #region -- Populate --
        public void Populate(HL7SegmentCategories _hL7SegmentCategories)
        {
            Repopulate();
        }

        private void Repopulate()
        {
            this.tvHL7.Nodes.Clear();
            TreeNode root = new TreeNode();
            _HL7.msgRootnode.Treenode = root;

            root.Text = RootnodeText;
            this.tvHL7.Nodes.Add(root);

            foreach (MsgNode msgChildNode in _HL7.msgRootnode.Children)
            {
                TreenodeHL7Base treenode = new TreenodeHL7Base(msgChildNode, this.SkjulTomme, this.Normalvisning);

                // -- Legger kun til noden hvis den ikke skal vises -- 
                if (!treenode.NodeIsHidden)
                {
                    root.Nodes.Add(treenode);
                }
                PopulateRecursively(msgChildNode);
            }

            // Ekspanderer root node og neste level
            root.Expand();
        }

        private void PopulateRecursively(MsgNode node)
        {
            // -- MSH felt 0 inneholder skilletegn. Ignorer dette for denne noden --
            if (node.Parent.Name == "MSH" && node.Index_L2 == 0)
            {
                return;
            }

            foreach (MsgNode msgChildNode in node.Children)
            {
                if (node.Children.Count > 1)
                {
                    TreenodeHL7Base treenode = new TreenodeHL7Base(msgChildNode, this.SkjulTomme, this.Normalvisning);
                    if (!treenode.NodeIsHidden)
                    {
                        node.Treenode.Nodes.Add(treenode);
                        PopulateRecursively(msgChildNode);
                    }
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
            //this._HL7SegmentCategories = this._HL7._HL7SegmentCategories;
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
                fi = new FileInfo(Properties.Settings.Default.MsgFilename);

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
                Properties.Settings.Default.MsgFilename = _HL7.MsgFile.FullName;
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
            _HL7.ImportHL7MsgFile2(str);
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
            try
            {
                TreenodeHL7Base selectedNode = (TreenodeHL7Base)tvHL7.SelectedNode;

                if (selectedNode != null)
                {
                    HL7MappingSegmentString mappingSegment = selectedNode._HL7Segment;
                    if (mappingSegment != null)
                    {
                        string value = mappingSegment.SegmentName;
                        if (value != null)
                        {
                            Clipboard.SetText(value);
                        }
                    }
                }
            }
            catch (InvalidCastException ec)
            {
                MessageBox.Show("Ingen gyldig felt valgt. Prøv igjen.", "Kopiere Verdi til utklippstavlen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("En feil oppstod. Prøv igjen.", "Kopiere Verdi til utklippstavlen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void copyNavnOgVerdiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreenodeHL7Base selectedNode = (TreenodeHL7Base)tvHL7.SelectedNode;

                if (selectedNode != null)
                {
                    HL7MappingSegmentString mappingSegment = selectedNode._HL7Segment;
                    if (mappingSegment != null)
                    {
                        string value = mappingSegment.SegmentName;
                        if (value != null)
                        {
                            Clipboard.SetText(mappingSegment.SectionName + "\t" + mappingSegment.SegmentName + "\t" + value);
                        }
                    }
                }
            }
            catch (InvalidCastException ec)
            {
                MessageBox.Show("Ingen gyldig felt valgt. Prøv igjen.", "Kopiere Felt/Verdi til utklippstavlen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("En feil oppstod. Prøv igjen.", "Kopiere Felt/Verdi til utklippstavlen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //private void rbSkjulTommeNode_CheckedChanged(object sender, EventArgs e)
        //{
        //    _Visningsmodus = Visningsmodus.SkjulTomme;
        //    VisningsModusPropertyWrite();
        //    Repopulate();
        //}

        //private void rbNormalvisning_CheckedChanged(object sender, EventArgs e)
        //{
        //    _Visningsmodus = Visningsmodus.Normalvisning;
        //    VisningsModusPropertyWrite();
        //    Repopulate();
        //}

        //private void rbVisAlle_CheckedChanged(object sender, EventArgs e)
        //{
        //    _Visningsmodus = Visningsmodus.VisAlle;
        //    VisningsModusPropertyWrite();
        //    Repopulate();
        //}

        private void chkSkjulTomme_CheckedChanged(object sender, EventArgs e)
        {
            this.SkjulTomme = chkSkjulTomme.Checked;
            Repopulate();
        }

        private void chkNormalVisning_CheckedChanged(object sender, EventArgs e)
        {
            this.Normalvisning = chkNormalVisning.Checked;
            Repopulate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Ikke implementert");
        }
    }
}
