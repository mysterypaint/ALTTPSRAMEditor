namespace ALTTPSRAMEditor
{
    partial class NameChangingFormJP
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
            // NameChangingFormJP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ALTTPSRAMEditor.Properties.Resources.filename_JP;
            this.ClientSize = new System.Drawing.Size(528, 232);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NameChangingFormJP";
            this.Text = "Change Player Name (JPN)";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NameChangingFormJP_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}