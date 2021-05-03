using System;
using System.Collections.Generic;
using Tis100.interpeter;

namespace Tis100
{
    public class Programmer
    {
        public Programmer()
        {
            
        }
        List<Reference> references = new List<Reference>();
        List<Action> actions = new List<Action>();
        public bool program(TisCore core, List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                string line = removeComment(lines[i]);
                line = getReference(line, i);
                EFunction eFunction = getFunction(line);
                line = line.Substring(3).Trim();
                if (!functionBasedPart(eFunction, line, core))
                    return false;
            }
            return true;
        }

        private string removeComment(string line)
        {
            if (!line.Contains("#"))
                return line;
            return line.Substring(0, line.IndexOf('#')).Trim();
        }

        private string getReference(string line, int lineNumber)
        {
            string rest;
            if(!line.Contains(":"))
                return line;
            int pos = line.IndexOf(':');
            string tag = line.Substring(0, pos);
            rest = line.Substring(pos + 1);
            tag = tag.Trim();
            rest = rest.Trim();
            references.Add(new Reference(tag, lineNumber));
            return rest;
        }

        private EFunction getFunction(string line)
        {
            line = line.Trim();
            if(line.Length<3)
                throw new Exception();
            string threeLetters = line.Substring(0, 3);
            switch (threeLetters)
            {
                case "MOV": return EFunction.MOV;
                case "ADD": return EFunction.ADD;
                case "SUB": return EFunction.SUB;
                case "JEZ": return EFunction.JEZ;
                case "JLZ": return EFunction.JLZ;
                case "JGZ": return EFunction.JGZ;
                case "JRO": return EFunction.JRO;
                case "NOP": return EFunction.NOP;
                case "SWP": return EFunction.SWP;
                case "SAV": return EFunction.SAV;
                case "NEG": return EFunction.NEG;
                case "JMP": return EFunction.JMP;
            }
            throw new Exception("no known function");
        }

        private bool functionBasedPart(EFunction function, string line,TisCore core)
        {
            switch (function)
            {
                //source
                case EFunction.ADD:
                    break;
                case EFunction.JRO:
                    break;
                case EFunction.SUB:
                    break;
                // source and destination
                case EFunction.MOV:
                    break;
                //reference
                case EFunction.JEZ:
                    break;
                case EFunction.JGZ:
                    break;
                case EFunction.JLZ:
                    break;
                case EFunction.JMP:
                    break;
                //nothing
                
                
                case EFunction.NEG:
                    actions.Add( new Action(() => core.Neg()));
                    break;
                case EFunction.NOP:
                    actions.Add( new Action(() => core.Add(0)));
                    break;
                case EFunction.SAV:
                    actions.Add(new Action(() => core.Sav()));
                    break;
                case EFunction.SWP:
                    actions.Add(new Action(() => core.Swp()));
                    break;
            }
        }

    }
}