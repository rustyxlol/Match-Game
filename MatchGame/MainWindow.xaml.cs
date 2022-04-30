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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSeconds;
        int matchesFound;
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;

            SetupGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSeconds++;
            timeTextBlock.Text = (tenthsOfSeconds/10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again";
            }
        }

        private void SetupGame()
        {
            List<string> animalEmojis = new List<string>()
            {
                "🐍", "🐍",
                "🐸", "🐸",
                "🐔", "🐔",
                "🐰", "🐰",
                "🦅", "🦅",
                "🦋", "🦋",
                "🐞", "🐞",
                "🕷", "🕷",
            };

            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmojis.Count);
                    string nextEmoji = animalEmojis[index];
                    textBlock.Text = nextEmoji;
                    animalEmojis.RemoveAt(index);
                }
            }
            timer.Start();
            tenthsOfSeconds = 0;
            matchesFound = 0;

        }


        TextBlock lastBlockClicked;
        bool findingMatch = false;
        
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (lastBlockClicked.Text == textBlock.Text)
            {
                textBlock.Visibility = Visibility.Hidden;
                matchesFound++;
                findingMatch = false;
            }
            else
            {
                lastBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }


        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetupGame();
            }
        }
    }
}
