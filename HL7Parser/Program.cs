using System;
using System.Collections.Generic;
using System.IO;
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
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FileInfo fi = null;
            try
            {
                if (args.Length == 1)
                {
                    try
                    {
                        fi = new FileInfo(args[0]);
                    }
                    catch
                    {
                        // Ikke gjør noe. Åpne formen uten parameter.
                    }
                }

                if (fi != null)
                {
                    Application.Run(new FormHL7Viewer(fi));
                }
                else
                {
                    Application.Run(new FormHL7Viewer(true));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Feil oppstod. \r\n\n" + ex.Message + "\r\n\n" + ex.StackTrace, "HL7 viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
