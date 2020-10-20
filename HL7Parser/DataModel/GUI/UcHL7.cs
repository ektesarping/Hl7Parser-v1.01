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
using System.Timers;
using System.Security.Permissions;

namespace HL7Viewer.DataModel.GUI
{
    public partial class UcHL7 : UserControl
    {
        public HL7 _HL7 { get; set; } //= new HL7();  // TODO: 201008 Fjern static hvis eventhandler fra FileUpdated ikke skal brukes

        public bool TooltipEnable
        {
            get
            {
                return Properties.Settings.Default.TooltipEnable;
            }
            set
            {
                Properties.Settings.Default.TooltipEnable = value;
                Properties.Settings.Default.Save();

                toolTipTreenode.Active = value;
            }
        }

        public string MsgFileFilename
        {
            get
            { return Properties.Settings.Default.MsgFilename; }

            set
            {
                Properties.Settings.Default.MsgFilename = value;
                Properties.Settings.Default.Save();
            }
        }


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

        private bool _debug;
        public bool DebugMode
        {
            get { return _debug; }
            set
            {
                _debug = value;
                debugToolStripMenuItem.Visible = _debug;
                this.Refresh();
            }
        }


        /// <summary>
        /// Normalvisning som settes i GUI av brukeren. Collapser noder som er merket i mppingfilen som CollapseDefault.
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
            this.DebugMode = false;

            #region  -- Splash screen --
            // --  Sjekk om splash screen skal vises (Vises kun første gang på den aktuelle dagen)  --
            if (!((Properties.Settings.Default.SplashScreenDisplayedDate.Year == DateTime.Now.Year) &&
                (Properties.Settings.Default.SplashScreenDisplayedDate.Month == DateTime.Now.Month) &&
                (Properties.Settings.Default.SplashScreenDisplayedDate.Day == DateTime.Now.Day)))
            {
                // -- Vis splash screen --
                SetTimerSplashScreen();
                Properties.Settings.Default.SplashScreenDisplayedDate = DateTime.Now;
                Properties.Settings.Default.Save();
            }
            #endregion  -- Splash screen --
        }

        #region --  Timer splash screen -- 
        private System.Timers.Timer aTimer;
        private UcSplashScreen ucSplashScreen;

        private void SetTimerSplashScreen()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(Properties.Settings.Default.SplashScreenDelay);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;

            SplashScreenShow();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            SplashScreenHide();
            aTimer.Enabled = false;
            aTimer.Stop();
        }

        private void resetSplashscreenTimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime newDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            Properties.Settings.Default.SplashScreenDisplayedDate = newDate;
            Properties.Settings.Default.Save();
        }

        private void visSplashScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTimerSplashScreen();
        }

        public void SplashScreenShow()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(SplashScreenShow));
                return;
            }
            else
            {
                ucSplashScreen = new UcSplashScreen();
                this.Controls.Add(ucSplashScreen);
                ucSplashScreen.Location = new Point(60, 60);
                ucSplashScreen.Show();
                ucSplashScreen.BringToFront();
                ucSplashScreen.Refresh();
                this.Refresh();
            }
        }


        public void SplashScreenHide()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(SplashScreenHide));
                return;
            }
            else
            {
                ucSplashScreen.Hide();
                if (this.Controls.Contains(ucSplashScreen))
                {
                    this.Controls.Remove(ucSplashScreen);
                }
            }
        }


        #endregion --  Timer splash screen -- 



        public void PostInitialize(bool openPreviousMsgFile)
        {
            this._HL7 = new HL7();
            this._HL7.PostInitialize();
            //AttachFileWatchForMappingFiles(); // TODO: 201008 Fjernes hvis FileSystemWatcher ikke skal brukes.

            this._HL7._UcHl7 = this; // Hack for å få tilgang til metoder i denne usercontrollen etter at mappingfil/meldingsfil er endret. (Automatisk reloading)

            this.PopulateCboMappings();

            // -- Henter lagret verdi for tooltip enabled --
            toolTipToolStripMenuItem.Checked = TooltipEnable;

            // -- Åpne sist brukte HL7 fil --
            // Sjekker først at MsgFilename ikke er tomt.
            if (!String.IsNullOrWhiteSpace(this.MsgFileFilename) && (!String.IsNullOrEmpty(this.MsgFileFilename)))
            {
                try
                {
                    FileInfo fi = new FileInfo(this.MsgFileFilename);
                    Properties.Settings.Default.Save();
                    if (fi.Exists)
                    {
                        this.OpenMessageFile(fi);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunne ikke åpne forrige meldingsfil.\r\n\nException\r\n" + ex.Message + "\r\n\nStacktrace\r\n" + ex.StackTrace, "Åpne forrige meldingsfil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

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
                }
                else
                {
                    // --  Hvis det kun er én mapping, sett denne som selected --
                    if (this._HL7.HL7Mappings.Count > 0)
                    {
                        this._HL7.MappingSelected = this._HL7.HL7Mappings[0];
                        MessageBox.Show("Ingen mapping var valgt. Antar mapping " + this._HL7.MappingSelected.Name, "Lese inn mapping fil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // -- Sette inn navnt på valgte mapping i comboboksen --
                cboMappingFiles.Text = this._HL7.MappingSelected.DisplayName;

                // -- Legge til 'Add mapping file' i bunnen av 
                cboMappingFiles.Items.Add(((object)TEXT_ADD_MAPPING));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboMappingFiles.SelectedItem != null)
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
                            //newMapping.ImportMapping();

                            _HL7.HL7Mappings.Add(newMapping);
                            // -- Setter den innleste mappingen som selected --
                            _HL7.MappingSelected = newMapping;
                            _HL7.ImportHL7MsgFile();

                            this.PopulateCboMappings();
                            // --> Lagre fileinfi.Fullname i PRoperties.Settings.Default
                            this.Repopulate();
                        }
                        else
                        {
                            MessageBox.Show("Kunne ikke lese inn mappingen fra filen " + dlg.FileName, "Importere mapping (Kode 201016-0840)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    Hl7Mapping mappingTmp = _HL7.HL7Mappings.GetBydisplayName(this.cboMappingFiles.SelectedItem.ToString());
                    if (mappingTmp != null)
                    {
                        _HL7.MappingSelected = mappingTmp;
                        _HL7.ImportHL7MsgFile();
                        this.Repopulate();
                    }
                }
            }
            else
            {
                MessageBox.Show("Ingen mapping valgt. (Kode 201016-0830)", "Valg av mapping", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (this.cboMappingFiles.SelectedItem == null)
            {
                Hl7Mapping mappingTmp = _HL7.HL7Mappings[0]; //  .GetBydisplayName(this.cboMappingFiles.SelectedItem.ToString());
                if (mappingTmp != null)
                {
                    _HL7.MappingSelected = mappingTmp;
                    _HL7.ImportHL7MsgFile();
                    this.Repopulate();
                }
            }
        }


        #region -- Populate --
        public void Populate(HL7SegmentCategories _hL7SegmentCategories)
        {
            Repopulate();
        }

        private TreenodeHL7Base SelectedTreenode { get; set; }

        private void Repopulate()
        {
            tvHL7.SuspendLayout();
            this.tvHL7.Nodes.Clear();
            TreenodeHL7Base rootNode = new TreenodeHL7Base(true);
            this._HL7.MsgRootnode.Treenode = rootNode;
            rootNode.MsgNode = this._HL7.MsgRootnode;

            rootNode.Text = RootnodeText;
            this.tvHL7.Nodes.Add(rootNode);

            foreach (MsgNode msgChildNode in this._HL7.MsgRootnode.Children)
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
            tvHL7.ResumeLayout();
        }



        /// <summary>
        /// Finner treenode med samme MsgNode som argumentet node.
        /// Etter repopulate blir alle treenodes opprettet på nytt. For å 
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
                    if (childnode._HL7Segment.CollapsedDefault && Normalvisning == true) // CollapseDefault er satt i mappingen, Normalvisning valgt av brukeren i checkbox.
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

            this.MsgFileFilename = fi.FullName;
            this._HL7.MsgFile = fi;
            this._HL7.ImportHL7MsgFile(this._HL7.MsgFile);
            //_HL7SegmentCategories = this._HL7._HL7SegmentCategories;
            this.Populate(this._HL7._HL7SegmentCategories);
        }

        public void ReloadMsgFile()
        {
            this._HL7.ImportHL7MsgFile();
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
                fi = new FileInfo(this.MsgFileFilename);

                if (fi.Directory.Exists)
                {
                    dlg.InitialDirectory = fi.DirectoryName;
                }

                if (this._HL7.MsgFile != null)
                {
                    if (this._HL7.MsgFile.Exists)
                    {
                        dlg.InitialDirectory = this._HL7.MsgFile.DirectoryName;
                    }
                }
            }
            catch (Exception ex)
            { }


            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                this._HL7.MsgFile = new FileInfo(dlg.FileName);
                this.MsgFileFilename = this._HL7.MsgFile.FullName;
                //Properties.Settings.Default.MsgFilename = this._HL7.MsgFile.FullName;
                //Properties.Settings.Default.Save();
                OpenMessageFile(this._HL7.MsgFile);
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
            string str = this._HL7.ToReport();
            Clipboard.SetText(str);
        }

        private void limInnHL7FilFraClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = Clipboard.GetText();
            this._HL7.ImportHL7MsgFile(str);
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

                if (fi.Extension.ToLower() == ".csv")
                {
                    Hl7Mapping newMapping = this._HL7.HL7Mappings.ImportMapping(fi);
                    this._HL7.MappingSelected = newMapping;
                    PopulateCboMappings();
                }
                else
                {
                    OpenMessageFile(fi);
                    this.MsgFileFilename = this._HL7.MsgFile.FullName;
                    //Properties.Settings.Default.MsgFilename = _HL7.MsgFile.FullName;
                    //Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunne ikke åpne filen" + fileName + "\r\n" + ex.Message, "Drag drop file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreenodeHL7Base selectedNode = (TreenodeHL7Base)tvHL7.SelectedNode;

                if (selectedNode != null)
                {
                    HL7MappingSegment mappingSegment = selectedNode._HL7Segment;
                    if (mappingSegment != null)
                    {
                        //string value = mappingSegment.SegmentName;
                        string value = selectedNode.MsgNode.Value;
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
            string str = this._HL7.MappingSelected.ToReportHeading();
            str += this._HL7.MappingSelected.ToReport();
            Clipboard.SetText(str);
        }

        private void tvHL7_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedTreenode = (TreenodeHL7Base)tvHL7.SelectedNode;
        }

        private void toolStripMenuItemExpandSubnodes_Click(object sender, EventArgs e)
        {
            TreeNode nodeTmp = tvHL7.SelectedNode;
            nodeTmp.ExpandAll();
        }

        private void toolStripMenuItemCollapseSubnodes_Click(object sender, EventArgs e)
        {
            TreeNode nodeTmp = tvHL7.SelectedNode;
            nodeTmp.Collapse(false);
        }

        private void fjernMappingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormRemoveMapping formRemoveMapping = new FormRemoveMapping(this._HL7.HL7Mappings);
            formRemoveMapping.Show();
        }

        private void tvHL7_KeyDown(object sender, KeyEventArgs e)
        {
            ManageKeyEvents(sender, e);
        }

        private void UcHL7_KeyDown(object sender, KeyEventArgs e)
        {
            ManageKeyEvents(sender, e);
        }

        private void ManageKeyEvents(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.Alt && e.KeyCode == Keys.D)
            {
                this.DebugMode = !this.DebugMode;
            }
        }

        #region  -- Filesystem watcher --
        /// <summary>
        /// Setter opp abonnement på fileWatcher event for mappingfiler.
        /// </summary>
        //private void AttachFileWatchForMappingFiles()
        //{
        //    foreach (Hl7Mapping mapping in this._HL7.HL7Mappings)
        //    {
        //        AddWatchOnDirectory(mapping.FileInfo.DirectoryName, ".csv");
        //    }
        //}


        /// <summary>
        /// Setter opp abonnement på fileWatcher event ved oppdatering av gjeldende msgfile.
        /// </summary>
        /// <param name="fi"></param>
        //private void AttachWatchForMsgFile(FileInfo fi)
        //{
        //    AddWatchOnDirectory(fi.DirectoryName, fi.Extension);
        //}



        //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        //private static void AddWatchOnDirectory(string path, string fileFilter)
        //{
        //    // https://docs.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=netcore-3.1

        //    // Create a new FileSystemWatcher and set its properties.
        //    FileSystemWatcher watcher = new FileSystemWatcher();
        //    watcher.Path = path;
        //    /* Watch for changes in LastAccess and LastWrite times, and 
        //       the renaming of files or directories. */
        //    watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
        //       | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        //    // Only watch text files.
        //    watcher.Filter = fileFilter;

        //    // Add event handlers.
        //    watcher.Changed += new FileSystemEventHandler(OnChanged);
        //    watcher.Created += new FileSystemEventHandler(OnChanged); // Kommenteres ut
        //    watcher.Deleted += new FileSystemEventHandler(OnChanged); // Kommenteres ut
        //    watcher.Renamed += new RenamedEventHandler(OnChanged);    // Kommenteres ut

        //    // Begin watching.
        //    watcher.EnableRaisingEvents = true;

        //}

        //private static void OnChanged(object source, FileSystemEventArgs e)
        //{
        //    string path = e.FullPath;
        //    try
        //    {
        //        FileInfo fi = new FileInfo(e.FullPath);

        //        // -- Les inn mapping på nytt --
        //        if (fi.Extension.ToLower() == ".csv")
        //        {
        //            if (_HL7.MappingSelected.FileInfo.FullName.ToLower() == fi.FullName.ToLower())
        //            {
        //                Hl7Mapping newMapping = this._HL  _HL7.HL7Mappings.ImportMapping(fi);
        //                this._HL7.MappingSelected = newMapping;
        //                //this._HL7.PopulateCboMappings();

        //                // -- Repoulate msg fil --
        //                this._HL7._UcHl7.Repopulate();  // HACK: Gå veien om _HL7 klassen som er static for å referere UcHL7. UCHl7 er referert i property i Hl7 klassen som er static.
        //            }
        //        }
        //        else if (fi.Extension.ToLower() == ".hl7")
        //        {
        //            this._HL7._UcHl7.OpenMessageFile(fi); ;  // HACK: Gå veien om _HL7 klassen som er static for å referere UcHL7. UCHl7 er referert i property i Hl7 klassen som er static.
        //            //Properties.Settings.Default.MsgFilename = this._HL7.MsgFile.FullName;
        //            //Properties.Settings.Default.Save();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Kontroller at mapping og meldingsfil er korrekt valgt (Kode 201006-1)", "Intern feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }

        //}

        #endregion  -- Filesystem watcher --

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvHL7.SuspendLayout();
            //             // -- Alt 1: Les inn MappingSelected fra fil på nytt --
            //             this._HL7.MappingSelected = this._HL7.MappingSelected.ImportMapping();

            // -- Alt 2: Les inn alle mappinger på nytt --
            foreach (Hl7Mapping mapping in this._HL7.HL7Mappings)
            {
                mapping.ImportMapping();
            }

            // Leser inn meldingsfilen på nytt for å oppdatere mot oppdatert mapping.
            this._HL7.ImportHL7MsgFile();

            // Oppdater combobox i tilfelle DisplayName er oppdatert.
            this._HL7._UcHl7.PopulateCboMappings();

            // -- Repoulate msg fil --
            this._HL7._UcHl7.Repopulate();  // HACK: Gå veien om _HL7 klassen som er static for å referere UcHL7. UCHl7 er referert i property i Hl7 klassen som er static.

            tvHL7.ResumeLayout();
        }


        private int TOOLTIP_LINE_LENGTH_BEFORE_LINEBREAK = 80;
        private int MIN_LINE_LENGHT_TO_SHOW_TOOLTIP = 5; //40;
        private void tvHL7_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            if (TooltipEnable == true)
            {
                try
                {
                    TreenodeHL7Base node = (TreenodeHL7Base)e.Node;
                    //activeTooltipnode = (TreenodeHL7Base)e.Node;

                    string strTmp = node.MsgNode.ErrorMsg;
                    if (!string.IsNullOrEmpty(strTmp))
                    {
                        strTmp += "\r\n"; // Legger til lnjeskift hvis noden har error status.
                    }

                    strTmp += HL7.InsertLinebreaks(node.MsgNode.Value, TOOLTIP_LINE_LENGTH_BEFORE_LINEBREAK);
                    if (strTmp.Length > MIN_LINE_LENGHT_TO_SHOW_TOOLTIP)
                    {
                        toolTipTreenode.RemoveAll();
                        toolTipTreenode.SetToolTip(tvHL7, strTmp); //  MsgNode.Value);
                    }
                    else
                    {
                        toolTipTreenode.RemoveAll();
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Exception (Ikke kritisk): " + ex.Message + "\r\n\n" + ex.StackTrace, "Internal warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void tvHL7_MouseLeave(object sender, EventArgs e)
        {
            //toolTipTreenode.RemoveAll();
        }

        private void toolTipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TooltipEnable = !TooltipEnable;
            toolTipToolStripMenuItem.Checked = TooltipEnable;
        }
    }
}
