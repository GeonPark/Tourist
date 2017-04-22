using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
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
        private ListViewItem FileListView;
        public Main()
        {
            InitializeComponent();
            settings = new Tourist.TouristSetting();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            InitGMap();
        }
        private void setImageListView(string[] filelist)
        {
            // 리스트뷰 초기화
            detailView.Items.Clear();

            // GMap 초기화
            TouristGmap.Overlays.Clear();
            InitGMap();

            foreach (string objFile in filelist)
            {
                // Exif Format을 가진 이미지파일만 작업 수행
                if (Tourist.TouristBasicUtil.checkExifFile(objFile))
                {

                    Tourist.PropertyFileInfo pf = Tourist.TouristFileInfo.getFileInfo(objFile);
                    Tourist.ExifValue ef = Tourist.TouristExifParser.getImageInfo(objFile);
                    FileListView = new ListViewItem(pf.Filename);
                    FileListView.SubItems.Add(Tourist.TouristBasicUtil.GetFileSize(pf.Filesize));
                    FileListView.SubItems.Add(pf.CreateTime.ToString());
                    FileListView.SubItems.Add(pf.LastWriteTime.ToString());
                    FileListView.SubItems.Add(pf.LastAccessTime.ToString());
                    detailView.Items.Add(FileListView);
                    setGMapMarker(ef.Latitue, ef.Longitude, pf.Filename);
                }
            }
            // 초기 Marker가 겹치는 현상때문에 Zoom으로 맵리로딩
            TouristGmap.Zoom = 6;
            detailView.EndUpdate();
            if (detailView.Items.Count == 0)
                detailViewLabel.Visible = true;
            else
                detailViewLabel.Visible = false;
        }

        private void DeveloperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/exploitforme");
        }

        private void ExifLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TouristDirBrowser.ShowDialog() == DialogResult.OK)
            {
                selectedPath = TouristDirBrowser.SelectedPath;
                label_selectedPath.Text = selectedPath;

                // ListView Setting
                setImageListView(System.IO.Directory.GetFiles(selectedPath));
            }
            else
                MessageBox.Show("분석대상 디렉터리를 지정해주십시오.", "Tourist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void detailView_Click(object sender, EventArgs e)
        {
            MessageBox.Show("click");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TouristGmap.Overlays.Clear();
            InitGMap();

        }

        #region GMap 설정관련 메소드
        public void InitGMap()
        {
            // GMap 기본설정
            TouristGmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            TouristGmap.Position = new GMap.NET.PointLatLng(37.478683, 126.878648);
            TouristGmap.DragButton = System.Windows.Forms.MouseButtons.Left;
        }

        public void setGMapMarker(double lat, double log, string filename)
        {
            GMap.NET.WindowsForms.GMapOverlay markersOverlay = new GMap.NET.WindowsForms.GMapOverlay("markers");
            GMap.NET.WindowsForms.Markers.GMarkerGoogle marker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                new GMap.NET.PointLatLng(lat, log), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue);


            // Marker Tooltip 
            marker.ToolTipText = filename;
            marker.ToolTip.Fill = Brushes.White;
            marker.ToolTip.Foreground = Brushes.Black;
            marker.ToolTip.Stroke = Pens.Black;
            marker.ToolTip.TextPadding = new Size(20, 10);
            marker.ToolTipMode = GMap.NET.WindowsForms.MarkerTooltipMode.OnMouseOver;

            markersOverlay.Markers.Add(marker);
            TouristGmap.Overlays.Add(markersOverlay);
        }
        #endregion
    }
}
