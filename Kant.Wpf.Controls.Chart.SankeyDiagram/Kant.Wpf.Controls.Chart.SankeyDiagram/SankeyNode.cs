﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Kant.Wpf.Controls.Chart
{
    public class SankeyNode
    {
        public SankeyNode()
        {
        }

        public SankeyNode(Rectangle shape, TextBlock label)
        {
            Shape = shape;
            Label = label;
        }

        public Rectangle Shape { get; set; }

        public TextBlock Label { get; set; }

        public double Position { get; set; }

        public double PreviousOccupiedLength { get; set; }

        public double NextOccupiedLength { get; set; }

        public bool IsHighlight { get; set; }
    }
}
