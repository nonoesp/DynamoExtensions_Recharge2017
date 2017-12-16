using System.Windows;
using CefSharp;
using System.IO;

namespace RechargeViewExtension
{
    /// <summary>
    /// Interaction logic for RechargeWindow.xaml
    /// </summary>
    public partial class RechargeWindow : Window
    {
        public RechargeWindow()
        {
            if (!Cef.IsInitialized)
            {
                var settings = new CefSettings { RemoteDebuggingPort = 8088 };
                //to fix Fickering set disable-gpu to true
                //settings.CefCommandLineArgs.Add("disable-gpu", "1");
                settings.SetOffScreenRenderingBestPerformanceArgs();
                //Cef.Initialize(settings);

                settings.RegisterScheme(new CefCustomScheme
                {
                    SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                    SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
                    /*
                    IsSecure = true //treated with the same security rules as those applied to "https" URLs
                                    //SchemeHandlerFactory = new InMemorySchemeAndResourceHandlerFactory()
                                    */
                });

                Cef.Initialize(settings);
            }

            InitializeComponent();
        }
    }
}