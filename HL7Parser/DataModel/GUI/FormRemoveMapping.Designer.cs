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
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fjernMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hL7MappingsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.hL7MappingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewMappings = new System.Windows.Forms.DataGridView();
            this.ColName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFilename2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMappings)).BeginInit();
            this.SuspendLayout();
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
            // dataGridViewMappings
            // 
            this.dataGridViewMappings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMappings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName2,
            this.colFilename2});
            this.dataGridViewMappings.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridViewMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMappings.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMappings.Name = "dataGridViewMappings";
            this.dataGridViewMappings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMappings.Size = new System.Drawing.Size(561, 153);
            this.dataGridViewMappings.TabIndex = 4;
            // 
            // ColName2
            // 
            this.ColName2.HeaderText = "Navn";
            this.ColName2.Name = "ColName2";
            this.ColName2.Width = 58;
            // 
            // colFilename2
            // 
            this.colFilename2.HeaderText = "Filnavn";
            this.colFilename2.Name = "colFilename2";
            this.colFilename2.Width = 66;
            // 
            // FormRemoveMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 153);
            this.Controls.Add(this.dataGridViewMappings);
            this.Name = "FormRemoveMapping";
            this.Text = "FormRemoveMapping";
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hL7MappingsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMappings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource hL7MappingsBindingSource1;
        private System.Windows.Forms.BindingSource hL7MappingsBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fjernMappingToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewMappings;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilename2;
    }
}