﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace kor.Wpf.Controls.Chart
{
    public class SankeyNode : Element
    {
        public SankeyNode()
        {
        }

        public SankeyNode(Rectangle shape, TextBlock label)
        {
            Shape = shape;
            Label = label;
            InLinks = new List<SankeyLink>();
            OutLinks = new List<SankeyLink>();
        }

        public List<SankeyLink> InLinks { get; set; }

        public List<SankeyLink> OutLinks { get; set; }

        public Rectangle Shape { get; set; }

        public TextBlock Label { get; set; }

        public double LabelHeight { get; set; }

        public double LabelWidth { get; set; }

        public bool IsLabelSizeMeasured { get; set; }

        public string Name { get; set; }

        public double Y { get; set; }

        public double X { get; set; }
    }
}
