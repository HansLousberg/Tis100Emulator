using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tis100;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tis100.Tests
{
    [TestClass()]
    public class ProgrammerTests
    {
        [TestMethod()]
        public void programTest()
        {
            TisCore t1 = new TisCore();
            Programmer p1 = new Programmer();
            ComPort c1 = new ComPort(5);
            List<string> code = new List<string>
            {
                "S: NOP",
                "JMP S"
            };
            t1.UpInBound = c1;
            p1.program(t1, code);
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void programMovTest()
        {
            int input = 5;
            TisCore t1 = new TisCore();
            Programmer p1 = new Programmer();
            ComPort inComPort = new ComPort(input);
            ComPort outComPort = new ComPort();
            List<string> code = new List<string>
            {
                "MOV UP DOWN"
            };
            t1.UpInBound = inComPort;
            t1.DownOutBound = outComPort;
            p1.program(t1, code);

            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            int val = outComPort.Value;


            Assert.AreEqual(input,val);
        }
        [TestMethod()]
        [ExpectedException(typeof(NoValueException))]
        public void programNopReadTooSoonTest()
        {
            int input = 5;
            TisCore t1 = new TisCore();
            Programmer p1 = new Programmer();
            ComPort inComPort = new ComPort(input);
            ComPort outComPort = new ComPort();
            List<string> code = new List<string>
            {
                "MOV UP ACC",
                "NOP",
                "MOV ACC DOWN"
            };
            t1.UpInBound = inComPort;
            t1.DownOutBound = outComPort;
            p1.program(t1, code);

            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            t1.ExecuteNextFunction();
            outComPort.ProgressState();

            int val = outComPort.Value;
        }

        [TestMethod()]
        public void programNopTest()
        {
            int input = 5;
            TisCore t1 = new TisCore();
            Programmer p1 = new Programmer();
            ComPort inComPort = new ComPort(input);
            ComPort outComPort = new ComPort();
            List<string> code = new List<string>
            {
                "MOV UP ACC",
                "NOP",
                "MOV ACC DOWN"
            };
            t1.UpInBound = inComPort;
            t1.DownOutBound = outComPort;
            p1.program(t1, code);

            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            t1.ExecuteNextFunction();
            outComPort.ProgressState();

            int val = outComPort.Value;
            Assert.AreEqual(input, val);
        }

        [TestMethod()]
        public void programAddTest()
        {
            int input = 5;
            TisCore t1 = new TisCore();
            Programmer p1 = new Programmer();
            ComPort inComPort = new ComPort(input);
            ComPort outComPort = new ComPort();
            List<string> code = new List<string>
            {
                "MOV UP ACC",
                "ADD ACC",
                "MOV ACC DOWN"
            };
            t1.UpInBound = inComPort;
            t1.DownOutBound = outComPort;
            p1.program(t1, code);

            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            int val = outComPort.Value;


            Assert.AreEqual(input*2, val);
        }

        [TestMethod()]
        public void programSubTest()
        {
            int input = 5;
            int expectedValue = -5;
            TisCore t1 = new TisCore();
            Programmer p1 = new Programmer();
            ComPort inComPort = new ComPort(input);
            ComPort outComPort = new ComPort();
            List<string> code = new List<string>
            {
                "SUB "+input,
                "MOV ACC DOWN"
            };
            t1.UpInBound = inComPort;
            t1.DownOutBound = outComPort;
            p1.program(t1, code);

            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            int val = outComPort.Value;


            Assert.AreEqual(expectedValue, val);
        }

        [TestMethod()]
        public void programJezTest()
        {
            int input = 5;
            int expectedValue = 5;
            TisCore t1 = new TisCore();
            Programmer p1 = new Programmer();
            ComPort inComPort = new ComPort(input);
            ComPort outComPort = new ComPort();
            List<string> code = new List<string>
            {
                "JEZ REF",
                "MOV ACC DOWN",
                "REF: ADD " + input,
                "MOV ACC DOWN"
            };
            t1.UpInBound = inComPort;
            t1.DownOutBound = outComPort;
            p1.program(t1, code);

            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            t1.ExecuteNextFunction();
            outComPort.ProgressState();
            int val = outComPort.Value;


            Assert.AreEqual(expectedValue, val);
        }

    }
}