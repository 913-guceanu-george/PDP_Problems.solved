using Parallelizing_techniques_hamiltonian.HamiltonianCycleFinder;

internal class Program
{
    private static void Main(string[] args)
    {
        int[,] adjacencyMatrix = {
            {0, 1, 1, 1, 0},
            {1, 0, 1, 0, 1},
            {1, 1, 0, 1, 1},
            {1, 0, 1, 0, 1},
            {0, 1, 1, 1, 0}
        };

        int startVertex = 0;

        HamiltonianCycleFinder finder = new HamiltonianCycleFinder(adjacencyMatrix);
        List<int> hamiltonianCycle = finder.FindHamiltonianCycle(startVertex);

        if (hamiltonianCycle != null)
        {
            Console.WriteLine("Hamiltonian cycle found: " + string.Join(" -> ", hamiltonianCycle));
        }
        else
        {
            Console.WriteLine("No Hamiltonian cycle found.");
        }
    }
}
