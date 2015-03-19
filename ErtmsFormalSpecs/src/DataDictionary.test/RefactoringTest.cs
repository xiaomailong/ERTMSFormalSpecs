using System;
using DataDictionary.Compare;
using DataDictionary.Interpreter;
using DataDictionary.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factory = DataDictionary.Generated.Factory;

namespace DataDictionary.test
{
    [TestClass]
    public class RefactoringTest : BaseModelTest
    {
        [TestMethod]
        public void TestRefactorStructureName()
        {
            Dictionary test = CreateDictionary("Test");
            NameSpace n1 = CreateNameSpace(test, "N1");
            Structure s1 = CreateStructure(n1, "S1");
            StructureElement el1 = CreateStructureElement(s1, "E1", "Boolean");
            Structure s2 = CreateStructure(n1, "S2");
            StructureElement el2 = CreateStructureElement(s2, "E2", "S1");

            Compiler.Compile_Synchronous(true);

            s1.Name = "NewS1";
            Compiler.Refactor(s1);

            Assert.AreEqual("NewS1", el2.TypeName);
        }

    }
}
