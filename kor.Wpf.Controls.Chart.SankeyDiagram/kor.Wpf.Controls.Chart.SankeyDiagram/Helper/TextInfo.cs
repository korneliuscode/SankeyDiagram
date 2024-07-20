﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace kor.Wpf.Controls.Helper
{
	public class TextInfo
	{
		public TextInfo()
		{
		}

		public string Text { get; set; }

		public FlowDirection FlowDirection { get; set; }

		public Typeface Typeface { get; set; }

		public double FontSize { get; set; }

		public Brush Foreground { get; set; }

		public Thickness Margin { get; set; }
	}
}
