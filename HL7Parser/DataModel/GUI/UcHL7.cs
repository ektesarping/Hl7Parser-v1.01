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
        public HL7 _HL7 { get; set; } //= new HL7();


        //public HL7Segments _HL7Segments { get; set; }
        public HL7SegmentCategories _HL7SegmentCategories { get; set; } // = new HL7SegmentCategories();

        public string RootnodeText { get; set; }

        private bool IsInitializingCheckboxes { get; set; } = false;


        private bool SkjulTomme
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
                    this.Repopulate();
                }
            }
        }


        /// <summary>
        /// Normalvisning. Collapser noder som er merket i mppingfilen.
        /// </summary>
        private bool Normalvisning
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
                    this.Repopulate();
                }
                
            }
            
        }


        public UcHL7()
        {
            InitializeComponent();
            //PostInitialize();
        }

        public void PostInitialize()
        { 
            this._HL7 = new HL7();
            _HL7.PostInitialize();


            // -- Åpne sist brukte HL7 fil --
            try
            {
                FileInfo fi = new FileInfo(Properties.Settings.Default.MsgFilename);
                if (fi.Exists)
                {
                    this.OpenMessageFile(fi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunne ikke åpne forrige meldingsfil.\r\n\nException\r\n" + ex.Message + "\r\n\nStacktrace\r\n" + ex.StackTrace , "Åpne forrige meldingsfil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.PopulateCboMappings();


            // -- Initialiserer status for checkboxer --
            IsInitializingCheckboxes = true;
            this.chkSkjulTomme.Checked = Properties.Settings.Default.SkjulTommeNoder;
            this.chkNormalVisning.Checked = Properties.Settings.Default.Normalvisning;
            IsInitializingCheckboxes = false;
            //PopulateCboMappings();
        }

        private const string TEXT_ADD_MAPPING = "Legg til mapping...";

        public void PopulateCboMappings()
        {
            // -- Fylle cbobox med mappinger --
            {
                cboMappingFiles.Items.Clear();
                foreach (Hl7Mapping mapping in this._HL7.HL7Mappings)
                {
                    cboMappingFiles.Items.Add((object)mapping);
                }

                // -- Setter selected mapping --
                if (this._HL7.MappingSelected != null)
                {
                    cboMappingFiles.SelectedItem = (object)this._HL7.MappingSelected;
                    cboMappingFiles.Text = this._HL7.MappingSelected.DisplayName;
                }

                // --  Hvis det kun er én mapping, sett denne som selected --


                // -- Legge til 'Add mapping file'
                cboMappingFiles.Items.Add(((object)TEXT_ADD_MAPPING));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboMappingFiles.SelectedItem.ToString() == (string)TEXT_ADD_MAPPING)
            {
                // -- Legger til mapping --
                DialogResult res = new DialogResult();
                OpenFileDialog dlg = new OpenFileDialog();

                // Prøver å finne directory for selected mapping.
                string dir = String.Empty;
                if (_HL7.MappingSelected != null)
                {
                    if (_HL7.MappingSelected.MappingFileFullPath != null)
                    {
                        FileInfo fi = new FileInfo(_HL7.MappingSelected.MappingFileFullPath);
                        dir = fi.DirectoryName;
                    }
                }
                if (!String.IsNullOrEmpty(dir))
                {
                    dlg.InitialDirectory = dir;
                }
                dlg.Filter = "Mapping filer (*.csv)|*.csv*|Alle filer (*.*)|*.*";

                res = dlg.ShowDialog();
                if (res == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(dlg.FileName);
                    if (fi.Exists)
                    {
                        // -- Import mapping --
                        Hl7Mapping newMapping = new Hl7Mapping(fi);
                        newMapping.ImportMapping();

                        this._HL7.HL7Mappings.Add(newMapping);
                        // -- Setter den innleste mappingen som selected --
                        this._HL7.MappingSelected = newMapping;
                        this._HL7.ImportHL7MsgFile();

                        this.PopulateCboMappings();
                        // --> Lagre fileinfi.Fullname i PRoperties.Settings.Default
                        this.Repopulate();
                    }
                    else
                    {
                        MessageBox.Show("Kunne ikke lese inn mappingen fra filen " + dlg.FileName, "Importere mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else
            {
                Hl7Mapping mappingTmp = _HL7.HL7Mappings.GetBydisplayName(this.cboMappingFiles.SelectedItem.ToString());
                if (mappingTmp != null)
                {
                    this._HL7.MappingSelected = mappingTmp;
                    this._HL7.ImportHL7MsgFile();
                    this.Repopulate();
                }
            }
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

        private TreenodeHL7Base SelectedTreenode { get; set; }

        private void Repopulate()
        {
            this.tvHL7.Nodes.Clear();
            TreenodeHL7Base rootNode = new TreenodeHL7Base(true);
            _HL7.msgRootnode.Treenode = rootNode;

            rootNode.Text = RootnodeText;
            this.tvHL7.Nodes.Add(rootNode);

            foreach (MsgNode msgChildNode in _HL7.msgRootnode.Children)
            {
                TreenodeHL7Base treenode = new TreenodeHL7Base(msgChildNode, this.SkjulTomme, this.Normalvisning);

                // -- Legger kun til noden hvis den ikke skal vises -- 
                if (!treenode.NodeIsHidden)
                {
                    rootNode.Nodes.Add(treenode);
                }
                PopulateRecursively(msgChildNode);
            }

            // Ekspanderer root node og kjører gjennom for de som skal være 
            rootNode.Expand();
            ManageExpandChildTreenode(rootNode);


            // -- Setter tilbake fokus på den selectede noden --
            if (SelectedTreenode != null)
            {
                TreenodeHL7Base nodeTmp = GetTreenode(SelectedTreenode.MsgNode);
                if (nodeTmp != null)
                {
                    tvHL7.SelectedNode = nodeTmp;
                }
                else
                {
                    // Fant ingen selectednode. 
                    if (tvHL7.Nodes.Count >= 1)
                    {
                        tvHL7.SelectedNode = tvHL7.Nodes[0];
                    }
                }
            }
            else
            {
                if (tvHL7.Nodes.Count >= 1)
                {
                    tvHL7.SelectedNode = tvHL7.Nodes[0];
                }
            }
        }



        /// <summary>
        /// Finner treenode som med samme MsgNode som argumentet node.
        /// Etter repopulate blir alle treenodes opprettet på nytt. for å 
        /// selecte fokus på den samme noden etter repoulate må det søkes gjennom alle treenodes.
        /// </summary>
        /// <param name="msgNode"></param>
        /// <returns></returns>
        private TreenodeHL7Base GetTreenode(MsgNode msgNode)
        {
            foreach (TreenodeHL7Base treenode in tvHL7.Nodes)
            {
                if (treenode._HL7Segment == null)
                {
                    continue;
                }

                if (treenode._HL7Segment.Level == msgNode.Level
                    && treenode._HL7Segment.Index_L1 == msgNode.Index_L1
                    && treenode._HL7Segment.Index_L2 == msgNode.Index_L2
                    )
                {
                    return treenode;
                }
            }
            return null;
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

        private void ManageExpandChildTreenode(TreenodeHL7Base node)
        {
            foreach (TreenodeHL7Base childnode in node.Nodes)
            {
                if (childnode._HL7Segment != null)
                {
                    if (childnode._HL7Segment.CollapsedDefault)
                    { childnode.Collapse(); }
                    else
                    { childnode.Expand(); }
                }
                else // root node
                {
                    childnode.Expand();
                }
                ManageExpandChildTreenode(childnode);
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

        public void ReloadMsgFile()
        {
            _HL7.ImportHL7MsgFile();
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
            try
            {
                TreenodeHL7Base selectedNode = (TreenodeHL7Base)tvHL7.SelectedNode;

                if (selectedNode != null)
                {
                    HL7MappingSegment mappingSegment = selectedNode._HL7Segment;
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
                    HL7MappingSegment mappingSegment = selectedNode._HL7Segment;
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


        private void chkSkjulTomme_CheckedChanged(object sender, EventArgs e)
        {
            this.SkjulTomme = chkSkjulTomme.Checked;
            if (!IsInitializingCheckboxes)
            {
                Repopulate();
            }
        }

        private void chkNormalVisning_CheckedChanged(object sender, EventArgs e)
        {
            this.Normalvisning = chkNormalVisning.Checked;
            if (!IsInitializingCheckboxes)
            {
                Repopulate();
            }
        }

        private void kopierMappingsegmenterTilUtklippstavleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = String.Empty;
            str = _HL7.MappingSelected.ToReport();
            Clipboard.SetText(str);
        }

        private void tvHL7_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedTreenode = (TreenodeHL7Base)tvHL7.SelectedNode;
        }
    }
}
