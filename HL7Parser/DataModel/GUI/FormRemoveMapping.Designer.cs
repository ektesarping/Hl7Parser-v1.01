namespace HL7Viewer.DataModel.GUI
{
    partial class FormRemoveMapping
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvMappings = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fjernMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hL7MappingsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.hL7MappingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileFullPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColChkSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMappings
            // 
            this.dgvMappings.AllowUserToAddRows = false;
            this.dgvMappings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colFileFullPath,
            this.ColChkSelect});
            this.dgvMappings.ContextMenuStrip = this.contextMenuStrip2;
            this.dgvMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMappings.Location = new System.Drawing.Point(0, 0);
            this.dgvMappings.Name = "dgvMappings";
            this.dgvMappings.ReadOnly = true;
            this.dgvMappings.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvMappings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMappings.Size = new System.Drawing.Size(636, 173);
            this.dgvMappings.TabIndex = 1;
            this.dgvMappings.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvMappings_DataError);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fjernMappingToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(152, 26);
            // 
            // fjernMappingToolStripMenuItem
            // 
            this.fjernMappingToolStripMenuItem.Name = "fjernMappingToolStripMenuItem";
            this.fjernMappingToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.fjernMappingToolStripMenuItem.Text = "Fjern mapping";
            this.fjernMappingToolStripMenuItem.Click += new System.EventHandler(this.fjernMappingToolStripMenuItem_Click);
            // 
            // hL7MappingsBindingSource1
            // 
            this.hL7MappingsBindingSource1.DataSource = typeof(HL7Viewer.DataModel.HL7Mappings);
            // 
            // hL7MappingsBindingSource
            // 
            this.hL7MappingsBindingSource.DataSource = typeof(HL7Viewer.DataModel.HL7Mappings);
            // 
            // colName
            // 
            this.colName.HeaderText = "Navn";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 58;
            // 
            // colFileFullPath
            // 
            this.colFileFullPath.HeaderText = "Filnavn";
            this.colFileFullPath.Name = "colFileFullPath";
            this.colFileFullPath.ReadOnly = true;
            this.colFileFullPath.Width = 66;
            // 
            // ColChkSelect
            // 
            this.ColChkSelect.DataPropertyName = "Name";
            this.ColChkSelect.HeaderText = "Valg";
            this.ColChkSelect.Name = "ColChkSelect";
            this.ColChkSelect.ReadOnly = true;
            this.ColChkSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColChkSelect.Width = 34;
            // 
            // FormRemoveMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 173);
            this.Controls.Add(this.dgvMappings);
            this.Name = "FormRemoveMapping";
            this.Text = "FormRemoveMapping";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMappings;
        private System.Windows.Forms.BindingSource hL7MappingsBindingSource1;
        private System.Windows.Forms.BindingSource hL7MappingsBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fjernMappingToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileFullPath;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColChkSelect;
    }
}