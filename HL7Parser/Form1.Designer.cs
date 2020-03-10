﻿namespace HL7Parser
{
    partial class FormHL7Viewer
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
            HL7Parser.DataModel.HL7 hL71 = new HL7Parser.DataModel.HL7();
            HL7Parser.DataModel.Hl7Mapping hl7Mapping1 = new HL7Parser.DataModel.Hl7Mapping();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHL7Viewer));
            this.ucHL71 = new HL7Parser.DataModel.GUI.UcHL7();
            this.SuspendLayout();
            // 
            // ucHL71
            // 
            hL71.Content = null;
            hL71.Fi = null;
            hl7Mapping1.Name = null;
            hl7Mapping1.VersionInfo = null;
            hL71.Mapping = hl7Mapping1;
            hL71.MappingFileFi = ((System.IO.FileInfo)(resources.GetObject("hL71.MappingFileFi")));
            hL71.MsgFile = null;
            hL71.Name = null;
            hL71.VersionInfo = null;
            this.ucHL71._HL7 = hL71;
            this.ucHL71._HL7SegmentCategories = null;
            this.ucHL71.AllowDrop = true;
            this.ucHL71.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucHL71.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHL71.Location = new System.Drawing.Point(0, 0);
            this.ucHL71.Name = "ucHL71";
            this.ucHL71.RootnodeText = null;
            this.ucHL71.Size = new System.Drawing.Size(552, 530);
            this.ucHL71.TabIndex = 0;
            this.ucHL71.DragDrop += new System.Windows.Forms.DragEventHandler(this.ucHL71_DragDrop);
            this.ucHL71.DragEnter += new System.Windows.Forms.DragEventHandler(this.ucHL71_DragEnter);
            // 
            // FormHL7Viewer
            // 
            this.AllowDrop = true;
            this.ClientSize = new System.Drawing.Size(552, 530);
            this.Controls.Add(this.ucHL71);
            this.Name = "FormHL7Viewer";
            this.Text = "Hemit HL7 viewer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copySegmentsToClipboardToolStripMenuItem;
        private DataModel.GUI.UcHL7 ucHL71;
    }
}
