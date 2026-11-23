using System.Windows;

namespace kor.Wpf.Controls.Chart
{
    public delegate void SankeyNodeClickEventHandler(object sender, SankeyNodeClickEventArgs e);

    /// <summary>
    /// the node a click hit, by its name as the datas give it
    /// </summary>
    public class SankeyNodeClickEventArgs : RoutedEventArgs
    {
        public SankeyNodeClickEventArgs(RoutedEvent routedEvent, object source, string name) : base(routedEvent, source)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
