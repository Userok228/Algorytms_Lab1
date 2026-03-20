try
{
    while (true)
    {
        Console.WriteLine("Оберіть частину\n");
        Console.WriteLine("1. A");
        Console.WriteLine("2. B\n");
        string method = Console.ReadLine();

        switch (method)
        {
            case ("1"):
                {
                    Console.WriteLine("\nВедіть матрицю A (щоб завершити введення матриці - введіть порожній рядок):");
                    string[] input1 = Console.ReadLine().Split(' ');

                    int colN = input1.Length;
                    int rowN = colN;
                    double[,] matrixA = new double[colN, rowN];
                    bool isSquare = true;
                    int rank = 0;

                    (colN, rowN, matrixA, isSquare) = AutoInputMatrix(input1, colN, rowN, matrixA, isSquare);

                    Console.WriteLine("Введена матриця:");
                    ShowMatrix(matrixA, rowN, colN);

                    double[,] processedMatrixA = matrixA;
                    for (int rs = 0; rs < rowN && rs < colN; rs++)
                    {
                        (processedMatrixA, rank) = ZhordanStep(matrixA, rowN, colN, rs, rs, rank);

                    }

                    if (isSquare) // обчислення розв'язку системи рівнянь можливе лише для квадратної матриці, тому якщо матриця не квадратна, виводимо відповідне повідомлення і не виконуємо подальші обчислення
                    {
                        Console.WriteLine("Обернена матриця:");
                        ShowMatrix(processedMatrixA, rowN, colN);
                        Console.WriteLine("Ранг введеної матриці: " + rank);

                        Console.WriteLine("\nВведіть матрицю B:");

                        double[] matrixB = new double[colN];
                        for (int i = 0; i < colN; i++)
                        {
                            string input = Console.ReadLine();
                            matrixB[i] = Convert.ToDouble(input);
                        }
                        double[] solutions = SolutionCalcA(processedMatrixA, matrixB);
                        Console.WriteLine("\nРозв'язки системи рівнянь^");
                        ShowSolutionsA(solutions);
                    }
                    else
                    {
                        Console.WriteLine("Ранг введеної матриці: " + rank);
                        Console.WriteLine("Матриця не є квадратною, подальші обчислення не можуть бути виконані.");
                    }
                }
                break;
            case "2":
                {
                    Console.WriteLine("\nВедіть матрицю A (щоб завершити введення матриці - введіть порожній рядок):");
                    string[] input1 = Console.ReadLine().Split(' ');

                    int colN = input1.Length;
                    int rowN = colN;
                    double[,] matrixA = new double[colN, rowN];
                    bool isSquare = true;
                    int rank = 0;

                    (colN, rowN, matrixA, isSquare) = AutoInputMatrix(input1, colN, rowN, matrixA, isSquare);
                    double[] matrixB = new double[rowN];
                    Console.WriteLine("Введіть одинарний стовпчик: ");
                    for (int i = 0; i < rowN; i++)
                    {
                        string input = Console.ReadLine();
                        matrixB[i] = Convert.ToDouble(input);
                    }
                    Console.WriteLine("\nОберіть задачу: \n1) Максимізація цільової функції\n2) Мінімізація цільової функції");
                    string func = Console.ReadLine();
                    while (func != "1" && func != "2")
                    {
                        Console.WriteLine("\nЗадача була вибрана некоректно. Спробуйте ще раз.");
                        func = Console.ReadLine();
                    }
                    Console.WriteLine("\nВведіть цільову функцію Z: ");
                    string[] inputZ = Console.ReadLine().Split(' ');
                    double[] matrixZ = new double[colN + 1];

                    // При пошуку мінімуму функції, всі коефіцієнти цільової функції змінюються на протилежні, тому якщо користувач вибрав мінімізацію, то всі коефіцієнти цільової функції змінюються на протилежні
                    if (func == "2")
                    {
                        for (int i = 0; i < colN; i++)
                        {
                            matrixZ[i] = (-1 * Convert.ToDouble(inputZ[i]));
                        }
                    }
                    else
                        for (int i = 0; i < colN; i++)
                        {
                            matrixZ[i] = Convert.ToDouble(inputZ[i]);
                        }
                    matrixZ[colN] = 0;

                    // Складання основної матриці з матриці A, одинарного стовпчика та цільової функції
                    double[,] matrixMain = new double[rowN + 1, colN + 1];
                    for (int i = 0; i < rowN; i++)
                    {
                        for (int j = 0; j < colN; j++)
                        {
                            matrixMain[i, j] = -matrixA[i, j];
                        }
                        matrixMain[i, colN] = matrixB[i];
                    }
                    for (int j = 0; j < colN; j++)
                    {
                        matrixMain[rowN, j] = -matrixZ[j];
                    }

                    //Створення масивів для збереження індексів X-ів та Y-ів (X-и більше нуля, Y-и - меньше)
                    int[] up = new int[colN];
                    for (int i = 0; i < colN; i++)
                    {
                        up[i] = i + 1;
                    }
                    int[] left = new int[rowN];
                    for (int i = 0; i < rowN; i++)
                    {
                        left[i] = (i + 1) * (-1);
                    }

                    Console.WriteLine("\nВведена сновна матриця:");
                    ShowMatrix(matrixMain, rowN + 1, colN + 1);
                    bool hasNegative = true;
                    double[] matrixSolutions = new double[colN];
                    for (int i = 0; i < rowN; i++)
                    {
                        if (matrixMain[i, colN] < 0)
                        {
                            hasNegative = false;
                            for (int j = 0; j < colN + 1; j++)
                            {
                                if (matrixMain[i, j] < 0)
                                {
                                    int processedRow;
                                    bool stepResult;
                                    // Усунення мінусів в одинарному рядку за допомогою МЖВ для пошуку опорного розв'язку
                                    (stepResult, processedRow) = SolutionStepB(matrixMain, rowN, colN, i, j);
                                    hasNegative = true;
                                    if (!stepResult)
                                    {
                                        Console.WriteLine("\nОпорний розв'язок відсутній: обчислюваний рядок при пошуку опорного розв'язку не було знайдено.");
                                        Environment.Exit(0); // зробити так, щоб програма виходила на нoвий цикл а не завершувала роботу
                                    }
                                    else
                                    {
                                        matrixMain = StepMZHV(matrixMain, rowN + 1, colN + 1, processedRow, j);
                                        int tempLeft = left[i];
                                        int tempUp = up[j];
                                        left[i] = tempUp;
                                        up[j] = tempLeft;
                                        // Зміна x-ів та y-ів після виконання МЖВ
                                        if (left[i] > 0)
                                        {
                                            matrixSolutions[left[i] - 1] = matrixMain[i, colN];
                                        }
                                        if (up[j] > 0)
                                        {
                                            matrixSolutions[up[j] - 1] = 0;
                                        }

                                    }

                                    break;
                                }
                            }

                            i = -1;
                        }
                    }

                    if (!hasNegative)
                    {
                        Console.WriteLine("\nНемає розв'язків: в одинарному стовпчику є від'ємні елементи при відсутніх від'ємних елементах у відповідних рядках матриці.");
                        Environment.Exit(0);
                    }

                    Console.WriteLine("\nМатриця після пошуку опорного розв'язку: ");
                    ShowMatrix(matrixMain, rowN + 1, colN + 1);

                    Console.WriteLine("\nОпорний розв'язок:");
                    for (int i = 0; i < colN; i++)
                    {
                        Console.WriteLine("X" + (i + 1) + " = " + matrixSolutions[i]);
                    }

                    for (int j = 0; j < colN + 1; j++)
                    {
                        if (matrixMain[rowN, j] < 0)
                        {
                            int processedRow;
                            bool stepResult;
                            (stepResult, processedRow) = SolutionStepB(matrixMain, rowN, colN, rowN, j);
                            hasNegative = true;
                            if (!stepResult)
                            {
                                Console.WriteLine("\nОптимальний розв'язок відсутній: обчислюваний рядок при пошуку опорного розв'язку не було знайдено.");
                                Environment.Exit(0); // зробити так, щоб програма виходила на нoвий цикл а не завершувала роботу
                            }
                            else
                            {
                                matrixMain = StepMZHV(matrixMain, rowN + 1, colN + 1, processedRow, j);
                                int tempLeft = left[processedRow];
                                int tempUp = up[j];
                                left[processedRow] = tempUp;
                                up[j] = tempLeft;
                                for (int i = 0; i < rowN; i++)
                                {
                                    if (left[i] > 0)
                                    {
                                        matrixSolutions[left[i] - 1] = matrixMain[i, colN];
                                    }
                                    if (up[j] > 0)
                                    {
                                        matrixSolutions[up[j] - 1] = 0;
                                    }
                                }
                                if (Math.Round(matrixSolutions[1], 2) == 0.66)
                                {
                                    j = j;
                                }

                            }
                            j = -1;
                        }
                    }
                    Console.WriteLine("\nМатриця після пошуку оптимального розв'язку: ");
                    ShowMatrix(matrixMain, rowN + 1, colN + 1);
                    Console.WriteLine("\nОптимальний розв'язок:");
                    for (int i = 0; i < colN; i++)
                    {
                        Console.WriteLine("X" + (i + 1) + " = " + matrixSolutions[i]);
                    }
                    if (func == "2")
                    {
                        Console.WriteLine("\nmin значення цільової функції Z: " + (-1 * matrixMain[rowN, colN]) + "\n");
                    }
                    else
                        Console.WriteLine("\nmax значення цільової функції Z: " + (matrixMain[rowN, colN]) + "\n");

                    break;

                }
        }
    }
    (int colN, int rowN, double[,] matrixA, bool isSquare) AutoInputMatrix(string[] input1, int colN, int rowN, double[,] matrixA, bool isSquare)
    {
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
        return (colN, rowN, matrixA, isSquare);
    }

    (double[,], int) ZhordanStep(double[,] inputMatrix, int rowN, int colN, int r, int c, int rank)
    {
        double[,] outputMatrix = inputMatrix;


        double ars = outputMatrix[r, c]; // збереження розв'язувального елемента
        if (ars != 0)
        {
            outputMatrix[r, c] = 1; // розв'язувальний елемент стає одиницею

            for (int i = 0; i < rowN; i++)
            {
                for (int j = 0; j < colN; j++)
                {
                    if (i != r && j != c) // приміняємо формулу для всїх елементів, крім тих, що знаходяться в рядку та стовпці розв'язувального елемента
                    {
                        outputMatrix[i, j] = outputMatrix[i, j] * ars - (outputMatrix[i, r] * outputMatrix[c, j]);
                    }
                }
            }
            for (int i = 0; i < colN; i++)
            {
                if (i != r)
                {
                    outputMatrix[r, i] = outputMatrix[r, i] * (-1); // міняємо знак елементів рядка розв'язувального елемента
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

    double[] SolutionCalcA(double[,] matrixA, double[] matrixB)
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

    void ShowSolutionsA(double[] matrix)
    {
        int n = matrix.Length;
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine("X" + (i + 1) + " = " + matrix[i]);
        }
        Console.WriteLine();
    }

    double[,] StepMZHV(double[,] inputMatrix, int rowN, int colN, int r, int c)
    {
        double[,] outputMatrix = inputMatrix;


        double ars = outputMatrix[r, c]; // збереження розв'язувального елемента
        if (ars != 0)
        {
            outputMatrix[r, c] = 1; // розв'язувальний елемент стає одиницею

            for (int i = 0; i < rowN; i++)
            {
                for (int j = 0; j < colN; j++)
                {
                    if (i != r && j != c) // приміняємо формулу для всїх елементів, крім тих, що знаходяться в рядку та стовпці розв'язувального елемента
                    {
                        outputMatrix[i, j] = outputMatrix[i, j] * ars - (outputMatrix[i, c] * outputMatrix[r, j]);
                    }
                }
            }
            for (int i = 0; i < rowN; i++)
            {
                if (i != r)
                {
                    outputMatrix[i, c] = outputMatrix[i, c] * (-1); // міняємо знак елементів стовпчика розв'язувального елемента
                }
            }

            for (int i = 0; i < rowN; i++)
            {
                for (int j = 0; j < colN; j++)
                {
                    outputMatrix[i, j] = outputMatrix[i, j] / ars; // ділимо всі елементи на розв'язувальний елемент
                }
            }

        }

        return (outputMatrix);
    }

    (bool, int) SolutionStepB(double[,] matrixMain, int rowN, int colN, int r, int c)
    {
        double leastPositive = double.MaxValue;
        int processedRow = -1;
        for (int i = 0; i < rowN; i++)
        {
            double current = matrixMain[i, colN] / matrixMain[i, c];
            if (current >= 0)
            {
                if (current == 0)
                {
                    if (matrixMain[i, colN] >= 0 && matrixMain[i, c] > 0)
                    {
                        leastPositive = current;
                        processedRow = i;
                    }
                    else if (matrixMain[i, colN] < 0 && matrixMain[i, c] < 0)
                    {
                        leastPositive = current;
                        processedRow = i;
                    }
                    continue;
                }
                if (current < leastPositive)
                {
                    leastPositive = current;
                    processedRow = i;
                }
                else
                if (current == leastPositive)
                {
                    if (i == r)
                        processedRow = i;
                }
            }
        }
        if (processedRow != -1)
        {
            return (true, processedRow);
        }
        else
        {
            return (false, -1);
        }
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
