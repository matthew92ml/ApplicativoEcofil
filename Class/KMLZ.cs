using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

namespace ApplicativoEcofil.Class
{
	internal class KMLZ
	{
		public KMLZ(string path, Stations stations, GMapControl gMap, Garbage garbage)
		{
			try
			{
				if (path != null)
				{
					if (path.Contains("kmz"))
					{
						CreateKmZ(CreateData(gMap, stations, garbage), path);
						return;
					}
					string text = path;
					if (text.Contains("kml"))
					{
						CreateKmL(CreateData(gMap, stations, garbage), path);
						return;
					}
				}
				MessageBox.Show("Formato non valido", "Kml", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Kml", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		private static KmlFile CreateData(GMapControl gMap, Stations stationCity, Garbage garbage)
		{
			Style rStyle;
			while (true)
			{
				try
				{
					if (gMap.Overlays.Count == 0)
					{
						return null;
					}
					SharpKml.Dom.Point[] point = new SharpKml.Dom.Point[gMap.Overlays[1].Markers.Count];
					Description[] descriptions = new Description[gMap.Overlays[1].Markers.Count];
					SharpKml.Dom.Placemark[] placemarks = new SharpKml.Dom.Placemark[gMap.Overlays[1].Markers.Count];
					LineString[] route = new LineString[gMap.Overlays[0].Routes.Count];
					SharpKml.Dom.Placemark[] placemarkroute = new SharpKml.Dom.Placemark[gMap.Overlays[0].Routes.Count];
					Folder doc = new();
					if (gMap.Overlays.Count > 0)
					{
						Parallel.For(0, gMap.Overlays[1].Markers.Count, new ParallelOptions{MaxDegreeOfParallelism = Environment.ProcessorCount},(int index)=>
						{
							int index2 = stationCity.latitude.IndexOf(gMap.Overlays[1].Markers[index].Position.Lat);
							string[] array = gMap.Overlays[1].Markers[index].ToolTipText.Split(' ');
							Color color2 = array[0].Contains("Partenza") ? Color.Empty : Color.FromName(garbage.color[Array.FindIndex(garbage.type, (string m) => m == garbage.type.Where((string x) => x.All(gMap.Overlays[1].Markers[index].ToolTipText.ToUpper().Contains)).First())]);
							Uri href = array[0].Contains("Partenza") ? new Uri("http://maps.google.com/mapfiles/kml/paddle/go.png") : new Uri("http://maps.google.com/mapfiles/kml/pal5/icon14.png");
							Style style = new()
                            {
								Id = "Station" + stationCity.nStation[index2],
								Icon = array[0].Contains("Partenza") ? new IconStyle
								{
									Icon = new IconStyle.IconLink(href),
									Scale = 1.1
								} : new IconStyle
								{
									Color = new Color32(color2.A, color2.B, color2.G, color2.R),
									ColorMode = ColorMode.Random,
									Icon = new IconStyle.IconLink(href),
									Scale = 1.1
								}
							};
							doc.AddStyle(style);
							point[index] = new SharpKml.Dom.Point
							{
								Coordinate = new Vector(gMap.Overlays[1].Markers[index].Position.Lat, gMap.Overlays[1].Markers[index].Position.Lng)
							};
							string[] array2 = gMap.Overlays[1].Markers[index].ToolTipText.Replace("Stazione", "").Replace(stationCity.nStation[index2].ToString(), "").Split('\n');
							string text = string.Empty;
							for (int i = 0; i < array2.Length; i++)
							{
								text += (array2[i] == "") ? "" : ("<h3>" + array2[i] + "</h3>");
							}
							descriptions[index] = new Description
							{
								Text = "<![CDATA[" + text + "]]>"
							};
							placemarks[index] = new SharpKml.Dom.Placemark
							{
								Name = "Stazione " + stationCity.nStation[index2],
								Description = descriptions[index],
								StyleUrl = new Uri("#" + style.Id, UriKind.Relative),
								Geometry = point[index]
							};
                        try{doc.AddFeature(placemarks[index]);}
						catch (Exception ex){}});
						Color color = Color.FromKnownColor(KnownColor.White);
						rStyle = new Style
						{
							Id = "routeStyle",
							Line = new LineStyle
							{
								Color = new Color32(color.A, color.B, color.G, color.R),
								Width = 5.0,
								ColorMode = ColorMode.Random
							}
						};
						doc.AddStyle(rStyle);
						Parallel.For(0, gMap.Overlays[0].Routes.Count, new ParallelOptions
						{
							MaxDegreeOfParallelism = Environment.ProcessorCount
						}, (int index)=>
						{
							CoordinateCollection coordinateCollection = new();
							foreach (PointLatLng point2 in gMap.Overlays[0].Routes[index].Points)
							{
								coordinateCollection.Add(new Vector(point2.Lat, point2.Lng));
							}
							route[index] = new LineString
							{
								AltitudeMode = AltitudeMode.ClampToGround,
								Extrude = true,
								Tessellate = true,
								Coordinates = coordinateCollection
							};
							placemarkroute[index] = new SharpKml.Dom.Placemark
							{
								Name = "Route " + index,
								Geometry = route[index],
								StyleUrl = new Uri("#" + rStyle.Id, UriKind.Relative)
							};
							doc.AddFeature(placemarkroute[index]);
						});
					}
					Kml root = new()
                    {
						Feature = doc
					};
					Serializer serializer = new();
					serializer.Serialize(root);
					Parser parser = new();
					parser.ParseString(serializer.Xml, namespaces: true);
					root = (Kml)parser.Root;
					return KmlFile.Create(root, duplicates: false);
				}
				catch (Exception)
				{
				}
			}
		}
		private static void CreateKmZ(KmlFile file, string root)
		{
			using KmzFile kmzFile = KmzFile.Create(file);
			using FileStream stream = File.OpenWrite(root);
			kmzFile.Save(stream);
		}
		private static void CreateKmL(KmlFile file, string root)
		{
			using FileStream stream = File.OpenWrite(root);
			file.Save(stream);
		}
	}
}
