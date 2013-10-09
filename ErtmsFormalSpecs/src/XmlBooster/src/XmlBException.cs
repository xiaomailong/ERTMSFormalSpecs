using System;

namespace XmlBooster 
{


	public class XmlBException: Exception
		{

			private string theMsg;

			public XmlBException (string msg)
	{
		theMsg = msg;
	}

	public string getMsg()
{
	return theMsg;
}
}
}