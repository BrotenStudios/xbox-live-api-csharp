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
using Microsoft.Xbox.Services.System;
using Windows.Security.Authentication.Web.Core;
using System.Threading.Tasks;
using Microsoft.Xbox.Services.Stats.Manager;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.Leaderboard;
using Microsoft.Xbox.Services.Shared;

#pragma warning disable 4014

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPIntegration
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        XboxLiveUser xblUser;
        public MainPage()
        {
            this.InitializeComponent();
            xblUser = new XboxLiveUser();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<Windows.System.User> users =  await Windows.System.User.FindAllAsync();
            Windows.System.User user = null;
            if (users.Count > 0)
                user = users[0];
            xblUser.SignInAsync(null).ContinueWith( (Task<SignInResult> result) => 
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    SignInResult res = result.Result;
                    if (res.Status == SignInStatus.Success)
                    {
                        textBlock.Text = xblUser.Gamertag;
                        StatsManager.Singleton.AddLocalUser(xblUser);
                    }
                    else
                    {
                        textBlock.Text = "Not Signed In";
                    }

                });
            });

        }

        int jumps = 0;

        private void StatsButton_Click(object sender, RoutedEventArgs e)
        {
            if (xblUser.IsSignedIn)
            {
                StatsManager.Singleton.SetStatAsInteger(xblUser, "Jumps", jumps++);
                StatsManager.Singleton.RequestFlushToService(xblUser);
                StatsManager.Singleton.DoWork();
            }
        }

        private void leaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            if(xblUser.IsSignedIn)
            {
                XboxLiveContext services = new XboxLiveContext(xblUser);
                try
                {
                    services.LeaderboardService.GetLeaderboardAsync("Jumps", new LeaderboardQuery()).ContinueWith((Task<LeaderboardResult> lbResult) =>
                    {
                        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            LeaderboardResult lbRes = lbResult.Result;
                            leaderboardData.Text = "\nrows: " + lbRes.Rows.Count + "\n";
                            foreach (LeaderboardRow row in lbRes.Rows)
                            {
                                leaderboardData.Text += row.Gamertag + ": " + row.Rank + "\n";
                            }
                        });
                    });
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
