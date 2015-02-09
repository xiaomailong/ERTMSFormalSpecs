using System;
using System.Collections.Generic;

namespace XmlBooster
{
    public class XmlBErrorList : List<XmlBValidationError>
    {
        public void add(IXmlBBase el, Type elT, string field, EXmlBValidationErrorCause cause)
        {
            Add(new XmlBValidationError(el, elT, field, cause));
        }
    }
}