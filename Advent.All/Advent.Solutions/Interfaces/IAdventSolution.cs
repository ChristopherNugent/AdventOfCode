using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Advent.Solutions.Interfaces
{
    public interface IAdventSolution
    {
        /// <summary>
        /// The problem number of this solution. Used to populate the console interface.
        /// </summary>
        int ProblemNumber { get; }

        /// <summary>
        /// Execute the problem and write the solution to the provided stream output,
        /// as if it were the console.
        /// </summary>
        /// <param name="consoleReader"></param>
        /// <param name="consoleWriter"></param>
        void Execute(StreamReader consoleReader, StreamWriter consoleWriter );
    }
}
