namespace XmlBooster
{
    public interface IXmlBBase
    {
        void setNext(IXmlBBase n);
        IXmlBBase getNext();
        void setSon(IXmlBBase n);
        void setFather(IXmlBBase f);
        IXmlBBase getFather();
        void __setDirty(bool val);
        bool __getDirty();
        void validate(XmlBErrorList errList);
    }
}