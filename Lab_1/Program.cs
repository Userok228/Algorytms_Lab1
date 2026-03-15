try
{
    Console.WriteLine("Ведіть матрицю A (щоб завершити введення матриці - введіть порожній рядок):");
    string[] input1 = Console.ReadLine().Split(' ');

    int colN = input1.Length;
    int rowN = colN;
    double[,] matrixA = new double[colN, rowN];
    bool isSquare = true;
    int rank = 0;

    // Створення квадратної матриці на основі першого рядка введення
    for (int j = 0; j < colN; j++)
    {
        matrixA[0, j] = Convert.ToDouble(input1[j]);
    }

    // Метод для введення матриці рядок за рядком, з перевіркою на правильність введення та можливістю завершити введення порожнім рядком
    for (int curRow = 1; true; curRow++)
    {
        string[] input = Console.ReadLine().Split(' ');

        if (input.Length != colN && input.Length != 1)
        {
            Console.WriteLine("Рядок був введений некоректно (невірна кількість елементів). Спробуйте ще раз:");
            curRow--;
            continue;
        }
        if (input[0] == "")
        {
            if (input.Length != 1)
            {
                Console.WriteLine("Рядок був введений некоректно. Спробуйте ще раз:");
                curRow--;
                continue;
            }
            if (curRow != colN)
            {// Якщо кількість введених рядків менша за кількість стовпців, створюється нова матриця з відповідними розмірами
                if (curRow < colN)
                {
                    double[,] matrixRect = new double[curRow, colN];
                    for (int k = 0; k < curRow; k++)
                    {
                        for (int j = 0; j < colN; j++)
                        {
                            matrixRect[k, j] = matrixA[k, j];
                        }
                    }
                    rowN = curRow;
                    isSquare = false;
                    matrixA = matrixRect;
                }
            }
            break;
        }
        if (curRow < colN)
            for (int j = 0; j < colN; j++)
            {
                matrixA[curRow, j] = Convert.ToDouble(input[j]);
            }
        else
        {
            // Якщо кількість введених рядків більша за кількість стовпців, кожен новий введений рядок створюється нова матриця з відповідними розмірами
            double[,] matrixRect = new double[curRow + 1, colN];
            for (int r = 0; r < curRow; r++)
            {
                for (int c = 0; c < colN; c++)
                {
                    matrixRect[r, c] = matrixA[r, c];
                }
            }
            for (int c = 0; c < colN; c++)
            {
                matrixRect[curRow, c] = Convert.ToDouble(input[c]);
            }
            rowN = curRow + 1;
            matrixA = matrixRect;
            isSquare = false;

        }

    }
    Console.WriteLine("Введена матриця:\n");
    ShowMatrix(matrixA, rowN, colN);

    (double[,] processedMatrixA, rank) = Zhordan(matrixA, rowN, colN, rank);
    if (isSquare) // обчислення розв'язку системи рівнянь можливе лише для квадратної матриці, тому якщо матриця не квадратна, виводимо відповідне повідомлення і не виконуємо подальші обчислення
    {
        Console.WriteLine("Обернена матриця:\n");
        ShowMatrix(processedMatrixA, rowN, colN);
        Console.WriteLine("Ранг введеної матриці: " + rank);

        Console.WriteLine("Введіть матрицю B:");

        double[] matrixB = new double[colN];
        for (int i = 0; i < colN; i++)
        {
            string input = Console.ReadLine();
            matrixB[i] = Convert.ToDouble(input);
        }
        double[] solutions = SolutionCalc(processedMatrixA, matrixB);
        ShowSolutions(solutions);
    }
    else
    {
        Console.WriteLine("Ранг введеної матриці: " + rank);
        Console.WriteLine("Матриця не є квадратною, подальші обчислення не можуть бути виконані.");
    }
    (double[,], int) Zhordan(double[,] inputMatrix, int rowN, int colN, int rank)
    {
        double[,] outputMatrix = inputMatrix;
        for (int rs = 0; rs < rowN && rs < colN; rs++)
        {

            double ars = outputMatrix[rs, rs]; // збереження розв'язувального елемента
            if (ars != 0)
            {
                outputMatrix[rs, rs] = 1; // розв'язувальний елемент стає одиницею

                for (int i = 0; i < rowN; i++)
                {
                    for (int j = 0; j < colN; j++)
                    {
                        if (i != rs && j != rs) // приміняємо формулу для всїх елементів, крім тих, що знаходяться в рядку та стовпці розв'язувального елемента
                        {
                            outputMatrix[i, j] = outputMatrix[i, j] * ars - (outputMatrix[i, rs] * outputMatrix[rs, j]);
                        }
                    }
                }
                for (int s = 0; s < colN; s++)
                {
                    if (s != rs)
                    {
                        outputMatrix[rs, s] = outputMatrix[rs, s] * (-1); // міняємо знак елементів рядка розв'язувального елемента
                    }
                }

                for (int i = 0; i < rowN; i++)
                {
                    for (int j = 0; j < colN; j++)
                    {
                        outputMatrix[i, j] = outputMatrix[i, j] / ars; // ділимо всі елементи на розв'язувальний елемент
                    }
                }


                rank++; // ранг збільшується на одиницю, якщо розв'язувальний елемент не дорівнює нулю
            }
        }
        return (outputMatrix, rank);
    }

    void ShowMatrix(double[,] matrix, int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                //Console.Write(matrix[i, j] + " ");
                Console.Write(Math.Round(matrix[i, j], 3) + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    double[] SolutionCalc(double[,] matrixA, double[] matrixB)
    {
        int n = matrixB.Length;
        double[] solutions = new double[n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                solutions[i] += matrixB[j] * matrixA[i, j]; // для пошуку X-ів, перемножуємо кожен елемент рядка оберненої матриці на відповідний елемент матриці B і сумуємо отримані добутки
            }
        }
        return solutions;
    }
    void ShowSolutions(double[] matrix)
    {
        int n = matrix.Length;
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine("X" + (i + 1) + " = " + matrix[i]);
        }
        Console.WriteLine();
    }

}
catch (FormatException ex)
{
    Console.WriteLine("Помилка формату: " + ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine("Сталася помилка: " + ex.Message);
}
