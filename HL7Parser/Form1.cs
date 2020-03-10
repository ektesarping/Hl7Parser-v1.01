using HL7Parser.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Parser
{
    public partial class FormHL7Viewer : Form
    {

        //public HL7 _HL7 { get; set; } = new HL7();

        //public FileInfo MsgFile { get; set; }

        //public FileInfo MappingFileFi { get; set; }
        //private const string MappingFileName = "Mapping_HL7.csv"; 

        public FormHL7Viewer()
        {
            InitializeComponent();

            //this.ucHL7TreeView1 = new DataModel.GUI.UcHL7();
            //this.ucHL7TreeView1 = new DataModel.GUI.UcHL7();

            //FileInfo executableFi = new FileInfo( Application.ExecutablePath);
            //MappingFileFi = new FileInfo(Path.Combine(executableFi.DirectoryName, "AdditionalFiles", MappingFileName));

            try
            {
                FileInfo fi = new FileInfo(Properties.Settings.Default.Filename);
                if (fi.Exists)
                {
                    this.ucHL71.OpenMessageFile(fi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunne ikke åpne forrige meldingsfil.", "Åpne forrige meldingsfil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Test()
        {
            //FileInfo fiMapping = new FileInfo(@"C:\DailyTemp\200225\HL7 Parameterlist 200226.txt");
            //_HL7.Mapping.ImportMapping(fiMapping);

            //FileInfo fiMsgFile = new FileInfo(@"C:\DailyTemp\200225\Eks2.hl7");
            //_HL7.ImportHL7MsgFile(fiMsgFile);

            //this.ucHL7TreeView1.RootnodeText = fiMsgFile.FullName;
            //this.ucHL7TreeView1._HL7SegmentCategories = this._HL7._HL7SegmentCategories;
            //this.ucHL7TreeView1.Populate();
        }

        private void ucHL71_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void ucHL71_DragEnter(object sender, DragEventArgs e)
        {

        }

        //private void OpenMessageFile(FileInfo fi)
        //{
        //    ucHL7TreeView1._HL7.Mapping.ImportMapping();

        //    //this._HL7 = new HL7();
        //    //FileInfo fiMapping = new FileInfo(@"C:\DailyTemp\200225\HL7 Parameterlist 200226.txt");
        //    //_HL7.Mapping.ImportMapping(fiMapping);

        //    this.ucHL7TreeView1.RootnodeText = fi.FullName;

        //    ucHL7TreeView1._HL7.MsgFile = fi;
        //    _HL7.ImportHL7MsgFile(MsgFile);
        //    this.ucHL7TreeView1._HL7SegmentCategories = this._HL7._HL7SegmentCategories;
        //    this.ucHL7TreeView1.Populate(this._HL7._HL7SegmentCategories);
        //}


        //private void OpenMessageFile()
        //{
        //    OpenFileDialog dlg = new OpenFileDialog();
        //    dlg.DefaultExt = "HL7";
        //    dlg.Filter = "HL7 filer (*.HL7)|*.HL7|Alle filer (*.*)|*.*";

        //    FileInfo fi = new FileInfo(Properties.Settings.Default.Filename);
        //    if (fi.Directory.Exists)
        //    {
        //        dlg.InitialDirectory = fi.DirectoryName;
        //    }

        //    if (MsgFile != null)
        //    {
        //        if (MsgFile.Exists)
        //        {
        //            dlg.InitialDirectory = MsgFile.DirectoryName;
        //        }
        //    }
        //    DialogResult res = dlg.ShowDialog();
        //    if (res== DialogResult.OK)
        //    {
        //        MsgFile = new FileInfo(dlg.FileName);
        //        Properties.Settings.Default.Filename = MsgFile.FullName;
        //        Properties.Settings.Default.Save();
        //        OpenMessageFile(MsgFile);
        //    }
        //}

        //private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    ucHL7TreeView1.OpenMessageFile();
        //}

        //private void copySegmentsToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Clipboard.SetText(this._HL7._HL7SegmentCategories.ToReport());
        //}
    }
}
