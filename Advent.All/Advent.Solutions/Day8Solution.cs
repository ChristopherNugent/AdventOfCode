using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Advent.Solutions.Interfaces;
using System.Linq;

namespace Advent.Solutions
{
    class Day8Solution : IAdventSolution
    {
        public int ProblemNumber => 8;

        public void Execute(
            StreamReader consoleReader,
            StreamWriter consoleWriter)
        {
            var program = LoadInstructions();
            var firstResolvedState = Run(program);

            consoleWriter.WriteLine($"Program result: {firstResolvedState}");

            var terminatingResult = TweakUntilTermination(program);
            consoleWriter.WriteLine($"Terminating program result: {terminatingResult}");
        }

        private ProgramResult Run(IReadOnlyList<Instruction> program)
        {
            var state = new HandheldState();
            var used = new HashSet<Instruction>();
            while (state.LineNumber < program.Count)
            {
                var instruction = program[state.LineNumber];
                if (used.Contains(instruction))
                {
                    return new ProgramResult(false, state);
                }
                state.Apply(instruction);
                used.Add(instruction);
            }
            return new ProgramResult(true, state);
        }

        private ProgramResult TweakUntilTermination(List<Instruction> program)
        {
            foreach( var instruction in program )
            {
                if (instruction.Type == InstructionType.Acc)
                    continue;
                var oldType = instruction.Type;
                var newType = oldType == InstructionType.Jmp
                    ? InstructionType.Nop
                    : InstructionType.Jmp;
                instruction.Type = newType;
                var result = Run(program);
                if (result.Terminates)
                    return result;
                instruction.Type = oldType;
            }
            return null;
        }

        private class ProgramResult
        {
            public bool Terminates { get; }
            public HandheldState FinalState { get; }

            public ProgramResult( bool terminates, HandheldState state )
            {
                Terminates = terminates;
                FinalState = state;
            }

            public override string ToString()
            {
                return $"Terminates: {Terminates} | {FinalState}";
            }
        }
       

        private List<Instruction> LoadInstructions()
            => InputUtils.SplitLines(Resources.Day8Input)
                .Select(Instruction.Parse)
                .ToList();

        private class HandheldState
        {
            public int Accumulator { get; private set; }
            public int LineNumber { get; private set; }

            public int ExecutionCount { get; private set; }

            public void Apply( Instruction instruction )
            {
                ExecutionCount++;
                switch( instruction.Type )
                {
                    case InstructionType.Acc:
                        Accumulator += instruction.Argument;
                        LineNumber++;
                        return;
                    case InstructionType.Jmp:
                        LineNumber += instruction.Argument;
                        return;
                    case InstructionType.Nop:
                        LineNumber++;
                        return;
                }
            }

            public override string ToString()
            {
                return $"Line Number: {LineNumber} | Accumlator: {Accumulator} | Exec Count: {ExecutionCount}";
            }
        }

        private enum InstructionType
        {
            Acc,
            Jmp,
            Nop
        }

        private class Instruction
        {
            public InstructionType Type { get; set; }
            public int Argument { get; }

            public Instruction( InstructionType type, int arg )
            {
                Type = type;
                Argument = arg;
            }

            public static Instruction Parse( string text )
            {
                var tokens = text.Split();
                var cmdToken = tokens[0];
                var argToken = tokens[1];

                InstructionType type;
                switch(cmdToken)
                {
                    case "acc":
                        type = InstructionType.Acc;
                        break;
                    case "jmp":
                        type = InstructionType.Jmp;
                        break;
                    default:
                        type = InstructionType.Nop;
                        break;
                }

                var arg = int.Parse(argToken);

                return new Instruction(type, arg);
            }
        }
    }
}
