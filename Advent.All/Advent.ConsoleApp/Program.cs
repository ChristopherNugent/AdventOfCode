using System;
using Autofac;
using System.Linq;
using System.Collections.Generic;
using Advent.Solutions.Interfaces;
using System.Diagnostics;
using System.IO;

namespace Advent.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerSetup.GetContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                var continueGoing = true;
                var solutionDictionary = scope.Resolve<IEnumerable<IAdventSolution>>().ToDictionary(s => s.ProblemNumber);
                var problemRanges = Range.CreateRanges(solutionDictionary.Keys).ToList();
                while (continueGoing)
                {
                    Console.WriteLine($"Problems: {string.Join(", ", problemRanges)}");
                    Console.Write("Please select a problem number, or 'exit': ");
                    var inputText = Console.ReadLine();
                    if (inputText == "exit")
                    {
                        continueGoing = false;
                        break; 
                    }

                    if(!int.TryParse(inputText, out var problemNumber ) || !solutionDictionary.ContainsKey(problemNumber))
                    {
                        Console.WriteLine("Try again...\n");
                        continue;
                    }


                    Console.WriteLine($"Starting solution for problem {problemNumber}...\n");
                    Stopwatch stopwatch;
                    using (var reader = new StreamReader(Console.OpenStandardInput()))
                    using (var writer = new StreamWriter(Console.OpenStandardOutput()))
                    {
                        stopwatch = Stopwatch.StartNew();
                        var solution = solutionDictionary[problemNumber];
                        solution.Execute(reader, writer);
                        stopwatch.Stop();
                    }
                    Console.WriteLine($"\nMeasured {stopwatch.ElapsedMilliseconds} ms for solution time.\n");
                }
            }
            Console.WriteLine("Goodbye.");
        }
    }
}
