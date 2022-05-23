using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
namespace TabExtension
{
    public interface IUserControl
    {
        string Name { get; }
        UserControl Control { get; }
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
        private static List<IUserControl> GetUserControl() {
            List<IUserControl> GetUserControl = new List<IUserControl>();
            if (Directory.Exists("Tabs"))
            {
                string[] files = Directory.GetFiles("Tabs");
                foreach (string file in files)
                    if (file.EndsWith(".Tabs"))
                    {
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
