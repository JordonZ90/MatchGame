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
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();              
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }

        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐵", "🐵",
                "🐶", "🐶",
                "🐱", "🐱",
                "🐰", "🐰",
                "🐸", "🐸",
                "🐭", "🐭",
                "🐔", "🐔",
                "🐲", "🐲",
            };

            Random random = new Random(); // Create a new random number generator

            // Find every TextBlock in the main grid
            // and repeat the flowwowing statements for each of them
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {

                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count); // Pick a random number
                                                                // between 0 and the number
                                                                // of the emoji left in the list and call it "index"
                    string nextEmoji = animalEmoji[index]; // Use the random number called "index" to get a random emoji from the list
                    textBlock.Text = nextEmoji; // Update the textblock with the random emoji from the list
                    animalEmoji.RemoveAt(index); // Remove the random emoji from the list

                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false; 
        // findingMatch keeps track if whether or not the player just clicked on the first
        // animal in a pair and is now trying to find its match

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            // Player clicked the first animal in a pair. So it makes that animal invisible
            // And keeps track of its TextBlock in case it needs to make it visible again
            if (findingMatch == false)                        
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            // Player found a match, so it makes the second animal in the pair invisble and unclickable
            // Then resets findingMAtch so the next animal clicked on is the first one in a pair again
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            // Player clicks on an animal that doesn't match, so it makes the first animal that was clicked
            // Visible again and resets findingMatch
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Resets the game if all 8 matched pairs have been found
            // Otherwise does nothing since the game would still be running
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
