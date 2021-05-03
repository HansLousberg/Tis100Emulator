using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tis100
{
    public class TisCore : ILogicBlock
    {
        public TisCore()
        {
        }
        public TisCore(int functionLimit)
        {
            this.functionLimit = functionLimit;
        }
        //Actions.Add(() => this.mov(EDirections.DOWN, EDirections.LEFT));

        public int x = -1;
        public int y = -1;
        
        int functionLimit = 15;

        int acc = 0;
        int bak = 0;

        private ComPort upOutbound;
        public ComPort UpOutBound
        {
            set { upOutbound = value; }
        }
        private ComPort rightOutbound;
        public ComPort RightOutBound
        {
            set { rightOutbound = value; }
        }
        private ComPort downOutbound;
        public ComPort DownOutBound
        {
            set { downOutbound = value; }
        }
        private ComPort leftOutbound;
        public ComPort LeftOutBound
        {
            set { leftOutbound = value; }
        }

        private ComPort upInBound;
        public ComPort UpInBound
        {
            set { upInBound = value; }
        }
        private ComPort rightInBound;
        public ComPort RightInBound
        {
            set { rightInBound = value; }
        }
        private ComPort downInBound;
        public ComPort DownInBound
        {
            set { downInBound = value; }
        }
        private ComPort leftInBound;
        public ComPort LeftInBound
        {
            set { leftInBound = value; }
        }

        private int functionCounter = 0;
        private List<Action> Actions = new List<Action>();

        private ComPort watchingPort = null;

        private EDirections lastPort = EDirections.ANY;

        public int Acc
        {
            get { return acc; }
        }
        
        public bool Mov(EDirections source, EDirections destination)
        {
            ComPort input = getValueSource(source);
            if(input.State!=ComPortStates.READABLE)
                return false;
            return Mov(input.Value, destination);
        }
        public bool Mov(int source, EDirections destination)
        {
            if (destination == EDirections.ACC)
            {
                acc = source;
                SetFunctionCounter();
            }
            else
            {
                ComPort output = GetComPort(destination);
                if(!output.SetValue(source))
                    return false;
                watchingPort = output;
            }
            return true;

        }
        public void Sub(int value)
        {
            acc -= value;
            SetFunctionCounter();
        }
        public bool Sub(EDirections source)
        {
            ComPort sourcePort = getValueSource(source);
            if (sourcePort.State != ComPortStates.READABLE)
                return false;
            Sub(sourcePort.Value);
            return true;
        }
        public void Add(int value)
        {
            acc += value;
            SetFunctionCounter();
        }
        public bool Add(EDirections source)
        {
            ComPort sourcePort = getValueSource(source);
            if (sourcePort.State != ComPortStates.READABLE)
                return false;
            Add(sourcePort.Value);
            return true;
        }
        public void Sav()
        {
            bak = acc;
            SetFunctionCounter();
        }
        public void Swp()
        {
            int tmp = bak;
            bak = acc;
            acc = tmp;
            SetFunctionCounter();
        }

        public void Neg()
        {
            acc = acc * -1;
        }

        public void Jnz(int nextInstruction)
        {
            if (acc != 0)
            {
                SetFunctionCounter(nextInstruction);
            }
            else
            {
                SetFunctionCounter();
            }
        }

        public void Jez(int nextInstruction)
        {
            if (acc == 0)
            {
                SetFunctionCounter(nextInstruction);
            }
            else
            {
                SetFunctionCounter();
            }
        }
        public void Jgz(int nextInstruction)
        {
            if (acc > 0)
            {
                SetFunctionCounter(nextInstruction);
            }
            else
            {
                SetFunctionCounter();
            }
        }

        public void Jlz(int nextInstruction)
        {
            if (acc < 0)
            {
                SetFunctionCounter(nextInstruction);
            }
            else
            {
                SetFunctionCounter();
            }
        }

        public void Jmp(int nextInstruction)
        {
            SetFunctionCounter(nextInstruction);
        }


        public void Jro(int addToInstruction)
        {
            SetFunctionCounter(functionCounter+addToInstruction);
        }

        public bool Jro(EDirections sourcePort)
        {
            ComPort sourceComPort = getValueSource(sourcePort);
            if (sourceComPort.State != ComPortStates.READABLE)
                return false;
            Jro(sourceComPort.Value);
            return true;
        }


        public void ExecuteNextFunction()
        {
            if (watchingPort?.State == ComPortStates.READ)//that questionmark is appearantly null propegation
            {
                SetFunctionCounter();
                watchingPort.ConfirmRead();
                watchingPort = null;
            }

            if(functionCounter < Actions.Count)
                Actions[functionCounter]();
        }

        public void AddFunction(Action action)
        {
            Actions.Add(action);
        }

        public bool SetFunctions(List<Action> actions)
        {
            if (actions.Count > functionLimit)
                return false;
            this.Actions = actions;
            return true;
        }


        private void SetFunctionCounter()
        {
            functionCounter += 1;
            if (functionCounter >= Actions.Count)
                functionCounter = 0;
        }

        private void SetFunctionCounter(int nextFuntion)
        {
            functionCounter = nextFuntion;
            if (functionCounter >= Actions.Count)
                functionCounter = 0;
        }

        private ComPort getValueSource(EDirections source)
        {
            switch (source)
            {
                case EDirections.UP:
                    return upInBound;
                case EDirections.RIGHT:
                    return rightInBound;
                case EDirections.DOWN:
                    return downInBound;
                case EDirections.LEFT:
                    return leftInBound;
                case EDirections.ACC:
                    ComPort accPort = new ComPort(acc);
                    accPort.ProgressState();
                    return accPort;
                case EDirections.ANY:
                    throw new NotImplementedException("cannot use the ANY port yet");
                case EDirections.LAST:
                    return getValueSource(lastPort);
                case EDirections.NIL:
                    ComPort nilPort1 = new ComPort(0);
                    return nilPort1;
                default:
                    ComPort nilPort2 = new ComPort(0);
                    return nilPort2;
            }
        }

        private ComPort GetComPort(EDirections destination)
        {
            switch (destination)
            {
                case EDirections.UP:
                    return upOutbound;
                case EDirections.RIGHT:
                    return rightOutbound;
                case EDirections.DOWN:
                    return downOutbound;
                case EDirections.LEFT:
                    return leftOutbound;
                case EDirections.ACC:
                    throw new NotImplementedException("cannot get ComPort ACC yet");
                //not shure if this is the best method
                case EDirections.ANY:
                    throw new NotImplementedException("cannot use the ANY port yet");
                case EDirections.LAST:
                    return GetComPort(lastPort);
                case EDirections.NIL:
                    return new ComPort();
                default:
                    return new ComPort();
            }
        }
    }
}
















