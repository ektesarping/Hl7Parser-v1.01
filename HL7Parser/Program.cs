using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Viewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormHL7Viewer());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Feil oppstod. \r\n\n" + ex.Message + "\r\n\n" + ex.StackTrace, "HL7 viewer" , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
