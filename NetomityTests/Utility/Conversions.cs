using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Utility;
using Netomity.Core;
using Netomity.Core.Enum;

namespace NetomityTests.Utility
{
    [TestClass]
    public class ConversionsTests
    {
        [TestMethod]
        public void HexToBytesTest()
        {
            var r = Conversions.HexToBytes("11FF44");
            Assert.AreEqual(r[0], 0x11);
            Assert.AreEqual(r[1], 0xff);
            Assert.AreEqual(r[2], 0x44);

        }

        [TestMethod]
        public void HexToBytesTestExtraneous()
        {
            var r = Conversions.HexToBytes("11.FF.44");
            Assert.AreEqual(r[0], 0x11);
            Assert.AreEqual(r[1], 0xff);
            Assert.AreEqual(r[2], 0x44);

        }

        [TestMethod]
        public void BytesToAsciiTest()
        {
            var bA = new byte[] {0x34, 0x23};

            var r = Conversions.BytesToAscii(bA);
            Assert.AreEqual("4#", r);
        }

        [TestMethod]
        public void AsciiToHexForLeadingZeroTests()
        {
            var d = "026219057B0F11FF";
            var r1 = Conversions.HexToAscii(d);
            var r4 = Conversions.HexToBytes(d);
            var r5 = Conversions.BytesToAscii(r4);
            var r3 = Conversions.AsciiToBytes(r1);
            var r2 = Conversions.AsciiToHex(r1);
            Assert.AreEqual(d, r2);
        }

        [TestMethod]
        public void HexToBytesUnsignedTests()
        {
            var d = "026219057B0F11FF";
            var r1 = Conversions.HexToBytes(d);
            Assert.AreEqual(0xff, r1[7]);
        }

        [TestMethod]
        public void ValueToStringEnumTest()
        {
            var st = Conversions.ValueToStringEnum<CommandType>(CommandType.On.ToString());
            Assert.AreEqual(CommandType.On, st);
        }

        [TestMethod]
        public void BinaryToBytesTest()
        {
            var str = "01100000 10011111 00100000 11011111";
            var bytes = Conversions.BinaryStrToBytes(str);
            Assert.AreEqual(96, bytes[0]);
            Assert.AreEqual(223, bytes[3]);

        }

        [TestMethod]
        public void BinaryToAsciiTest()
        {
            var str = "01100000 10011111 00100000 11011111";
            var ascii = Conversions.BinaryStrToAscii(str);
            Assert.AreEqual(@"` ß", ascii);
 
        }

        [TestMethod]
        public void ByteReverseTest()
        {
            var str = "01100000";
            var bytes = Conversions.BinaryStrToBytes(str);
            var rBytes = Conversions.BytesReverse(bytes);
            Assert.AreEqual(rBytes[0], 6 );
        }

    }
}
