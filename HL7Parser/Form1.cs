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

        public FormHL7Viewer()
        {
            InitializeComponent();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            this.Text += " - V" + version;

            // -- Åpne sist brukte HL7 fil --
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
    }
}
