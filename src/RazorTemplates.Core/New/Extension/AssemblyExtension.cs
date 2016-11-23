﻿#region  Microsoft Public License
/* This code is part of Xipton.Razor v2.5
 * (c) Jaap Lamfers, 2012 - jaap.lamfers@xipton.net
 * Licensed under the Microsoft Public License (MS-PL) http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
 */
#endregion

using System;
using System.IO;
using System.Reflection;

namespace Rhythm.Text.Templating.Extension
{
    public static class AssemblyExtension
    {
        public static string GetFileName(this Assembly assembly)
        {
            return assembly == null 
                ? null 
                : assembly.CodeBase.Replace("file:///", string.Empty).Replace("/", "\\");
        }

        //http://msdn.microsoft.com/en-us/library/ms173100.aspx
        public static bool IsManagedAssembly(this string self) {

            if (self == null || !File.Exists(self)) {
                return false;
            }

            try{
                AssemblyName.GetAssemblyName(self);
                return true;
            }
            catch (BadImageFormatException){
                return false;
            }
            catch (FileLoadException){
                return true;
            }
            catch{
                return false;
            }

        }

    }

}
