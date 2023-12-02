using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace ApplicativoEcofil.Class
{
	internal class Maps
	{
		public static void FirstRoute(GMapControl gMapWeb, List<Dictionary<int, string>> table, KeyValuePair<int, string> comboBoxNumberCity, Stations stationForm)
		{
			Clean(gMapWeb);
			GMapOverlay markers = new("markers");
			List<PointLatLng> list = new()
            {
				new PointLatLng(stationForm.latitude[comboBoxNumberCity.Key], stationForm.longitude[comboBoxNumberCity.Key])
			};
			table = table.OrderBy((Dictionary<int, string> x) => x[0]).ToList();
			foreach (Dictionary<int, string> item2 in table)
			{
				if (uint.Parse(item2[0]) == uint.Parse(comboBoxNumberCity.Value))
				{
					if (item2.Count != 3)
					{
						int num = Function.IndexMax(item2);
                        GMarkerGoogle gMarkerGoogle = new(list[0], GMarkerGoogleType.green_big_go)
                        {
                            ToolTipText = "Partenza \n Stazione " + item2[0] + " \n " + item2[num - 1] + " " + item2[num] + "% \n "
                        };
                        GMapMarker item = gMarkerGoogle;
						markers.Markers.Add(item);
					}
					else
					{
						GMapMarker item = new GMarkerGoogle(list[0], GMarkerGoogleType.green_big_go)
						{
							ToolTipText = "Partenza "
						};
						markers.Markers.Add(item);
					}
				}
				else if (item2.Count != 3 && item2[0] != comboBoxNumberCity.Value)
				{
					int num2 = Function.IndexMax(item2);
					list.Add(new PointLatLng(Convert.ToDouble(item2[1]), Convert.ToDouble(item2[2])));
					string text = item2[num2 + 1];
                    GMarkerGoogle gMarkerGoogle = new(new PointLatLng(Convert.ToDouble(item2[1]), Convert.ToDouble(item2[2])), text switch
                    {
                        "brown" => GMarkerGoogleType.red_pushpin,
                        "yellow" => GMarkerGoogleType.yellow_pushpin,
                        "Darkgreen" => GMarkerGoogleType.green_pushpin,
                        "gray" => GMarkerGoogleType.yellow_pushpin,
                        "LightGray" => GMarkerGoogleType.yellow_pushpin,
                        "LightSteelBlue" => GMarkerGoogleType.lightblue_pushpin,
                        "Olive" => GMarkerGoogleType.green_pushpin,
                        "Darkblue" => GMarkerGoogleType.purple_pushpin,
                        "PaleVioletRed" => GMarkerGoogleType.purple_pushpin,
                        _ => GMarkerGoogleType.red_pushpin,
                    })
                    {
                        ToolTipText = "Stazione " + item2[0] + " \n " + item2[num2 - 1] + " " + item2[num2] + "% \n"
                    };
                    GMapMarker item = gMarkerGoogle;
					markers.Markers.Add(item);
					if (stationForm.ecocentre[table.IndexOf(item2)])
					{
						list.Remove(new PointLatLng(Convert.ToDouble(item2[1]), Convert.ToDouble(item2[2])));
					}
				}
			}
			Tuple<GMapOverlay, List<PointLatLng>> tuple = RouteElaboration(list);
			GMapOverlay routes = tuple.Item1;
			List<PointLatLng> point = tuple.Item2;
			gMapWeb.Overlays.Add(routes);
			Task.Run(()=>
			{
				Parallel.For(0, routes.Routes.Count , new ParallelOptions
				{
					MaxDegreeOfParallelism = Environment.ProcessorCount
				}, (int i)=>
				{
					GMapMarker gMapMarker = markers.Markers.Where((GMapMarker x) => x.Position == point[i]).First();
					GMapMarker gMapMarker2 = markers.Markers.Where((GMapMarker x) => x.Position == point[i + 1]).First();
					string text2 = (i < routes.Routes.Count) ? Uri.EscapeDataString(gMapMarker2.Position.Lat.ToString().Replace(',', '.') + " , " + gMapMarker2.Position.Lng.ToString().Replace(',', '.')) : "";
					string text3 = (!string.IsNullOrEmpty(text2)) ? (" Percorso: http://maps.google.com/maps?q=" + text2 + "&ll=" + text2 + "&z=17") : "";
					gMapMarker.ToolTipText += $"Prossima stazione la n.{gMapMarker2.ToolTipText.Split('\r', '\n', ' ')[1]} \n Distanza:{routes.Routes[i].Distance:0.00} Km  Durata:{routes.Routes[i].Duration} \n {text3}";
					gMapMarker2.ToolTipText += (i == routes.Routes.Count-1) ? "Ultima stazione" : "";
				});
			}).Wait();
			gMapWeb.Overlays.Add(markers);
			gMapWeb.Zoom = 15.0;
			Refresh(gMapWeb);
		}
		public static void Clean(GMapControl map)
		{
			while (map.Overlays.Count > 0)
			{
				for (int num = map.Overlays.Count - 1; num >= 0; num--)
				{
					map.Overlays.RemoveAt(num);
				}
				Refresh(map);
			}
		}
		public static void Refresh(GMapControl map)
		{
			map.Zoom--;
			map.Zoom++;
		}
		public static Tuple<GMapOverlay, List<PointLatLng>> RouteElaboration(List<PointLatLng> points)
		{
			int num = 0;
			List<PointLatLng> list = new() { points[0] };
			for (int i = 1; i <= points.Count - 1; i++)
			{
				if (points[0] == points[i])
				{
					points.Remove(points[i]);
					break;
				}
			}
			while (points.Count > 1)
			{
				List<double> distance = new();
				PointLatLng start = new(points[num].Lat, points[num].Lng);
				int num2 = num;
				for (int j = 0; j <= points.Count - 1; j++)
				{
					PointLatLng end = new(points[j].Lat, points[j].Lng);
					GDirections direction = new();
					DirectionsStatusCode directions = GMapProviders.GoogleMap.GetDirections(out direction, start, end, avoidHighways: false, avoidTolls: false, walkingMode: false, sensor: true, metric: true);
					GMapRoute gMapRoute = new(direction.Route, "");
					distance.Add(gMapRoute.Distance);
					gMapRoute.Clear();
				}
				num = distance.FindIndex(0, (double m) => m == distance.Where((double x) => x != 0.0).Min());
				list.Add(points[num]);
				points.Remove(points[num2]);
				if (num > num2)
				{
					num--;
				}
			}
            GMapOverlay gMapOverlay = new("routes");
			for (int k = 0; k <= list.Count - 2; k++)
			{
				try
				{
					GDirections direction2 = new();
					DirectionsStatusCode directions2 = GMapProviders.GoogleMap.GetDirections(out direction2, list[k], list[k + 1], avoidHighways: false, avoidTolls: false, walkingMode: false, sensor: true, metric: true);
					GMapRoute gMapRoute2 = new(direction2.Route, "route" + k)
					{
						Stroke = new Pen(Color.OrangeRed, 3f)
						{
							StartCap = LineCap.Square,
							CustomEndCap = new AdjustableArrowCap(4f, 8f, isFilled: false)
						}
					};
					gMapRoute2.Duration = direction2.Duration;
					gMapOverlay.Routes.Add(gMapRoute2);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Map", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			return Tuple.Create(gMapOverlay, list);
		}
	}
}
