using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALTTPSRAMEditor
{
    public partial class NameChangingForm : Form
    {
        public NameChangingForm()
        {
            InitializeComponent();
        }

        private void NameChangingForm_Load(object sender, EventArgs e)
        {
            // Name Changing Form Initialization

        }

        private void NameChangingForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Close the Name Changing form if we hit Escape
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}