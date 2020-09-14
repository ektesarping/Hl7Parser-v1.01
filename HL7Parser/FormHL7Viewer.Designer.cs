namespace HL7Viewer
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
            HL7Viewer.DataModel.HL7 hL71 = new HL7Viewer.DataModel.HL7();
            HL7Viewer.DataModel.Hl7Mapping hl7Mapping1 = new HL7Viewer.DataModel.Hl7Mapping();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHL7Viewer));
            this.ucHL7 = new HL7Viewer.DataModel.GUI.UcHL7();
            this.SuspendLayout();
            // 
            // ucHL71
            // 
            hL71.Content = null;
            hL71.Fi = null;
            hl7Mapping1.Name = null;
            hl7Mapping1.VersionInfo = null;
            hL71.MappingSelected = hl7Mapping1;
            hL71.MappingFileFi = ((System.IO.FileInfo)(resources.GetObject("hL71.MappingFileFi")));
            hL71.MsgFile = null;
            hL71.Name = null;
            hL71.VersionInfo = null;
            this.ucHL7._HL7 = hL71;
            this.ucHL7._HL7SegmentCategories = null;
            this.ucHL7.AllowDrop = true;
            this.ucHL7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucHL7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHL7.Location = new System.Drawing.Point(0, 0);
            this.ucHL7.Name = "ucHL71";
            this.ucHL7.RootnodeText = null;
            this.ucHL7.Size = new System.Drawing.Size(552, 530);
            this.ucHL7.TabIndex = 0;
            // 
            // FormHL7Viewer
            // 
            this.AllowDrop = true;
            this.ClientSize = new System.Drawing.Size(552, 530);
            this.Controls.Add(this.ucHL7);
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
        private DataModel.GUI.UcHL7 ucHL7;
    }
}

