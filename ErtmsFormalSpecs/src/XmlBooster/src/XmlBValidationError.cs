using System;

namespace XmlBooster
{
    public class XmlBValidationError
    {
        public XmlBValidationError(IXmlBBase el, Type elT, string field, EXmlBValidationErrorCause cause)
        {
            Element = el;
            ElementType = elT;
            FieldName = field;
            Cause = cause;
        }

        public IXmlBBase Element { get; private set; }
        public Type ElementType { get; private set; }
        public string FieldName { get; private set; }
        public EXmlBValidationErrorCause Cause { get; private set; }

        public override string ToString()
        {
            return FieldName + "," + ElementType + ":" + Cause;
        }
    }
}