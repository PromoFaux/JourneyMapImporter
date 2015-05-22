using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JourneymapWaypoinImporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Stream myStream = null;
        public string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = appdata + "\\.minecraft\\mods\\VoxelMods\\voxelMap";
            ofd.Filter = "points files (*.points)|*.points";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtVoxel.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.SelectedPath = appdata + "\\.minecraft\\journeymap\\data\\mp";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtJourney.Text = fbd.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtVoxel.Text != "" || txtJourney.Text != "")
            {
                import(txtVoxel.Text, txtJourney.Text);
            }
            else
            {
                MessageBox.Show("No Paths Selected!");
            }
        }

        private void import(string vmFile, string jmFolder)
        {
            // TextWriter tw = new StreamWriter(vmFile, true);

            string[] fileEntries = Directory.GetFiles(jmFolder, "*.json");


           if (fileEntries != null)
            {
                foreach (string fileName in fileEntries)
                {
                    using (StreamReader file = File.OpenText(fileName))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        journeymapwaypoint jw = (journeymapwaypoint)serializer.Deserialize(file, typeof(journeymapwaypoint));
                        string linetoadd = "name:" + jw.name +
                                           ",x:" + jw.x +
                                           ",z:" + jw.z +
                                           ",y:" + jw.y +
                                           ",enabled:" + jw.enable +
                                           ",red:" + ((float)jw.r / 255).ToString() +
                                           ",green:" + ((float)jw.g / 255).ToString() +
                                           ",blue:" + ((float)jw.b / 255).ToString() +
                                           ",suffix:" +
                                           ",world:,dimensions:" + jw.dimensions[0] + "#";

                        using (StreamWriter sw = File.AppendText(vmFile))
                        {
                            sw.WriteLine(linetoadd);
                        }

                        listBox1.Items.Add("Waypoint imported! (" + jw.name + ")");
                    }
                }


                listBox1.Items.Add("All Done");
                //// Recurse into subdirectories of this directory. 
                //string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                //foreach (string subdirectory in subdirectoryEntries)
                //    //ProcessDirectory(subdirectory);

            }
           else
            {
                MessageBox.Show("No JourneyMap Files in this folder! Choose another path!");
            }
        }
     

    }
}

public class journeymapwaypoint
{
    public string name { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }
    public int r { get; set; }
    public int g { get; set; }
    public int b { get; set; }
    public string enable { get; set; }

    public string type { get; set; }
    public string origin { get; set; }
    public List<string> dimensions { get; set; }




}
