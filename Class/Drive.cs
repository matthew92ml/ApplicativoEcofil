using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Win32;

namespace ApplicativoEcofil.Class
{
	internal class Drive
	{
		private static readonly string[] Scopes = new string[7]
		{
			DriveService.Scope.Drive,
			DriveService.Scope.DriveAppdata,
			DriveService.Scope.DriveMetadata,
			DriveService.Scope.DriveFile,
			DriveService.Scope.DriveMetadataReadonly,
			DriveService.Scope.DriveReadonly,
			DriveService.Scope.DriveScripts
		};
		public Drive(string path, string folder)
		{
			try
			{
				DriveService driveService = new(new BaseClientService.Initializer
				{
					HttpClientInitializer = GetCredential(),
					ApplicationName = "Ecofil"
				});
				driveService.HttpClient.Timeout=TimeSpan.FromMinutes(2.0);
				CreateFolder(driveService, folder);
				FilesResource.ListRequest listRequest = driveService.Files.List();
				listRequest.Fields = "files(id, name)";
					if (!listRequest.Execute().Files.Any((Google.Apis.Drive.v3.Data.File x) => x.Name == folder + ".kmz"))
					{
						UploadFile(driveService, path, driveService.Files.List().Execute().Files.Where((Google.Apis.Drive.v3.Data.File x) => x.MimeType == "application/vnd.google-apps.folder" && x.Name == folder).First().Id);
					}
					else
					{
						UpdateFile(driveService, path, driveService.Files.List().Execute().Files.Where((Google.Apis.Drive.v3.Data.File x) => x.MimeType != "application/vnd.google-apps.folder" && x.Name == folder + ".kmz").First().Id);
					}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		private static UserCredential GetCredential()
		{
			string text = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "//drive//";
			using FileStream stream = new(text + "client_id.json", FileMode.Open, FileAccess.Read);
			string folder = text + "token.json";
			return GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromStream(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(folder, fullPath: true)).Result;
		}
		public static void CreateFolder(DriveService _service, string folder)
		{
			Google.Apis.Drive.v3.Data.File body = new()
            {
				Name = folder,
				MimeType = "application/vnd.google-app.folder"
			};
			if (!_service.Files.List().Execute().Files.Select((Google.Apis.Drive.v3.Data.File x) => x.Name).Contains(folder))
			{
				FilesResource.CreateRequest createRequest = _service.Files.Create(body);
				createRequest.Fields = "id";
				createRequest.Execute();
			}
		}
		private static void UploadFile(DriveService _service, string pathFile, string folderId)
		{
			if (System.IO.File.Exists(pathFile))
			{
				Google.Apis.Drive.v3.Data.File body = new()
                {
					Name = Path.GetFileName(pathFile),
					MimeType = GetMimeType(pathFile),
					Parents = new List<string> { folderId }
				};
				byte[] array = System.IO.File.ReadAllBytes(pathFile);
				MemoryStream stream = new(array);
				try
				{
					Task.Factory.StartNew(() =>
					{ 
					FilesResource.CreateMediaUpload request = _service.Files.Create(body, stream, GetMimeType(pathFile));
					new ProgressWork(request, array.LongLength).ShowDialog();
					return;
				});
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			MessageBox.Show("File inesistente", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		private static void UpdateFile(DriveService _service, string pathFile, string folderId)
		{
			if (File.Exists(pathFile))
			{
				Google.Apis.Drive.v3.Data.File body = new();
				byte[] array = File.ReadAllBytes(pathFile);
				MemoryStream stream = new(array);
					try
					{
						FilesResource.UpdateMediaUpload request = _service.Files.Update(body, folderId, stream, GetMimeType(pathFile));
						new ProgressWork(request, array.LongLength).ShowDialog();
						return;
				}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
			}
			MessageBox.Show("File inesistente", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		private static string GetMimeType(string fileName)
		{
			string result = "application/unknown";
			string name = Path.GetExtension(fileName).ToLower();
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(name);
			if (registryKey != null && registryKey.GetValue("Content Type") != null)
			{
				result = registryKey.GetValue("Content Type").ToString();
			}
			return result;
		}
	}
}