using System;
using System.Windows.Forms;
using TabExtension;
namespace Exemple
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();
        private void Form1_Load(object sender, EventArgs e) => tabControl1.LoadTabs();
    }
}
