using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarButton
{
	public class CalendarClickEventArgs : EventArgs
	{
		public CalendarClickEventArgs()
		{
		}

		public CalendarClickEventArgs(object dataKey)
		{
			this.DataKey = dataKey;
		}

		public object DataKey { get; set; }
	}
}
