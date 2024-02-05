using System.Net.Mime;
using System.Runtime.InteropServices;
using Kse.Algorithms.Samples;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {


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

        var finder = new MinHeap.DijikstraFinder(map, start, finish);
        List<Point> path = finder.FindPath();

        if (path.Count > 0)
        {
            Console.WriteLine("Your path is FOUND!");
            foreach (var point in path)
            {
                Console.WriteLine($"{point.Column}, {point.Row}");
                if (!point.Equals(start) && !point.Equals(finish))
                {
                    map[point.Column, point.Row] = "*";
                }
                else if (point.Equals(start))
                {
                    map[point.Column, point.Row] = "A";
                }
                else if (point.Equals(finish))
                {
                    map[point.Column, point.Row] = "B";
                }
            }
        }
        else
            {
                Console.WriteLine("Path is NOT FOUND!");
            }
        new MapPrinter().Print(map);
        }
    }


// node
public struct Node
{
    public int Cost;
    public Point Position;

    public Node(int cost, Point position)
    {
        Cost = cost;
        Position = position; 
    }
}

// heap
public class MinHeap
{
    private Node[] elements;
    private int size;
    private Dictionary<Point, int> positions;
    
    public MinHeap(int maxSize)
    {
        elements = new Node[maxSize]; // initialize heap with the elements number = size
        positions = new Dictionary<Point, int>();
    }
    
    // complementary methods 
    private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
    private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
    private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

    private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < size;
    private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < size;
    private bool IsRoot(int elementIndex) => elementIndex == 0;
    
    public bool IsEmpty() => size == 0;

    public Node Peek() // return min element (root)
    {
        if (size == 0)
        {
            throw new Exception("Nothing to peek!");
        }

        return elements[0]; // return root
    }

    public Node Pop() // get min element (root)
    {
        if (size == 0)
        {
            throw new Exception("Nothing to pop!");
        }

        var root = elements[0]; // root
        elements[0] = elements[--size]; 
        positions[elements[0].Position] = 0; // set new root
        positions.Remove(root.Position);
        HeapifyDown();
        
        return root;
    }

    public void Add(Node element) //add new node
    {
        if (size == elements.Length)
            throw new Exception("Exceeded number of cells!");
        elements[size] = element;
        positions[element.Position] = size; 
        HeapifyUp(size++); // reassembly the tree to add new elements
    }
    
    // heapify 
    private void HeapifyDown() // when pop 
    {
        int index = 0;
        while (HasLeftChild(index))
        {
            var smallerChildIndex = GetLeftChildIndex(index);
            if (HasRightChild(index) && elements[GetRightChildIndex(index)].Cost < elements[smallerChildIndex].Cost)
            {
                smallerChildIndex = GetRightChildIndex(index);
            }

            if (elements[index].Cost <= elements[smallerChildIndex].Cost)
            {
                break; 
            }
            Swap(index, smallerChildIndex);
            index = smallerChildIndex;
        }
    }

    private void HeapifyUp(int index) // when add
    {
        while (!IsRoot(index) && elements[index].Cost < elements[GetParentIndex(index)].Cost)
        {
            Swap(index, GetParentIndex(index));
            index = GetParentIndex(index);
        }
    }

    public void DecreaseKey(Point position, int newCost)
    {
        if (!positions.TryGetValue(position, out int index))
        {
            throw new Exception("Element is not found!");
        }

        elements[index].Cost = newCost;
        HeapifyUp(index);
    }

    private void Swap(int firstIndex, int secondIndex)
    {
        (elements[firstIndex], elements[secondIndex]) = (elements[secondIndex], elements[firstIndex]);

        positions[elements[firstIndex].Position] = firstIndex;
        positions[elements[secondIndex].Position] = secondIndex;
    }

    public class DijikstraFinder
    {
        private string[,] maze;
        private Point start, finish;
        private Dictionary<Point, int> distances;
        private Dictionary<Point, Point?> predecessors;
        private MinHeap openSet;

        public DijikstraFinder(string[,] maze, Point start, Point finish)
        {
            this.maze = maze;
            this.start = start;
            this.finish = finish;
            distances = new Dictionary<Point, int>();
            predecessors = new Dictionary<Point, Point?>();
            openSet = new MinHeap(maze.GetLength(0) * maze.GetLength(1));
        }

        public List<Point> FindPath()
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                for (int x = 0; x < maze.GetLength(0); x++)
                {
                    var point = new Point(x, y);
                    distances[point] = int.MaxValue;
                    predecessors[point] = null;
                    openSet.Add(new Node(int.MaxValue, point));
                }
            }

            distances[start] = 0;
            openSet.DecreaseKey(start, 0);

            while (!openSet.IsEmpty())
            {
                var currentNode = openSet.Pop();
                var currentPoint = currentNode.Position;

                if (currentPoint.Equals(finish))
                {
                    return ReconstructPath(predecessors, finish);
                }

                foreach (var neighbor in GetNeighbors(currentPoint))
                {
                    int newCost = distances[currentPoint] + 1;
                    if (newCost < distances[neighbor])
                    {
                        distances[neighbor] = newCost;
                        predecessors[neighbor] = currentPoint;
                        openSet.DecreaseKey(neighbor, newCost);
                    }
                }
            }

            return new List<Point>();
        }

        private List<Point> GetNeighbors(Point point)
        {
            var neighbors = new List<Point>();
            var directions = new Point[] { new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0) };

            foreach (var dir in directions)
            {
                var nextPoint = new Point(point.Column + dir.Column, point.Row + dir.Row);
                if (nextPoint.Column >= 0 && nextPoint.Column < maze.GetLength(0) && nextPoint.Row >= 0 &&
                    nextPoint.Row < maze.GetLength(1) && maze[nextPoint.Column, nextPoint.Row] == MapGenerator.Space)
                {
                    neighbors.Add(nextPoint);
                }
            }

            return neighbors;
        }

        private List<Point> ReconstructPath(Dictionary<Point, Point?> predecessors, Point current)
        {
            var path = new List<Point>();
            Point? currentPredecessor = current;
            while (currentPredecessor != null)
            {
                path.Insert(0, currentPredecessor.Value);
                currentPredecessor= predecessors[currentPredecessor.Value];
            }

            return path;
        }
    }
}
    
    // dict with points and distances where all distances = 10000000, start = 0; add to heap;
    // check neighbours if neighbour == wall, distance = 1000000, if not wall = 1;
    // update distances and heapify;
    // return path;
    
