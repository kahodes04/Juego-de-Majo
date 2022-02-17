using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JuegodeMajoWPF
{
    /// <summary>
    /// Interaction logic for TeamNameWindow.xaml
    /// </summary>
    public partial class TeamNameWindow
    {
        private List<string> teamnamestwo;
        public TeamNameWindow(List<String> teamnames)
        {
            InitializeComponent();
            teamnamestwo = teamnames;
            TabItem tab = (TabItem)this.tabcontrol.Items.GetItemAt(0);
            for(int i = 0; i < teamnames.Count; i++)
            {
                TabItem tabitem = new TabItem();
                tabitem.Header = teamnames[i];
                tabitem.Content = tab.Content;
                this.tabcontrol.Items.Add(tabitem);
            }
            this.tabcontrol.Items.RemoveAt(0);
        }

        private void Buttonapplyname_Click(object sender, RoutedEventArgs e)
        {
            applyname();
        }

        private void textboxnameinput_onkeydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                applyname();
        }

        private void applyname()
        {
            if (!String.IsNullOrWhiteSpace(this.textboxnameinput.Text))
            {
                ((TabItem)this.tabcontrol.Items.GetItemAt(this.tabcontrol.SelectedIndex)).Header = this.textboxnameinput.Text.Trim(); 
                teamnamestwo[this.tabcontrol.SelectedIndex] = this.textboxnameinput.Text.Trim();
            }
            this.textboxnameinput.Clear();
        }

        private void Tabcontrol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.textboxnameinput.Clear();
        }
    }
}
