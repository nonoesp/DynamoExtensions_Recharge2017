using System;
using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using CefSharp;

namespace RechargeViewExtension
{
    class RechargeWindowViewModel : NotificationObject, IDisposable
    {
        private string selectedNodesText = "Begin selecting ";

        // Variable for storing a reference to our loaded parameters
        private ReadyParams readyParams;

        // Display the number of nodes in the active workspace
        public string SelectedNodesText => $"Active nodes:\n{getNodeTypes()}";

        public string getNodeTypes()
        {
            string output = "";

            foreach (NodeModel node in readyParams.CurrentWorkspaceModel.Nodes)
            {
                string nickName = node.NickName;
                output += nickName + "\n";
            }

            return output;
        }

        public RechargeWindowViewModel(ReadyParams p)
        {
            // Save a reference to our loaded parameters which
            // is required in order to access the workspaces
            readyParams = p;

            // Subscribe to NodeAdded and NodeRemoved events
            p.CurrentWorkspaceModel.NodeAdded += CurrentWorkspaceModel_NodesChanged;
            p.CurrentWorkspaceModel.NodeRemoved += CurrentWorkspaceModel_NodesChanged;
        }

        // Define what happens when the event above if triggered
        private void CurrentWorkspaceModel_NodesChanged(NodeModel obj)
        {
            // Event used to notify UI or bound elements that
            // the data has changed and needs to be updated
            RaisePropertyChanged("SelectedNodesText");
        }

        // Very important - unsubscribe from our events to prevent a memory leak
        public void Dispose()
        {
            readyParams.CurrentWorkspaceModel.NodeAdded -= CurrentWorkspaceModel_NodesChanged;
            readyParams.CurrentWorkspaceModel.NodeRemoved -= CurrentWorkspaceModel_NodesChanged;
        }
    }
}
