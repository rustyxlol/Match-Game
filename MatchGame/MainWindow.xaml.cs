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
    using System.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal);
        int tenthsOfSeconds;
        int matchesFound;
        float bestTime = 99999999/10F;
        float currentTime;
        SoundPlayer matchSound = new SoundPlayer("../../../assets/Match.wav");
        SoundPlayer gameFinishSound = new SoundPlayer("../../../assets/Finish.wav");
        SoundPlayer missMatchSound = new SoundPlayer("../../../assets/MissMatch.wav");
        SoundPlayer newHighScoreSound = new SoundPlayer("../../../assets/wow.wav");
        SoundPlayer startGameSound = new SoundPlayer("../../../assets/StartGame.wav");
        SoundPlayer welcomeSound = new SoundPlayer("../../../assets/welcome.wav");
        public MainWindow()
        {
            welcomeSound.Load();
            welcomeSound.Play();
            InitializeComponent();

            highScoreTextBlock.Text = "";
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSeconds++;
            currentTime = tenthsOfSeconds / 100F;
            timeTextBlock.Text = currentTime.ToString("0.00s");
            if (matchesFound == 8)
            {
                if (currentTime < bestTime)
                {
                    newHighScoreSound.Play();
                    bestTime = currentTime;
                }
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text;
                highScoreTextBlock.Text = "Highscore: " + bestTime.ToString("0.00s");
            }
        }

        private void SetupGame()
        {

            newHighScoreSound.Load();
            matchSound.Load();
            gameFinishSound.Load();
            missMatchSound.Load();
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
                if (textBlock.Name != "timeTextBlock" && textBlock.Name != "highScoreTextBlock")
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

                matchSound.Play();
                textBlock.Visibility = Visibility.Hidden;
                matchesFound++;
                findingMatch = false;
            }
            else
            {
                lastBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
                missMatchSound.Play();
            }
            if(matchesFound == 8)
            {
                playagainBtn.Visibility = Visibility.Visible;
                gameFinishSound.Play();
            }
        }       
        private void startBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startGameSound.Play();
            SetupGame();
            startBtn.Visibility = Visibility.Hidden;
        }

        private void playagainBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(matchesFound == 8)
            {
                SetupGame();
                playagainBtn.Visibility = Visibility.Hidden;
            }
        }
    }
}
