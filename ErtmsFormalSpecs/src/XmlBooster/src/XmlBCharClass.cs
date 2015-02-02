namespace XmlBooster
{
    public class XmlBCharClass
    {
        private bool[] table = null;

        public XmlBCharClass()
        {
            table = new bool[256];
            for (int i = 0; i < 256; i++) table[i] = false;
        }

        public void init(string initstring)
        {
            for (int j = 0; j < initstring.Length; j++) init(initstring[j]);
        }

        public void init(char initChar)
        {
            table[initChar] = true;
        }

        public bool accept(char ch)
        {
            return table[ch];
        }

        public virtual bool validate(string thestring)
        {
            for (int i = 0; i < thestring.Length; i++)
            {
                if (!accept(thestring[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}