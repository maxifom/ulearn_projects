using System.Collections.Generic;

namespace FluentApi.Graph
{
	public class Graph
	{
		private readonly List<GraphEdge> edges = new List<GraphEdge>();
		private readonly Dictionary<string, GraphNode> nodes = new Dictionary<string, GraphNode>();

		public Graph(string graphName, bool directed, bool strict)
		{
			GraphName = graphName;
			Directed = directed;
			Strict = strict;
		}

		public string GraphName { get; }
		public bool Directed { get; }
		public bool Strict { get; }

		public IEnumerable<GraphEdge> Edges => edges;
		public IEnumerable<GraphNode> Nodes => nodes.Values;

		public GraphNode AddNode(string name)
		{
			GraphNode result;
			if (!nodes.TryGetValue(name, out result))
				nodes.Add(name, result = new GraphNode(name));
			return result;
		}

		public GraphEdge AddEdge(string sourceNode, string destinationNode)
		{
			var result = new GraphEdge(sourceNode, destinationNode, Directed);
			edges.Add(result);
			return result;
		}
	}
}