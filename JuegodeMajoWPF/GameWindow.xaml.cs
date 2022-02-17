using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow
    {
        DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);
        Random r = new Random();
        List<classteam> listclassteam;
        List<String> listwords;
        List<String> listwordsleft;
        List<String> teamnames;
        int roundtime;
        int currindex;
        int currgame;
        int currstartteam;
        int currnextteam;
        
        public GameWindow(List<String> _teamnames, List<String> _wordlist, int _roundtime)
        {
            InitializeComponent();
            roundtime = _roundtime;
            teamnames = _teamnames;
            listwords = _wordlist;
            this.textblocktime.Text = roundtime.ToString();
            currindex = 0;
            currgame = 0;
            currnextteam = 0;
            currstartteam = -1;
            listclassteam = new List<classteam>();
            InitializeTeams();
        }

        private void GameInit()
        {
            currgame++;
            if (currgame < 5)
            {
                this.textblockcurrentgame.Text = $"{FindResource("Game").ToString()} {currgame}";
                this.textblockstartgame.Text = $"{FindResource("StartGame").ToString()} ({currgame})";

                if ((currstartteam + 2) > teamnames.Count) currstartteam = 0;
                else currstartteam++;
                currnextteam = currstartteam;

                this.textblockcurrentteam.Text = teamnames[currstartteam];

                listwordsleft = new List<string>(listwords);

            }
            else FinalInit();

        }

        private void SetupTimer()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int timeleft = int.Parse(this.textblocktime.Text);

            if (timeleft > 0)
                this.textblocktime.Text = (timeleft - 1).ToString();
            else
            {
                timer.Stop();
                this.textblocktime.Text = Convert.ToString(roundtime);
                ProgressToNextTeam();
            }
                
        }

        private void ProgressToNextTeam()
        {
            if ((currnextteam + 2) > teamnames.Count) currnextteam = 0;
            else currnextteam++;
            this.textblockcurrentteam.Text = teamnames[currnextteam];
            this.textblockwordtoguess.Text = "-";
            this.textblockstartgame.Text = $"{FindResource("ContinueGame").ToString()} ({currgame})";
            this.buttoncorrect.Visibility = Visibility.Hidden;
            this.buttonfalse.Visibility = Visibility.Hidden;
            this.buttonstart.Visibility = Visibility.Visible;
        }

        private void InitializeTeams()
        {
            this.stackpanelpointslist.Children.RemoveAt(0);
            this.stackpanelpointslist.Children.RemoveAt(0);
            for (int i = 0; i < teamnames.Count; i++)
            {
                //create instance of classteam with the teamname teamnames[i] and setting score to 0
                listclassteam.Add(new classteam(teamnames[i], 0));

                this.textblockcurrentgame.Text = "-";
                this.textblockcurrentteam.Text = "-";

                Style style = this.textblocklistteamname.Style as Style;
                Thickness margin = new Thickness();
                margin.Left = 40;

                //add textblock for teamname to the stackpanel that displays points
                this.stackpanelpointslist.Children.Add(new TextBlock
                {
                    Name = $"textboxteamname{i + 1}",
                    Text = teamnames[i],
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Style = style,
                    FontSize = 30,
                    FontWeight = FontWeights.Light,
                    Foreground = Brushes.Black
                });

                //add textblock for points to the stackpanel that displays points
                this.stackpanelpointslist.Children.Add(new TextBlock
                {
                    Name = $"textboxteamname{i + 1}",
                    Text = $"0 {FindResource("Points")}",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Style = style,
                    FontSize = 30,
                    FontWeight = FontWeights.Light,
                    Margin = margin,
                    Foreground = Brushes.Green
                });
            }
            SetupTimer();
            GameInit();
        }



        private void Buttoncorrect_click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < listclassteam.Count; i++)
                if (listclassteam[i].classteamname == teamnames[currnextteam])
                {
                    listclassteam[i].classteampoints++;
                    break;
                }

            listwordsleft.RemoveAt(currindex);
            listclassteam.Sort((y, x) => x.classteampoints.CompareTo(y.classteampoints));

            int j = -1;
            string tmp = "";
            for (int i = 0; i < listclassteam.Count; i++)
            {
                ((TextBlock)this.stackpanelpointslist.Children[++j]).Text = listclassteam[i].classteamname;
                
                if(listclassteam[i].classteampoints == 1)
                {
                    tmp = FindResource("Points").ToString(); 
                    ((TextBlock)this.stackpanelpointslist.Children[++j]).Text = $"{Convert.ToString(listclassteam[i].classteampoints)} {tmp.Substring(0, (tmp.Length - 1))}";
                }
                    
                else
                    ((TextBlock)this.stackpanelpointslist.Children[++j]).Text = $"{Convert.ToString(listclassteam[i].classteampoints)} {FindResource("Points")}";
            }

            //check if the bowl ran out of words
            if (listwordsleft.Count != 0)
            {
                //get random number, 0 is inclusive and listwordsleft.count is non inclusive, that's why there's no -1
                currindex = r.Next(0, listwordsleft.Count);
                this.textblockwordtoguess.Text = listwordsleft[currindex];
            }
            else
            {
                timer.Stop();
                this.textblockwordtoguess.Text = "-";
                this.buttoncorrect.Visibility = Visibility.Hidden;
                this.buttonfalse.Visibility = Visibility.Hidden;
                this.buttonstart.Visibility = Visibility.Visible;
                this.textblocktime.Text = roundtime.ToString();
                GameInit();
            }
        }

        private void FinalInit()
        {
            
        }

        private void Buttonincorrect_click(object sender, RoutedEventArgs e)
        {
            //get random number, 0 is inclusive and listwordsleft.count is non inclusive, that's why there's no -1
            currindex = r.Next(0, listwordsleft.Count);
            this.textblockwordtoguess.Text = listwordsleft[currindex];
        }

        private class classteam
        {
            public string classteamname { get; set; }
            public int classteampoints { get; set; }

            public classteam(string _classteamname, int _classteampoints)
            {
                classteamname = _classteamname;
                classteampoints = _classteampoints;
            }
        }

        private void buttonstart_Click(object sender, RoutedEventArgs e)
        {
            this.buttoncorrect.Visibility = Visibility.Visible;
            this.buttonfalse.Visibility = Visibility.Visible;
            this.buttonstart.Visibility = Visibility.Hidden;
            currindex = r.Next(0, listwordsleft.Count - 1);
            this.textblockwordtoguess.Text = listwordsleft[currindex];
            timer.Start();
        }
    }
}
