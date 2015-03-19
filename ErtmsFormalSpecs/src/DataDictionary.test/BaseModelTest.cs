using DataDictionary.Interpreter;
using DataDictionary.Types;

namespace DataDictionary.test
{
    public class BaseModelTest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseModelTest()
        {
            Generated.acceptor.setFactory(new ObjectFactory());    
        }

        

        /// <summary>
        /// The EFS System to test
        /// </summary>
        protected EFSSystem System
        {
            get { return EFSSystem.INSTANCE; }
        }

        /// <summary>
        /// The Compiler
        /// </summary>
        protected Compiler Compiler
        {
            get { return System.Compiler; }
        }

        /// <summary>
        /// The factory used to create elements
        /// </summary>
        protected Generated.Factory Factory
        {
            get { return Generated.acceptor.getFactory(); }
        }

        /// <summary>
        /// Creates a dictionary in the system
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Dictionary CreateDictionary(string name)
        {
            Dictionary retVal = (Dictionary) Factory.createDictionary();

            System.AddDictionary(retVal);
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        /// Creates a namespace in the dictionary provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected NameSpace CreateNameSpace(Dictionary enclosing, string name)
        {
            NameSpace retVal = (NameSpace) Factory.createNameSpace();

            enclosing.appendNameSpaces(retVal);
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        /// Creates a structure in the namespace provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Structure CreateStructure(NameSpace enclosing, string name)
        {
            Structure retVal = (Structure)Factory.createStructure();

            enclosing.appendStructures(retVal);
            retVal.Name = name;

            return retVal;
        }

        /// <summary>
        /// Creates a structure element in the structure provided
        /// </summary>
        /// <param name="enclosing"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected StructureElement CreateStructureElement(Structure enclosing, string name, string type)
        {
            StructureElement retVal = (StructureElement)Factory.createStructureElement();

            enclosing.appendElements(retVal);
            retVal.Name = name;
            retVal.TypeName = type;

            return retVal;
        }

    }
}
