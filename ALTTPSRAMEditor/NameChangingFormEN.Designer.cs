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
            this.kbdENCharA = new System.Windows.Forms.PictureBox();
            this.kbdENCharB = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.kbdENCharA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kbdENCharB)).BeginInit();
            this.SuspendLayout();
            // 
            // kbdENCharA
            // 
            this.kbdENCharA.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.kbdENCharA.Location = new System.Drawing.Point(56, 252);
            this.kbdENCharA.Name = "kbdENCharA";
            this.kbdENCharA.Size = new System.Drawing.Size(32, 32);
            this.kbdENCharA.TabIndex = 0;
            this.kbdENCharA.TabStop = false;
            this.kbdENCharA.Click += new System.EventHandler(this.kbdENCharA_Click);
            // 
            // kbdENCharB
            // 
            this.kbdENCharB.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.kbdENCharB.Location = new System.Drawing.Point(88, 252);
            this.kbdENCharB.Name = "kbdENCharB";
            this.kbdENCharB.Size = new System.Drawing.Size(32, 32);
            this.kbdENCharB.TabIndex = 1;
            this.kbdENCharB.TabStop = false;
            // 
            // NameChangingFormEN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::ALTTPSRAMEditor.Properties.Resources.filename_USA;
            this.ClientSize = new System.Drawing.Size(512, 729);
            this.Controls.Add(this.kbdENCharB);
            this.Controls.Add(this.kbdENCharA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "NameChangingFormEN";
            this.Text = "Change Player Name (USA/EUR)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NameChangingFormEN_FormClosed);
            this.Load += new System.EventHandler(this.NameChangingForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NameChangingForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.kbdENCharA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kbdENCharB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox kbdENCharA;
        private System.Windows.Forms.PictureBox kbdENCharB;
    }
}