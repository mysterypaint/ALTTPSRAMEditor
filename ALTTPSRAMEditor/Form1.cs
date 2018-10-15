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
        const int srm_size = 0x2000;
        const int srm_randomizer_size = 16 * 1024;
        const int srm_randomizer_size_2 = 32 * 1024;
        static SRAM sdat;
        static String fname = "";

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
                        helperText.Text = "Opened " + fname;
                        sdat = new SRAM(bytes);
                        radioFile1.Enabled = true;
                        radioFile2.Enabled = true;
                        radioFile3.Enabled = true;
                        buttonCopy.Enabled = true;
                        buttonErase.Enabled = true;
                        updatePlayerName(1);
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

        private void UpdateFilename(String str)
        {
            fileNameBox.Text = str;
        }

        private void SaveSRM()
        {
            if (fname.Equals("") || fname.Equals(null))
            {
                helperText.Text = "Load a file first!";
                return; // Abort saving if there isn't a valid file open.
            }
            byte[] outputData = sdat.MergeSaveData();
            File.WriteAllBytes(fname, outputData);
            helperText.Text = "Saved file at " + fname;
            buttonWrite.Enabled = false;
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

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (radioFile1.Checked)
            {
                sdat.CopyFile(1);
                helperText.Text = "Copied File 1!";
            }
            else if (radioFile2.Checked)
            {
                sdat.CopyFile(2);
                helperText.Text = "Copied File 2!";
            }
            else if (radioFile3.Checked)
            {
                sdat.CopyFile(3);
                helperText.Text = "Copied File 3!";
            }
            buttonWrite.Enabled = true;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            if (radioFile1.Checked)
            {
                sdat.WriteFile(1);
                helperText.Text = "Wrote to File 1!";
            }
            else if (radioFile2.Checked)
            {
                sdat.WriteFile(2);
                helperText.Text = "Wrote to File 2!";
            }
            else if (radioFile3.Checked)
            {
                sdat.WriteFile(3);
                helperText.Text = "Wrote to File 3!";
            }
        }

        private void buttonErase_Click(object sender, EventArgs e)
        {
            int selFile = 1;
            if (radioFile1.Checked)
                selFile = 1;
            else if (radioFile2.Checked)
                selFile = 2;
            else if (radioFile3.Checked)
                selFile = 3;

                DialogResult dialogResult = MessageBox.Show("You are about to PERMANENTLY ERASE File " + selFile + "! Are you sure you want to erase it? There is no undo!", "Erase File " + selFile + "?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                sdat.EraseFile(selFile);
                helperText.Text = "Erased File " + selFile + ".";
            }
        }

        private void radioFile1_CheckedChanged(object sender, EventArgs e)
        {
            updatePlayerName(1);
        }

        private void radioFile2_CheckedChanged(object sender, EventArgs e)
        {
            updatePlayerName(2);
        }

        private void radioFile3_CheckedChanged(object sender, EventArgs e)
        {
            updatePlayerName(3);
        }

        private void updatePlayerName(int slot)
        {
            SaveSlot savslot;
            switch (slot)
            {
                default:
                case 1:
                    savslot = sdat.GetSaveSlot(1);
                    break;
                case 2:
                    savslot = sdat.GetSaveSlot(2);
                    break;
                case 3:
                    savslot = sdat.GetSaveSlot(3);
                    break;
            }

            if (!savslot.IsEmpty())
            {
                String playerName = savslot.GetPlayerName();
                fileNameBox.Text = playerName;
                fileNameBox.Enabled = true;
            }
            else
            {
                fileNameBox.Enabled = false;
            }
        }

        private void buttonApplyChanges_Click(object sender, EventArgs e)
        {
            buttonWrite.Enabled = false;

            // Determine which file we're editing, and then load its data.
            SaveSlot savslot;
            int slot = 1;
            if (radioFile2.Checked)
            {
                slot = 2;
                savslot = sdat.GetSaveSlot(2);
            }
            else if (radioFile3.Checked)
            {
                slot = 3;
                savslot = sdat.GetSaveSlot(3);
            }
            else
                savslot = sdat.GetSaveSlot(1);

            // Cap the player name to 6 characters because of in-game limitations; Store it to the save data.
            savslot.SetPlayerName(fileNameBox.Text.Substring(0,6));
            
            // Update the playerName textbox to emphasize the 6 character limit.
            updatePlayerName(slot);
        }
    }
}
