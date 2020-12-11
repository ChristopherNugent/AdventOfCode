using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Advent.Solutions.Interfaces;
using System.Linq;

namespace Advent.Solutions
{
    internal class Day5Solution : IAdventSolution
    {
        public int ProblemNumber => 5;

        public void Execute(StreamReader consoleReader, StreamWriter writer)
        {
            var seats = GetProblemInput();

            var highestSeatId = seats.Max(s => s.SeatId);
            writer.WriteLine($"Highest seat id: {highestSeatId}");

            var seatsIdsTaken = seats.Select(s => s.SeatId).ToHashSet();
            for(var s = 0; s < highestSeatId; s++)
            {
                if( !seatsIdsTaken.Contains(s) 
                    && seatsIdsTaken.Contains(s + 1)
                    && seatsIdsTaken.Contains(s - 1) )
                {
                    writer.WriteLine($"My seat id: {s}");
                }
            }

        }

        private List<Seat> GetProblemInput()
                => Resources.Day5Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                    .Select(Seat.Parse)
                    .ToList();

        private class Seat
        {
            public int SeatId { get; }

            public Seat( int seatId )
            {
                SeatId = seatId;
            }

            public static Seat Parse( string boardingPass )
            {
                var binarySeatId = boardingPass
                    .Replace('F', '0')
                    .Replace('B', '1')
                    .Replace('L', '0')
                    .Replace('R', '1');

                var seatId = Convert.ToInt32(binarySeatId, 2);
                return new Seat(seatId);
            }
        }
    }
}
