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


        public HL7Mappings _Hl7Mappings { get; set; }
        //{
        //    get
        //    {
        //        //return (HL7Mappings)this.dataGridViewMappings.DataSource;
        //    }
        //    set
        //    {
        //        //this.dataGridViewMappings.DataSource = (HL7Mappings)value;
        //    }
        //}

        public FormRemoveMapping(HL7Mappings hl7Mappings)
        {
            InitializeComponent();
            this._Hl7Mappings = hl7Mappings;
            //dataGridViewTEST.Refresh();
            Populate();
        }


        private const int COL_DISPLAYNAME = 0;
        private const int COL_FILENAME = 1;
        private const int COL_CHKBOX = 2;


        public void Populate()
        {
            dataGridViewMappings.Rows.Clear();
            foreach (Hl7Mapping mapping in this._Hl7Mappings)
            {
                DataGridViewRow row = new DataGridViewRow();
                int n = dataGridViewMappings.Rows.Add(row);
                row.Tag = mapping;
                dataGridViewMappings.Rows[n].Cells[COL_DISPLAYNAME].Value = mapping.DisplayName;
                dataGridViewMappings.Rows[n].Cells[COL_FILENAME].Value = mapping.MappingFileFullPath;
            }
        }


        private void fjernMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //for (int i = dataGridViewMappings.Rows.Count - 1; i >= 0; i--)
            //{
            //    DataGridViewRow row = dataGridViewMappings.Rows[i];

            //    bool tmpChk = (bool)row.Cells[COL_CHKBOX].Value;
            //    if ((bool) row.Cells[COL_CHKBOX].Value == true)
            //    {
            //        Hl7Mapping mappingToRemove = (Hl7Mapping)row.Tag;
            //        dataGridViewMappings.Rows.Remove(row);
            //    }
            //}

            for (int i = dataGridViewMappings.SelectedRows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dataGridViewMappings.SelectedRows[i];

                Hl7Mapping mappingToRemove = (Hl7Mapping)row.Tag;
                this._Hl7Mappings.Remove(mappingToRemove);
                dataGridViewMappings.Rows.Remove(row);
            }


            #region -- fjernet --
            //    string filename = dataGridViewTEST.SelectedRows[i].Cells[2].FormattedValue.ToString();
            //Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
            //_Hl7Mappings.Remove(mappingToRemove);
            //dataGridViewTEST.SelectedRows.
            //}

            dataGridViewMappings.Refresh();


            //List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();
            //foreach (DataGridViewRow row in dataGridViewTEST.SelectedRows)
            //{
            //    rowsToRemove.Add(row);
            //}

            //try
            //{
            //    foreach (DataGridViewRow row in rowsToRemove)  // dataGridViewTEST.SelectedRows)
            //    {
            //        if (row.Cells.[COL_CHKBOX].Value == true)
            //        { 


            //        string filename = row.Cells[2].FormattedValue.ToString();
            //        Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
            //        _Hl7Mappings.Remove(mappingToRemove);
            //        //dataGridViewTEST.Rows.Remove(row);
            //    }
            //    dataGridViewTEST.DataSource = null;
            //    dataGridViewTEST.DataSource = _Hl7Mappings;
            //}
            //catch (Exception ex)
            //{
            //    int y = 0;
            //}

            //this.InitializeComponent();


            //dataGridViewTEST.DataSource = null;
            //foreach (DataGridViewRow row in rowsToRemove)
            //{
            //    string filename = row.Cells[2].FormattedValue.ToString();
            //    Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
            //    _Hl7Mappings.Remove(mappingToRemove);
            //}

            //try
            //{
            //    for (int i = dataGridViewTEST.SelectedRows.Count - 1; i >= 0; i--)
            //    {
            //        string filename = dataGridViewTEST.SelectedRows[i].Cells[2].FormattedValue.ToString();
            //        Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
            //        _Hl7Mappings.Remove(mappingToRemove);
            //        dataGridViewTEST.SelectedRows.
            //    }
            //}
            //catch (DataException ex)
            //{

            //}

            //foreach (DataGridViewRow row in dataGridViewTEST.SelectedRows)
            //{
            //    string filename = row.Cells[2].FormattedValue.ToString();
            //    Hl7Mapping mappingToRemove = this._Hl7Mappings.Get(filename);
            //    _Hl7Mappings.Remove(mappingToRemove);
            //}
            #endregion -- fjernet --
        }


        private void dataGridViewTEST_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //dataGridViewTEST.Refresh(); // Make sure this comes first
            //dataGridViewTEST.Parent.Refresh(); // Make sure this comes second
            dataGridViewMappings.DataSource = this.hL7MappingsBindingSource1;
            //dataGridViewTEST.DataBind();
            dataGridViewMappings.Refresh();
        }
    }
}
