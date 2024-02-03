using Kse.Algorithms.Samples;

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
    private int[] elements; // keep heap elements 
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
    // complementary methods 
    private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
    private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
    private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

    private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < size;
    private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < size;
    private bool IsRoot(int elementIndex) => elementIndex == 0;

    private int GetLeftChild(int elementIndex) => elements[GetLeftChildIndex(elementIndex)];
    private int GetRightChild(int elementIndex) => elements[GetRightChildIndex(elementIndex)];
    private int GetParent(int elementIndex) => elements[GetParentIndex(elementIndex)];

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
        elements[0] = elements[size - 1]; // set new root
        size--;
        HeapifyDown(); // reassambly the tree 
        
        return root;
    }

    public void Add(int element) //add new node
    {
        if (size == elements.Length)
            Resize(); // if no enough space
        elements[size] = element;
        size++;
        HeapifyUp(); // reassembly the tree to add new elements
    }

    private void Resize() // if not enough space for adding
    {
        int[] newElements = new int[elements.Length * 2];
        Array.Copy(elements, newElements, elements.Length);
        elements = newElements;
    }
    
    // heapify 
    private void HeapifyDown() // when pop 
    {
        int index = 0;
        while (HasLeftChild(index))
        {
            var smallerIndex = GetLeftChildIndex(index);
            if (HasRightChild(index) && GetRightChild(index) < GetLeftChildIndex(index))
            {
                smallerIndex = GetRightChildIndex(index);
            }

            if (elements[smallerIndex] >= elements[index])
            {
                break; 
            }
            Swap(smallerIndex, index);
            index = smallerIndex;
        }
    }

    private void HeapifyUp() // when add
    {
        var index = size - 1;
        while (!IsRoot(index) && elements[index] < GetParent(index))
        {
            var parentIndex = GetParentIndex(index);
            Swap(parentIndex, index);
            index = parentIndex;
        }
    }
    
    // dict with points and distances where all distances = 10000000, start = 0; add to heap;
    // check neighbours if neighbour == wall, distance = 1000000, if not wall = 1;
    // update distances and heapify;
    // return path;
    
}