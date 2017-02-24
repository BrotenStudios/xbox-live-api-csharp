// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// 

namespace UWPIntegration
{
    using System;
    using System.Linq;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    using Microsoft.Xbox.Services;
    using Microsoft.Xbox.Services.Leaderboard;
    using Microsoft.Xbox.Services.Social.Manager;
    using Microsoft.Xbox.Services.Stats.Manager;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly XboxLiveUser xblUser;

        public MainPage()
        {
            this.InitializeComponent();
            this.xblUser = new XboxLiveUser();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var signInResult = await this.xblUser.SignInAsync(null);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (signInResult.Status == SignInStatus.Success)
                {
                    this.textBlock.Text = this.xblUser.Gamertag;
                    StatsManager.Singleton.AddLocalUser(this.xblUser);
                    SocialManager.Instance.AddLocalUser(this.xblUser, SocialManagerExtraDetailLevel.None);
                }
                else
                {
                    this.textBlock.Text = "Not Signed In";
                }
            });
        }

        private int jumps;

        private async void leaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.xblUser.IsSignedIn)
            {
                XboxLiveContext services = new XboxLiveContext(this.xblUser);
                var leaderboardResult = await services.LeaderboardService.GetLeaderboardAsync("jumps", new LeaderboardQuery());

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.leaderboardData.Text = "\nrows: " + leaderboardResult.Rows.Count + "\n";
                    foreach (LeaderboardRow row in leaderboardResult.Rows)
                    {
                        this.leaderboardData.Text += row.Gamertag + ": " + row.Rank + "\n";
                    }
                });
            }
        }

        private void WriteStats_Click(object sender, RoutedEventArgs e)
        {
            if (!this.xblUser.IsSignedIn) return;

            StatsManager.Singleton.SetStatAsInteger(this.xblUser, "jumps", this.jumps++);
        }

        private void ReadStats_Click(object sender, RoutedEventArgs e)
        {
            if (!this.xblUser.IsSignedIn) return;

            var statNames = StatsManager.Singleton.GetStatNames(this.xblUser);
            this.StatsData.Text = string.Join(Environment.NewLine, statNames.Select(n => StatsManager.Singleton.GetStat(this.xblUser, n)).Select(s => $"{s.Name} ({s.Type}) = {s.Value}"));
        }

        private void StatsDoWork_Click(object sender, RoutedEventArgs e)
        {
            if (!this.xblUser.IsSignedIn) return;

            StatsManager.Singleton.DoWork();
        }
    }
}