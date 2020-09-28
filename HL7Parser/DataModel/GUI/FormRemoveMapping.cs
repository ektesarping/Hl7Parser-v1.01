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

        private void fjernMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvMappings.SelectedRows)
            {
                rowsToRemove.Add(row);
            }

            

            try
            {
                foreach (DataGridViewRow row in rowsToRemove)  // dgvMappings.SelectedRows)
                {
                    string filename = row.Cells[2].FormattedValue.ToString();
                    Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
                    _Hl7Mappings.Remove(mappingToRemove);
                    //dgvMappings.Rows.Remove(row);
                }
                dgvMappings.DataSource = null;
                dgvMappings.DataSource = _Hl7Mappings;
            }
            catch (Exception ex)
            {
                int y = 0;
            }

            this.InitializeComponent();
            dgvMappings.Refresh();

            //dgvMappings.DataSource = null;
            //foreach (DataGridViewRow row in rowsToRemove)
            //{
            //    string filename = row.Cells[2].FormattedValue.ToString();
            //    Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
            //    _Hl7Mappings.Remove(mappingToRemove);
            //}

            //try
            //{
            //    for (int i = dgvMappings.SelectedRows.Count - 1; i >= 0; i--)
            //    {
            //        string filename = dgvMappings.SelectedRows[i].Cells[2].FormattedValue.ToString();
            //        Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
            //        _Hl7Mappings.Remove(mappingToRemove);
            //        dgvMappings.SelectedRows.
            //    }
            //}
            //catch (DataException ex)
            //{

            //}

            //foreach (DataGridViewRow row in dgvMappings.SelectedRows)
            //{
            //    string filename = row.Cells[2].FormattedValue.ToString();
            //    Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
            //    _Hl7Mappings.Remove(mappingToRemove);
            //}
        }


        private void dgvMappings_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //dgvMappings.Refresh(); // Make sure this comes first
            //dgvMappings.Parent.Refresh(); // Make sure this comes second
            dgvMappings.DataSource = this.hL7MappingsBindingSource1;
            //dgvMappings.DataBind();
            dgvMappings.Refresh();
        }
    }
}
