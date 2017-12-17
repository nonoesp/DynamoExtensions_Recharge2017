using System;
using System.Windows;
using System.Windows.Controls;
using Dynamo.Wpf.Extensions;

namespace RechargeViewExtension
{
    /// <summary>
    /// This is a View Extension sample created during the 2017 Recharge.
    /// The goal is provide clarity and new documentation around creating
    /// extensions for Dynamo as there are currently very limited resources.
    /// The GitHub repo will include step by step instructions walking through
    /// the development process. Please feel free to make a PR adding any
    /// additional points that have been omitted.
    /// </summary>
    public class RechargeViewExtension : IViewExtension
    {
        // Create a variable for our menu item, this is how the
        // user will launch the pop-up window within Dynamo
        private MenuItem rechargeMenuItem;

        public void Dispose() { }

        public void Startup(ViewStartupParams p) { }

        public void Loaded(ViewLoadedParams p)
        {
            // Specify the text displayed on the menu item
            rechargeMenuItem = new MenuItem { Header = "LiteVP" };

            // Define the behavior when menu item is clicked
            rechargeMenuItem.Click += (sender, args) =>
            {
                // Instantiate a viewModel and window
                var viewModel = new RechargeWindowViewModel(p);
                var window = new RechargeWindow
                {
                    // Set the data context for the main grid in the window
                    // This refers to the main grid also seen in our xaml file
                    MainGrid = { DataContext = viewModel },

                    // Set the owner of the window to the Dynamo window.
                    Owner = p.DynamoWindow
                };

                // Set the window position
                window.Left = window.Owner.Left + 800;
                window.Top = window.Owner.Top + 800;

                // Show a modeless window.
                window.Show();
            };

            // add the menu item to our loaded parameters
            p.AddMenuItem(MenuBarType.View, rechargeMenuItem);
        }

        public void Shutdown() { }

        public string UniqueId
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        public string Name
        {
            get
            {
                return "Recharge View Extension";
            }
        }

    }
}
