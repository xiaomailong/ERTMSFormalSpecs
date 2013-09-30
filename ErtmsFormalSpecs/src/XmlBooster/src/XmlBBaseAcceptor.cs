using System.IO;
using System.Collections;

namespace XmlBooster 
{

	public abstract class XmlBBaseAcceptor
	{
		public  const char XMLB_AMPERSAND = '&';
		public  const char XMLB_QUOT = '"';
		public  const char XMLB_APOS = '\'';
		public  const char XMLB_LESS = '<';
		public  const char XMLB_GREATER = '>';
		public  const char XMLB_NBSP = ' ';

		public  const int WS_PRESERVE = 0;
		public  const int WS_REPLACE = 1;
		public  const int WS_COLLAPSE = 2;

		public static void connectSons (IXmlBBase father, IEnumerable l)
		{
			if (l != null)
			{
				foreach (IXmlBBase item in l)
					connectSon (father, item);
			}
		}

		public static void connectSon (IXmlBBase father, IXmlBBase son)
		{
			if (son != null)
				son.setFather (father);
		}

		public abstract bool genericUnParse(TextWriter pw, IXmlBBase obj);
		public abstract IXmlBBase genericAccept (XmlBContext ctxt);
		public bool genericSave (IXmlBBase obj,
			string fName)
		{
			FileStream fs;
			StreamWriter pw;

			fs = new FileStream(fName, FileMode.Create);
			pw = new StreamWriter (fs);
			genericUnParse(pw, obj);
			pw.Flush();
			fs.Close();
			return true;
		}

		private static XmlBConverter theConv;
		public static void setConverter (XmlBConverter conv)
		{	
			theConv = conv;
		}

		public static XmlBConverter getConverter ()
		{	
			return theConv;
		}
  
		virtual public IXmlBBase[] genericSubElements(IXmlBBase obj)
		{
			return new IXmlBBase[0];
		}
	}
}