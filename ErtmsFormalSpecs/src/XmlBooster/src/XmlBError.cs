using System;

namespace XmlBooster
{
    public class XmlBError : Exception
    {
        private int thePtr;
        private string theMsg;

        public XmlBError(string msg, int ptr)
        {
            theMsg = msg;
            thePtr = ptr;
        }

        public string getMsg()
        {
            return theMsg;
        }

        public int getPtr()
        {
            return thePtr;
        }
    }
}