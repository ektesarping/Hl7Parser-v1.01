using HL7Viewer.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Viewer
{
    public partial class FormHL7Viewer : Form
    {

        public FormHL7Viewer(bool openPreviousMsgFile)
        {
            InitializeComponent();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            this.Text += " - V" + version;

            ucHL7.PostInitialize(openPreviousMsgFile);
        }

        public FormHL7Viewer(FileInfo fi) : this(false)
        {
            Properties.Settings.Default.MsgFilename = fi.FullName;
            Properties.Settings.Default.Save();
            this.ucHL7.OpenMessageFile(fi);
        }
    }
}
