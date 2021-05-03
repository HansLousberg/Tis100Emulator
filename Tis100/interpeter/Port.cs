using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tis100.interpeter
{
    public class Port
    {
        public Port(string portString)
        {
            this.portString = portString;
            setup();
        }

        private EDirections direction;
        private int integer = 0;
        private bool isDirection = false;
        private bool isInteger = false;
        private string portString;

        public bool IsDirection
        {
            get { return isDirection; }
        }

        public bool IsInteger
        {
            get { return isInteger; }
        }

        private void setup()
        {
            if(isPort())
                return;
            if(isNumber())
                return;
        }

        private bool isPort()
        {
            switch (portString)
            {
                case "UP":
                    direction = EDirections.UP;
                    isDirection = true;
                    return true;
                case "DOWN":
                    direction = EDirections.DOWN;
                    isDirection = true;
                    return true;
                case "LEFT":
                    direction = EDirections.LEFT;
                    isDirection = true;
                    return true;
                case "RIGHT":
                    direction = EDirections.RIGHT;
                    isDirection = true;
                    return true;
                case "LAST":
                    direction = EDirections.LAST;
                    isDirection = true;
                    return true;
                case "ANY":
                    direction = EDirections.ANY;
                    isDirection = true;
                    return true;
                case "ACC":
                    direction = EDirections.ACC;
                    isDirection = true;
                    return true;
                case "NIL":
                    direction = EDirections.NIL;
                    isDirection = true;
                    return true;
            }
            return false;
        }

        private bool isNumber()
        {
            int i = 0;
            if (portString[i] == '-')
                i++;
            while (i < portString.Length)
            {
                if (!System.Char.IsDigit(portString[i]))
                    return false;
                i++;
            }
            integer = Convert.ToInt32(portString);
            isInteger = true;
            return true;
        }

        public EDirections GetDirections()
        {
            return direction;
        }

        public int getInteger()
        {
            return integer;
        }
    }
}
