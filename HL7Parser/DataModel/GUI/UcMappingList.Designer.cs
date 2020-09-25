namespace HL7Viewer.DataModel.GUI
{
    partial class UcMappingList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColBtnRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.versionInfoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileInfoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mappingFileFullPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hL7MappingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.versionInfoDataGridViewTextBoxColumn,
            this.fileInfoDataGridViewTextBoxColumn,
            this.displayNameDataGridViewTextBoxColumn,
            this.mappingFileFullPathDataGridViewTextBoxColumn,
            this.ColBtnRemove});
            this.dataGridView1.DataSource = this.hL7MappingsBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(0, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(289, 180);
            this.dataGridView1.TabIndex = 0;
            // 
            // ColBtnRemove
            // 
            this.ColBtnRemove.DataPropertyName = "Name";
            this.ColBtnRemove.HeaderText = "Fjern mapping";
            this.ColBtnRemove.Name = "ColBtnRemove";
            this.ColBtnRemove.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // versionInfoDataGridViewTextBoxColumn
            // 
            this.versionInfoDataGridViewTextBoxColumn.DataPropertyName = "VersionInfo";
            this.versionInfoDataGridViewTextBoxColumn.HeaderText = "VersionInfo";
            this.versionInfoDataGridViewTextBoxColumn.Name = "versionInfoDataGridViewTextBoxColumn";
            this.versionInfoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fileInfoDataGridViewTextBoxColumn
            // 
            this.fileInfoDataGridViewTextBoxColumn.DataPropertyName = "FileInfo";
            this.fileInfoDataGridViewTextBoxColumn.HeaderText = "FileInfo";
            this.fileInfoDataGridViewTextBoxColumn.Name = "fileInfoDataGridViewTextBoxColumn";
            this.fileInfoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // displayNameDataGridViewTextBoxColumn
            // 
            this.displayNameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
            this.displayNameDataGridViewTextBoxColumn.HeaderText = "DisplayName";
            this.displayNameDataGridViewTextBoxColumn.Name = "displayNameDataGridViewTextBoxColumn";
            this.displayNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mappingFileFullPathDataGridViewTextBoxColumn
            // 
            this.mappingFileFullPathDataGridViewTextBoxColumn.DataPropertyName = "MappingFileFullPath";
            this.mappingFileFullPathDataGridViewTextBoxColumn.HeaderText = "MappingFileFullPath";
            this.mappingFileFullPathDataGridViewTextBoxColumn.Name = "mappingFileFullPathDataGridViewTextBoxColumn";
            this.mappingFileFullPathDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // hL7MappingsBindingSource
            // 
            this.hL7MappingsBindingSource.DataSource = typeof(HL7Viewer.DataModel.HL7Mappings);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(214, 183);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Lukk";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // UcMappingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dataGridView1);
            this.Name = "UcMappingList";
            this.Size = new System.Drawing.Size(293, 209);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn versionInfoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileInfoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mappingFileFullPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn ColBtnRemove;
        private System.Windows.Forms.BindingSource hL7MappingsBindingSource;
        private System.Windows.Forms.Button btnClose;
    }
}
