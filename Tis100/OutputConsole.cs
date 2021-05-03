using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tis100
{
    class OutputConsole : ILogicBlock
    {
        public OutputConsole()
        {
            
        }

        public OutputConsole(List<int> expectedValues)
        {
            this.expectedValues = expectedValues;
        }
        public void ExecuteNextFunction()
        {
            Mov();
        }
        private ComPort inBoundComPort;
        public ComPort InboundComPort
        {
            get { return inBoundComPort; }
            set { inBoundComPort = value; }
        }
        private List<int> expectedValues = new List<int>();
        public List<int> ExpectedValues
        {
            set { expectedValues = value; }
        }
        private List<int> actualValues = new List<int>();

        public bool OutputEqualsExpectedValues()
        {
            if (expectedValues.Count != actualValues.Count)
                return false;
            for(int i =0;i< expectedValues.Count; i++)
            {
                if (expectedValues[i] != actualValues[i])
                    return false;
            }
            return true;
        }

        private void Mov()
        {
            if(inBoundComPort.State!= ComPortStates.READABLE)
                return;
            int newValue = inBoundComPort.Value;
            actualValues.Add(newValue);
            Console.WriteLine(newValue);
        }
    }
}
