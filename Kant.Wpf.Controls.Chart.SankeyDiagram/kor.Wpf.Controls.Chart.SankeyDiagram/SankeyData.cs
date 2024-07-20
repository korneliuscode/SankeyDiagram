﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace kor.Wpf.Controls.Chart
{
    public class SankeyData
    {
        public SankeyData()
        {
        }

        public SankeyData(string from, string to, double weight, Brush linkBrush = null)
        {
            if(string.IsNullOrEmpty(from))
            {
                throw new ArgumentOutOfRangeException("from node name is null");
            }

            if(string.IsNullOrEmpty(to))
            {
                throw new ArgumentOutOfRangeException("to node name is null");
            }

            From = from;
            To = to;
            Weight = weight;

            if(linkBrush != null)
            {
                LinkBrush = linkBrush;
            }
        }

        public string From { get; set; }

        public string To { get; set; }

        public double Weight { get; set; }

        public Brush LinkBrush { get; set; }
    }
}
