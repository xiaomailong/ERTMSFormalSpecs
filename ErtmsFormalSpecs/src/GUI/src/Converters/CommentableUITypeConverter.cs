﻿// ------------------------------------------------------------------------------
// -- Copyright ERTMS Solutions
// -- Licensed under the EUPL V.1.1
// -- http://joinup.ec.europa.eu/software/page/eupl/licence-eupl
// --
// -- This file is part of ERTMSFormalSpec software and documentation
// --
// --  ERTMSFormalSpec is free software: you can redistribute it and/or modify
// --  it under the terms of the EUPL General Public License, v.1.1
// --
// -- ERTMSFormalSpec is distributed in the hope that it will be useful,
// -- but WITHOUT ANY WARRANTY; without even the implied warranty of
// -- MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// --
// ------------------------------------------------------------------------------
namespace GUI.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Globalization;
    using DataDictionary;

    /// <summary>
    /// Converts IExpressionable to string, by getting the Expression property
    /// </summary>
    public class CommentableUITypeConverter : StringConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            string retVal = "<unknown>";
            ICommentable commentable = value as ICommentable;
            if (commentable != null)
            {
                retVal = commentable.Comment;
                if (retVal != null)
                {
                    int index = retVal.IndexOf("\n");
                    if (index > 0)
                    {
                        retVal = retVal.Substring(0, index) + "...";
                    }
                }
                else
                {
                    retVal = "";
                }
            }

            return retVal;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }
    }
}
