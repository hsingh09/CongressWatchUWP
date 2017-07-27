using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CongressWatchUWP.RemoteAPIs;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CongressWatchUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Representative> m_myReps;

        enum currentMode
        {
            myReps,
            idealHouse,
            idealSenate
        };

        public MainPage()
        {
            this.InitializeComponent();
            m_myReps = new List<Representative>();
        }

        private async void Zip_Button_Click(object sender, RoutedEventArgs e)
        {
            RepList.Items.Clear();

            string zip = "98122";
            if (zipcodeBox.Text != string.Empty)
                zip = zipcodeBox.Text;
             List<Representative> reps = await RemoteAPIs.RESTClient.GetRepresentativesAsync("http://congresswatch.azurewebsites.net/zipcode/" + zip);
            foreach (Representative r in reps)
            {
                RepresentativeView repView = new RepresentativeView(r);
                
                RepList.Items.Add(repView);
                if (m_myReps.Contains(r))
                    repView.isFav = true;

            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;
            RepresentativeView repView = toggleButton.DataContext as RepresentativeView;
            if (repView != null)
            {
                m_myReps.Add(repView.rep);
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;
            RepresentativeView repView = toggleButton.DataContext as RepresentativeView;
            m_myReps.Remove(repView.rep);
        }

        private void MyReps_Click(object sender, RoutedEventArgs e)
        {
            MakeUniqueYellowButton(currentMode.myReps);
            ZipCodeSearch.Visibility = Visibility.Collapsed;
            FindMyRepsButton.Visibility = Visibility.Visible;
            RepList.Items.Clear();
            
            foreach (Representative r in m_myReps)
            {
                RepresentativeView repView = new RepresentativeView(r);
                RepList.Items.Add(repView);
                if (m_myReps.Contains(r))
                    repView.isFav = true;
            }
        }

        private async void IdealHouse_Click(object sender, RoutedEventArgs e)
        {
            MakeUniqueYellowButton(currentMode.idealHouse);
            ZipCodeSearch.Visibility = Visibility.Collapsed;
            FindMyRepsButton.Visibility = Visibility.Collapsed;
            RepList.Items.Clear();
            
            List<Representative> reps = await RemoteAPIs.RESTClient.GetRepresentativesAsync("http://congresswatch.azurewebsites.net/house");
            foreach (Representative r in reps)
            {
                RepresentativeView repView = new RepresentativeView(r);
                RepList.Items.Add(repView);
                if (m_myReps.Contains(r))
                    repView.isFav = true;
            }
        }

        private async void IdealSenate_Click(object sender, RoutedEventArgs e)
        {
            MakeUniqueYellowButton(currentMode.idealSenate);
            ZipCodeSearch.Visibility = Visibility.Collapsed;
            FindMyRepsButton.Visibility = Visibility.Collapsed;

            RepList.Items.Clear();

            
            List<Representative> reps = await RemoteAPIs.RESTClient.GetRepresentativesAsync("http://congresswatch.azurewebsites.net/senate");
            foreach (Representative r in reps)
            {
                RepresentativeView repView = new RepresentativeView(r);
                RepList.Items.Add(repView);
                if (m_myReps.Contains(r))
                    repView.isFav = true;

            }
        }

        private void FindMyRepsButton_Click(object sender, RoutedEventArgs e)
        {
            ZipCodeSearch.Visibility = Visibility.Visible;
            FindMyRepsButton.Visibility = Visibility.Collapsed;

        }

        private void MakeUniqueYellowButton(currentMode mode)
        {
            IdealHouse.Background = new SolidColorBrush(Colors.DarkGray);
            IdealSenate.Background = new SolidColorBrush(Colors.DarkGray);
            MyReps.Background = new SolidColorBrush(Colors.DarkGray);

            switch (mode)
            {
                case currentMode.myReps:
                    MyReps.Background = new SolidColorBrush(Colors.Yellow);
                    break;
                case currentMode.idealHouse:
                    IdealHouse.Background = new SolidColorBrush(Colors.Yellow);
                    break;
                case currentMode.idealSenate:
                    IdealSenate.Background = new SolidColorBrush(Colors.Yellow);
                    break;
                default:
                    break;
            }
        }
    }

    class RepresentativeView
    {
        public Representative rep { get; set; }
        public string displayName { get; set; }
        public SolidColorBrush displayColor { get; set; }
        public int repId { get; set; }
        public string chamber { get; set; }
        public string party { get; set; }
        public bool? isFav { get; set; }

    public RepresentativeView(Representative r)
        {
            rep = r;
            displayName = rep.firstName + " " + rep.lastName;
            isFav = false;

            switch(rep.party)
            {
                case "0":
                    displayColor = new SolidColorBrush(Colors.RoyalBlue);
                    break;
                case "1":
                    displayColor = new SolidColorBrush(Colors.Crimson);
                    break;
                case "2":
                    displayColor = new SolidColorBrush(Colors.SeaGreen);
                    break;
                default:
                    displayColor = new SolidColorBrush(Colors.Purple);
                    break;
            }
            switch (rep.chamber)
            {
                case 0:
                    chamber = "House";
                    break;
                case 1:
                    chamber = "Senate";
                    break;
                default:
                    break;
            }
            // need to convert rep.charter from 0||1 for house/senate
            // need to convert rep.party from 0..9 for dem, rep , green other
        }
    }
}
