﻿#region  Microsoft Public License
/* This code is part of Xipton.Razor v2.5
 * (c) Jaap Lamfers, 2012 - jaap.lamfers@xipton.net
 * Licensed under the Microsoft Public License (MS-PL) http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
 */
#endregion
using System;
using System.Globalization;
using System.IO;

namespace Rhythm.Text.Templating.Core {

    public class HelperResult : ILiteralString {
        private readonly Action<TextWriter> _action;

        public HelperResult(Action<TextWriter> action) {
            if (action == null) throw new ArgumentNullException("action");
            _action = action;
        }

        public override string ToString() {
            using (var writer = new StringWriter(CultureInfo.InvariantCulture)) {
                _action(writer);
                return writer.ToString();
            }
        }


    }
}
