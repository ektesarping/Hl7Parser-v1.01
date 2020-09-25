namespace HL7Viewer.DataModel.GUI
{
    partial class UcHL7
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
            this.tvHL7 = new System.Windows.Forms.TreeView();
            this.contextMenuStripTreeview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyNavnOgVerdiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExpandSubnodes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCollapseSubnodes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripHL7 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.åpneHL7FilToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kopierSegmenterTilClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.limInnHL7FilFraClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kopierMappingsegmenterTilUtklippstavleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkSkjulTomme = new System.Windows.Forms.CheckBox();
            this.chkNormalVisning = new System.Windows.Forms.CheckBox();
            this.cboMappingFiles = new System.Windows.Forms.ComboBox();
            this.fjernMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTreeview.SuspendLayout();
            this.menuStripHL7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvHL7
            // 
            this.tvHL7.AllowDrop = true;
            this.tvHL7.ContextMenuStrip = this.contextMenuStripTreeview;
            this.tvHL7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvHL7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvHL7.Location = new System.Drawing.Point(0, 24);
            this.tvHL7.Name = "tvHL7";
            this.tvHL7.Size = new System.Drawing.Size(563, 390);
            this.tvHL7.TabIndex = 0;
            this.tvHL7.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvHL7_AfterSelect);
            this.tvHL7.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvHL7_DragDrop);
            this.tvHL7.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvHL7_DragEnter);
            // 
            // contextMenuStripTreeview
            // 
            this.contextMenuStripTreeview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.copyNavnOgVerdiToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItemExpandSubnodes,
            this.toolStripMenuItemCollapseSubnodes});
            this.contextMenuStripTreeview.Name = "contextMenuStripTreeview";
            this.contextMenuStripTreeview.Size = new System.Drawing.Size(272, 98);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.copyToolStripMenuItem.Text = "Kopier Verdi";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // copyNavnOgVerdiToolStripMenuItem
            // 
            this.copyNavnOgVerdiToolStripMenuItem.Name = "copyNavnOgVerdiToolStripMenuItem";
            this.copyNavnOgVerdiToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.copyNavnOgVerdiToolStripMenuItem.Text = "Kopier Navn og Verdi";
            this.copyNavnOgVerdiToolStripMenuItem.Click += new System.EventHandler(this.copyNavnOgVerdiToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(268, 6);
            // 
            // toolStripMenuItemExpandSubnodes
            // 
            this.toolStripMenuItemExpandSubnodes.Name = "toolStripMenuItemExpandSubnodes";
            this.toolStripMenuItemExpandSubnodes.Size = new System.Drawing.Size(271, 22);
            this.toolStripMenuItemExpandSubnodes.Text = "Ekspander noder under valgte node";
            this.toolStripMenuItemExpandSubnodes.Click += new System.EventHandler(this.toolStripMenuItemExpandSubnodes_Click);
            // 
            // toolStripMenuItemCollapseSubnodes
            // 
            this.toolStripMenuItemCollapseSubnodes.Name = "toolStripMenuItemCollapseSubnodes";
            this.toolStripMenuItemCollapseSubnodes.Size = new System.Drawing.Size(271, 22);
            this.toolStripMenuItemCollapseSubnodes.Text = "Slå sammen noder under valgte node";
            this.toolStripMenuItemCollapseSubnodes.Click += new System.EventHandler(this.toolStripMenuItemCollapseSubnodes_Click);
            // 
            // menuStripHL7
            // 
            this.menuStripHL7.AllowDrop = true;
            this.menuStripHL7.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.menuStripHL7.Location = new System.Drawing.Point(0, 0);
            this.menuStripHL7.Name = "menuStripHL7";
            this.menuStripHL7.Size = new System.Drawing.Size(563, 24);
            this.menuStripHL7.TabIndex = 2;
            this.menuStripHL7.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.åpneHL7FilToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // åpneHL7FilToolStripMenuItem
            // 
            this.åpneHL7FilToolStripMenuItem.Name = "åpneHL7FilToolStripMenuItem";
            this.åpneHL7FilToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.åpneHL7FilToolStripMenuItem.Text = "Åpne HL7 fil";
            this.åpneHL7FilToolStripMenuItem.Click += new System.EventHandler(this.åpneHL7FilToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kopierSegmenterTilClipboardToolStripMenuItem,
            this.limInnHL7FilFraClipboardToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // kopierSegmenterTilClipboardToolStripMenuItem
            // 
            this.kopierSegmenterTilClipboardToolStripMenuItem.Name = "kopierSegmenterTilClipboardToolStripMenuItem";
            this.kopierSegmenterTilClipboardToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.kopierSegmenterTilClipboardToolStripMenuItem.Text = "Kopier segmenter til clipboard";
            this.kopierSegmenterTilClipboardToolStripMenuItem.Click += new System.EventHandler(this.kopierSegmenterTilClipboardToolStripMenuItem_Click);
            // 
            // limInnHL7FilFraClipboardToolStripMenuItem
            // 
            this.limInnHL7FilFraClipboardToolStripMenuItem.Name = "limInnHL7FilFraClipboardToolStripMenuItem";
            this.limInnHL7FilFraClipboardToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.limInnHL7FilFraClipboardToolStripMenuItem.Text = "Lim inn HL7 fil fra clipboard";
            this.limInnHL7FilFraClipboardToolStripMenuItem.Click += new System.EventHandler(this.limInnHL7FilFraClipboardToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kopierMappingsegmenterTilUtklippstavleToolStripMenuItem,
            this.fjernMappingToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // kopierMappingsegmenterTilUtklippstavleToolStripMenuItem
            // 
            this.kopierMappingsegmenterTilUtklippstavleToolStripMenuItem.Name = "kopierMappingsegmenterTilUtklippstavleToolStripMenuItem";
            this.kopierMappingsegmenterTilUtklippstavleToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.kopierMappingsegmenterTilUtklippstavleToolStripMenuItem.Text = "Kopier Mappingsegmenter til utklippstavle";
            this.kopierMappingsegmenterTilUtklippstavleToolStripMenuItem.Click += new System.EventHandler(this.kopierMappingsegmenterTilUtklippstavleToolStripMenuItem_Click);
            // 
            // chkSkjulTomme
            // 
            this.chkSkjulTomme.AutoSize = true;
            this.chkSkjulTomme.Location = new System.Drawing.Point(138, 4);
            this.chkSkjulTomme.Name = "chkSkjulTomme";
            this.chkSkjulTomme.Size = new System.Drawing.Size(83, 17);
            this.chkSkjulTomme.TabIndex = 3;
            this.chkSkjulTomme.Text = "Skjul tomme";
            this.chkSkjulTomme.UseVisualStyleBackColor = true;
            this.chkSkjulTomme.CheckedChanged += new System.EventHandler(this.chkSkjulTomme_CheckedChanged);
            // 
            // chkNormalVisning
            // 
            this.chkNormalVisning.AutoSize = true;
            this.chkNormalVisning.Location = new System.Drawing.Point(221, 4);
            this.chkNormalVisning.Name = "chkNormalVisning";
            this.chkNormalVisning.Size = new System.Drawing.Size(92, 17);
            this.chkNormalVisning.TabIndex = 4;
            this.chkNormalVisning.Text = "Normalvisning";
            this.chkNormalVisning.UseVisualStyleBackColor = true;
            this.chkNormalVisning.CheckedChanged += new System.EventHandler(this.chkNormalVisning_CheckedChanged);
            // 
            // cboMappingFiles
            // 
            this.cboMappingFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMappingFiles.FormattingEnabled = true;
            this.cboMappingFiles.Location = new System.Drawing.Point(320, 2);
            this.cboMappingFiles.Name = "cboMappingFiles";
            this.cboMappingFiles.Size = new System.Drawing.Size(232, 21);
            this.cboMappingFiles.TabIndex = 5;
            this.cboMappingFiles.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // fjernMappingToolStripMenuItem
            // 
            this.fjernMappingToolStripMenuItem.Name = "fjernMappingToolStripMenuItem";
            this.fjernMappingToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.fjernMappingToolStripMenuItem.Text = "Fjern mapping";
            this.fjernMappingToolStripMenuItem.Click += new System.EventHandler(this.fjernMappingToolStripMenuItem_Click);
            // 
            // UcHL7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cboMappingFiles);
            this.Controls.Add(this.chkNormalVisning);
            this.Controls.Add(this.chkSkjulTomme);
            this.Controls.Add(this.tvHL7);
            this.Controls.Add(this.menuStripHL7);
            this.Name = "UcHL7";
            this.Size = new System.Drawing.Size(563, 414);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.UcHL7_DragDrop);
            this.contextMenuStripTreeview.ResumeLayout(false);
            this.menuStripHL7.ResumeLayout(false);
            this.menuStripHL7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvHL7;
        private System.Windows.Forms.MenuStrip menuStripHL7;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem åpneHL7FilToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kopierSegmenterTilClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem limInnHL7FilFraClipboardToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeview;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyNavnOgVerdiToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkSkjulTomme;
        private System.Windows.Forms.CheckBox chkNormalVisning;
        private System.Windows.Forms.ComboBox cboMappingFiles;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kopierMappingsegmenterTilUtklippstavleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExpandSubnodes;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCollapseSubnodes;
        private System.Windows.Forms.ToolStripMenuItem fjernMappingToolStripMenuItem;
    }
}
