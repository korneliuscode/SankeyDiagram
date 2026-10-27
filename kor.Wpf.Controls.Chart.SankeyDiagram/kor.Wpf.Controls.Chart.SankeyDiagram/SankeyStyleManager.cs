using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace kor.Wpf.Controls.Chart
{
	public class SankeyStyleManager
    {
        #region Constructor

        public SankeyStyleManager(SankeyDiagram diagram)
        {
            this.diagram = diagram;
            SetDefaultStyles();
        }

        #endregion

        #region Methods

        public void SetDefaultStyles()
        {
            var opacity = diagram.PaletteOpacity;
            diagram.NodeGap = 5;
            diagram.NodeThickness = 10;
            diagram.HighlightOpacity = 1.0;
            diagram.LoweredOpacity = 0.25;
            diagram.UsePallette = SankeyPalette.NodesLinks;
            defaultNodeLinksPalette = GetNodeLinksPalette(opacity);
            DefaultLinkBrush = new SolidColorBrush(Colors.Gray) { Opacity = opacity };
        }

        public void UpdateLinkCurvature(double curvature, IReadOnlyList<SankeyLink> links)
        {
            if(!(curvature > 0 && curvature <= 1) || links == null || links.Count == 0)
            {
                return;
            }

            foreach(var link in links)
            {
                if(link.Shape.Data == null)
                {
                    continue;
                }

                var figure = ((PathGeometry)(link.Shape.Data)).Figures[0];
                var startPoint = figure.StartPoint;
                var line1EndPoint = ((LineSegment)figure.Segments[0]).Point;
                var line2EndPoint = ((LineSegment)figure.Segments[2]).Point;
                var bezier1 = (BezierSegment)figure.Segments[1];
                var bezier2 = (BezierSegment)figure.Segments[3];
                var bezier1ControlPoint1 = new Point(); 
                var bezier1ControlPoint2 = new Point();
                var bezier2ControlPoint1 = new Point();
                var bezier2ControlPoint2 = new Point();

                if (diagram.SankeyFlowDirection == FlowDirection.TopToBottom)
                {
                    var length = line2EndPoint.Y - line1EndPoint.Y;
                    bezier2ControlPoint2.X = startPoint.X;
                    bezier1ControlPoint1.X = line1EndPoint.X;
                    bezier2ControlPoint1.X = line2EndPoint.X;
                    bezier1ControlPoint2.X = line2EndPoint.X + link.Width;
                    bezier2ControlPoint2.Y = bezier1ControlPoint1.Y = curvature * length + startPoint.Y;
                    bezier2ControlPoint1.Y = bezier1ControlPoint2.Y = (1 - curvature) * length + startPoint.Y;
                }
                else
                {
                    var length = line2EndPoint.X - line1EndPoint.X;
                    bezier2ControlPoint2.Y = startPoint.Y;
                    bezier1ControlPoint1.Y = line1EndPoint.Y;
                    bezier2ControlPoint1.Y = line2EndPoint.Y;
                    bezier1ControlPoint2.Y = line2EndPoint.Y + link.Width;
                    bezier2ControlPoint2.X = bezier1ControlPoint1.X = curvature * length + startPoint.X;
                    bezier2ControlPoint1.X = bezier1ControlPoint2.X = (1 - curvature) * length + startPoint.X;
                }

                bezier1.Point1 = bezier1ControlPoint1;
                bezier1.Point2 = bezier1ControlPoint2;
                bezier2.Point1 = bezier2ControlPoint1;
                bezier2.Point2 = bezier2ControlPoint2;
            }
        }

        public void SetNodeBrush(SankeyNode node)
        {
            var brushCheck = diagram.NodeBrushes != null && diagram.NodeBrushes.Keys.Contains(node.Name);

            if (brushCheck)
            {
                node.Shape.Fill = diagram.NodeBrushes[node.Name].CloneCurrentValue();
            }
            else
            {
                if (diagram.UsePallette != SankeyPalette.None)
                {
                    node.Shape.Fill = defaultNodeLinksPalette[DefaultNodeLinksPaletteIndex].CloneCurrentValue();
                    DefaultNodeLinksPaletteIndex++;

                    if (DefaultNodeLinksPaletteIndex >= defaultNodeLinksPalette.Count)
                    {
                        DefaultNodeLinksPaletteIndex = 0;
                    }
                }
            }

            node.OriginalBrush = node.Shape.Fill.CloneCurrentValue();
        }

        public Brush GetLinkBrush(SankeyNode toNode)
        {
            var nodeBrush = toNode.OriginalBrush ?? toNode.Shape.Fill;
            var brush = nodeBrush is SolidColorBrush solid ? new SolidColorBrush(solid.Color) : nodeBrush.CloneCurrentValue();
            brush.Opacity = diagram.LinkOpacity;

            return brush;
        }

        public void UpdateNodeBrushes(Brush brush, IReadOnlyDictionary<int, IReadOnlyList<SankeyNode>> nodes)
        {
            if(diagram == null || brush == null || nodes == null || nodes.Count < 2)
            {
                return;
            }

            if(diagram.UsePallette == SankeyPalette.NodesLinks)
            {
                return;
            }

            ClearHighlight();

            foreach(var levelNodes in nodes.Values)
            {
                foreach(var node in levelNodes)
                {
					node.Shape.Fill = brush.CloneCurrentValue();
                }
            }
        }
       
        public void ChangeToolTipTemplate(ControlTemplate newTemplate, IReadOnlyList<SankeyLink> links)
        {
            if(links == null || newTemplate == null)
            {
                return;
            }

            foreach(var link in links)
            {
                var toolTip = link.Shape.ToolTip as ToolTip;

                if(toolTip != null)
                {
                    toolTip.Template = newTemplate;
                }
            }
        }

        public void ClearHighlight()
        {
            diagram.SetCurrentValue(SankeyDiagram.HighlightNodeProperty, null);
            diagram.SetCurrentValue(SankeyDiagram.HighlightLinkProperty, null);
        }

        public void ChangeLabelsVisibility(bool showLabels, IReadOnlyList<TextBlock> labels)
        {
            if(labels == null)
            {
                return;
            }

            foreach (var label in labels)
            {
                label.Visibility = showLabels ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public void HighlightingNode(string highlightNode, IReadOnlyDictionary<int, IReadOnlyList<SankeyNode>> nodes, IReadOnlyList<SankeyLink> links)
        {
            if (string.IsNullOrEmpty(highlightNode) && string.IsNullOrEmpty(diagram.HighlightNode) || nodes == null || nodes.Count < 2 || links == null || links.Count == 0)
            {
                return;
            }

            // reset each element's brush first
            RecoverHighlights(links, false);

            var resetBrushes = true;
            var highlightNodes = new List<string>();
            var minimizeNodes = new List<string>();

            // check whether highlight node exists
            if (!string.IsNullOrEmpty(highlightNode))
            {
                if ((from node in nodes.Values.SelectMany(n => n) where node.Name == highlightNode select node).Count() == 1)
                {
                    resetBrushes = false;
                };
            }

            // reset highlight if highlighting the same node twice
            if (highlightNode == diagram.HighlightNode && (from node in nodes.Values.SelectMany(n => n) where node.Name == diagram.HighlightNode & node.IsHighlight select node).Count() == 1)
            {
                RecoverHighlights(links);
                ClearHighlight();

                return;
            }

            if(string.IsNullOrEmpty(highlightNode))
            {
                return;
            }

            // for node, link highlighting switching
            diagram.SetCurrentValue(SankeyDiagram.HighlightLinkProperty, null);

            // increasing opacity of the correlated element while lower the others  
            Highlighting(links, resetBrushes, diagram.HighlightOpacity, diagram.LoweredOpacity, highlightNodes, minimizeNodes, new Func<SankeyLink, bool>(link => { return link.FromNode.Name == highlightNode || link.ToNode.Name == highlightNode; }));
        }

        public void HighlightingLink(SankeyLinkFinder linkFinder, IReadOnlyDictionary<int, IReadOnlyList<SankeyNode>> nodes, IReadOnlyList<SankeyLink> links)
        {
            if (linkFinder == null && diagram.HighlightLink == null || nodes == null || nodes.Count < 2 || links == null || links.Count == 0)
            {
                return;
            }

            // reset each element's brush first
            RecoverHighlights(links, false);

            var resetBrushes = true;
            var highlightNodes = new List<string>();
            var minimizeNodes = new List<string>();

            // check whether highlight node exist
            if (linkFinder != null)
            {
                var fromNode = (from node in nodes.Values.SelectMany(n => n) where node.Name == linkFinder.From select node).FirstOrDefault();
                var toNode = (from node in nodes.Values.SelectMany(n => n) where node.Name == linkFinder.To select node).FirstOrDefault();

                if (fromNode != null && toNode != null)
                {
                    if ((from link in links where link.FromNode.Name == linkFinder.From & link.ToNode.Name == linkFinder.To select link).Count() > 0)
                    {
                        resetBrushes = false;
                    }
                };
            }

            // reset highlight if highlighting the same link twice
            if (diagram.HighlightLink != null && linkFinder != null)
            {
                if ((diagram.HighlightLink.From == linkFinder.From && diagram.HighlightLink.To == linkFinder.To) && (from link in links where (link.FromNode.Name == diagram.HighlightLink.From && link.ToNode.Name == diagram.HighlightLink.To) & link.IsHighlight select link).Count() == 1)
                {
                    RecoverHighlights(links);
                    ClearHighlight();

                    return;
                }
            }

            if(linkFinder == null)
            {
                return;
            }

            // for node, link highlighting switching
            diagram.SetCurrentValue(SankeyDiagram.HighlightNodeProperty, null);

            // increasing opacity of the correlated element while lower the others  
            Highlighting(links, resetBrushes, diagram.HighlightOpacity, diagram.LoweredOpacity, highlightNodes, minimizeNodes, new Func<SankeyLink, bool>(link => { return link.FromNode.Name == linkFinder.From && link.ToNode.Name == linkFinder.To; }));
        }

        private void Highlighting(IReadOnlyList<SankeyLink> links, bool resetBrushes, double highlightOpacity, double loweredOpacity, List<string> highlightNodes, List<string> minimizeNodes, Func<SankeyLink, bool> isHighlightedElement)
        {
            var setStyle = new Action<FrameworkElement, Style>((e, style) =>
            {
                if (style != null)
                {
                    e.Style = style;
                }
            });

            foreach (var link in links)
            {
                if (!resetBrushes)
                {
                    if (isHighlightedElement(link))
                    {
                        link.FromNode.Shape.Fill.Opacity = link.ToNode.Shape.Fill.Opacity = link.Shape.Fill.Opacity = highlightOpacity;
                        link.IsHighlight = true;
                        link.FromNode.IsHighlight = true;
                        link.ToNode.IsHighlight = true;
                        highlightNodes.Add(link.FromNode.Name);
                        highlightNodes.Add(link.ToNode.Name);
                        link.ToNode.Label.Opacity = link.FromNode.Label.Opacity = highlightOpacity;

                        if (diagram.HighlightBrush != null)
                        {
                            link.Shape.Fill = diagram.HighlightBrush.CloneCurrentValue();
                            link.FromNode.Shape.Fill = diagram.HighlightBrush.CloneCurrentValue();
                            link.ToNode.Shape.Fill = diagram.HighlightBrush.CloneCurrentValue();
                        }

                        if (diagram.HighlightLabelStyle != null)
                        {
                            link.ToNode.Label.Style = link.FromNode.Label.Style = diagram.HighlightLabelStyle;
                        }
                    }
                    else
                    {
                        var minimizeOpacity = link.Shape.Fill.Opacity - loweredOpacity < 0 ? 0 : link.Shape.Fill.Opacity - loweredOpacity;
                        link.Shape.Fill.Opacity = minimizeOpacity;
                        link.IsHighlight = false;

                        // prevent changing node's brush again
                        if (!highlightNodes.Exists(n => n == link.FromNode.Name) && !minimizeNodes.Exists(n => n == link.FromNode.Name))
                        {
                            MinimizeNode(link.FromNode, loweredOpacity, minimizeNodes);
                        }

                        if (!highlightNodes.Exists(n => n == link.ToNode.Name) && !minimizeNodes.Exists(n => n == link.ToNode.Name))
                        {
                            MinimizeNode(link.ToNode, loweredOpacity, minimizeNodes);
                        }
                    }
                }
                else
                {
                    RecoverHighlights(link);
                }
            }
        }

        private void RecoverHighlights(IReadOnlyList<SankeyLink> links, bool resetHighlightStatus = true)
        {
            foreach (var link in links)
            {
                RecoverHighlights(link, resetHighlightStatus);
            }
        }

        private void RecoverHighlights(SankeyLink link, bool resetHighlightStatus = true)
        {
            link.Shape.Fill = link.OriginalBrush.CloneCurrentValue();
            link.FromNode.Shape.Fill = link.FromNode.OriginalBrush.CloneCurrentValue();
            link.ToNode.Shape.Fill = link.ToNode.OriginalBrush.CloneCurrentValue();
            link.ToNode.Label.Style = link.FromNode.Label.Style = diagram.LabelStyle;
            link.ToNode.Label.Opacity = link.FromNode.Label.Opacity = OriginalLabelOpacity;

            if (resetHighlightStatus)
            {
                link.IsHighlight = false;
                link.FromNode.IsHighlight = false;
                link.ToNode.IsHighlight = false;
            }
        }

        // nodes and labels are lowered from their own opacity, so they stay readable while translucent links fade out
        private void MinimizeNode(SankeyNode node, double loweredOpacity, List<string> minimizeNodes)
        {
            node.Shape.Fill.Opacity = Math.Max(0, node.OriginalBrush.Opacity - loweredOpacity);
            node.IsHighlight = false;
            minimizeNodes.Add(node.Name);
            node.Label.Opacity = Math.Max(0, OriginalLabelOpacity - loweredOpacity);
        }

        private List<Brush> GetNodeLinksPalette(double opacity)
        {
            return PaletteColors.Select(color => (Brush)new SolidColorBrush(color) { Opacity = opacity }).ToList();
        }

        /// <summary>
        /// colours the palette hands out to nodes in order, also for callers that colour nodes through NodeBrushes
        /// </summary>
        public static readonly IReadOnlyList<Color> PaletteColors = new List<Color>()
        {
            (Color)ColorConverter.ConvertFromString("#4A6F9E"),
            (Color)ColorConverter.ConvertFromString("#8E3F66"),
            (Color)ColorConverter.ConvertFromString("#556055"),
            (Color)ColorConverter.ConvertFromString("#B0553A"),
            (Color)ColorConverter.ConvertFromString("#3D8880"),
            (Color)ColorConverter.ConvertFromString("#B03F42"),
            (Color)ColorConverter.ConvertFromString("#326E52"),
            (Color)ColorConverter.ConvertFromString("#B87040"),
            (Color)ColorConverter.ConvertFromString("#7A7A38"),
            (Color)ColorConverter.ConvertFromString("#C06030"),
            (Color)ColorConverter.ConvertFromString("#4A709E"),
            (Color)ColorConverter.ConvertFromString("#8E4065"),
            (Color)ColorConverter.ConvertFromString("#556253"),
            (Color)ColorConverter.ConvertFromString("#3E8882"),
            (Color)ColorConverter.ConvertFromString("#AF4045"),
            (Color)ColorConverter.ConvertFromString("#607A96"),
            (Color)ColorConverter.ConvertFromString("#3A756C"),
            (Color)ColorConverter.ConvertFromString("#7A9150"),
            (Color)ColorConverter.ConvertFromString("#8A4547"),
            (Color)ColorConverter.ConvertFromString("#326E96"),
            (Color)ColorConverter.ConvertFromString("#A08A30"),
            (Color)ColorConverter.ConvertFromString("#384F7C"),
            (Color)ColorConverter.ConvertFromString("#A83E38"),
            (Color)ColorConverter.ConvertFromString("#608048"),
            (Color)ColorConverter.ConvertFromString("#5B82B8"),
            (Color)ColorConverter.ConvertFromString("#7A6B9E"),
            (Color)ColorConverter.ConvertFromString("#4E8B7A"),
            (Color)ColorConverter.ConvertFromString("#B87A4A"),
            (Color)ColorConverter.ConvertFromString("#6B8AAD"),
            (Color)ColorConverter.ConvertFromString("#9B6B6B"),
            (Color)ColorConverter.ConvertFromString("#7A9B6B"),
            (Color)ColorConverter.ConvertFromString("#AD7A4A"),
            (Color)ColorConverter.ConvertFromString("#6B7A9B"),
            (Color)ColorConverter.ConvertFromString("#9B7A6B"),
            (Color)ColorConverter.ConvertFromString("#6B9B7A"),
            (Color)ColorConverter.ConvertFromString("#8C6B9B"),
            (Color)ColorConverter.ConvertFromString("#9B8C6B"),
            (Color)ColorConverter.ConvertFromString("#6B8C9B"),
            (Color)ColorConverter.ConvertFromString("#9B6B8C"),
            (Color)ColorConverter.ConvertFromString("#7A9B8C"),
            (Color)ColorConverter.ConvertFromString("#8C7A6B"),
            (Color)ColorConverter.ConvertFromString("#6B9B8C"),
            (Color)ColorConverter.ConvertFromString("#9B8C7A"),
            (Color)ColorConverter.ConvertFromString("#7A8C9B"),
            (Color)ColorConverter.ConvertFromString("#8C9B7A")
        };

        #endregion

        #region Fields & Properties

        public int DefaultNodeLinksPaletteIndex { get; set; }

        public Brush DefaultLinkBrush { get; set; }

        public double OriginalLabelOpacity { get; set; }

        private SankeyDiagram diagram;

        private List<Brush> defaultNodeLinksPalette;

        #endregion
    }
}
