using DataDictionary.Generated;
using NUnit.Framework;
using NameSpace = DataDictionary.Types.NameSpace;
using Range = DataDictionary.Types.Range;

namespace DataDictionary.test
{
    [TestFixture]
    public class RuleCheckerTest : BaseModelTest
    {
        /// <summary>
        ///     Tests that the Max and Min values for ranges have the right precision
        /// </summary>
        [Test]
        public void TestMaxMinValuesInRange()
        {
            Dictionary dictionary = CreateDictionary("Test");
            NameSpace nameSpace = CreateNameSpace(dictionary, "NameSpace");

            Range r1 = CreateRange(nameSpace, "r1", acceptor.PrecisionEnum.aIntegerPrecision, "0.5", "100");
            Range r2 = CreateRange(nameSpace, "r2", acceptor.PrecisionEnum.aDoublePrecision, "0.0", "100");
            Range r3 = CreateRange(nameSpace, "r3", acceptor.PrecisionEnum.aIntegerPrecision, "A", "100.0");

            RuleCheckerVisitor visitor = new RuleCheckerVisitor(dictionary);
            visitor.visit(nameSpace);

            Assert.True(HasMessage(r1, "Invalid min value for integer range : must be an integer"));
            Assert.False(HasMessage(r1, "Invalid max value for integer range : must be an integer"));

            Assert.False(HasMessage(r2, "Invalid min value for float range : must have a decimal part"));
            Assert.True(HasMessage(r2, "Invalid max value for float range : must have a decimal part"));

            Assert.True(HasMessage(r3, "Cannot parse min value for range"));
            Assert.False(HasMessage(r3, "Cannot parse max value for range"));
        }
    }
}