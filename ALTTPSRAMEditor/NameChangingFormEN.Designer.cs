namespace ALTTPSRAMEditor
{
    partial class NameChangingFormEN
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
            this.SuspendLayout();
            // 
            // NameChangingFormEN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::ALTTPSRAMEditor.Properties.Resources.filename_USA;
            this.ClientSize = new System.Drawing.Size(256, 361);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "NameChangingFormEN";
            this.Text = "Change Player Name (USA/EUR)";
            this.Load += new System.EventHandler(this.NameChangingForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NameChangingForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}