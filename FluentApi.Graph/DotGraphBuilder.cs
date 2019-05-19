using System;
using System.Collections.Generic;
using System.Globalization;

namespace FluentApi.Graph
{
    /// <summary>
    /// Возможные формы вершин
    /// </summary>
    public enum NodeShape
    {
        Box,
        Ellipse
    }

    /// <summary>
    /// Интерфейс для построения графа
    /// </summary>
    public interface IGraphBuilder
    {
        /// <summary>
        /// Добавляет вершину с данным именем
        /// </summary>
        /// <param name="nodeName">
        /// Имя вершины
        /// </param>
        /// <returns>
        /// Строителя графа
        /// </returns>
        GraphNodeBuilder AddNode(string nodeName);

        /// <summary>
        /// Добавляет ребро от начальной вершине к конечной
        /// </summary>
        /// <param name="sourceNode">Начальная вершина</param>
        /// <param name="destinationNode">Конечная вершина</param>
        /// <returns>
        /// Строителя графа
        /// </returns>
        GraphEdgeBuilder AddEdge(string sourceNode, string destinationNode);
        
        /// <summary>
        /// Получает строковое представление графа
        /// </summary>
        /// <returns>
        /// Строковое представление графа
        /// </returns>
        string Build();
    }

    /// <inheritdoc />
    public class DotGraphBuilder : IGraphBuilder
    {
        private readonly Graph graph;

        private DotGraphBuilder(string graphName, bool directed)
        {
            graph = new Graph(graphName, directed, false);
        }

        /// <inheritdoc />
        public GraphNodeBuilder AddNode(string nodeName)
        {
            return new GraphNodeBuilder(graph.AddNode(nodeName), this);
        }

        /// <inheritdoc />
        public GraphEdgeBuilder AddEdge(string sourceNode, string destinationNode)
        {
            var edge = graph.AddEdge(sourceNode, destinationNode);
            return new GraphEdgeBuilder(edge, this);
        }

        /// <inheritdoc />
        public string Build() => graph.ToDotFormat();

        /// <summary>
        /// Создает новый направленный граф с данным именем
        /// </summary>
        /// <param name="graphName">
        /// Имя графа
        /// </param>
        /// <returns>
        /// Строителя направленного графа с данным именем
        /// </returns>
        public static IGraphBuilder DirectedGraph(string graphName)
        {
            return new DotGraphBuilder(graphName, directed: true);
        }

        /// <summary>
        /// Создает новый ненаправленный граф с данным именем
        /// </summary>
        /// <param name="graphName">
        /// Имя графа
        /// </param>
        /// <returns>
        /// Строителя ненаправленного графа с данным именем
        /// </returns>
        public static IGraphBuilder NondirectedGraph(string graphName)
        {
            return new DotGraphBuilder(graphName, directed: false);
        }
    }

    /// <inheritdoc />
    public class GraphBuilder : IGraphBuilder
    {
        protected readonly IGraphBuilder parent;

        public GraphBuilder(IGraphBuilder parent)
        {
            this.parent = parent;
        }

        /// <inheritdoc />
        public GraphNodeBuilder AddNode(string nodeName)
        {
            return parent.AddNode(nodeName);
        }

        /// <inheritdoc />
        public GraphEdgeBuilder AddEdge(string sourceNode, string destinationNode)
        {
            return parent.AddEdge(sourceNode, destinationNode);
        }

        /// <inheritdoc />
        public string Build() => parent.Build();
    }

    /// <inheritdoc />
    public class GraphEdgeBuilder : GraphBuilder
    {
        private readonly GraphEdge edge;

        public GraphEdgeBuilder(GraphEdge edge, IGraphBuilder parent) : base(parent)
        {
            this.edge = edge;
        }

        /// <summary>
        /// Добавляет атрибуты к ребру
        /// </summary>
        /// <param name="applyAttributes">
        /// Действия для добавления атрибутов
        /// </param>
        /// <returns>
        /// Строителя графа с добавленными в ребро атрибутами 
        /// </returns>
        public IGraphBuilder With(Action<EdgeCommonAttributesConfig> applyAttributes)
        {
            applyAttributes(new EdgeCommonAttributesConfig(edge));
            return parent;
        }
    }

    /// <inheritdoc />
    public class GraphNodeBuilder : GraphBuilder
    {
        private readonly GraphNode node;

        public GraphNodeBuilder(GraphNode node, IGraphBuilder parent) : base(parent)
        {
            this.node = node;
        }

        /// <summary>
        /// Добавляет атрибуты к вершине
        /// </summary>
        /// <param name="applyAttributes">
        /// Действия для добавления атрибутов
        /// </param>
        /// <returns>
        /// Строителя графа с добавленными в вершину атрибутами 
        /// </returns>
        public IGraphBuilder With(Action<NodeCommonAttributesConfig> applyAttributes)
        {
            applyAttributes(new NodeCommonAttributesConfig(node));
            return parent;
        }
    }

    /// <summary>
    /// Общие атрибуты вершин и ребер
    /// </summary>
    /// <typeparam name="TConfig">
    /// Данные атрибуты
    /// </typeparam>
    public class CommonAttributesConfig<TConfig>
        where TConfig : CommonAttributesConfig<TConfig>
    {
        private readonly IDictionary<string, string> attributes;

        public CommonAttributesConfig(IDictionary<string, string> attributes)
        {
            this.attributes = attributes;
        }

        /// <summary>
        /// Устанавливает метку объекта (ребра или вершины)
        /// </summary>
        /// <param name="label">
        /// Название метки
        /// </param>
        /// <returns>
        /// Атрибуты объекта после установки метки
        /// </returns>
        public TConfig Label(string label)
        {
            attributes["label"] = label;
            return (TConfig) this;
        }

        /// <summary>
        /// Устанавливает размер шрифта объекта (ребра или вершины)
        /// </summary>
        /// <param name="sizeInPt">
        /// Размер шрифта
        /// </param>
        /// <returns>
        /// Атрибуты объекта после установки размера шрифта
        /// </returns>
        public TConfig FontSize(float sizeInPt)
        {
            attributes["fontsize"] = sizeInPt.ToString(CultureInfo.InvariantCulture);
            return (TConfig) this;
        }

        /// <summary>
        /// Устанавливает цвет объекта (ребра или вершины)
        /// </summary>
        /// <param name="color">
        /// Название цвета
        /// </param>
        /// <returns>
        /// Атрибуты объекта после установки цвета
        /// </returns>
        public TConfig Color(string color)
        {
            attributes["color"] = color;
            return (TConfig) this;
        }
    }

    /// <inheritdoc />
    public class NodeCommonAttributesConfig : CommonAttributesConfig<NodeCommonAttributesConfig>
    {
        private readonly GraphNode node;

        public NodeCommonAttributesConfig(GraphNode node) : base(node.Attributes)
        {
            this.node = node;
        }

        /// <summary>
        /// Добавляет форму к вершине
        /// </summary>
        /// <param name="shape">
        /// Форма для добавления
        /// </param>
        /// <returns>
        /// Атрибуты вершины с новой формой
        /// </returns>
        public NodeCommonAttributesConfig Shape(NodeShape shape)
        {
            node.Attributes["shape"] = shape.ToString().ToLowerInvariant();
            return this;
        }
    }

    /// <inheritdoc />
    public class EdgeCommonAttributesConfig : CommonAttributesConfig<EdgeCommonAttributesConfig>
    {
        private readonly GraphEdge edge;

        public EdgeCommonAttributesConfig(GraphEdge edge) : base(edge.Attributes) => this.edge = edge;

        /// <summary>
        /// Добавляет вес к ребру
        /// </summary>
        /// <param name="weight">
        /// Вес для добавления к ребру
        /// </param>
        /// <returns>
        /// Атрибуты ребра с добавленным весом
        /// </returns>
        public EdgeCommonAttributesConfig Weight(double weight)
        {
            edge.Attributes["weight"] = weight.ToString(CultureInfo.InvariantCulture);
            return this;
        }
    }
}