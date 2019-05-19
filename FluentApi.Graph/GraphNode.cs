using System.Collections.Generic;

namespace FluentApi.Graph
{
	public class GraphNode
	{
		public readonly Dictionary<string, string> Attributes = new Dictionary<string, string>();
		public string Name { get; }

		public GraphNode(string name)
		{
			Name = name;
		}
	}
}