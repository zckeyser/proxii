using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Selectors;
using System;
using System.Collections.Generic;

namespace Proxii.Test.Unit.Selectors
{
    [TestClass]
    public class ArgumentTypeSelectorTest
    {
        [TestMethod]
        public void Unit_ArgumentTypeSelector_NoFilter()
        {
            var selector = new ArgumentTypeSelector();

            var result = selector.ContainsTypeDefinition(new[] { typeof(string), typeof(int) });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Unit_ArgumentTypeSelector_OneFilter_HasMatch()
        {
            var argTypes = new List<Type> { typeof(string), typeof(int) };
            var selector = new ArgumentTypeSelector();

            selector.AddArgumentDefinition(argTypes);

            var result = selector.ContainsTypeDefinition(new[] { typeof(string), typeof(int) });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Unit_ArgumentTypeSelector_OneFilter_NoMatches()
        {
            var argTypes = new List<Type> { typeof(string), typeof(long) };
            var selector = new ArgumentTypeSelector();

            selector.AddArgumentDefinition(argTypes);

            var result = selector.ContainsTypeDefinition(new[] { typeof(string), typeof(int) });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Unit_ArgumentTypeSelector_MultipleFilters_HasMatch()
        {
            var argTypes = new List<Type> { typeof(string), typeof(int) };
            var otherArgTypes = new List<Type> { typeof(string), typeof(double) };
            var selector = new ArgumentTypeSelector();

            selector.AddArgumentDefinition(argTypes);
            selector.AddArgumentDefinition(otherArgTypes);

            var result = selector.ContainsTypeDefinition(new[] { typeof(string), typeof(int) });

            Assert.IsTrue(result);

            result = selector.ContainsTypeDefinition(new[] { typeof(string), typeof(double) });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Unit_ArgumentTypeSelector_MultipleFilters_NoMatches()
        {
            var argTypes = new List<Type> { typeof(string), typeof(long) };
            var otherArgTypes = new List<Type> { typeof(string), typeof(double) };
            var selector = new ArgumentTypeSelector();

            selector.AddArgumentDefinition(argTypes);
            selector.AddArgumentDefinition(otherArgTypes);

            var result = selector.ContainsTypeDefinition(new[] { typeof(string), typeof(int) });

            Assert.IsFalse(result);
        }
    }
}
