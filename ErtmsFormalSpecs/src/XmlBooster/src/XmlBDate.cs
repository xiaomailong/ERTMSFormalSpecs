using System;

namespace XmlBooster 
{
	
	[Serializable()]
	public class xmlBDate
	{
		string theStr;

		public xmlBDate (string a)
		{
			theStr = a;
		}
		//---------------------------------------
		public string toString()
		{
			return theStr;
		}
		//---------------------------------------
	}
}