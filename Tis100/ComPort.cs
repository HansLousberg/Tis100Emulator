using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tis100
{
    public class ComPort
    {
        public ComPort()
        {

        }
        public ComPort(int value)
        {
            this.value = value;
            state = ComPortStates.READABLE;
        }

        private int value = 0;
        private ComPortStates state = ComPortStates.AVAILABLE;
        public ComPortStates State
        {
            get { return state; }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Value
        {
            get
            {
                if (state!= ComPortStates.READABLE)
                    throw new NoValueException();
                state = ComPortStates.READ;
                return value;
                
            }
        }

        public int GetValueNonDestructive
        {
            get
            {
                if (state != ComPortStates.READABLE)
                    throw new NoValueException();
                return value;
            }
              
        }

        public bool SetValue(int value)
        {
            if (state != ComPortStates.AVAILABLE)
            {
                return false;
            }
            state = ComPortStates.SET;
            this.value = value;
            return true;
        }
        
        public void ProgressState()
        {
            
            if (state == ComPortStates.SET)
                state = ComPortStates.READABLE;
        }

        public void ConfirmRead()
        {
            if (state == ComPortStates.READ)
                state = ComPortStates.AVAILABLE;
        }
    }
}
