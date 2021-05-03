using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tis100
{
    public class Tis100GameBoard
    {
        private int xLength = 4;

        private int yLength = 3;
        //console input verbind met 0,1
        //output is 2,2
        private TisCore[,] Cores;
        private List<ComPort> Comports = new List<ComPort>();
        private List<InputConsole> inputConsoles;
        private List<OutputConsole> outputConsoles;
        public Tis100GameBoard()
        {
            Cores = new TisCore[xLength, yLength];
            inputConsoles = new List<InputConsole>();
            outputConsoles = new List<OutputConsole>();
            //create Cores
            for (int i = 0; i < Cores.GetLength(0); i++)
            {
                for(int j = 0; j < Cores.GetLength(1);j++)
                {
                    Cores[i, j] = new TisCore();
                    Cores[i, j].x = i;
                    Cores[i, j].y = j;

                }
            }

            //create and bind ComPorts
            //horizontal ComPorts
            createHorizontalComports();
            createVerticalComports();
            
            
        }

        public bool Program(List<List<string>> coreStrings)
        {
            Programmer programmer = new Programmer();
            for (int x = 0; x < Cores.GetLength(0); x++)
            {
                for (int y = 0; y < Cores.GetLength(1); y++)
                {
                    int count = y * Cores.GetLength(1) + x;
                    programmer.program(Cores[x, y], coreStrings[count]);
                }
            }
            return true;
        }

        internal Tis100GameBoard(ETileType[,] tileTypes, InputConsole[] inputConsoles, OutputConsole[] outputConsoles,int xLength,int yLength)
        {
            this.xLength = xLength;
            this.yLength = yLength;
            Cores = new TisCore[xLength,yLength];
            createTiles(tileTypes);
            createHorizontalComports();
            createVerticalComports();
            bindInputConsoles(inputConsoles);
            bindOutputConsoles(outputConsoles);
        }

        private void createTiles(ETileType[,] tileTypes)
        {
            for (int y = 0; y < this.yLength; y++)
            {
                for (int x = 0; x < this.xLength; x++)
                {
                    switch (tileTypes[x, y])
                    {
                        case (ETileType.TILE_COMPUTE):
                            Cores[x, y] = new TisCore();
                            break;
                        default:
                            throw new NotImplementedException("only compute tile's are implemented at the moment");
                    }

                }
            }
        }

        private void createHorizontalComports()
        {
            for (int i = 0; i + 1 < Cores.GetLength(0); i++)
            {
                for (int j = 0; j < Cores.GetLength(1); j++)
                {
                    ComPort newComPort = new ComPort();
                    Comports.Add(newComPort);
                    Cores[i, j].RightOutBound = newComPort;
                    Cores[i + 1, j].LeftInBound = newComPort;
                }
            }
        }

        private void createVerticalComports()
        {
            for (int i = 0; i < Cores.GetLength(0); i++)
            {
                for (int j = 0; j + 1 < Cores.GetLength(1); j++)
                {
                    ComPort newComPort = new ComPort();
                    Comports.Add(newComPort);
                    Cores[i, j].DownOutBound = newComPort;
                    Cores[i, j + 1].UpInBound = newComPort;
                }
            }
        }

        private void bindInputConsoles(InputConsole[] inputConsoles)
        {
            this.inputConsoles = new List<InputConsole>();
            for(int i = 0; i < inputConsoles.Length;i++)
            {
                if (inputConsoles[i] != null)
                {
                    this.inputConsoles.Add(inputConsoles[i]);
                    ComPort newComPort = new ComPort();
                    Comports.Add(newComPort);
                    inputConsoles[i].OutBoundComPort = newComPort;
                    Cores[i, 0].UpInBound = newComPort;
                }
            }
        }

        private void bindOutputConsoles(OutputConsole[] outputConsoles)
        {
            this.outputConsoles = new List<OutputConsole>();
            for (int i = 0; i < outputConsoles.Length; i++)
            {
                if (outputConsoles[i] != null)
                {
                    this.outputConsoles.Add(outputConsoles[i]);
                    ComPort newComPort = new ComPort();
                    Comports.Add(newComPort);
                    outputConsoles[i].InboundComPort = newComPort;
                    Cores[i, yLength-1].DownOutBound = newComPort;
                }
            }
        }
        /*
        private void addInputConsole(InputConsole inputConsole,int posX)
        {
            inputConsoles.Add(inputConsole);
            ComPort newComPort = new ComPort();
            inputConsole.OutBoundComPort = newComPort;
            Cores[posX, 0].UpInBound = newComPort;
        }
        */
        public bool OutputEqualsExpectedOutput()
        {
            foreach (OutputConsole outputConsole in outputConsoles)
            {
                if (!outputConsole.OutputEqualsExpectedValues())
                    return false;
            }
            return true;
        }

        public void Step()
        {
            foreach (InputConsole inputConsole in inputConsoles)
            {
                inputConsole.ExecuteNextFunction();
            }
            foreach (OutputConsole outputConsole in outputConsoles)
            {
                outputConsole.ExecuteNextFunction();
            }
            for (int i = 0; i < Cores.GetLength(0); i++)
            {
                for(int j = 0; j < Cores.GetLength(1); j++)
                {
                    Cores[i, j].ExecuteNextFunction();

                }
            }
            foreach (ComPort comPort in Comports)
            {
                comPort.ProgressState();
            }
            
        }

        public void WriteACC()
        {
            Console.WriteLine("acc values are");
            foreach(TisCore core in Cores)
            {
                Console.WriteLine("ACC = " + core.Acc);
            }
        }
        
        public void SetTestSetup()
        {
            /*
            List<int> input = new List<int>
            {
                0,
                2,
                3,
                4,
                5
            };
            
            List<int> output = new List<int>
            {
                2,
                3,
                4,
                5
            };
            SetInput(input);
            SetExpectedValues(output);
            */

            Action a1 = new Action(() => Cores[1, 0].Mov(EDirections.UP, EDirections.ACC));
            Cores[1, 0].AddFunction(a1);
            Action a13 = new Action(()=> Cores[1,0].Jez(0));
            Cores[1,0].AddFunction(a13);
            Action a12 = new Action(() => Cores[1, 0].Mov(EDirections.ACC, EDirections.DOWN));
            Cores[1, 0].AddFunction(a12);

            Action a2 = new Action(() => Cores[1, 1].Mov(EDirections.UP, EDirections.ACC));
            Cores[1, 1].AddFunction(a2);
            Action a22 = new Action(() => Cores[1, 1].Mov(EDirections.ACC, EDirections.DOWN));
            Cores[1, 1].AddFunction(a22);

            Action a3 = new Action(() => Cores[1, 2].Mov(EDirections.UP, EDirections.ACC));
            Cores[1, 2].AddFunction(a3);
            Action a32 = new Action(() => Cores[1, 2].Mov(EDirections.ACC, EDirections.RIGHT));
            Cores[1, 2].AddFunction(a32);

            Action a4 = new Action(() => Cores[2, 2].Mov(EDirections.LEFT, EDirections.ACC));
            Cores[2, 2].AddFunction(a4);
            Action a42 = new Action(() => Cores[2, 2].Mov(EDirections.ACC, EDirections.DOWN));
            Cores[2, 2].AddFunction(a42);

        }
    }
}
