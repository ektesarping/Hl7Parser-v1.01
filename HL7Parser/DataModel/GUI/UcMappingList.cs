﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Viewer.DataModel.GUI
{
    public partial class UcMappingList : UserControl
    {



        public UcMappingList()
        {
            InitializeComponent();
           // this.dgvMappings.DataSource = mappings;
        }



        private void Populate()
        {
            //this.dgvMappings.DataSource = mappings; // Ikke nødv. Satt i property.

        }
            

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
