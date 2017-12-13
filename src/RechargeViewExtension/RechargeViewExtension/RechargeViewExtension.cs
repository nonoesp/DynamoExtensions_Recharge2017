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
        public void Dispose() { }

        public void Startup(ViewStartupParams p) { }

        public void Loaded(ViewLoadedParams p) { }

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
