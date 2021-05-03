using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tis100.interpeter
{
    public class ReferenceManager
    {
        public ReferenceManager()
        {
            
        }
        List<Reference> references = new List<Reference>();

        public bool AddReference(string name,int lineNumber)
        {
            char[] blackListCharacters = new char[]{ ' ', ',', ':' };
            name.Trim(blackListCharacters);
            foreach (Reference reference in references)
            {
                if (reference.Name == name)
                    return false;
            }
            references.Add(new Reference(name,lineNumber));
            return true;
        }

        public int getLineNumber(string referenceName)
        {
            foreach (Reference reference in references)
            {
                if (reference.Name == referenceName)
                {
                    return reference.LineNumber;
                }
            }
            throw new Exception("reference does not exist in current context");
        }
    }
}
