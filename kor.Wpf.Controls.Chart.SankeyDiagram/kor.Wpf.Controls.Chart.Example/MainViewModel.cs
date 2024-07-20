﻿//using kor.Wpf.Toolkit.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace kor.Wpf.Controls.Chart.Example
{
    public class MainViewModel : ViewModelBase
    {
        #region Constructor

        public MainViewModel()
        {
            random = new Random();
            sankeyLabelStyle1 = (Style)Application.Current.FindResource("SankeyLabelStyle1");
            sankeyLabelStyle2 = (Style)Application.Current.FindResource("SankeyLabelStyle2");
            sankeyToolTipTemplate1 = (ControlTemplate)Application.Current.FindResource("SankeyToolTipTemplate1");
            sankeyToolTipTemplate2 = (ControlTemplate)Application.Current.FindResource("SankeyToolTipTemplate2");
            SankeyLabelStyle = sankeyLabelStyle2;
            SankeyToolTipTemplate = sankeyToolTipTemplate1;

            //var datas = new List<SankeyData>()
            //{
            //    new SankeyData("Agricultural 'waste'", "Bio-conversion", 124.729),
            //    new SankeyData("Bio-conversion","Liquid", 0.597),
            //    new SankeyData("Bio-conversion", "Losses", 26.862),
            //    new SankeyData("Bio-conversion", "Solid", 280.322),
            //    new SankeyData("Bio-conversion", "Gas", 81.144),
            //    new SankeyData("Biofuel imports", "Liquid", 35),
            //    new SankeyData("Biomass imports", "Solid", 35),
            //    new SankeyData("Coal imports", "Coal", 11.606),
            //    new SankeyData("Coal reserves", "Coal", 63.965),
            //    new SankeyData("Coal", "Solid", 75.571),
            //    new SankeyData("District heating", "Industry", 10.639),
            //    new SankeyData("District heating", "Heating and cooling - commercial", 22.505),
            //    new SankeyData("District heating", "Heating and cooling - homes", 46.184),
            //    new SankeyData("Electricity grid", "Over generation / exports", 104.453),
            //    new SankeyData("Electricity grid", "Heating and cooling - homes", 113.726),
            //    new SankeyData("Electricity grid", "H2 conversion", 27.14),
            //    new SankeyData("Electricity grid", "Industry", 342.165),
            //    new SankeyData("Electricity grid", "Road transport", 37.797),
            //    new SankeyData("Electricity grid", "Agriculture", 4.412),
            //    new SankeyData("Electricity grid", "Heating and cooling - commercial", 40.858),
            //    new SankeyData("Electricity grid", "Losses", 56.691),
            //    new SankeyData("Electricity grid", "Rail transport", 7.863),
            //    new SankeyData("Electricity grid", "Lighting & appliances - commercial", 90.008),
            //    new SankeyData("Electricity grid", "Lighting & appliances - homes", 93.494),
            //    new SankeyData("Gas imports", "Ngas", 40.719),
            //    new SankeyData("Gas reserves", "Ngas", 82.233),
            //    new SankeyData("Gas", "Heating and cooling - commercial", 0.129),
            //    new SankeyData("Gas", "Losses", 1.401),
            //    new SankeyData("Gas", "Thermal generation", 151.891),
            //    new SankeyData("Gas", "Agriculture", 2.096),
            //    new SankeyData("Gas", "Industry", 48.58),
            //    new SankeyData("Geothermal", "Electricity grid", 7.013),
            //    new SankeyData("H2 conversion", "H2", 20.897),
            //    new SankeyData("H2 conversion", "Losses", 6.242),
            //    new SankeyData("H2", "Road transport", 20.897),
            //    new SankeyData("Hydro", "Electricity grid", 6.995),
            //    new SankeyData("Liquid", "Industry", 121.066),
            //    new SankeyData("Liquid", "International shipping", 128.69),
            //    new SankeyData("Liquid", "Road transport", 135.835),
            //    new SankeyData("Liquid", "Domestic aviation", 14.458),
            //    new SankeyData("Liquid", "International aviation", 206.267),
            //    new SankeyData("Liquid", "Agriculture", 3.64),
            //    new SankeyData("Liquid", "National navigation", 33.218),
            //    new SankeyData("Liquid", "Rail transport", 4.413),
            //    new SankeyData("Marine algae", "Bio-conversion", 4.375),
            //    new SankeyData("Ngas", "Gas", 122.952),
            //    new SankeyData("Nuclear", "Thermal generation", 839.978),
            //    new SankeyData("Oil imports", "Oil", 504.287),
            //    new SankeyData("Oil reserves", "Oil", 107.703),
            //    new SankeyData("Oil", "Liquid", 611.99),
            //    new SankeyData("Other waste", "Solid", 56.587),
            //    new SankeyData("Other waste", "Bio-conversion", 77.81),
            //    new SankeyData("Pumped heat",  "Heating and cooling - homes", 193.026),
            //    new SankeyData("Pumped heat", "Heating and cooling - commercial", 70.672),
            //    new SankeyData("Solar PV", "Electricity grid", 59.901),
            //    new SankeyData("Solar Thermal", "Heating and cooling - homes", 19.263),
            //    new SankeyData("Solar", "Solar Thermal", 19.263),
            //    new SankeyData("Solar", "Solar PV", 59.901),
            //    new SankeyData("Solid", "Agriculture", 0.882),
            //    new SankeyData("Solid", "Thermal generation", 400.12),
            //    new SankeyData("Solid", "Industry", 46.477),
            //    new SankeyData("Thermal generation", "Electricity grid", 525.531),
            //    new SankeyData("Thermal generation", "Losses", 787.129),
            //    new SankeyData("Thermal generation", "District heating", 79.329),
            //    new SankeyData("Tidal", "Electricity grid", 9.452),
            //    new SankeyData("UK land based bioenergy", "Bio-conversion", 182.01),
            //    new SankeyData("Wave", "Electricity grid", 19.013),
            //    new SankeyData("Wind", "Electricity grid", 289.366)
            //};

            var datas = new List<SankeyData>()
            {
                new SankeyData("A", "C", 255),
                new SankeyData("A", "D", 355),
                new SankeyData("B", "C", 555),
                new SankeyData("B", "D", 255),
                new SankeyData("B", "E", 1555),
                new SankeyData("C", "H", 155),
                new SankeyData("D", "F", 25),
                new SankeyData("D", "G", 155),
                new SankeyData("D", "H", 15),
                new SankeyData("D", "I", 55),
                new SankeyData("E", "H", 1555),
                new SankeyData("B", "G", 255),
                new SankeyData("A", "E", 95),
                new SankeyData("E", "I", 1555),
                new SankeyData("C", "G", 755),
                new SankeyData("C", "F", 455),
            };

            SankeyDatas = datas;
            SankeyShowLabels = true;
            SankeyLinkCurvature = 0.95;
            //SankeyFlowDirection = FlowDirection.TopToBottom;
        }

        #endregion

        #region Commands

        private ICommand changeDatas;
        public ICommand ChangeDatas
        {
            get
            {
                return GetCommand(changeDatas, new RelayCommand(() =>
                {
                    var datas = new List<SankeyData>();
                    var count = 0;

                    while (count < 100)
                    {
                        datas.Add(new SankeyData(random.Next(9).ToString(), random.Next(10, 19).ToString(), random.Next(55, 155)));
                        count++;
                    }

                    SankeyDatas = datas;
                }));
            }
        }

        private ICommand clearDiagram;
        public ICommand ClearDiagram
        {
            get
            {
                return GetCommand(clearDiagram, new RelayCommand(() =>
                {
                    SankeyDatas = null;
                }));
            }
        }

        private ICommand clearHighlight;
        public ICommand ClearHighlight
        {
            get
            {
                return GetCommand(clearHighlight, new RelayCommand(() =>
                {
                    HighlightSankeyLink = null;
                    HighlightSankeyNode = null;
                }));
            }
        }

        private ICommand highlightingNode;
        public ICommand HighlightingNode
        {
            get
            {
                return GetCommand(highlightingNode, new RelayCommand(() =>
                {
                    //var fromNodes = new List<string>();

                    //foreach(var data in SankeyDatas)
                    //{
                    //    if(!fromNodes.Exists(n => n == data.From))
                    //    {
                    //        fromNodes.Add(data.From);
                    //    }
                    //}

                    //HighlightSankeyNode = fromNodes[random.Next(fromNodes.Count)];
                    //HighlightSankeyNode = random.Next(25).ToString();
                    HighlightSankeyNode = "H";
                    //HighlightSankeyNode = "5";
                    //HighlightSankeyNode = "";
                    //HighlightSankeyNode = "Z";
                }));
            }
        }

        private ICommand highlightingLink;
        public ICommand HighlightingLink
        {
            get
            {
                return GetCommand(highlightingLink, new RelayCommand(() =>
                {
                    HighlightSankeyLink = new SankeyLinkFinder("C", "F");
                    //HighlightSankeyLink = new SankeyLinkFinder("5", random.Next(9, 20).ToString());
                }));
            }
        }

        private ICommand changeStyles;
        public ICommand ChangeStyles
        {
            get
            {
                return GetCommand(changeStyles, new RelayCommand(() =>
                {
                    if (random.Next(2) == 1)
                    {
                        SankeyNodeBrushes = new Dictionary<string, Brush>()
                        {
                            { "A", new SolidColorBrush(Colors.Brown) { Opacity = 0.35 } },
                            { "B", new SolidColorBrush(Colors.Aqua) { Opacity = 0.25 } },
                            { "C", new SolidColorBrush(Colors.CornflowerBlue) { Opacity = 0.15 } },
                            { "D", new SolidColorBrush(Colors.DimGray) { Opacity = 0.45 } },
                            { "E", new SolidColorBrush(Colors.Firebrick) { Opacity = 0.65 } },
                        };
                    }
                    else
                    {
                        SankeyNodeBrushes = null;
                    }

                    SankeyLinkCurvature = random.Next(1, 11) * 0.1;
                    SankeyFlowDirection = random.Next(2) == 1 ? FlowDirection.TopToBottom : FlowDirection.LeftToRight;
                    SankeyShowLabels = random.Next(2) == 1 ? false : true;
                    SankeyLabelStyle = random.Next(2) == 1 ? sankeyLabelStyle1 : sankeyLabelStyle2;
                    SankeyToolTipTemplate = random.Next(2) == 1 ? sankeyToolTipTemplate1 : sankeyToolTipTemplate2;
                    SankeyHighlightMode = random.Next(2) == 1 ? HighlightMode.MouseEnter : HighlightMode.MouseLeftButtonUp;
                }));
            }
        }

        #endregion

        #region Fields & Properties

        private List<SankeyData> sankeyDatas;
        public IReadOnlyList<SankeyData> SankeyDatas
        {
            get
            {
                return sankeyDatas;
            }
            private set
            {
                if (value != sankeyDatas)
                {
                    sankeyDatas = value == null ? null : value.ToList();
                    RaisePropertyChanged(() => SankeyDatas);
                }
            }
        }

        private double sankeyLinkCurvature;
        public double SankeyLinkCurvature
        {
            get
            {
                return sankeyLinkCurvature;
            }
            set
            {
                if (value != sankeyLinkCurvature)
                {
                    sankeyLinkCurvature = value;
                    RaisePropertyChanged(() => SankeyLinkCurvature);
                }
            }
        }

        private Dictionary<string, Brush> sankeyNodeBrushes;
        public IReadOnlyDictionary<string, Brush> SankeyNodeBrushes
        {
            get
            {
                return sankeyNodeBrushes;
            }
            private set
            {
                if(value != sankeyNodeBrushes)
                {
                    sankeyNodeBrushes = value == null ? null : value.ToDictionary(item => item.Key, item => item.Value);
                    RaisePropertyChanged(() => SankeyNodeBrushes);
                }
            }
        }

        private string highlightSankeyNode;
        public string HighlightSankeyNode
        {
            get
            {
                return highlightSankeyNode;
            }
            set
            {
                highlightSankeyNode = value;
                RaisePropertyChanged(() => HighlightSankeyNode);
            }
        }

        private SankeyLinkFinder highlightSankeyLink;
        public SankeyLinkFinder HighlightSankeyLink
        {
            get
            {
                return highlightSankeyLink;
            }
            set
            {
                highlightSankeyLink = value;
                RaisePropertyChanged(() => HighlightSankeyLink);
            }
        }

        private FlowDirection sankeyFlowDirection;
        public FlowDirection SankeyFlowDirection
        {
            get
            {
                return sankeyFlowDirection;
            }
            set
            {
                if(value != sankeyFlowDirection)
                {
                    sankeyFlowDirection = value;
                    RaisePropertyChanged(() => SankeyFlowDirection);
                }
            }
        }

        private bool sankeyShowLabels;
        public bool SankeyShowLabels
        {
            get
            {
                return sankeyShowLabels;
            }
            set
            {
                if (value != sankeyShowLabels)
                {
                    sankeyShowLabels = value;
                    RaisePropertyChanged(() => SankeyShowLabels);
                }
            }
        }

        private HighlightMode sankeyHighlightMode;
        public HighlightMode SankeyHighlightMode
        {
            get
            {
                return sankeyHighlightMode;
            }
            set
            {
                if (value != sankeyHighlightMode)
                {
                    sankeyHighlightMode = value;
                    RaisePropertyChanged(() => SankeyHighlightMode);
                }
            }
        }

        private Style sankeyLabelStyle;
        public Style SankeyLabelStyle
        {
            get
            {
                return sankeyLabelStyle;
            }
            set
            {
                if (value != sankeyLabelStyle)
                {
                    sankeyLabelStyle = value;
                    RaisePropertyChanged(() => SankeyLabelStyle);
                }
            }
        }

        private ControlTemplate sankeyToolTipTemplate;
        public ControlTemplate SankeyToolTipTemplate
        {
            get
            {
                return sankeyToolTipTemplate;
            }
            set
            {
                if (value != sankeyToolTipTemplate)
                {
                    sankeyToolTipTemplate = value;
                    RaisePropertyChanged(() => SankeyToolTipTemplate);
                }
            }
        }

        private Style sankeyLabelStyle1;

        private Style sankeyLabelStyle2;

        private ControlTemplate sankeyToolTipTemplate1;

        private ControlTemplate sankeyToolTipTemplate2;

        private Random random;

        #endregion
    }
	public abstract class ViewModelBase : ObservableObject
	{
		protected ICommand GetCommand(ICommand c, RelayCommand command)
		{
			if (c == null)
			{
				c = command;
			}

			return c;
		}

		protected ICommand GetCommand<T>(ICommand c, RelayCommand<T> command)
		{
			if (c == null)
			{
				c = command;
			}

			return c;
		}
	}
	public class RelayCommand : ICommand
	{
		#region Constructors

		public RelayCommand()
		{
		}

		/// <summary>
		/// Creates a new command that can always execute.
		/// </summary>
		/// <param name="execute">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
		/// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
		public RelayCommand(Action execute) : this(execute, null)
		{
		}

		/// <summary>
		/// Creates a new command.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand(Action execute, Func<bool> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}

			this.execute = execute;
			this.canExecute = canExecute;
		}

		#endregion

		#region ICommand 

		public virtual bool CanExecute(object parameter)
		{
			return canExecute == null ? true : canExecute();
		}

		public virtual void Execute(object parameter)
		{
			execute();
		}

		///<summary>
		///Occurs when changes occur that affect whether or not the command should execute.
		///</summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (canExecute != null)
				{
					CommandManager.RequerySuggested += value;
				}
			}
			remove
			{
				if (canExecute != null)
				{
					CommandManager.RequerySuggested -= value;
				}
			}
		}

		#endregion

		#region Fields

		private readonly Action execute = null;

		private readonly Func<bool> canExecute = null;

		#endregion
	}
	public class RelayCommand<T> : RelayCommand
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
		/// </summary>
		/// <param name="execute">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
		/// <remarks><seealso cref="CanExecute"/> will always return true.</remarks>
		public RelayCommand(Action<T> execute) : this(execute, null)
		{
		}

		/// <summary>
		/// Creates a new command.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}

			this.execute = execute;
			this.canExecute = canExecute;
		}

		#endregion

		#region ICommand 

		///<summary>
		///Defines the method that determines whether the command can execute in its current state.
		///</summary>
		///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		///<returns>
		///true if this command can be executed; otherwise, false.
		///</returns>
		public override bool CanExecute(object parameter)
		{
			return canExecute == null ? true : canExecute((T)parameter);
		}

		///<summary>
		///Defines the method to be called when the command is invoked.
		///</summary>
		///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
		public override void Execute(object parameter)
		{
			execute((T)parameter);
		}

		#endregion

		#region Fields

		private readonly Action<T> execute = null;

		private readonly Predicate<T> canExecute = null;

		#endregion
	}
	public abstract class ObservableObject : INotifyPropertyChanged
	{
		protected void RaisePropertyChanged<T>(Expression<Func<T>> action)
		{
			var propertyName = GetPropertyName(action);
			RaisePropertyChanged(propertyName);
		}

		private static string GetPropertyName<T>(Expression<Func<T>> action)
		{
			var expression = (MemberExpression)action.Body;
			var propertyName = expression.Member.Name;
			return propertyName;
		}

		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
