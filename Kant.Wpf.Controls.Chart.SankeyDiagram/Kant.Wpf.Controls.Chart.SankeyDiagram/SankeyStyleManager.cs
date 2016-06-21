﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kant.Wpf.Controls.Chart
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
            var opacity = 0.55;
            diagram.NodeIntervalSpace = 5;
            diagram.NodeThickness = 10;
            diagram.NodeBrush = new SolidColorBrush(Colors.Black);
            diagram.HighlightOpacity = 1.0;
            diagram.LoweredOpacity = 0.25;
            diagram.LinkPoint1Curveless = 0.4;
            diagram.LinkPoint2Curveless = 0.6;
            var labelStye = new Style(typeof(TextBlock));
            labelStye.Setters.Add(new Setter(TextBlock.MarginProperty, new Thickness(2)));
            diagram.LabelStyle = labelStye;
            diagram.UseNodeLinksPalette = true;
            ResettedHighlightNodeBrushes = new Dictionary<string, Brush>();
            ResettedHighlightLinkBrushes = new List<SankeyLinkStyleFinder>();
            DefaultNodeLinksPalette = GetNodeLinksPalette(opacity);
            DefaultLinkBrush = new SolidColorBrush(Colors.Gray) { Opacity = opacity };
        }

        public void ChangeLabelsVisibility(bool showLabels, List<TextBlock> labels)
        {
            if(labels == null)
            {
                return;
            }

            foreach (var label in labels)
            {
                label.Visibility = showLabels ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void Highlighting(Dictionary<int, List<SankeyLink>> links, bool resetBrushes, double highlightOpacity, double loweredOpacity, List<string> highlightNodes, List<string> minimizeNodes, Func<SankeyLink, bool> check)
        {
            var setStyle = new Action<FrameworkElement, Style>((e, style) =>
            {
                if(style != null)
                {
                    e.Style = style;
                }
            });

            foreach (var levelLinks in links.Values)
            {
                foreach (var link in levelLinks)
                {
                    if (!resetBrushes)
                    {
                        if (check(link))
                        {
                            link.FromNode.Shape.Fill.Opacity = link.ToNode.Shape.Fill.Opacity = link.Shape.Stroke.Opacity = highlightOpacity;
                            link.IsHighlight = true;
                            link.FromNode.IsHighlight = true;
                            link.ToNode.IsHighlight = true;
                            highlightNodes.Add(link.FromNode.Label.Text);
                            highlightNodes.Add(link.ToNode.Label.Text);
                            link.ToNode.Label.Opacity = link.FromNode.Label.Opacity = highlightOpacity;

                            if(diagram.HighlightBrush != null)
                            {
                                link.Shape.Stroke = diagram.HighlightBrush.CloneCurrentValue();
                                link.FromNode.Shape.Fill = diagram.HighlightBrush.CloneCurrentValue();
                                link.ToNode.Shape.Fill = diagram.HighlightBrush.CloneCurrentValue();
                            }

                            if(diagram.HighlightLabelStyle != null)
                            {
                                link.ToNode.Label.Style = link.FromNode.Label.Style = diagram.HighlightLabelStyle;
                            }
                        }
                        else
                        {
                            var minimizeOpacity = link.Shape.Stroke.Opacity - loweredOpacity < 0 ? 0 : link.Shape.Stroke.Opacity - loweredOpacity;
                            link.Shape.Stroke.Opacity = minimizeOpacity;
                            link.IsHighlight = false;

                            // prevent changing node's brush again
                            if (!highlightNodes.Exists(n => n == link.FromNode.Label.Text) && !minimizeNodes.Exists(n => n == link.FromNode.Label.Text))
                            {
                                MinimizeNode(link.FromNode, minimizeOpacity, minimizeNodes);
                            }

                            if (!highlightNodes.Exists(n => n == link.ToNode.Label.Text) && !minimizeNodes.Exists(n => n == link.ToNode.Label.Text))
                            {
                                MinimizeNode(link.ToNode, minimizeOpacity, minimizeNodes);
                            }
                        }
                    }
                    else
                    {
                        ResetHighlights(link);
                    }
                }
            }
        }

        public void ResetHighlights(Dictionary<int, List<SankeyLink>> links, bool resetHighlightStatus = true)
        {
            foreach (var levelLinks in links.Values)
            {
                foreach (var link in levelLinks)
                {
                    ResetHighlights(link, resetHighlightStatus);
                }
            }
        }

        private void ResetHighlights(SankeyLink link, bool resetHighlightStatus = true)
        {
            link.Shape.Stroke = ResettedHighlightLinkBrushes.Find(l => l.From == link.FromNode.Label.Text && l.To == link.ToNode.Label.Text).Brush.CloneCurrentValue();
            link.FromNode.Shape.Fill = ResettedHighlightNodeBrushes[link.FromNode.Label.Text].CloneCurrentValue();
            link.ToNode.Shape.Fill = ResettedHighlightNodeBrushes[link.ToNode.Label.Text].CloneCurrentValue();
            link.ToNode.Label.Style = link.FromNode.Label.Style = diagram.LabelStyle;
            link.ToNode.Label.Opacity = link.FromNode.Label.Opacity = ResettedLabelOpacity;

            if (resetHighlightStatus)
            {
                link.IsHighlight = false;
                link.FromNode.IsHighlight = false;
                link.ToNode.IsHighlight = false;
            }
        }

        private void MinimizeNode(SankeyNode node, double minimizeOpacity, List<string> minimizeNodes)
        {
            node.Shape.Fill.Opacity = minimizeOpacity;
            node.IsHighlight = false;
            minimizeNodes.Add(node.Label.Text);
            node.Label.Opacity = minimizeOpacity;
        }

        private List<Brush> GetNodeLinksPalette(double opacity)
        {
            return new List<Brush>()
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0095fb")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff0000")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffa200")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00f2c8")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7373ff")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#91bc61")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dc89d9")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff100")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#44c5f1")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#85e91f")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00b192")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1cbe65")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#278bcc")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#954ab3")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f3bc00")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e47403")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ce3e29")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d8dddf")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#60e8a4")) { Opacity = opacity },
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffb5ff")) { Opacity = opacity }
            };
        }

        #endregion

        #region Fields & Properties

        public int DefaultNodeLinksPaletteIndex { get; set; }

        public Brush DefaultLinkBrush { get; set; }

        public List<Brush> DefaultNodeLinksPalette { get; set; }

        #region highlight

        public double ResettedLabelOpacity { get; set; }

        public Dictionary<string, Brush> ResettedHighlightNodeBrushes { get; set; }

        public List<SankeyLinkStyleFinder> ResettedHighlightLinkBrushes { get; set; }

        #endregion

        private SankeyDiagram diagram;

        #endregion
    }
}