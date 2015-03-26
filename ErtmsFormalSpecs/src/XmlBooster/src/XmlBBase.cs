using System;
using System.Collections;
using System.IO;
using System.Text;

namespace XmlBooster
{
    public abstract class XmlBBase : IXmlBBase
    {
        private IXmlBBase aNext, aFather, aSibling, aFirstSon;

        public void setNext(IXmlBBase n)
        {
            aNext = n;
        }

        public IXmlBBase getNext()
        {
            return aNext;
        }

        public void setSon(IXmlBBase n)
        {
            XmlBBase n1;

            if (n != null)
            {
                n1 = (XmlBBase) n;
                n1.aFather = this;
                n1.aSibling = aFirstSon;
            }
            aFirstSon = n;
        }

        public void setSon(ICollection l)
        {
            if (l != null)
            {
                foreach (IXmlBBase item in l)
                    setSon(item);
            }
        }

        public void setFather(IXmlBBase f)
        {
            aFather = f;
        }

        public IXmlBBase getFather()
        {
            return aFather;
        }

        public abstract void parse(XmlBContext ctxt, String endingTag);
        //  throws XmlBException, XmlBRecoveryException;
        public abstract void parseBody(XmlBContext ctxt);
        //   throws XmlBException, XmlBRecoveryException;

        public abstract void unParse(TextWriter pw, bool typeId,
            String headingTag,
            String endingTag);

        public abstract void unParseBody(TextWriter pw);

        public void unParse(TextWriter pw, bool typeId)
        {
            unParse(pw, this, typeId, null, null);
        }

        public void unParse(TextWriter pw, IXmlBBase el, bool typeId,
            String headingTag,
            String endingTag)
        {
            XmlBBase el2;

            if (el == null)
            {
                return;
            }
            el2 = (XmlBBase) el;

            while (el2 != null)
            {
                el2.unParse(pw, typeId, headingTag, endingTag);
                el2 = (XmlBBase) el2.aNext;
            }
        }

        public void unParse(TextWriter pw, ArrayList l, bool typeId,
            String headingTag,
            String endingTag)
        {
            if (l != null)
                foreach (IXmlBBase item in l)
                    unParse(pw, item, typeId, headingTag, endingTag);
        }

        public void unParse(TextWriter pw, IEnumerable l, bool typeId,
            String headingTag,
            String endingTag)
        {
            if (l != null)
                foreach (IXmlBBase item in l)
                    unParse(pw, item, typeId, headingTag, endingTag);
        }

        public String ToXMLString()
        {
            //ByteArrayOutputStream os;
            StringWriter pw;

            // os = new ByteArrayOutputStream();
            // pw = new TextWriter(os);
            pw = new StringWriter();
            unParse(pw, false);
            pw.Flush();
            // return new String(os.toByteArray());
            return pw.ToString();
            //new String(os.toByteArray());
        }

        public virtual void subElements(ArrayList l)
        {
        }

        public IXmlBBase[] subElements()
        {
            ArrayList l = new ArrayList();
            subElements(l);
            for (int i = l.Count - 1; i >= 0; i--)
                if (l[i] == null)
                    l.Remove(i);
            if (l.Count == 0)
                return null;
            IXmlBBase[] res = new IXmlBBase[l.Count];
            l.CopyTo(res, 0);
            return res;
        }

//		public override String ToString()
//		{
//			return ToXMLString();
//		}


        public void save(String filename, int bufSize, Encoding encoding)
        {
            FileStream fs;
            StreamWriter pw;

            fs = new FileStream(filename, FileMode.Create);
            pw = new StreamWriter(fs, encoding);
            unParse(pw, false);
            pw.Flush();
            fs.Close();
        }

        public void save(String filename, int bufSize)
        {
            save(filename, bufSize, Encoding.ASCII);
        }

        private const int DEFAULT_BUF_SIZE = 16*1024;

        public void save(String filename)
        {
            save(filename, DEFAULT_BUF_SIZE);
        }

        public void save(String filename, Encoding encoding)
        {
            save(filename, DEFAULT_BUF_SIZE, encoding);
        }

        public abstract void dispatch(XmlBBaseVisitor v);
        public abstract void dispatch(XmlBBaseVisitor v, bool visitSubNodes);

        private bool __dirty = false;

        public void __setDirty(bool val)
        {
            __dirty = val;
        }

        public bool __getDirty()
        {
            return __dirty;
        }

        public virtual void NotifyControllers(Lock aLock)
        {
        }

        public virtual void validate(XmlBErrorList errList)
        {
        }

        /// <summary>
        ///     Performs a full text search
        /// </summary>
        /// <param name="search"></param>
        /// <returns>True if the string provided if found in the object</returns>
        public abstract bool find(Object search);
    }
}