﻿#region  Microsoft Public License
/* This code is part of Xipton.Razor v2.5
 * (c) Jaap Lamfers, 2012 - jaap.lamfers@xipton.net
 * Licensed under the Microsoft Public License (MS-PL) http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
 */
#endregion

using System;

namespace Rhythm.Text.Templating.Extension
{
    public static class TypeExtension
    {
        public static object CreateInstance(this Type type)
        {
            return type == null ? null : Activator.CreateInstance(type, true);
        }

        public static T CreateInstance<T>(this Type type) {
            return type == null ? default(T) : (T)Activator.CreateInstance(type, true);
        }
    }
}