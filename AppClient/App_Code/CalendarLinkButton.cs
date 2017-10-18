using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CalendarButton
{
	[
	DefaultEvent("CalendarClick"), 
	ToolboxData("<{0}:CalendarLinkButton runat=\"server\">CalendarLinkButton</{0}:CalendarLinkButton>")
	]
	public class CalendarLinkButton : LinkButton
	{
		protected override void RaisePostBackEvent(string eventArgument)
		{
			base.RaisePostBackEvent(eventArgument);

			if (!String.IsNullOrEmpty(eventArgument))
			{
				this.OnCalendarClick(new CalendarClickEventArgs(eventArgument));
			}
		}

		protected virtual void OnCalendarClick(CalendarClickEventArgs e)
		{
			CalendarClickEventHandler handler = 
				(CalendarClickEventHandler)base.Events[EventCalendarClick];
			if (handler != null)
			{
				handler(this, e);
			}
		}

		public event CalendarClickEventHandler CalendarClick
		{
			add
			{
				base.Events.AddHandler(EventCalendarClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventCalendarClick, value);
			}
		}

		private static readonly object EventCalendarClick;
	}
}
