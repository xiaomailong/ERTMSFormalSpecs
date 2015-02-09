namespace XmlBooster
{
    public abstract class XmlBBaseVisitor
    {
        public virtual void visit(IXmlBBase obj)
        {
        }

        public virtual void visit(IXmlBBase obj, bool visitSubNodes)
        {
        }

        public abstract void dispatch(IXmlBBase obj);
        public abstract void dispatch(IXmlBBase obj, bool visitSubNodes);
    }
}