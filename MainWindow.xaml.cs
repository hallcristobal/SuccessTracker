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

namespace SuccessTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string RateHeader = "Success Rate: ";
        private string LastRateHeader = "Rate Last ";
        private string StreakHeader = "Current Streak: ";
        private string BestStreakHeader = "Best Streak: ";
        private List<bool> CurrentStreak;

        private int LastRateAmmount = 10;
        private int Success = 0;
        private int Attempts = 0;
        private int Streak = 0;
        private int BestStreak = 0;

        private bool LastRateEnabled = true;

        public MainWindow()
        {
            CurrentStreak = new List<bool>();
            InitializeComponent();
        }

        private void btnSuccess_Click(object sender, RoutedEventArgs e)
        {
            Success++;
            Attempts++;
            Streak++;
            CurrentStreak.Add(true);
            Update();
        }

        private void brnFail_Click(object sender, RoutedEventArgs e)
        {
            Attempts++;

            if (Streak > BestStreak)
                BestStreak = Streak;

            Streak = 0;
            CurrentStreak.Add(false);
            Update();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Success = Attempts = Streak = BestStreak = 0;
            CurrentStreak.Clear();
            Update();
        }
        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            cmTextBox.Text = LastRateAmmount.ToString();
        }

        private void cmTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter || e.Key == Key.Return)
            {
                int newAmnt = 0;
                if (int.TryParse(((TextBox)sender).Text, out newAmnt))
                {
                    LastRateAmmount = newAmnt;
                    if (newAmnt != 0)
                        LastRateEnabled = true;

                    Update();
                }
            }
        }
        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            LastRateEnabled = ((MenuItem)sender).IsChecked;
            Update();
        }

        private void Update()
        {
            if (Streak > BestStreak)
                BestStreak = Streak;

            lblAttemptCount.Content = Attempts;
            lblSuccessCount.Content = Success;
            lblSuccessStreak.Content = StreakHeader + Streak;
            lblBestStreak.Content = BestStreakHeader + BestStreak;

            if (Attempts > 0)
            {
                float rate = ((float)Success / (float)Attempts) * 100.0f;
                lblRate.Content = RateHeader + rate.ToString("N2") + '%';
            }
            else
                lblRate.Content = RateHeader + "0.00%";

            if(LastRateAmmount == 0 || !LastRateEnabled)
                lblLastRate.Content = (char)0x2014;
            else if (CurrentStreak.Count > 0)
            {
                float rAttempts = 0, rSuccess = 0;
                int stop = CurrentStreak.Count > LastRateAmmount ? CurrentStreak.Count - LastRateAmmount : 0;
                for(int i = CurrentStreak.Count - 1; i >= stop; i--)
                {
                    rAttempts++;
                    if (CurrentStreak[i])
                        rSuccess++;
                }
                float rate = rSuccess / rAttempts * 100.0f;
                lblLastRate.Content = LastRateHeader + LastRateAmmount.ToString() + ": " + rate.ToString("N2") + '%';
            }
            else
                lblLastRate.Content = LastRateHeader + LastRateAmmount.ToString() + ": " + "0.00%";
        }

    }
}
