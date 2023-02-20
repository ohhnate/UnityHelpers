// GraphVisualizer.cs - A simple example of how to visualize a generic graph data structure in Unity.
// Version 1.0.0
// Author: Unity
// Website: https://github.com/ohhnate
//
// This is an example use case of the Graph implementation, which allows adding, removing and querying nodes and edges.
// In this script, we visualize the graph in the Unity editor using Gizmos, and we assume that Gizmos are enabled for the visualization to work properly.
// This can be useful for debugging and understanding how a graph behaves in a Unity project.
//
// No accreditation is required but it would be highly appreciated <3

using UnityEngine;

public class GraphVisualizer : MonoBehaviour
{
    private Graph<Vector2Int> _graph;

    private void Start()
    {
        _graph = new Graph<Vector2Int>();
        _graph.AddVertex(new Vector2Int(0, 0));
        _graph.AddVertex(new Vector2Int(1, 1));
        _graph.AddVertex(new Vector2Int(2, 2));
        _graph.AddVertex(new Vector2Int(3, 3));
        _graph.AddEdge(new Vector2Int(0, 0), new Vector2Int(1, 1));
        _graph.AddEdge(new Vector2Int(1, 1), new Vector2Int(2, 2));
        _graph.AddEdge(new Vector2Int(2, 2), new Vector2Int(3, 3));
        _graph.AddEdge(new Vector2Int(0, 0), new Vector2Int(2, 2));
        _graph.AddEdge(new Vector2Int(1, 1), new Vector2Int(3, 3));
    }

    private void OnDrawGizmos()
    {
        if (_graph == null) return;
        
        foreach (Vector2Int vertex in _graph.Vertices)
        {
            Gizmos.color = Color.white;
            Vector3 newVertex = new Vector3(vertex.x, vertex.y, 0);
            Gizmos.DrawSphere(newVertex, 0.2f);
            foreach (Vector2Int neighbor in _graph.GetNeighbors(vertex))
            {
                Vector3 newNeighbor = new Vector3(neighbor.x, neighbor.y, 0);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(newVertex, newNeighbor);
            }
        }
    }
}