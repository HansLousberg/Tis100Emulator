using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tis100
{
    public class BoardBuilder
    {
        private InputConsole[] inputConsoles;
        private OutputConsole[] outputConsoles;
        private ETileType[,] tiles;
        private int xLength = 4;
        private int yLength = 3;

        public BoardBuilder()
        {
            
        }

        public BoardBuilder(int x, int y)
        {
            xLength = x;
            yLength = y;
        }

        public bool addStream(EStreamType streamType,int posistion,List<int> numbers)
        {
            if(streamType == EStreamType.STREAM_INPUT && inputConsoles ==null)
                inputConsoles = new InputConsole[xLength];
            if(streamType == EStreamType.STREAM_OUTPUT && outputConsoles==null)
                outputConsoles = new OutputConsole[xLength];
            switch (streamType)
            {
                case EStreamType.STREAM_INPUT:
                    if(inputConsoles[posistion]!=null)
                        throw new ArgumentException("there is already a inputstream on the given posistion");
                    inputConsoles[posistion] = new InputConsole(numbers);
                    break;
                case EStreamType.STREAM_OUTPUT:
                    if(outputConsoles[posistion]!=null)
                        throw new ArgumentException("there is already a ioutputstream on the given posistion");
                    outputConsoles[posistion] = new OutputConsole(numbers);
                    break;
                default:
                    return false;

            }
            return true;
        }

        public bool setLayout(List<ETileType> tiles)
        {
            if (tiles.Count < (xLength * yLength))
            {
                return false;
            }
            this.tiles = new ETileType[xLength,yLength];
            for (int y = 0; y < yLength; y++)
            {
                for(int x = 0; x < xLength; x++)
                {
                    this.tiles[x, y] = tiles[y * xLength + x];
                }
            }
            return true;
        }

        public Tis100GameBoard build()
        {
            if(tiles == null)
                throw new AggregateException("no tile layout was provided");
            if(inputConsoles==null)
                throw new AggregateException("There is no inputConsole");
            if(outputConsoles==null)
                throw new AggregateException("There is no outputConsole");
            //todo: some shit
            Tis100GameBoard board = new Tis100GameBoard(tiles,inputConsoles,outputConsoles,xLength,yLength);
            return board;
        }

    }
}
