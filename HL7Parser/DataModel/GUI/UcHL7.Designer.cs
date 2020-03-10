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
            this.chkHideEmptyFields = new System.Windows.Forms.CheckBox();
            this.menuStripHL7 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.åpneHL7FilToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kopierSegmenterTilClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.limInnHL7FilFraClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTreeview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyNavnOgVerdiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripHL7.SuspendLayout();
            this.contextMenuStripTreeview.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvHL7
            // 
            this.tvHL7.AllowDrop = true;
            this.tvHL7.ContextMenuStrip = this.contextMenuStripTreeview;
            this.tvHL7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvHL7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tvHL7.Location = new System.Drawing.Point(0, 24);
            this.tvHL7.Name = "tvHL7";
            this.tvHL7.Size = new System.Drawing.Size(432, 220);
            this.tvHL7.TabIndex = 0;
            this.tvHL7.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvHL7_DragDrop);
            this.tvHL7.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvHL7_DragEnter);
            // 
            // chkHideEmptyFields
            // 
            this.chkHideEmptyFields.AutoSize = true;
            this.chkHideEmptyFields.Location = new System.Drawing.Point(94, 5);
            this.chkHideEmptyFields.Name = "chkHideEmptyFields";
            this.chkHideEmptyFields.Size = new System.Drawing.Size(109, 17);
            this.chkHideEmptyFields.TabIndex = 1;
            this.chkHideEmptyFields.Text = "Skjul tomme felter";
            this.chkHideEmptyFields.UseVisualStyleBackColor = true;
            this.chkHideEmptyFields.CheckedChanged += new System.EventHandler(this.chkShowEmptyFields_CheckedChanged);
            // 
            // menuStripHL7
            // 
            this.menuStripHL7.AllowDrop = true;
            this.menuStripHL7.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStripHL7.Location = new System.Drawing.Point(0, 0);
            this.menuStripHL7.Name = "menuStripHL7";
            this.menuStripHL7.Size = new System.Drawing.Size(432, 24);
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
            // contextMenuStripTreeview
            // 
            this.contextMenuStripTreeview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.copyNavnOgVerdiToolStripMenuItem});
            this.contextMenuStripTreeview.Name = "contextMenuStripTreeview";
            this.contextMenuStripTreeview.Size = new System.Drawing.Size(181, 70);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyToolStripMenuItem.Text = "Copy Value";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // copyNavnOgVerdiToolStripMenuItem
            // 
            this.copyNavnOgVerdiToolStripMenuItem.Name = "copyNavnOgVerdiToolStripMenuItem";
            this.copyNavnOgVerdiToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyNavnOgVerdiToolStripMenuItem.Text = "Copy NAme and Value";
            this.copyNavnOgVerdiToolStripMenuItem.Click += new System.EventHandler(this.copyNavnOgVerdiToolStripMenuItem_Click);
            // 
            // UcHL7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.chkHideEmptyFields);
            this.Controls.Add(this.tvHL7);
            this.Controls.Add(this.menuStripHL7);
            this.Name = "UcHL7";
            this.Size = new System.Drawing.Size(432, 244);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.UcHL7_DragDrop);
            this.menuStripHL7.ResumeLayout(false);
            this.menuStripHL7.PerformLayout();
            this.contextMenuStripTreeview.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvHL7;
        private System.Windows.Forms.CheckBox chkHideEmptyFields;
        private System.Windows.Forms.MenuStrip menuStripHL7;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem åpneHL7FilToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kopierSegmenterTilClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem limInnHL7FilFraClipboardToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeview;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyNavnOgVerdiToolStripMenuItem;
    }
}
