using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tis100;
using Console = System.Console;

namespace tis100Emulator
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> core0Code = new List<string>(){};
            List<string> core1Code = new List<string>() {"MOV UP DOWN" };
            List<string> core2Code = new List<string>() { };
            List<string> core3Code = new List<string>() { };
            List<string> core4Code = new List<string>() { };
            List<string> core5Code = new List<string>() { "MOV UP DOWN" };
            List<string> core6Code = new List<string>() { };
            List<string> core7Code = new List<string>() { };
            List<string> core8Code = new List<string>() { };
            List<string> core9Code = new List<string>() {"MOV UP RIGHT" };
            List<string> core10Code = new List<string>() {"MOV LEFT DOWN" };
            List<string> core11Code = new List<string>() { };

            List<List<string>> Code = new List<List<string>>
            { core0Code,core1Code,core2Code,core3Code,core4Code,core5Code,core6Code,core7Code,core8Code,core9Code,core10Code,core11Code};

            BoardBuilder boardBuilder = new BoardBuilder();
            //todo give boardlayout
            List<ETileType> layout = new List<ETileType>()
            {
                ETileType.TILE_COMPUTE , ETileType.TILE_COMPUTE , ETileType.TILE_COMPUTE , ETileType.TILE_COMPUTE ,
                ETileType.TILE_COMPUTE,ETileType.TILE_COMPUTE,ETileType.TILE_COMPUTE,ETileType.TILE_COMPUTE,
                ETileType.TILE_COMPUTE,ETileType.TILE_COMPUTE,ETileType.TILE_COMPUTE,ETileType.TILE_COMPUTE
            };
            boardBuilder.setLayout(layout);
            List<int> inputStream = new List<int>(){0,1,2,3,4,5};
            List<int> outputSream = new List<int>(){0,1,2,3,4,5};
            boardBuilder.addStream(EStreamType.STREAM_INPUT, 1, inputStream);
            boardBuilder.addStream(EStreamType.STREAM_OUTPUT, 2, outputSream);
            Tis100GameBoard board = boardBuilder.build();
            Console.WriteLine("board was build");
            board.Program(Code);
            Console.WriteLine("board was programmed");
            while (!board.OutputEqualsExpectedOutput())
            {
                board.Step();
            }
            Console.WriteLine("End of program");
            Console.ReadLine();
            //todo bind input and output
            /* working builder
            BoardBuilder builder = new BoardBuilder();
            List<ETileType> tileTypes = new List<ETileType>();
            for (int i = 0; i < 12; i++)
            {
                tileTypes.Add(ETileType.TILE_COMPUTE);
            }
            builder.setLayout(tileTypes);
            List<int> inputStream = new List<int>();
            for (int i = 0; i < 39; i++)
            {
                inputStream.Add(i);
            }
            List<int> outputStream = new List<int>();
            for (int i = 1; i < 39; i++)
            {
                outputStream.Add(i);
            }
            builder.addStream(EStreamType.STREAM_INPUT, 1, inputStream);
            builder.addStream(EStreamType.STREAM_OUTPUT, 2, outputStream);
            Tis100GameBoard board = builder.build();
            board.SetTestSetup();
            int stepcounter = 0;
            Stopwatch t1 = new Stopwatch();

            t1.Start();
            while (!board.OutputEqualsExpectedOutput())
            {
                //Stopwatch t2 = new Stopwatch();
                //t2.Start();
                board.Step();
                stepcounter += 1;
                //t2.Stop();
                //
                board.WriteACC();
                //Console.WriteLine("time of step = " + t2.ElapsedMilliseconds);

            }
            t1.Stop();
            Console.WriteLine("time of program = " + t1.ElapsedMilliseconds);
            Console.WriteLine("end of program, number of steps = " + stepcounter);
            Console.ReadLine();
            */
        }
    }
}
