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
            this.ColBtnRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fjernToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.versionInfoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileInfoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mappingFileFullPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hL7MappingsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.hL7MappingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMappings)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMappings
            // 
            this.dgvMappings.AllowUserToAddRows = false;
            this.dgvMappings.AutoGenerateColumns = false;
            this.dgvMappings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.versionInfoDataGridViewTextBoxColumn,
            this.fileInfoDataGridViewTextBoxColumn,
            this.displayNameDataGridViewTextBoxColumn,
            this.mappingFileFullPathDataGridViewTextBoxColumn,
            this.ColBtnRemove});
            this.dgvMappings.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvMappings.DataSource = this.hL7MappingsBindingSource1;
            this.dgvMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMappings.Location = new System.Drawing.Point(0, 0);
            this.dgvMappings.Name = "dgvMappings";
            this.dgvMappings.ReadOnly = true;
            this.dgvMappings.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvMappings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMappings.Size = new System.Drawing.Size(636, 173);
            this.dgvMappings.TabIndex = 1;
            // 
            // ColBtnRemove
            // 
            this.ColBtnRemove.DataPropertyName = "Name";
            this.ColBtnRemove.HeaderText = "Fjern mapping";
            this.ColBtnRemove.Name = "ColBtnRemove";
            this.ColBtnRemove.ReadOnly = true;
            this.ColBtnRemove.Text = "Fjern";
            this.ColBtnRemove.Width = 79;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fjernToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // fjernToolStripMenuItem
            // 
            this.fjernToolStripMenuItem.Name = "fjernToolStripMenuItem";
            this.fjernToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.fjernToolStripMenuItem.Text = "Fjern";
            this.fjernToolStripMenuItem.Click += new System.EventHandler(this.fjernToolStripMenuItem_Click);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 60;
            // 
            // versionInfoDataGridViewTextBoxColumn
            // 
            this.versionInfoDataGridViewTextBoxColumn.DataPropertyName = "VersionInfo";
            this.versionInfoDataGridViewTextBoxColumn.HeaderText = "VersionInfo";
            this.versionInfoDataGridViewTextBoxColumn.Name = "versionInfoDataGridViewTextBoxColumn";
            this.versionInfoDataGridViewTextBoxColumn.ReadOnly = true;
            this.versionInfoDataGridViewTextBoxColumn.Width = 85;
            // 
            // fileInfoDataGridViewTextBoxColumn
            // 
            this.fileInfoDataGridViewTextBoxColumn.DataPropertyName = "FileInfo";
            this.fileInfoDataGridViewTextBoxColumn.HeaderText = "FileInfo";
            this.fileInfoDataGridViewTextBoxColumn.Name = "fileInfoDataGridViewTextBoxColumn";
            this.fileInfoDataGridViewTextBoxColumn.ReadOnly = true;
            this.fileInfoDataGridViewTextBoxColumn.Width = 66;
            // 
            // displayNameDataGridViewTextBoxColumn
            // 
            this.displayNameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
            this.displayNameDataGridViewTextBoxColumn.HeaderText = "DisplayName";
            this.displayNameDataGridViewTextBoxColumn.Name = "displayNameDataGridViewTextBoxColumn";
            this.displayNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.displayNameDataGridViewTextBoxColumn.Width = 94;
            // 
            // mappingFileFullPathDataGridViewTextBoxColumn
            // 
            this.mappingFileFullPathDataGridViewTextBoxColumn.DataPropertyName = "MappingFileFullPath";
            this.mappingFileFullPathDataGridViewTextBoxColumn.HeaderText = "MappingFileFullPath";
            this.mappingFileFullPathDataGridViewTextBoxColumn.Name = "mappingFileFullPathDataGridViewTextBoxColumn";
            this.mappingFileFullPathDataGridViewTextBoxColumn.ReadOnly = true;
            this.mappingFileFullPathDataGridViewTextBoxColumn.Width = 127;
            // 
            // hL7MappingsBindingSource1
            // 
            this.hL7MappingsBindingSource1.DataSource = typeof(HL7Viewer.DataModel.HL7Mappings);
            // 
            // hL7MappingsBindingSource
            // 
            this.hL7MappingsBindingSource.DataSource = typeof(HL7Viewer.DataModel.HL7Mappings);
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
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMappings;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn versionInfoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileInfoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mappingFileFullPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn ColBtnRemove;
        private System.Windows.Forms.BindingSource hL7MappingsBindingSource1;
        private System.Windows.Forms.BindingSource hL7MappingsBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fjernToolStripMenuItem;
    }
}