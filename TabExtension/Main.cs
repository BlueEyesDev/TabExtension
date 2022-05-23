using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Net;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
namespace TabExtension
{
    public interface IUserControl
    {
        string Name { get; }
        UserControl Control { get; }
    }
    public static class Config { 
        public static string Url { get; set; } = null;
        public static string DirPath { get; set; } = null;
    }
    public static class UserControlLoader
    {
        public static void LoadTabs(this TabControl tabControl) {
            List<IUserControl> list = UserControlLoader.GetUserControl();
            foreach (var item in list)
                if (!UserControlLoader.Isloaded.Contains(item.Name))
                {
                    TabPage newpage = new TabPage() { Text = item.Name };
                    newpage.Controls.Add(item.Control);
                    tabControl.TabPages.Add(newpage);
                    UserControlLoader.Isloaded.Add(item.Name);
                }
        }
        private static List<string> Isloaded => new List<string>();
        private static List<Dictionary<string, string>> JsonDownload = null;
        private static WebClient Client = new WebClient();
        private static string[] GetFiles(string DirPath) {
            if (Directory.Exists(DirPath)) {
                IEnumerable<string> query = Directory.GetFiles(DirPath).Where(item => item.EndsWith(".Tabs"));
                return query.ToArray();
            }
            return new string[] { };
        }
        private static List<IUserControl> GetUserControl() {
            List<IUserControl> GetUserControl = new List<IUserControl>();
            if (Config.Url != null) {
                
                JsonDownload = new JavaScriptSerializer().Deserialize<List<Dictionary<string, string>>>(Client.DownloadString(Config.Url));
                string teste = Client.DownloadString(Config.Url);
                foreach (var item in JsonDownload) {

                    KeyValuePair<string, string> JsonFile = (item).First();
                    string testess = $"{Config.DirPath}\\{JsonFile.Key}";
                    if (!File.Exists($"{Config.DirPath}\\{JsonFile.Key}"))
                        Client.DownloadFile($"{Config.Url}{JsonFile.Key}", $"{Config.DirPath}\\{JsonFile.Key}");
                    else if (File.Exists($"{Config.DirPath}\\{JsonFile.Key}") && JsonFile.Value != BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(File.ReadAllBytes($"{Config.DirPath}\\{JsonFile.Key}"))).ToLower().Replace("-", string.Empty))
                    {
                        File.Delete($"{Config.DirPath}\\{JsonFile.Key}");
                        Client.DownloadFile($"{Config.Url}{JsonFile.Key}", $"{Config.DirPath}\\{JsonFile.Key}");
                    }
                }
            }
            if (Config.DirPath != null) {
                foreach (string file in GetFiles(Config.DirPath)) {

                    Assembly.LoadFile(Path.GetFullPath(file));
                    Type interfaceType = typeof(IUserControl);
                    Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                        .ToArray();
                    foreach (Type type in types)
                        GetUserControl.Add((IUserControl)Activator.CreateInstance(type));
                }
            }
            return GetUserControl;
        }
    }
}
