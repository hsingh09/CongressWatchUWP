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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CongressWatchUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Zip_Button_Click(object sender, RoutedEventArgs e)
        {
            string zip = "98122";
            if (zipcodeBox.Text != string.Empty)
                zip = zipcodeBox.Text;
            List<Representative> reps = await RemoteAPIs.RESTClient.GetRepresentativesAsync("http://localhost:8081/zipcode/"+zip);
            foreach (Representative r in reps)
            {
                RepList.Items.Add(r.firstName + " " + r.lastName);
            }
        }
    }
}
