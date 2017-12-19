using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo;
using Autodesk.DesignScript.Geometry;
using Newtonsoft.Json.Linq;

namespace MinimalVP
{
    public class MinimalVP
    {
        public static string RenderMesh(string name, double[] vertices, int[] vertexIndicesbyTri)
        {
            // Build json object
            JObject meshObject = new JObject(
                new JProperty("name", name),
                new JProperty("vertices", vertices),
                new JProperty("faceIndices", vertexIndicesbyTri));

            // Viewer should identify this node is active via listening for an event

            // When the node updated flag is trigger the viewer injects the latest json object

            // We call a new JS function that builds or updates the mesh

            // return string representation
            string jsonString = meshObject.ToString();

            return jsonString;
        }
    }
}
