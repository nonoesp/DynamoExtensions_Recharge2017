using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo;
using Autodesk.DesignScript.Geometry;
using RechargeViewExtension;
using Newtonsoft.Json.Linq;

namespace LiteVP
{
    public class LiteVP
    {
        public static string BuildMesh(string name, Vector[] vertices, int[] vertexIndicesbyTri)
        {
            // Build json object
            List<double> meshVertices = new List<double>();
            
            for (int i = 0; i < vertices.Length; i += 3)
            {
                meshVertices.Add(vertices[i].X);
                meshVertices.Add(vertices[i].Y);
                meshVertices.Add(vertices[i].Z);
            }

            JObject meshObject = new JObject(
                new JProperty("name", name),
                new JProperty("vertices", meshVertices),
                new JProperty("faceIndices", vertexIndicesbyTri));

            string jsonString = meshObject.ToString();

            // Viewer should identify this node is active via listening for an event

            // When the node updated flag is trigger the viewer injects the latest json object

            // We call a new JS function that builds or updates the mesh

            // return string representation
            return jsonString;
        }
    }
}
