using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using MahApps.Metro.Controls;

namespace JuegodeMajoWPF
{
    /// <summary>
    /// Interaction logic for SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow
    {
        private List<String> namelist = new List<string>();
        private List<String> wordlist = new List<string>();
        Nullable<bool> fileopenresult = false;
        public SelectionWindow()
        {
            InitializeComponent();
            this.radiouserdic.IsChecked = true;
            ((System.Collections.Specialized.INotifyCollectionChanged)listboxwords.Items).CollectionChanged += mListBox_CollectionChanged;
        }

        private void mListBox_CollectionChanged(object sender,
    System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                checkifcontinue();
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                checkifcontinue();
        }

        private void checkifwordaddable()
        {
            string nowhitespace = System.Text.RegularExpressions.Regex.Replace(this.textboxwordinput.Text, @"\s", "");
            if (nowhitespace != String.Empty && !this.listboxwords.Items.Contains(nowhitespace))
                this.listboxwords.Items.Add(this.textboxwordinput.Text);
            this.textboxwordinput.Clear();
        }

        private void checkifcontinue()
        {
            if (String.IsNullOrEmpty(this.textboxtime.Text) || (String.IsNullOrEmpty(this.textboxnumofrandword.Text) && this.listboxwords.Items.IsEmpty) || (this.radiouserdic.IsChecked == true && (wordlist.Count == 0 || fileopenresult == false || (wordlist.Count > 0 && (wordlist.Count < int.Parse(this.textboxnumofrandword.Text) || wordlist.Count == 0)))))
            {
                this.buttonsecondplay.IsEnabled = false;
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(this.buttonsecondplay, false);
            }
            else
            {
                this.buttonsecondplay.IsEnabled = true;
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(this.buttonsecondplay, true);
            }
        }


        private void checkboxuserdic_checked(object sender, RoutedEventArgs e)
        {
            this.textboxnumofrandword.IsEnabled = true;
        }

        private void checkboxusardic_unchecked(object sender, RoutedEventArgs e)
        {
            this.textboxnumofrandword.Clear();
            this.textboxnumofrandword.IsEnabled = false;
        }

        private void radiouserdic_checked(object sender, RoutedEventArgs e)
        {
            this.textboxnumofrandword.IsEnabled = true;
            this.textboxwordinput.Clear();
            this.textboxwordinput.IsEnabled = false;
            this.buttonaddword.IsEnabled = false;
            this.buttondeleteword.IsEnabled = false;
            this.listboxwords.Items.Clear();
            this.listboxwords.IsEnabled = false;
        }

        private void radiouserdic_unchecked(object sender, RoutedEventArgs e)
        {
            this.textboxnumofrandword.Clear();
            this.textboxnumofrandword.IsEnabled = false;
            this.textboxwordinput.IsEnabled = true;
            this.buttonaddword.IsEnabled = true;
            this.buttondeleteword.IsEnabled = true;
            this.listboxwords.IsEnabled = true;
        }

        private void buttonaddword_click(object sender, RoutedEventArgs e)
        {
            checkifwordaddable();
        }

        private void textboxwordinput_onkeydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                checkifwordaddable();

            //enable continue button
        }

        private void textboxnumofrandword_check(object sender, TextCompositionEventArgs e)
        {
            //prevents any non-numeric characters from being entered
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            //enable continue button
            checkifcontinue();
        }

        private void textboxtime_check(object sender, TextCompositionEventArgs e)
        {
            //prevents any non-numeric characters from being entered
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            //enable continue button
            checkifcontinue();
        }

        private void textbosnumofrandword_previewkeydown(object sender, KeyEventArgs e)
        {
            //prevents the spacebar from inputting whitespace
            if (e.Key == Key.Space) e.Handled = true;
            checkifcontinue();
        }

        private void buttondeleteword_click(object sender, RoutedEventArgs e)
        {
            if (this.listboxwords.SelectedItem != null)
                this.listboxwords.Items.Remove(this.listboxwords.SelectedItem);
        }


        private void buttoneditteamnames_click(object sender, RoutedEventArgs e)
        {
            TeamNameWindow w = new TeamNameWindow(namelist);
            w.ShowDialog();
        }

        private void comboboxnumofteams_selchanged(object sender, SelectionChangedEventArgs e)
        {
            //change name of teams during the initialization of the window and when the selected item in the combobox is changed
            namelist.Clear();
            for (int i = 0; i < this.comboboxnumofteams.SelectedIndex + 2; i++)
                namelist.Add($"{FindResource("Team")} {i + 1}");
        }

        private void textboxnumofrandword_textchanged(object sender, TextChangedEventArgs e)
        {

            checkifcontinue();
        }

        private void buttonsecondplay_click(object sender, RoutedEventArgs e)
        {
            List<String> filteredwords;
            if (this.radiouserdic.IsChecked == true)
            {
                filteredwords = new List<string>();
                Random r = new Random();
                for (int i = 0; i < int.Parse(this.textboxnumofrandword.Text); i++)
                {
                    int rnd = r.Next(0, wordlist.Count - 1);
                    filteredwords.Add(wordlist[rnd].TrimEnd('\r', '\n'));
                    wordlist.RemoveAt(rnd);
                }
                GameWindow w = new GameWindow(namelist, filteredwords, int.Parse(this.textboxtime.Text));
                w.ShowDialog();
            }
            else
            {
                filteredwords = new List<string>();
                for(int i = 0; i < listboxwords.Items.Count; i++)
                    filteredwords.Add(listboxwords.Items[i].ToString());

                GameWindow w = new GameWindow(namelist, filteredwords, int.Parse(this.textboxtime.Text));
                w.ShowDialog();
            }
            this.Close();
        }

        private void Buttonloadfile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.Title = FindResource("OpenTextFile").ToString();
            dlg.InitialDirectory = $"{System.AppDomain.CurrentDomain.BaseDirectory}words";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT files|*.txt";
            fileopenresult = dlg.ShowDialog();
            
            if(fileopenresult == true)
            {
                wordlist = File.ReadAllText(dlg.FileName).Split('\n').ToList();
                checkifcontinue();
            }
        }
    }
}
