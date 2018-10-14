using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ALTTPSRAMEditor
{
    public partial class Form1 : Form
    {
        const int srm_size = 8 * 1024;
        const int srm_randomizer_size = 16 * 1024;
        const int srm_randomizer_size_2 = 32 * 1024;
        const int slot1 = 0xF00;
        const int slot2 = 0x1400;
        const int slot3 = 0x1900;
        const int mempointer = 0x1FFE;
        public int currsave = 00; // 00 - No File, 02 - File 1, 04 - File 2, 06 - File 3

        public Form1()
        {
            InitializeComponent();
        }

        private void opensrmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSRM();
        }

        private void OpenSRM()
        {
            OpenFileDialog fd1 = new OpenFileDialog();
            fd1.Filter = "SRAM|*.srm|All Files|*.*"; // Filter to show .srm files only.
            String fname = "";

            if (fd1.ShowDialog().Equals(DialogResult.OK))
            { // Prompt the user to open a file, then check if a valid file was opened.
                fname = fd1.FileName;

                try
                {   // Open the text file using a File Stream

                    byte[] bytes = File.ReadAllBytes(fname);
                    long fileSize = new System.IO.FileInfo(fname).Length;
                    if (fileSize == srm_randomizer_size || fileSize == srm_randomizer_size_2)
                        MessageBox.Show("Invalid SRAM File. (Randomizer saves aren't supported. Maybe one day...?)");
                    else if (fileSize == srm_size)
                    {
                        Console.WriteLine("Opened " + fname);
                        SRAM sdat = new SRAM(bytes);
                        radioFile1.Enabled = true;
                        radioFile2.Enabled = true;
                        radioFile3.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Invalid SRAM File.");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("The file could not be read:\n" + e.Message);
                }
            }

        }

        private void SaveSRM()
        {
            MessageBox.Show("teeeest");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Terminate the program if we select "Exit" in the Menu Bar
            System.Windows.Forms.Application.Exit();
        }

        private void saveCTRLSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSRM();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control) // Handle CTRL shortcuts
            {
                switch(e.KeyCode.ToString())
                {
                    case "O": // CTRL+O: Open file
                        OpenSRM();
                        break;
                    case "S": // CTRL+S: Save file
                        SaveSRM();
                        break;
                    default:
                        break;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tool_credits = "ALTTP SRAM Editor\n- Created by mysterypaint 2018\n\nSpecial thanks to alttp.run for the reverse-engineering documentation. http://alttp.run/hacking/index.php?title=SRAM_Map";
            MessageBox.Show(tool_credits);
        }
    }
}
