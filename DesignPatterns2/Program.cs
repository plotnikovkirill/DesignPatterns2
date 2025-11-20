using DesignPatterns2.Classes.AdditionalClasses;
using DesignPatterns2.Classes.MatrixClasses;
using DesignPatterns2.Classes.VectorClasses;

class Programm
{
    static void Main(string[] args)
    {
        int rows = 4;

        int columns = 4;

        int nonZeroElements = 6;

        int maxValue = 9;

       
        RegularMatrix regularMatrix = new RegularMatrix(rows, columns);
        MatrixStatistics statistics1 = new MatrixStatistics(regularMatrix);
        MatrixInitiator.FillMatrix(regularMatrix, nonZeroElements, maxValue);
        Console.WriteLine("Regular Matrix is");
        for (int i = 0; i < regularMatrix.RowNum; i++)
        {
            for (int j = 0; j < regularMatrix.ColumnNum; j++)
            {
                Console.Write(regularMatrix.GetElement(i, j) + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Summa: ");
        Console.WriteLine(statistics1.SumOfValues);
        Console.WriteLine("AVG: ");
        Console.WriteLine(statistics1.AverageValue);
        Console.WriteLine("MAXVal: ");
        Console.WriteLine(statistics1.MaxValue);
        Console.WriteLine("NotZeroCount: ");
        Console.WriteLine(statistics1.NotZeroCount);




        RAZMatrix RAZMatrix = new RAZMatrix(rows, columns);
        MatrixStatistics statistics2 = new MatrixStatistics(RAZMatrix);
        MatrixInitiator.FillMatrix(RAZMatrix, nonZeroElements, maxValue);
        Console.WriteLine("Razryazhenaya Matrix is");
        for (int i = 0; i < RAZMatrix.RowNum; i++)
        {
            for (int j = 0; j < RAZMatrix.ColumnNum; j++)
            {
                Console.Write(RAZMatrix.GetElement(i, j) + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Summa: ");
        Console.WriteLine(statistics2.SumOfValues);
        Console.WriteLine("AVG: ");
        Console.WriteLine(statistics2.AverageValue);
        Console.WriteLine("MAXVal: ");
        Console.WriteLine(statistics2.MaxValue);
        Console.WriteLine("NotZeroCount: ");
        Console.WriteLine(statistics2.NotZeroCount);
    }
}
