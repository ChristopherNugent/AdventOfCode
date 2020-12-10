using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Advent.Solutions.Interfaces
{
    public interface IAdventSolution
    {
        int ProblemNumber { get; }

        void Execute(StreamReader consoleReader, StreamWriter writer );
    }
}
