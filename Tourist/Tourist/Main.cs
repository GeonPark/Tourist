using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MetadataExtractor;
using GMap;

namespace Tourist
{
    public partial class Main : Form
    {
        private Tourist.TouristSetting settings;
        private string selectedPath = "";
        public Main()
        {
            InitializeComponent();
            settings = new Tourist.TouristSetting();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Tourist.TouristBasicUtil util = new Tourist.TouristBasicUtil();
            bool exif = util.checkExifFile("C:\\Users\\ParkGeon\\Downloads\\20170408_130859.jpg");

            TouristGmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            TouristGmap.Position = new GMap.NET.PointLatLng(37.478683, 126.878648);
            /*
            IEnumerable<Directory> test = MetadataExtractor.ImageMetadataReader.ReadMetadata("C:\\Users\\ParkGeon\\Downloads\\20170408_130859.jpg");
            foreach (var dir in test)
            {
                foreach (var tag in dir.Tags)
                {
                    MessageBox.Show(tag.DirectoryName + " / " + tag.Description);
                }
            }
             */
        }

        private void DeveloperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/namegpark");
        }

        private void ExifLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TouristDirBrowser.ShowDialog() == DialogResult.OK)
            {
                selectedPath = TouristDirBrowser.SelectedPath;
                label_selectedPath.Text = selectedPath;
            }
            else
                MessageBox.Show("분석대상 디렉터리를 지정해주십시오.", "Tourist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
