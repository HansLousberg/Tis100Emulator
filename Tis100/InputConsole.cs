using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tis100
{
    public class InputConsole : ILogicBlock
    {
        public InputConsole()
        {
            
        }

        public InputConsole(List<int> queue)
        {
            this.queue = queue;
        }

        public void ExecuteNextFunction()
        {
            if(!Mov())
                outBoundComPort.ConfirmRead();
        }

        private ComPort outBoundComPort;
        private List<int> queue = new List<int>();

        public List<int> Queue
        {
            set { queue = value; }
        }
        public ComPort OutBoundComPort
        {
            get { return outBoundComPort; }
            set { outBoundComPort = value; }
        }
        private int queueIndex = 0;
        private bool Mov()
        {
            if(outBoundComPort.State!= ComPortStates.AVAILABLE)
                return false;
            if (queueIndex >= queue.Count)
                return false;
            if(!outBoundComPort.SetValue(queue[queueIndex]))
                return false;
            queueIndex += 1;
            return true;
        }
    }
}
