using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Interfaces;
using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Models;
using Dynamo.Visualization;
using CefSharp;
using Newtonsoft.Json.Linq;

namespace RechargeViewExtension
{
    class RechargeWindowViewModel : NotificationObject, IDisposable
    {
        private string selectedNodesText = "Begin selecting ";

        // Variable for storing a reference to our loaded parameters
        private ReadyParams readyParams;

        public RenderPackageCache packageContent;

        // Find render nodes and build THREE meshes
        public string SelectedNodesText => $"{getNodeTypes()}";

        public string getNodeTypes()
        {
            // TODO this is a hack to remove and add a new mesh each render
            // We should check for existing mesh by name and simply update verts/indices
            // Mesh must be set similar to 'geometry.verticesNeedUpdate = true;'
            string output = "scene.remove(mesh);\n renderDynamoMesh(";

            /*
            foreach (NodeModel node in readyParams.CurrentWorkspaceModel.Nodes)
            {
                string nickName = node.Name;

                
                if(nickName == "MinimalVP.RenderMesh")
                {
                    // TODO - need to check if this is a list or not
                    // in order to properly handle replication
                    output += node.CachedValue.Data.ToString();
                }
                
            }
        */

            // TODO don't convert enums to lists
            List<double> verts = new List<double>();
            List<int> indices = new List<int>();
            List<double> norms = new List<double>();

            foreach (IRenderPackage p in packageContent.Packages)
            {
                verts = p.MeshVertices.ToList();
                indices = p.MeshIndices.ToList();
                norms = p.MeshNormals.ToList();
            }

            JObject meshObject = new JObject(
                new JProperty("name", "newMesh"),
                new JProperty("vertices", verts),
                new JProperty("faceIndices", indices));

            // How do I inject meshObject into view extension
            string jsonString = meshObject.ToString();

            output += jsonString;

            System.IO.File.WriteAllText(@"C:\Users\alfarok\Desktop\mesh.txt", jsonString);

            output += ");";

            return output;
        }

        public RechargeWindowViewModel(ReadyParams p)
        {
            // Save a reference to our loaded parameters which
            // is required in order to access the workspaces
            readyParams = p;

            // Subscribe to NodeAdded and NodeRemoved events
            //p.CurrentWorkspaceModel.NodeAdded += CurrentWorkspaceModel_NodesChanged;
            //p.CurrentWorkspaceModel.NodeRemoved += CurrentWorkspaceModel_NodesChanged;

            // TODO this could be dangerous if called in custom node ws
            var currentWS = p.CurrentWorkspaceModel as HomeWorkspaceModel;
            //currentWS.RefreshCompleted += CurrentWorkspaceModel_NodesChanged;

            foreach (NodeModel node in currentWS.Nodes)
            {
                if (node.Name == "Cuboid.ByLengths")
                {
                    node.RenderPackagesUpdated += CurrentWorkspaceModel_CuboidReady;
                }
                else if (node.Name == "Sphere.ByCenterPointRadius")
                {
                    node.RenderPackagesUpdated += CurrentWorkspaceModel_CuboidReady;
                }
            }
        }

        // Define what happens when the event above if triggered
        private void CurrentWorkspaceModel_NodesChanged(NodeModel obj)
        {
            // Event used to notify UI or bound elements that
            // the data has changed and needs to be updated
            RaisePropertyChanged("SelectedNodesText");
        }

        private void CurrentWorkspaceModel_NodesChanged(object sender, EventArgs e)
        {
            // Event used to notify UI or bound elements that
            // the data has changed and needs to be updated
            RaisePropertyChanged("SelectedNodesText");
        }

        private void CurrentWorkspaceModel_CuboidReady(NodeModel nodeModel, RenderPackageCache packages)
        {
            packageContent = packages;
            // Event used to notify UI or bound elements that
            // the data has changed and needs to be updated
            RaisePropertyChanged("SelectedNodesText");
        }

        // Very important - unsubscribe from our events to prevent a memory leak
        public void Dispose()
        {
            /*
            readyParams.CurrentWorkspaceModel.NodeAdded -= CurrentWorkspaceModel_NodesChanged;
            readyParams.CurrentWorkspaceModel.NodeRemoved -= CurrentWorkspaceModel_NodesChanged;

            var currentWS = readyParams.CurrentWorkspaceModel as HomeWorkspaceModel;
            currentWS.RefreshCompleted -= CurrentWorkspaceModel_NodesChanged;
            */
        }
    }
}
