using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tis100.interpeter
{
    internal class Reference
    {
        private string name;
        private int lineNumber;

        public Reference(string name, int lineNumber)
        {
            this.name = name;
            this.lineNumber = lineNumber;
        }
        public string Name
        {
            get { return name; }
        }

        public int LineNumber
        {
            get { return lineNumber; }
        }
    }
}
