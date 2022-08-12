using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using HotSearch.Utility;
using System;
using Windows.System;
using HotSearch.Model;
using Windows.Foundation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace HotSearch
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            InitTitleBar();
            LoadData("BaiduHotSearchItem");
        }
        private async void LoadData(string selectItem)
        {
            HotCore hotCore = new HotCore();
            switch (selectItem)
            {
                case "BaiduHotSearchItem":
                    DataList.ItemsSource = hotCore.BaiduHotSearch();
                    break;
                case "WeiboHotSearchItem":
                    DataList.ItemsSource = await hotCore.WeiboHotSearch();
                    break;
                case "ZhihuHotSearchItem":
                    DataList.ItemsSource = hotCore.ZhihuHotSearch();
                    break;
                case "ZhihuHotListItem":
                    DataList.ItemsSource = await hotCore.ZhihuHotList();
                    break;
                case "DouyinHotListItem":
                    DataList.ItemsSource = await hotCore.DouyinHotList();
                    break;
                case "KuaishouHotListItem":
                    DataList.ItemsSource = hotCore.KuaishouHotList();
                    break;
                case "WeixinHotWordsItem":
                    DataList.ItemsSource = hotCore.WeixinHotWords();
                    break;
                case "TiebaHotDiscussionItem":
                    DataList.ItemsSource = hotCore.TiebaHotDiscussion();
                    break;
                case "ToutiaoHotListItem":
                    DataList.ItemsSource = await hotCore.ToutiaoHotList();
                    break;
            }
        }
        private void DataList_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ToWeb();
        }
        private async void ToWeb()
        {
            HotInfo selectItem = (HotInfo)DataList.SelectedItem;
            if (selectItem.Link != null)
            {
                await Launcher.LaunchUriAsync(new Uri(selectItem.Link));
            }
        }
        private void InitTitleBar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            ApplicationViewTitleBar appTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                appTitleBar.ButtonBackgroundColor = Colors.Transparent;
                appTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                appTitleBar.ButtonForegroundColor = Colors.White;
                appTitleBar.ButtonInactiveForegroundColor = Colors.White;
            }
            else
            {
                appTitleBar.ButtonBackgroundColor = Colors.Transparent;
                appTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                appTitleBar.ButtonForegroundColor = Colors.Black;
                appTitleBar.ButtonInactiveForegroundColor = Colors.Black;
            }

            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout();

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);

            // Register a handler for when the size of the overlaid caption control changes.
            // For example, when the app moves to a screen with a different DPI.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

        }
        private void AppNav_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            Microsoft.UI.Xaml.Controls.NavigationViewItem selectItem = AppNav.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;
            LoadData(selectItem.Name);
        }
        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout();
        }
        private void UpdateTitleBarLayout()
        {
            // Get the size of the caption controls area and back button 
            // (returned in logical pixels), and move your content around as necessary.
            TitleBar.Margin = new Thickness(15, 10, 0, 0);
        }
    }
}