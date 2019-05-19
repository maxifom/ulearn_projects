using System.Collections.Generic;

namespace FluentApi.Graph
{
	public class GraphEdge
	{
		public readonly Dictionary<string, string> Attributes = new Dictionary<string, string>();

		public GraphEdge(string sourceNode, string destinationNode, bool directed)
		{
			SourceNode = sourceNode;
			DestinationNode = destinationNode;
			Directed = directed;
		}

		public string DestinationNode { get; }
		public string SourceNode { get; }
		public bool Directed { get; }
	}
}