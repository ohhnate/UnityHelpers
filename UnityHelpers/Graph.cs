// Graph.cs - A generic graph data structure for storing nodes and edges, and traversing them with 2 different algorithms.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This Graph implementation allows adding, removing and querying nodes and edges, as well as traversing the graph using 2 different algorithms.
// It can be useful for pathfinding and similar tasks in a Unity project or any other application.
// No accreditation is required but it would be highly appreciated <3

using System.Collections.Generic;

/// <summary>
/// A generic graph data structure that allows adding, removing and querying nodes and edges, as well as traversing the graph using 2 different algorithms.
/// </summary>
/// <typeparam name="T">The type of data that each node in the graph will store.</typeparam>
public class Graph<T>
{
    private readonly Dictionary<T, List<T>> _adjacencyList;

    public Graph()
    {
        _adjacencyList = new Dictionary<T, List<T>>();
    }

    /// <summary>
    /// Adds a new vertex to the graph.
    /// </summary>
    public void AddVertex(T vertex)
    {
        if (!_adjacencyList.ContainsKey(vertex))
        {
            _adjacencyList[vertex] = new List<T>();
        }
    }

    /// <summary>
    /// Adds a new edge to the graph.
    /// </summary>
    public void AddEdge(T source, T destination)
    {
        if (!_adjacencyList.ContainsKey(source))
        {
            AddVertex(source);
        }
        if (!_adjacencyList.ContainsKey(destination))
        {
            AddVertex(destination);
        }
        _adjacencyList[source].Add(destination);
        _adjacencyList[destination].Add(source); 
    }

    /// <summary>
    /// Adds a new edge to the directed graph.
    /// </summary>
    public void AddEdgeDirectedGraph(T source, T destination)
    {
        if (!_adjacencyList.ContainsKey(source))
        {
            AddVertex(source);
        }
        if (!_adjacencyList.ContainsKey(destination))
        {
            AddVertex(destination);
        }
        _adjacencyList[source].Add(destination);
    }

    /// <summary>
    /// Removes an edge from the graph.
    /// </summary>
    public void RemoveEdge(T source, T destination)
    {
        if (!_adjacencyList.ContainsKey(source) || !_adjacencyList.ContainsKey(destination)) return;
        
        _adjacencyList[source].Remove(destination);
        _adjacencyList[destination].Remove(source); 
    }

    /// <summary>
    /// Removes an edge from a directed graph.
    /// </summary>
    public void RemoveEdgeDirectedGraph(T source, T destination)
    {
        if (!_adjacencyList.ContainsKey(source) || !_adjacencyList.ContainsKey(destination)) return;
        
        _adjacencyList[source].Remove(destination);
    }

    /// <summary>
    /// Removes a vertex from the graph.
    /// </summary>
    public void RemoveVertex(T vertex)
    {
        if (!_adjacencyList.ContainsKey(vertex)) return;
        
        _adjacencyList.Remove(vertex);
        foreach (KeyValuePair<T, List<T>> pair in _adjacencyList)
        {
            List<T> edges = pair.Value;
            edges.Remove(vertex);
        }
    }

    /// <summary>
    /// Performs a breadth-first search on the graph and returns a list of vertices in BFS order.
    /// BFS is a systematic approach that guarantees finding the shortest path and requires
    /// MORE memory than BFS to store the stack but LESS memory to call the stack.
    /// </summary>
    public List<T> BreadthFirstSearch(T start)
    {
        List<T> result = new();
        if (!_adjacencyList.ContainsKey(start))
        {
            return result;
        }
        HashSet<T> visited = new();
        Queue<T> queue = new();
        visited.Add(start);
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            T vertex = queue.Dequeue();
            result.Add(vertex);
            foreach (T neighbor in _adjacencyList[vertex])
            {
                if (visited.Contains(neighbor)) continue;
                
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
            }
        }
        return result;
    }

    /// <summary>
    /// Performs a depth-first search on the graph and returns a list of vertices in DFS order.
    /// DFS is suited for exploring a particular branch or finding a path that meets a certain criterion and requires
    /// MORE memory than BFS to call the stack but LESS memory to store the stack.
    /// </summary>
    public List<T> DepthFirstSearch(T start)
    {
        List<T> result = new();
        if (!_adjacencyList.ContainsKey(start))
        {
            return result;
        }
        HashSet<T> visited = new();
        DepthFirstSearchHelper(start, visited, result);
        return result;
    }

    //recursive
    private void DepthFirstSearchHelper(T vertex, ISet<T> visited, ICollection<T> result)
    {
        visited.Add(vertex);
        result.Add(vertex);
        foreach (T neighbor in _adjacencyList[vertex])
        {
            if (!visited.Contains(neighbor))
            {
                DepthFirstSearchHelper(neighbor, visited, result);
            }
        }
    }
}