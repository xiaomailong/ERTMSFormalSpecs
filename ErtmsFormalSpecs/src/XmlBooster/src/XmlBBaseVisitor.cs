using System;
using System.Collections;
using System.IO;

namespace XmlBooster {
	public abstract class XmlBBaseVisitor
	{

		public virtual void visit (IXmlBBase obj)
		{
		}

        public virtual void visit(IXmlBBase obj, bool visitSubNodes)
		{
		}

		abstract public void dispatch(IXmlBBase obj);
		abstract public void dispatch(IXmlBBase obj, bool visitSubNodes);

	}
}