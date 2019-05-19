using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FluentApi.Graph
{
	///<summary>See format description here: http://www.graphviz.org/content/dot-language</summary>
	public class DotFormatWriter
	{
		private readonly TextWriter writer;

		public DotFormatWriter(TextWriter writer)
		{
			this.writer = writer;
		}

		public void Write(Graph graph)
		{
			writer.Write(
				"{0}graph {1} {{\n",
				graph.Directed ? "di" : "",
				EscapeId(graph.GraphName)
			);
			var isFirst = true;
			foreach (dynamic item in graph.Nodes.Cast<object>().Concat(graph.Edges))
			{
				if (!isFirst) writer.Write(";\n");
				isFirst = false;
				Write(item);
			}
			if (!isFirst) writer.Write("\n");
			writer.Write("}");
		}

		public void Write(GraphNode node)
		{
			writer.Write("    " + EscapeId(node.Name));
			WriteAttributes(node.Attributes);
		}

		public void Write(GraphEdge edge)
		{
			var edgeSign = edge.Directed ? "->" : "--";
			writer.Write("    {0} {1} {2}", EscapeId(edge.SourceNode), edgeSign, EscapeId(edge.DestinationNode));
			WriteAttributes(edge.Attributes);
		}

		public void WriteAttributes(IReadOnlyDictionary<string, string> attributes)
		{
			if (attributes.Count == 0) return;
			var attributesStr = attributes.OrderBy(a => a.Key).Select(a => EscapeId(a.Key) + "=" + EscapeId(a.Value));
			var attrs = string.Join("; ", attributesStr);
			writer.Write(" [{0}]", attrs);
		}

		public static string EscapeId(string id)
		{
			if (Regex.IsMatch(id, @"^[a-zA-Z_][a-zA-Z_0-9]*$") ||
				Regex.IsMatch(id, @"^[-]?(\.[0-9]+|[0-9]+(\.[0-9]*)?)$"))
				return id;
			return string.Format("\"{0}\"", id.Replace("\"", "\\\""));
		}
	}

	public static class DotFormatExtensions
	{
		public static string ToDotFormat(this Graph graph)
		{
			using (var s = new StringWriter())
			{
				new DotFormatWriter(s).Write(graph);
				s.Flush();
				return s.ToString();
			}
		}
	}
}
