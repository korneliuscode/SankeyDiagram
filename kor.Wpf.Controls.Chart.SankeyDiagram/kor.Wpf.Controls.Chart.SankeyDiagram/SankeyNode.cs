using System.Collections.Generic;
using System.Windows.Controls;
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

        /// <summary>
        /// the text the label and the tooltip show,
        /// the entry of NodeLabels for the name or the name itself
        /// </summary>
        public string DisplayName { get; set; }

        public double Y { get; set; }

        public double X { get; set; }
    }
}
