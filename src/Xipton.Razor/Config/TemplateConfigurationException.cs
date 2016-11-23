﻿#region  Microsoft Public License
/* This code is part of Xipton.Razor v2.5
 * (c) Jaap Lamfers, 2012 - jaap.lamfers@xipton.net
 * Licensed under the Microsoft Public License (MS-PL) http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
 */
#endregion

using System;
using System.Runtime.Serialization;
using Rhythm.Text.Core;

namespace Rhythm.Text.Config
{
    [Serializable]
    public class TemplateConfigurationException : TemplateException
    {

        public TemplateConfigurationException()
        {
        }

        public TemplateConfigurationException(string message) : base(message)
        {
        }

        public TemplateConfigurationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TemplateConfigurationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
