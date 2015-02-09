using System;

namespace XmlBooster
{
    public class XmlBRecoveryException : Exception
    {
        private String theMsg;

        public XmlBRecoveryException(String msg)
        {
            theMsg = msg;
        }

        public String getMsg()
        {
            return theMsg;
        }
    }
}