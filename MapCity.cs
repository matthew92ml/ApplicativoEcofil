using System;
using System.Drawing;
using GMap.NET;
using GMap.NET.MapProviders;
using System.Threading;
using ApplicativoEcofil.Class;
using ApplicativoEcofil.Properties;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicativoEcofil
{
    public partial class MapCity : Form
    {
        private List<Dictionary<int, string>> table;
        private KeyValuePair<int, string> comboGarbage;
        private readonly Garbage garbage;
        private readonly string userForm;
        private readonly Stations stationForm;
        private readonly City cityForm;
        private bool drag = false;
        private Point pmouse, pform;
        private DataGridView data;
        public MapCity(City city, string user, Stations station, Garbage garb)
        {
            userForm = user;
            cityForm = city;
            stationForm = station;
            garbage = garb;
            InitializeComponent();
            BackgroundImage = Resources.background1;
            Icon = Resources.favicon;
            gMapweb.Zoom = 16;
            Text += " " + cityForm.nameCity[Convert.ToInt32(userForm)];
            Region = new Region(FormCityFunction.GetRoundPath(DisplayRectangle, Height / 10));
            pictureLogo.Image = Resources.LogoEcofil2020;
            pictureLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureLogo.Scale(new SizeF(1.0F, 0.9F));
        }
        private void MapCity_Load(object sender, EventArgs e)
        {
            GMapProviders.GoogleMap.ApiKey = "AIzaSyBxu581tgc59858UvW7VMSV3OiiZ-EEz3U&v";
            gMapweb.ShowCenter = false;
            gMapweb.OnTileLoadStart += LoadSet;
            if (userForm == "0")
            {
                gMapweb.DragButton = MouseButtons.Left;
                gMapweb.MapProvider = GMapProviders.GoogleHybridMap;
                GMaps.Instance.Mode = AccessMode.ServerOnly;
                gMapweb.MinZoom = 2;
                gMapweb.MaxZoom = 18;
                gMapweb.Zoom = 8;
                gMapweb.Position = new PointLatLng(43, 11);
                gMapweb.ShowCenter = false;
                gMapweb.OnTileLoadComplete += SendFile;
            }
            else
            {
                gMapweb.DragButton = MouseButtons.Left;
                gMapweb.MapProvider = GMapProviders.GoogleHybridMap;
                GMaps.Instance.Mode = AccessMode.ServerOnly;
                gMapweb.MinZoom = 2;
                gMapweb.MaxZoom = 18;
                gMapweb.Zoom = 15;
                gMapweb.Position = new PointLatLng(cityForm.latitude[Convert.ToInt32(userForm)], cityForm.longitude[Convert.ToInt32(userForm)]);
                gMapweb.ShowCenter = false;
                gMapweb.OnTileLoadComplete += SendFile;
            }
        }
        private void LoadSet()
        {
        }
        private void SendFile(long time)
        {
           Task.Factory.StartNew(async() =>
            {
                await Upload(gMapweb, stationForm, cityForm, userForm, garbage);
            });
        }
        public void FunData(DataGridView dForm, int ncommand, List<Dictionary<int, string>> table2, KeyValuePair<int, string> combo2)
        {
            comboGarbage = combo2;
            table = table2;
            data = dForm;
            Invoke(() =>
            {
                Operation(ncommand);
            });
        }
        private void MapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new(() =>
            {
                FormCityFunction.SaveKMZ(gMapweb, data, stationForm, garbage);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
        private void CloseStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Map_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            pmouse = Cursor.Position;
            pform = Location;
        }
        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(pmouse));
                Location = Point.Add(pform, new Size(dif));
            }
        }
        private void ZoomPStripMenuItem_Click(object sender, EventArgs e)
        {
            gMapweb.Zoom++;
        }
        private void ZoomMStripMenuItem_Click(object sender, EventArgs e)
        {
            gMapweb.Zoom--;
        }
        private void Map_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
        private void Operation(int command)
        {
            switch (command)
            {
                case 1:
                    Maps.Clean(gMapweb);
                    break;
                case 2:
                    gMapweb.Refresh();
                    break;
                case 3:
                    Maps.FirstRoute(gMapweb, table, comboGarbage, stationForm);
                    break;
                case 4:
                    Close();
                    break;
                default:
                    break;
            }
        }
        private static async Task Upload(GMap.NET.WindowsForms.GMapControl gMapweb,Stations stationForm, City cityForm, string userForm, Garbage garbage)
        { await Task.Factory.StartNew(() => { 
            if (gMapweb.Overlays.Count > 0)
            {
                FormCityFunction.SendScpFile(gMapweb, stationForm, cityForm.nameCity[Convert.ToInt32(userForm)], cityForm.latitude[Convert.ToInt32(userForm)], cityForm.longitude[Convert.ToInt32(userForm)], garbage);
                Thread.Sleep(5000);
                FormCityFunction.SendDriveFile(gMapweb, stationForm, cityForm.nameCity[Convert.ToInt32(userForm)], cityForm.latitude[Convert.ToInt32(userForm)], cityForm.longitude[Convert.ToInt32(userForm)], garbage);
            } });
        }
}
}
