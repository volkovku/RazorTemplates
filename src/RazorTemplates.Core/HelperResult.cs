using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace Rhythm.Text
{

	public class HelperResult 
	{
		private readonly Action<TextWriter> _action;

		public HelperResult(Action<TextWriter> action)
		{
			if (action == null) throw new ArgumentNullException("action");
			_action = action;
		}

		public  string GetString()
		{
			using (var writer = new StringWriter(CultureInfo.InvariantCulture))
			{
				_action(writer);
				return writer.ToString();
			}
		}

		public override string ToString()
		{
			return GetString();
		}

		//public   IHtmlString ToString()
		//{
		//	return GetString();
		//}

	}
}
