// See https://aka.ms/new-console-template for more information

using AdventOfCode2023;

Console.WriteLine("Advent of Code 2023");

string[] input = File.ReadAllLines("InputData/day01.txt");
Console.WriteLine($"Day  1 - Puzzle 1: {Day01Solution.SolveFirstPuzzle(input)}");
Console.WriteLine($"Day  1 - Puzzle 2: {Day01Solution.SolveSecondPuzzle(input)}");
