using System;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using System.Windows.Controls;
using System.IO;

namespace RechargeViewExtension
{
    /// <summary>
    /// Interaction logic for RechargeWindow.xaml
    /// </summary>
    public partial class RechargeWindow : Window
    {
        private ChromiumWebBrowser browser;

        public ChromiumWebBrowser Browser
        {
            get
            {
                return this.browser;
            }
            set
            {
                this.browser = value;
            }
        }

        public RechargeWindow()
        {
            if (!Cef.IsInitialized)
            {
                var settings = new CefSettings { RemoteDebuggingPort = 8088 };
                settings.SetOffScreenRenderingBestPerformanceArgs();

                settings.RegisterScheme(new CefCustomScheme
                {
                    SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                    SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
                });

                Cef.Initialize(settings);
            }

            InitializeComponent();

            browser = new ChromiumWebBrowser();
            browser.Address = @"C:\Users\alfarok\Documents\DynamoExtensions_Recharge2017\src\RechargeViewExtension\RechargeViewExtension\Resources\index.html";
            MainGrid.Children.Add(browser);

            // verify browser is loader before calling any javascript
            browser.FrameLoadEnd += WebBrowserFrameLoadEnded;
        }

        // test the ability to update the geometry color after the initial HTML is loaded
        private void WebBrowserFrameLoadEnded(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                string code = "changeColor(mesh)";
                browser.GetMainFrame().ExecuteJavaScriptAsync(code);
            }
        }
    }
}