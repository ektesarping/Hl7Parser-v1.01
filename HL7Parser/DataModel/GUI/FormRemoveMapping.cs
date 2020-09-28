using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Viewer.DataModel.GUI
{
    public partial class FormRemoveMapping : Form
    {
        public HL7Mappings _Hl7Mappings
        {
            get 
            {
                return (HL7Mappings)this.dgvMappings.DataSource;
            }
            set
            {
                this.dgvMappings.DataSource = (HL7Mappings)value;
            }
        }

        public FormRemoveMapping(HL7Mappings hl7Mappings)
        {
            InitializeComponent();
            this._Hl7Mappings = hl7Mappings;
            dgvMappings.Refresh();
        }

        private void fjernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvMappings.SelectedRows)
            {
                string filename = row.Cells[2].FormattedValue.ToString();
                Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
                _Hl7Mappings.Remove(mappingToRemove);
            }
        }
    }
}
