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
        ReferenceManager references;
        List<Action> actions;
        public bool program(TisCore core, List<string> lines)
        {
            actions = new List<Action>();
            references = new ReferenceManager();
            for (int i = 0; i < lines.Count; i++)
            {
                string line = removeComment(lines[i]);
                line = getReference(line, i);

                line.Trim();
                char[] separators = new char[]{' ',','};
                string[] parts = line.Split(separators,StringSplitOptions.RemoveEmptyEntries);

                createAction(parts, core);
                
            }
            return core.SetFunctions(actions);
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
            references.AddReference(tag, lineNumber);
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
        

        private bool functionBasedPart(EFunction function, string[] lineParts, TisCore core)
        {
            switch (function)
            {
                //source and direction
                case EFunction.MOV:
                    if (lineParts.Length < 3)
                        return false;
                    Port sourceMov = new Port(lineParts[1]);
                    Port destinationMov = new Port(lineParts[2]);
                    if (sourceMov.IsDirection)
                    {
                        if (destinationMov.IsDirection)
                        {
                            EDirections sourceDirectionsMov = sourceMov.GetDirections();
                            EDirections destinationDirectionsMov = destinationMov.GetDirections();
                            actions.Add(()=> core.Mov(sourceDirectionsMov, destinationDirectionsMov));

                            return true;
                        }
                        return false;
                    }
                    else if (sourceMov.IsInteger)
                    {
                        if (destinationMov.IsDirection)
                        {
                            int sourceDirectionsIntMov = sourceMov.getInteger();
                            EDirections destinationDirectionsMov = destinationMov.GetDirections();
                            actions.Add(() => core.Mov(sourceDirectionsIntMov, destinationDirectionsMov));
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                //source only
                case EFunction.ADD:
                    if (lineParts.Length < 2)
                        return false;
                    Port sourceAdd = new Port(lineParts[1]);
                    if (sourceAdd.IsDirection)
                    {
                        EDirections sourceDirectionsAdd = sourceAdd.GetDirections();
                        actions.Add(()=> core.Add(sourceDirectionsAdd));
                    }
                    else if (sourceAdd.IsInteger)
                    {
                        int sourceIntegerAdd = sourceAdd.getInteger();
                        actions.Add(()=> core.Add(sourceIntegerAdd));
                    }
                    else
                    {
                        throw new Exception("source was not recognized");
                    }
                    return true;

                case EFunction.SUB:
                    if (lineParts.Length < 2)
                        return false;
                    Port sourceSub = new Port(lineParts[1]);
                    if (sourceSub.IsDirection)
                    {
                        EDirections sourceDirectionsSub = sourceSub.GetDirections();
                        actions.Add(() => core.Sub(sourceDirectionsSub));
                    }
                    else if (sourceSub.IsInteger)
                    {
                        int sourceIntegerSub = sourceSub.getInteger();
                        actions.Add(() => core.Sub(sourceIntegerSub));
                    }
                    else
                    {
                        throw new Exception("source was not recognized");
                    }
                    return true;
                case EFunction.JRO:
                    if (lineParts.Length < 2)
                        return false;
                    Port sourceJro = new Port(lineParts[1]);
                    if (sourceJro.IsDirection)
                    {
                        EDirections sourceDirectionsJro = sourceJro.GetDirections();
                        actions.Add(() => core.Jro(sourceDirectionsJro));
                    }
                    else if (sourceJro.IsInteger)
                    {
                        int sourceIntegerJro = sourceJro.getInteger();
                        actions.Add(() => core.Jro(sourceIntegerJro));
                    }
                    else
                    {
                        throw new Exception("source was not recognized");
                    }
                    return true;
                //nothing
                case EFunction.NEG: actions.Add(core.Neg);
                    return true;
                case EFunction.NOP: actions.Add(() => core.Add(0));
                    return true;
                case EFunction.SAV: actions.Add(core.Sav);
                    return true;
                case EFunction.SWP: actions.Add(core.Swp);
                    return true;
                //reference
                case EFunction.JEZ:
                    actions.Add(() => core.Jez(references.getLineNumber(lineParts[1])));
                    return true;
                case EFunction.JGZ:
                    actions.Add(() => core.Jgz(references.getLineNumber(lineParts[1])));
                    return true;
                case EFunction.JLZ:
                    actions.Add(() => core.Jlz(references.getLineNumber(lineParts[1])));
                    return true;
                case EFunction.JMP:
                    actions.Add(() => core.Jmp(references.getLineNumber(lineParts[1])));
                    return true;
            }
            //todo finish how to handle string manipulation foreach type of function
            throw new NotImplementedException();

        }

        private bool createAction(string[] lineParts, TisCore core)
        {
            EFunction eFunction = getFunction(lineParts[0]);
            functionBasedPart(eFunction, lineParts, core);
            return true;
        }

        private string[] getPortStrings(string line)
        {
            int space = line.IndexOf(' ');
            int comma = line.IndexOf(',');
            int splitIndex = -1;
            int caseIdentifier = 0;
            if (space != 0)
                caseIdentifier += 1;
            if (comma != 0)
                caseIdentifier += 2;
            switch (caseIdentifier)
            {
                case 0:
                    break;
                case 1://space only
                    splitIndex = space;
                    break;
                case 2: // comma only
                    splitIndex = comma;
                    break;
                case 3: // both
                    if (space < comma)
                        splitIndex = space;
                    else
                        splitIndex = comma;
                    break;
            }
            string[] result = new string[2];
            if (splitIndex == -1)
            {

                result[0] = line.Substring(0, splitIndex);
                char[] removeCharacters = new[] {' ', ','};
                result[1] = line.Substring(splitIndex).Trim(removeCharacters);

            }
            else
            {
                result[0] = line;
                result[1] = "";
            }
            return result;

        }

        

        

    }
}