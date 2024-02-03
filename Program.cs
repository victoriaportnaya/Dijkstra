﻿using Kse.Algorithms.Samples;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
});

string[,] map = generator.Generate();
new MapPrinter().Print(map);

// get start and finish point
Console.WriteLine("Type the starting point (col, row) >>");
int startCol = Convert.ToInt32(Console.ReadLine()); 
int startRow = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Type the finishing point (col, row) >>");
int finCol = Convert.ToInt32(Console.ReadLine()); 
int finRow = Convert.ToInt32(Console.ReadLine());

var start = new Point(startCol, startRow);
var finish = new Point(finCol, finRow);

// heap
public class MinHeap
{
    private readonly int[] elements; // keep heap elements 
    private int size; // starting size 

    public MinHeap(int size)
    {
        elements = new int[size]; // initialize heap with the elements number = size
    }

    private void Swap(int first, int second) // change elements in heapify
    {
        var temp = elements[first];
        elements[first] = elements[second];
        elements[second] = temp;
    }

    public bool IsEmpty() => size == 0;

    public int Peek() // return min element (root)
    {
        if (size == 0)
        {
            throw new Exception("Nothing to peek!");
        }

        return elements[0]; // return root
    }

    public int Pop() // get min elememy (root)
    {
        if (size == 0)
        {
            throw new Exception("Nothing to pop!");
        }

        var root = elements[0]; // root
        elements[0] = elements[size - 1] // set new root
        size--;
        ReCalculateDown(); // reassambly the tree 
        
        return root;
    }
}