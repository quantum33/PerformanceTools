using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PerformanceTools.Tests
{
    [TestClass]
    public class SizeUtilityTest
    {
        [TestMethod]
        public void GetSize_WithInteger_Success()
        {
            var itemCount = 10;
            var res = SizeUtility.GetSizeOf(typeof(int), itemCount);
            var res2 = SizeUtility.GetSizeOf<int>(itemCount);
            var expected = sizeof(int) * itemCount;

            Assert.AreEqual(expected, res);
            Assert.AreEqual(expected, res2);
        }

        [TestMethod]
        public void GetSize_WithStringType_NotSupportedExceptionMustBeThrown()
        {
            var mustBeTrue = false;
            try
            {
                SizeUtility.GetSizeOf(typeof(string), 24);
            }
            catch (NotSupportedException)
            {
                mustBeTrue = true;
            }

            Assert.IsTrue(mustBeTrue);
        }

        [TestMethod]
        public void GetSizeOfStringType_WithStringType_Success()
        {
            var itemCount = 12;
            var stringLength = 50;
            var res = SizeUtility.GetSizeOfStringType(stringLength) * itemCount;
            var expected = Helper.GetExpectedSizeForStringType(stringLength) * itemCount;

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void GetSize_WithFakeType_NotSupportedExceptionMustBeThrown()
        {
            var mustBeTrue = false;
            try
            {
                SizeUtility.GetSizeOf(typeof(Fake), 1);
            }
            catch (NotSupportedException)
            {
                mustBeTrue = true;
            }

            Assert.IsTrue(mustBeTrue);
        }

        [TestMethod]
        public void GetSize_WithObjectType_NotSupportedExceptionMustBeThrown()
        {
            var mustBeTrue = false;
            try
            {
                SizeUtility.GetSizeOf(typeof(object), 1);
            }
            catch (NotSupportedException)
            {
                mustBeTrue = true;
            }

            Assert.IsTrue(mustBeTrue);
        }

        [TestMethod]
        public void GetSize_MultiplePrimitiveTypes_Success()
        {
            var expected = 0;
            var types = new List<Type>
            {
                typeof(int),
                typeof(bool),
                typeof(double),
                typeof(decimal),
                typeof(float),
                typeof(byte),
            };

            var context = new Dictionary<Type, int>(types.Count);
            types.ForEach(t =>
            {
                var count = new Random().Next();
                context.Add(t, count);
                expected += t == typeof(string)
                                ? Helper.GetExpectedSizeForStringType(count)
                                : Marshal.SizeOf(t) * count;
            });

            var res = SizeUtility.GetSizeOf(context);
            Assert.AreEqual(expected, res);
        }
    }

    public class Fake { }
}
