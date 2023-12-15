static class MatrixMultiplication
{
    private static int[,] MatrixA, MatrixB, ResultMatrix;
    static int MatrixSize = 1000;

    public static void Run()
    {
        InitializeMatrices();
        // Row wise
        ComputeMatrixParallelRowWiseIndividualThreads(200);

        ComputeMatrixParallelRowWiseThreadPool(16, 200);

        // Column wise
        ComputeMatrixParallelColumnWiseIndividualThreads(200);

        ComputeMatrixParallelColumnWiseThreadPool(16, 200);

        // K-th element
        ComputeMatrixParallelKthElementIndividualThreads(200);

        ComputeMatrixParallelKthElementThreadPool(16, 200);
    }

    private static string MatrixToString(int[,] matrix)
    {
        string result = "\n";

        for (int i = 0; i < MatrixSize; i++)
        {
            for (int j = 0; j < MatrixSize; j++)
            {
                result += $"{matrix[i, j]} ";
            }
            result += "\n";
        }

        return result;
    }

    static void InitializeMatrices()
    {
        // Initialize matrices with random values for demonstration
        Random rand = new();
        MatrixA = new int[MatrixSize, MatrixSize];
        MatrixB = new int[MatrixSize, MatrixSize];
        ResultMatrix = new int[MatrixSize, MatrixSize];

        for (int i = 0; i < MatrixSize; i++)
        {
            for (int j = 0; j < MatrixSize; j++)
            {
                MatrixA[i, j] = rand.Next(1, 10);
                MatrixB[i, j] = rand.Next(1, 10);
            }
        }
        // Console.WriteLine("Matrix A:{0}", MatrixToString(MatrixA));
        // Console.WriteLine("Matrix B:{0}", MatrixToString(MatrixB));
    }

    // Row wise
    static void ComputeMatrixParallelRowWiseIndividualThreads(int numThreads)
    {
        DateTime start = DateTime.Now;
        Console.WriteLine("Computing matrix using row-wise approach with individual threads...");

        Thread[] threads = new Thread[numThreads];

        for (int i = 0; i < numThreads; i++)
        {
            int startRow = i * (MatrixSize / numThreads);
            int endRow = (i == numThreads - 1) ? MatrixSize : (i + 1) * (MatrixSize / numThreads);

            threads[i] = new Thread(() => ComputeRowWise(startRow, endRow));
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
        DateTime end = DateTime.Now;
        Console.WriteLine("Time taken: {0}", (end - start).Milliseconds);
    }
    static void ComputeMatrixParallelRowWiseThreadPool(int numThreads, int numTasks)
    {
        DateTime start = DateTime.Now;

        Console.WriteLine("Computing matrix using row-wise approach with thread pool...");

        Parallel.For(0, numTasks, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, i =>
        {
            int startRow = i * (MatrixSize / numTasks);
            int endRow = (i == numTasks - 1) ? MatrixSize : (i + 1) * (MatrixSize / numTasks);

            ComputeRowWise(startRow, endRow);
        });
        DateTime end = DateTime.Now;

        Console.WriteLine("Time taken: {0}", (end - start).Milliseconds);
    }

    // Column wise
    static void ComputeMatrixParallelColumnWiseIndividualThreads(int numThreads)
    {
        DateTime start = DateTime.Now;

        Console.WriteLine("Computing matrix using column-wise approach with individual threads...");

        Thread[] threads = new Thread[numThreads];

        for (int i = 0; i < numThreads; i++)
        {
            int startColumn = i * (MatrixSize / numThreads);
            int endColumn = (i == numThreads - 1) ? MatrixSize : (i + 1) * (MatrixSize / numThreads);

            threads[i] = new Thread(() => ComputeColumnWise(startColumn, endColumn));
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
        DateTime end = DateTime.Now;

        Console.WriteLine("Time taken: {0}", (end - start).Milliseconds);
    }
    static void ComputeMatrixParallelColumnWiseThreadPool(int numThreads, int numTasks)
    {
        DateTime start = DateTime.Now;

        Console.WriteLine("Computing matrix using column-wise approach with thread pool...");

        Parallel.For(0, numTasks, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, i =>
        {

            int startColumn = i * (MatrixSize / numTasks);
            int endColumn = (i == numTasks - 1) ? MatrixSize : (i + 1) * (MatrixSize / numTasks);

            ComputeColumnWise(startColumn, endColumn);
        });
        DateTime end = DateTime.Now;

        Console.WriteLine("Time taken: {0}", (end - start).Milliseconds);
    }

    // K-th element
    static void ComputeMatrixParallelKthElementIndividualThreads(int numThreads)
    {
        DateTime start = DateTime.Now;

        Console.WriteLine("Computing matrix using k-th element approach with individual threads...");

        Thread[] threads = new Thread[numThreads];

        for (int i = 0; i < numThreads; i++)
        {
            threads[i] = new Thread(() => ComputeKthElement(i, numThreads));
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
        DateTime end = DateTime.Now;

        Console.WriteLine("Time taken: {0}", (end - start).Milliseconds);
    }
    static void ComputeMatrixParallelKthElementThreadPool(int numThreads, int numTasks)
    {
        DateTime start = DateTime.Now;

        Console.WriteLine("Computing matrix using k-th element approach with thread pool...");

        Parallel.For(0, numTasks, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, i =>
        {
            ComputeKthElement(i, numTasks);
        });
        DateTime end = DateTime.Now;

        Console.WriteLine("Time taken: {0}", (end - start).Milliseconds);
    }

    static void ComputeRowWise(int startRow, int endRow)
    {
        for (int i = startRow; i < endRow; i++)
        {
            for (int j = 0; j < MatrixSize; j++)
            {
                ResultMatrix[i, j] = ComputeElement(i, j);
            }
        }
    }

    static void ComputeColumnWise(int startColumn, int endColumn)
    {
        for (int i = 0; i < MatrixSize; i++)
        {
            for (int j = startColumn; j < endColumn; j++)
            {
                ResultMatrix[i, j] = ComputeElement(i, j);
            }
        }
    }

    static void ComputeKthElement(int startElement, int k)
    {
        for (int i = 0; i < MatrixSize; i++)
        {
            for (int j = startElement; j < MatrixSize; j += k)
            {
                ResultMatrix[i, j] = ComputeElement(i, j);
            }
        }
    }

    static int ComputeElement(int row, int col)
    {
        // Function to compute a single element of the resulting matrix
        int result = 0;

        for (int k = 0; k < MatrixSize; k++)
        {
            result += MatrixA[row, k] * MatrixB[k, col];
        }

        return result;
    }

    static void PrintResultMatrix()
    {
        Console.WriteLine("\nResult Matrix:");
        for (int i = 0; i < MatrixSize; i++)
        {
            for (int j = 0; j < MatrixSize; j++)
            {
                Console.Write($"{ResultMatrix[i, j]} ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
