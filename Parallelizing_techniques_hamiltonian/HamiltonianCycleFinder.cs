namespace Parallelizing_techniques_hamiltonian.HamiltonianCycleFinder
{
    public class HamiltonianCycleFinder
    {
        private readonly int[,] graph;
        private readonly int vertices;
        private readonly object lockObject = new object();
        private List<int> currentPath;
        private bool cycleFound;

        public HamiltonianCycleFinder(int[,] adjacencyMatrix)
        {
            this.graph = adjacencyMatrix;
            this.vertices = adjacencyMatrix.GetLength(0);
            this.currentPath = new List<int>(vertices);
            this.cycleFound = false;
        }

        public List<int> FindHamiltonianCycle(int startVertex)
        {
            currentPath.Add(startVertex);
            Parallel.For(0, vertices, i =>
            {
                if (!cycleFound)
                {
                    ExplorePaths(i);
                }
            });

            return cycleFound ? currentPath : null;
        }

        private void ExplorePaths(int currentVertex)
        {
            if (currentPath.Count == vertices)
            {
                lock (lockObject)
                {
                    if (!cycleFound)
                    {
                        Console.WriteLine("Hamiltonian cycle found: " + string.Join(" -> ", currentPath));
                        cycleFound = true;
                    }
                }
                return;
            }

            if (!currentPath.Contains(currentVertex))
            {
                var newPaths = new List<int>(currentPath);
                newPaths.Add(currentVertex);

                Parallel.ForEach(GetNeighbors(currentVertex), neighbor =>
                {
                    ExplorePaths(neighbor);
                });
            }
        }

        private IEnumerable<int> GetNeighbors(int vertex)
        {
            for (int i = 0; i < vertices; i++)
            {
                if (graph[vertex, i] == 1)
                {
                    yield return i;
                }
            }
        }
    }
}
