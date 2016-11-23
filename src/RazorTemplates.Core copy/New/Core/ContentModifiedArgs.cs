﻿#region  Microsoft Public License
/* This code is part of Xipton.Razor v2.5
 * (c) Jaap Lamfers, 2012 - jaap.lamfers@xipton.net
 * Licensed under the Microsoft Public License (MS-PL) http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
 */
#endregion

using System;

namespace Rhythm.Text.Core
{
    public class ContentModifiedArgs : EventArgs
    {
        public ContentModifiedArgs(string modifiedResourceName)
        {
            if (modifiedResourceName == null) throw new ArgumentNullException("modifiedResourceName");
            ModifiedResourceName = modifiedResourceName;
        }

        public string ModifiedResourceName { get; private set; }
    }
}