using TabExtension;
using System.Windows.Forms;
namespace Tab_Exemple
{
    public class Main : IUserControl
    {
        public string Name => "This Is My Exemple2";

        public UserControl Control => new UserControl1();
    }
}
