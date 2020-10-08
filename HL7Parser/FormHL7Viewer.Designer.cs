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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHL7Viewer));
            this.ucHL7 = new HL7Viewer.DataModel.GUI.UcHL7();
            this.SuspendLayout();
            // 
            // ucHL7
            // 
            //this.ucHL7._HL7 = null;
            this.ucHL7._HL7SegmentCategories = null;
            this.ucHL7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucHL7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHL7.Location = new System.Drawing.Point(0, 0);
            this.ucHL7.Name = "ucHL7";
            this.ucHL7.RootnodeText = null;
            this.ucHL7.Size = new System.Drawing.Size(494, 321);
            this.ucHL7.TabIndex = 0;
            // 
            // FormHL7Viewer
            // 
            this.AllowDrop = true;
            this.ClientSize = new System.Drawing.Size(494, 321);
            this.Controls.Add(this.ucHL7);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormHL7Viewer";
            this.Text = "Hemit HL7 viewer";
            this.ResumeLayout(false);

        }

        private DataModel.GUI.UcHL7 ucHL7;

        #endregion

        //private System.Windows.Forms.MenuStrip menuStrip1;
        //private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        //private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        //private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        //private System.Windows.Forms.ToolStripMenuItem copySegmentsToClipboardToolStripMenuItem;
    }
}

