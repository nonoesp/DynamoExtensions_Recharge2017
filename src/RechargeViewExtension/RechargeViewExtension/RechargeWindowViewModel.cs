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
        public string transactionType = "";
        public string nodeGuid = "";

        // Find render nodes and build THREE meshes
        public string SelectedNodesText => $"{getNodeTypes()}";

        public string getNodeTypes()
        {
            // TODO this is a hack to remove and add a new mesh each render
            // We should check for existing mesh by name and simply update verts/indices
            // Mesh must be set similar to 'geometry.verticesNeedUpdate = true;'
            string output = "renderDynamoMesh(";

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
                new JProperty("name", nodeGuid),
                new JProperty("transactionType", transactionType), 
                new JProperty("vertices", verts),
                new JProperty("faceIndices", indices));

            string jsonString = meshObject.ToString();

            output += jsonString + ");";

            return output;
        }

        public RechargeWindowViewModel(ReadyParams p)
        {
            // Save a reference to our loaded parameters which
            // is required in order to access the workspaces
            readyParams = p;

            // Subscribe to NodeAdded and NodeRemoved events
            p.CurrentWorkspaceModel.NodeAdded += CurrentWorkspaceModel_NodeAdded;
            p.CurrentWorkspaceModel.NodeRemoved += CurrentWorkspaceModel_NodeRemoved;

            // TODO this could be dangerous if called in custom node ws
            var currentWS = p.CurrentWorkspaceModel as HomeWorkspaceModel;
            //currentWS.RefreshCompleted += CurrentWorkspaceModel_NodesChanged;

            /*
            foreach (NodeModel node in currentWS.Nodes)
            {
                node.RenderPackagesUpdated += CurrentWorkspaceModel_UpdateViewportGeometry;
                if (node.Name == "Cuboid.ByLengths")
                {
                    node.RenderPackagesUpdated += CurrentWorkspaceModel_UpdateViewportGeometry;
                }
                else if (node.Name == "Sphere.ByCenterPointRadius")
                {
                    node.RenderPackagesUpdated += CurrentWorkspaceModel_UpdateViewportGeometry;
                }
            }
            */
        }

        // When a new node is added to the workspace
        private void CurrentWorkspaceModel_NodeAdded(NodeModel node)
        {
            // Update viewport when nodes render package is updated
            node.RenderPackagesUpdated += CurrentWorkspaceModel_UpdateViewportGeometry;
            //RaisePropertyChanged("SelectedNodesText");
        }

        // When an existing node is removed from the workspace
        private void CurrentWorkspaceModel_NodeRemoved(NodeModel node)
        {
            // TODO before unregistering we must remove any geom related to this node
            // Should inject this string somehow
            // string guid = nodeModel.GUID.ToString();
            // var output = "scene.getObjectByName({guid}); \nscene.remove(output);"
            // Consider also removing from global string list?
            nodeGuid = node.GUID.ToString();
            transactionType = "remove";
            // Unregister when node is removed
            RaisePropertyChanged("SelectedNodesText");
            node.RenderPackagesUpdated -= CurrentWorkspaceModel_UpdateViewportGeometry;
        }

        private void CurrentWorkspaceModel_UpdateViewportGeometry(NodeModel nodeModel, RenderPackageCache packages)
        {
            packageContent = packages;
            nodeGuid = nodeModel.GUID.ToString();
            transactionType = "update";
            // Event used to notify UI or bound elements that
            // the data has changed and needs to be updated
            RaisePropertyChanged("SelectedNodesText"/*SelectedNodesText(nodeModel.GUID.ToString())*/);
        }

        // Very important - unsubscribe from our events to prevent a memory leak
        public void Dispose()
        {
            
            readyParams.CurrentWorkspaceModel.NodeAdded -= CurrentWorkspaceModel_NodeAdded;
            readyParams.CurrentWorkspaceModel.NodeRemoved -= CurrentWorkspaceModel_NodeRemoved;

            /*
            var currentWS = readyParams.CurrentWorkspaceModel as HomeWorkspaceModel;
            currentWS.RefreshCompleted -= CurrentWorkspaceModel_NodesChanged;
            */
        }
    }
}
