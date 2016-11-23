﻿#region  Microsoft Public License
/* This code is part of Xipton.Razor v2.5
 * (c) Jaap Lamfers, 2012 - jaap.lamfers@xipton.net
 * Licensed under the Microsoft Public License (MS-PL) http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
 */
#endregion

using System.Web.Razor;
using System.Web.Razor.Parser;

namespace Rhythm.Text.Core.Generator.VB
{
    public class XiptonVBCodeLanguage : VBRazorCodeLanguage, IXiptonCodeLanguage
    {
        public override ParserBase CreateCodeParser()
        {
            return new XiptonVBCodeParser();
        }
    }
}