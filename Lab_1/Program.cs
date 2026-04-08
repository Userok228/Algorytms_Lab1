using Lab_1;
using System;

try
{
    while (true)
    {
        Console.WriteLine("Оберіть частину:");
        Console.WriteLine("1. А\n2. B\n3. C\n4. D\n");
        string method = Console.ReadLine();
        while (method != "1" && method != "2" && method != "3" && method != "4")
        {
            Console.WriteLine("Частина обрана некоректно. Спробуйте ще раз:");
            method = Console.ReadLine();
        }

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

                    Console.WriteLine("\n----------Початкова симплекс-таблиця----------");
                    ShowMatrix(matrixA, rowN, colN);

                    bool protocol = false;
                    Console.WriteLine("\nВивести протокол обчислень? (Y або 1 щоб погодитись)");
                    string proto = Console.ReadLine();

                    if (proto == "1" || proto == "y" || proto == "Y")
                        protocol = true;

                    int step = 0;
                    double[,] processedMatrixA = matrixA;
                    for (int rs = 0; rs < rowN && rs < colN; rs++)
                    {
                        if (protocol)
                            Console.WriteLine("\nРозв'язувальний елемент: " + matrixA[rs, rs] + "(рядок: " + (rs + 1) + " стовпчик: " + (rs + 1) + ")");
                        (processedMatrixA, rank) = ZhordanStep(matrixA, rowN, colN, rs, rs, rank);

                        if (protocol)
                        {
                            step++;
                            Console.WriteLine("\nМатриця після " + step + " кроку ЗЖВ:");
                            ShowMatrix(processedMatrixA, rowN, colN);
                            Console.WriteLine("Оновлений ранг матриці: " + rank);
                        }

                    }

                    if (isSquare) // обчислення розв'язку системи рівнянь можливе лише для квадратної матриці, тому якщо матриця не квадратна, виводимо відповідне повідомлення і не виконуємо подальші обчислення
                    {
                        Console.WriteLine("\n----------Обернена матриця----------");

                        ShowMatrix(processedMatrixA, rowN, colN);

                        Console.WriteLine("Ранг введеної матриці: " + rank);

                        Console.WriteLine("\nВведіть матрицю B:");

                        double[] matrixB = new double[colN];
                        for (int i = 0; i < colN; i++)
                        {
                            string input = Console.ReadLine();
                            matrixB[i] = Convert.ToDouble(input);
                        }

                        int n = matrixB.Length;
                        double[] solutions = new double[n];
                        if (protocol)
                        {
                            Console.WriteLine("\n----------Пошук розв'язків системи рівнянь----------");
                        }
                        for (int i = 0; i < n; i++)
                        {
                            if (protocol)
                                Console.Write("X" + i + " = ");
                            for (int j = 0; j < n; j++)
                            {
                                if (protocol)
                                {
                                    if (j > 0)
                                        Console.Write(" + ");
                                    Console.Write(Math.Round(matrixB[j], 3) + " * " + Math.Round(matrixA[i, j], 3));
                                }
                                solutions[i] += matrixB[j] * matrixA[i, j]; // для пошуку X-ів, перемножуємо кожен елемент рядка оберненої матриці на відповідний елемент матриці B і сумуємо отримані добутки
                            }
                            if (protocol)
                                Console.WriteLine(" = " + solutions[i]);
                        }

                        if (!protocol)
                        {
                            Console.WriteLine("\nРозв'язки системи рівнянь:");
                            ShowSolutionsA(solutions);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ранг введеної матриці: " + rank);
                        Console.WriteLine("Матриця не є квадратною, подальші обчислення не можуть бути виконані.");
                    }
                    Console.WriteLine();
                }
                break;
            case "2":
                {
                    Console.WriteLine("\nВедіть матрицю A (щоб завершити введення матриці - введіть порожній рядок):");
                    string[] input1 = Console.ReadLine().Split(' ');

                    int colN = input1.Length;
                    int x = colN;
                    int rowN = colN;
                    double[,] matrixA = new double[colN, rowN];
                    bool isSquare = true;

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

                    // При пошуку мінімуму функції, всі коефіцієнти цільової функції змінюються на протилежні
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

                    Console.WriteLine("\n----------Вхідні параметри----------");
                    Console.Write("\nZ = ");
                    for (int i = 0; i < matrixZ.Length - 1; i++)
                    {
                        if (matrixZ[i] >= 0)
                        {
                            if (i > 0)
                                Console.Write("+" + matrixZ[i]);
                            else
                                Console.Write(matrixZ[i]);

                        }
                        else
                            Console.Write(matrixZ[i]);
                        if (i + 2 == matrixZ.Length)
                        {
                            Console.Write(" -> ");
                            if (func == "1")
                                Console.Write("max");
                            else
                                Console.Write("min");
                        }
                    }
                    Console.WriteLine("\n\nПочаткова симплекс-таблиця:");
                    ShowMatrix(matrixMain, rowN + 1, colN + 1);

                    bool protocol = false;
                    Console.WriteLine("\nВивести протокол обчислень? (Y або 1 щоб погодитись)");
                    string proto = Console.ReadLine();

                    if (proto == "1" || proto == "y" || proto == "Y")
                        protocol = true;

                    if (protocol)
                        Console.WriteLine("----------Пошук опорного розв'язку----------\n");

                    bool hasNegative = true;
                    bool stepResult = true;
                    for (int i = 0; i < rowN; i++)
                    {
                        if (matrixMain[i, colN] < 0)
                        {
                            hasNegative = false;
                            for (int j = 0; j < colN; j++)
                            {
                                if (matrixMain[i, j] < 0)
                                {
                                    int processedRow;

                                    // Усунення мінусів в одинарному стовпчику за допомогою МЖВ для пошуку опорного розв'язку
                                    (stepResult, processedRow) = SolutionStepBCD(matrixMain, rowN, colN, i, j);
                                    hasNegative = true;
                                    if (!stepResult)
                                    {
                                        Console.WriteLine("\nОпорний розв'язок відсутній: відсутні додатні добутки при знаходженні найменьшого невід'ємного добутку елементів результуючого та одинарного стовпчиків.");
                                        break;
                                    }
                                    else
                                    {
                                        if (protocol)
                                        {
                                            Console.WriteLine("Актуальна симплекс-таблиця:");
                                            ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                            Console.WriteLine("Розв'язувальний елемент: " + matrixMain[processedRow, j] + "(рядок " + (processedRow + 1) + ", стовпчик " + (j + 1) + ")");
                                        }

                                        matrixMain = StepMZHV(matrixMain, rowN + 1, colN + 1, processedRow, j);

                                        if (protocol)
                                        {
                                            Console.WriteLine("\nМатриця після  кроку МЖВ:");
                                            ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                        }

                                        int tempLeft = left[i];
                                        int tempUp = up[j];
                                        left[i] = tempUp;
                                        up[j] = tempLeft;
                                    }

                                    break;
                                }
                                if (!stepResult)
                                {
                                    break;
                                }
                            }
                            if (!hasNegative)
                                break;
                            i = -1;
                        }
                    }
                    if (!stepResult)
                        break;
                    if (!hasNegative)
                    {
                        Console.WriteLine("\nНемає розв'язків: в одинарному стовпчику є від'ємні елементи при відсутніх від'ємних елементах у відповідних рядках матриці.");
                        break;
                    }

                    Console.WriteLine("----------------------------------------\nМатриця після пошуку опорного розв'язку: ");
                    ShowMatrix(matrixMain, rowN + 1, colN + 1);

                    Console.WriteLine("Опорний розв'язок:");
                    ShowSolutionsB(matrixMain, left, x, colN);

                    if (protocol)
                        Console.WriteLine("\n----------Пошук оптимального розв'язку----------\n");

                    for (int j = 0; j < colN; j++)
                    {
                        if (matrixMain[rowN, j] < 0)
                        {
                            int processedRow;
                            (stepResult, processedRow) = SolutionStepBCD(matrixMain, rowN, colN, rowN, j);
                            if (!stepResult)
                            {
                                Console.WriteLine("\nОптимальний розв'язок відсутній: відсутні додатні добутки при знаходженні найменьшого невід'ємного добутку елементів результуючого та одинарного стовпчиків.");
                                break;
                            }
                            else
                            {
                                if (protocol)
                                {
                                    Console.WriteLine("Актуальна симплекс-таблиця:");
                                    ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                    Console.WriteLine("Розв'язувальний елемент: " + matrixMain[processedRow, j] + "(рядок " + (processedRow + 1) + ", стовпчик " + (j + 1) + ")");
                                }

                                matrixMain = StepMZHV(matrixMain, rowN + 1, colN + 1, processedRow, j);

                                if (protocol)
                                {
                                    Console.WriteLine("\nМатриця після  кроку МЖВ:");
                                    ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                }

                                int tempLeft = left[processedRow];
                                int tempUp = up[j];
                                left[processedRow] = tempUp;

                            }
                            j = -1;
                        }
                    }
                    if (!stepResult)
                    {
                        break;
                    }
                    Console.WriteLine("----------------------------------------\nМатриця після пошуку оптимального розв'язку: ");
                    ShowMatrix(matrixMain, rowN + 1, colN + 1);
                    Console.WriteLine("Оптимальний розв'язок:");
                    ShowSolutionsB(matrixMain, left, x, colN);

                    Console.WriteLine();
                    if (func == "2")
                    {
                        Console.WriteLine("\nmin значення цільової функції Z: " + (-1 * matrixMain[rowN, colN]) + "\n");
                    }
                    else
                        Console.WriteLine("\nmax значення цільової функції Z: " + (matrixMain[rowN, colN]) + "\n");

                    break;

                }
            case "3":
            case "4":
                {
                    
                    Console.WriteLine("\nВведіть матрицю A (щоб завершити введення матриці - введіть порожній рядок):");
                    string[] input1 = Console.ReadLine().Split(' ');

                    int colN = input1.Length;
                    int rowN = colN;
                    double[,] matrixA = new double[colN, rowN];
                    bool isSquare = true;

                    (colN, rowN, matrixA, isSquare) = AutoInputMatrix(input1, colN, rowN, matrixA, isSquare);
                    double[] columnOne = new double[rowN];
                    Console.WriteLine("Введіть одинарний стовпчик: ");
                    for (int i = 0; i < rowN; i++)
                    {
                        string input = Console.ReadLine();
                        columnOne[i] = Convert.ToDouble(input);
                    }

                    Console.WriteLine("\nВведіть (через пробіл) номери рядків у яких присутня рівність:");
                    string[] inputEq = Console.ReadLine().Split(' ');
                    while (inputEq.Length > 1 && inputEq.Contains(""))
                    {
                        Console.WriteLine("\nРядки введені некоректно. Спробуйте ще раз:");
                        inputEq = Console.ReadLine().Split(' ');
                    }
                    int[] eqRows = new int[inputEq.Length];
                    bool eq = false;
                    for (int i = 0; i < inputEq.Length; i++)
                    {
                        if (inputEq[i] != "")
                        {
                            eqRows[i] = Convert.ToInt32(inputEq[i]);
                            eq = true;
                        }
                        else
                        {
                            eqRows = new int[0];
                            break;
                        }
                    }


                    Console.WriteLine("\nОберіть задачу: \n1) Максимізація цільової функції\n2) Мінімізація цільової функції");
                    string func = Console.ReadLine();
                    while (func != "1" && func != "2")
                    {
                        Console.WriteLine("\nЗадача була обрана некоректно. Спробуйте ще раз.");
                        func = Console.ReadLine();
                    }
                    Console.WriteLine("\nВведіть (через пробіл) цільову функцію Z: ");
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
                        matrixMain[i, colN] = columnOne[i];
                    }
                    for (int j = 0; j < colN; j++)
                    {
                        matrixMain[rowN, j] = -matrixZ[j];
                    }

                    // зміна знаків у нульових рядках
                    if (eq)
                        for (int i = 0; i < rowN; i++)
                        {
                            for (int k = 0; k < eqRows.Length; k++)
                            {
                                if (eqRows[k] == (i + 1))
                                    for (int j = 0; j < colN + 1; j++)
                                    {
                                        matrixMain[i, j] = -matrixMain[i, j];
                                    }
                            }
                        }

                    //Створення масивів зі змінними, а також масивів для збереження їх розположення
                    Variable[] arrayX = new Variable[colN];
                    Variable[] arrayY = new Variable[rowN];

                    Variable[] left = new Variable[rowN];
                    Variable[] up = new Variable[colN];

                    for (int i = 0; i < colN; i++)
                    {
                        arrayX[i] = new Variable(i + 1, 'X');
                        up[i] = arrayX[i];

                    }
                    int yN = 0;
                    for (int i = 0; i < rowN; i++)
                    {
                        if (eqRows.Contains((i + 1)))
                        {
                            left[i] = new Variable(i + 1, '0');
                        }
                        else
                        {
                            arrayY[yN] = new Variable(yN + 1, 'Y');
                            left[i] = arrayY[yN];
                            yN++;
                        }
                    }

                    Console.WriteLine("\nВведіть (через пробіл) номери вільних X-ів (Якщо таких немає - введіть порожній рядок): ");
                    bool free = false;
                    Row[] delRows = new Row[colN];
                    for (int i = 0; i < colN; i++)
                    {
                        delRows[i] = new Row(-1, null);
                    }

                    string[] inputFree = Console.ReadLine().Split(' ');
                    while (inputFree[0] == "" && inputFree.Length != 1 || (inputFree.Length > arrayX.Length))
                    {
                        Console.WriteLine("\nВільні X-и введені некоректно. Спробуйте ще раз:");
                        inputFree = Console.ReadLine().Split(' ');
                    }
                    if (inputFree[0] != "")
                        free = true;

                    Console.WriteLine("----------Вхідні параметри----------");
                    Console.Write("\nZ = ");
                    for (int i = 0; i < matrixZ.Length - 1; i++)
                    {
                        if (matrixZ[i] >= 0)
                        {
                            if (i > 0)
                                Console.Write("+" + matrixZ[i]);
                            else
                                Console.Write(matrixZ[i]);

                        }
                        else
                            Console.Write(matrixZ[i]);
                        if (i + 2 == matrixZ.Length)
                        {
                            Console.Write(" -> ");
                            if (func == "1")
                                Console.Write("max");
                            else
                                Console.Write("min");
                        }
                    }
                    Console.WriteLine("\n\nПочаткова симплекс-таблиця:");
                    ShowMatrix(matrixMain, rowN + 1, colN + 1);

                    Console.Write("Вільні X-и: ");
                    if (!free)
                        Console.WriteLine("відсутні.");
                    else
                        for (int i = 0; i < inputFree.Length; i++)
                        {
                            Console.Write("X" + inputFree[i]);
                            if (i < inputFree.Length - 1)
                                Console.Write(", ");
                            else Console.WriteLine(".");
                        }
                    Console.Write("Нульові рядки: ");
                    if (!eq)
                        Console.WriteLine("відсутні.");
                    else
                        for (int i = 0; i < inputEq.Length; i++)
                        {
                            Console.Write(inputEq[i]);
                            if (i < inputEq.Length - 1)
                                Console.Write(", ");
                            else Console.WriteLine(".");
                        }
                    bool allZero = false;
                    bool protocol = false;
                    Console.WriteLine("\nВивести протокол обчислень? (Y або 1 щоб погодитись)");
                    string proto = Console.ReadLine();

                    if (proto == "1" || proto == "y" || proto == "Y")
                        protocol = true;

                    if (protocol && free)
                    {
                        Console.WriteLine("\n----------Видалення вільних змінних----------");
                    }

                    if (free)
                    {
                        free = true;
                        int[] freeX = new int[inputFree.Length];
                        for (int i = 0; i < inputFree.Length; i++)
                        {
                            freeX[i] = Convert.ToInt32(inputFree[i]);
                        }

                        foreach (int xIndex in freeX)
                        {
                            allZero = true;
                            for (int i = 0; i < rowN; i++)
                            {
                                if (Math.Round(matrixMain[i, xIndex - 1], 6) != 0)// Перший ненульовий елемент у стовпчику з вільним x-ом
                                {
                                    allZero = false;
                                    if (protocol)
                                    {
                                        Console.WriteLine("Актуальна симплекс-таблиця:");
                                        ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                        Console.WriteLine("Розв'язувальний елемент: " + matrixMain[i, xIndex - 1] + "(рядок " + (i + 1) + ", стовпчик " + (xIndex) + ")");
                                    }
                                    matrixMain = StepMZHV(matrixMain, rowN + 1, colN + 1, i, xIndex - 1);

                                    if (protocol)
                                    {
                                        Console.WriteLine("\nМатриця після  кроку МЖВ");
                                        ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                    }

                                    // Зміна X-ів та Y-ів після виконання МЖВ
                                    Variable tempLeft = left[i];
                                    Variable tempUp = up[xIndex - 1];
                                    left[i] = tempUp;
                                    up[xIndex - 1] = tempLeft;

                                    bool delSuccess = false;
                                    for (int m = 0; m < colN; m++)
                                    {
                                        if (delRows[m].GetIndex() < 0)
                                        {
                                            ElRow[] tempFormula = new ElRow[colN + 1];
                                            for (int k = 0; k < colN + 1; k++)
                                            {
                                                char xy;
                                                if (k < up.Length)
                                                {
                                                    if (up[k].type == Variable.VariableType.X)
                                                    {
                                                        xy = 'X';
                                                    }
                                                    else if (up[k].type == Variable.VariableType.Y)
                                                    {
                                                        xy = 'Y';
                                                    }
                                                    else
                                                    {
                                                        xy = '0';
                                                    }
                                                    tempFormula[k] = new ElRow(-matrixMain[i, k], xy, up[k].index);
                                                }
                                                else
                                                {
                                                    xy = '1';
                                                    tempFormula[k] = new ElRow(matrixMain[i, k], xy, 0);
                                                }


                                                delRows[m] = new Row(xIndex, tempFormula);
                                            }

                                            (rowN, matrixMain) = DeleteRow(matrixMain, rowN, colN, i);
                                            if (protocol)
                                            {
                                                Console.WriteLine("Матриця після видалення рядка:");
                                                ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                            }
                                            Variable[] tempL = new Variable[rowN];
                                            for (int l = 0; l < rowN; l++)
                                            {
                                                if (l < i)
                                                {
                                                    tempL[l] = left[l];

                                                }
                                                else if (l >= i)
                                                {
                                                    tempL[l] = left[l + 1];
                                                }
                                            }
                                            left = tempL;

                                            delSuccess = true;
                                            break;
                                        }

                                    }
                                    if (delSuccess)
                                        break;
                                }
                            }
                            if (allZero)
                            {
                                Console.WriteLine("\nНеможливо видалити вільну змінну X" + xIndex + ": у відповідному стовпчику відсутні ненульові елементи.");
                                break;
                            }
                        }
                    }

                    if (allZero)
                    {
                        break;
                    }
                    if (free)
                    {
                        Console.WriteLine("----------------------------------------\nМатриця після видалення вільних змінних:");
                        ShowMatrix(matrixMain, rowN + 1, colN + 1);
                    }

                    bool stepResult = true;
                    bool positive = true;

                    if (protocol && eq)
                        Console.WriteLine("\n----------Видалення нульових рядків----------\n");
                    for (int i = 0; i < rowN; i++)
                    {
                        if (left[i].type == 0)
                        {
                            positive = false;
                            for (int j = 0; j < colN; j++)
                            {
                                if (matrixMain[i, j] > 0)
                                {
                                    int processedRow;
                                    (stepResult, processedRow) = SolutionStepBCD(matrixMain, rowN, colN, i, j);
                                    if (!stepResult)
                                        break;
                                    if (protocol)
                                    {
                                        Console.WriteLine("Актуальна симплекс-таблиця:");
                                        ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                        Console.WriteLine("Розв'язувальний елемент: " + matrixMain[processedRow, j] + "(рядок " + (processedRow + 1) + ", стовпчик " + (j + 1) + ")");
                                    }

                                    matrixMain = StepMZHV(matrixMain, rowN + 1, colN + 1, processedRow, j);

                                    if (protocol)
                                    {
                                        Console.WriteLine("\nМатриця після  кроку МЖВ:");
                                        ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                    }

                                    Variable tempLeft = left[processedRow];
                                    Variable tempUp = up[j];
                                    left[processedRow] = tempUp;
                                    up[j] = tempLeft;

                                    // Зміна x-ів та y-ів після виконання МЖВ
                                    if (up[j].type == 0)
                                    {
                                        (colN, matrixMain) = DeleteColumn(matrixMain, rowN + 1, colN + 1, j);
                                        for (int u = 0; u < colN; u++)
                                        {
                                            if (up[u].type == 0)
                                            {
                                                Variable[] tempU = new Variable[colN];
                                                for (int k = 0; k < colN + 1; k++)
                                                {
                                                    if (k < u)
                                                    {
                                                        tempU[k] = up[k];
                                                    }
                                                    else if (k > u)
                                                    {
                                                        tempU[k - 1] = up[k];
                                                    }
                                                }
                                                up = tempU;
                                                break;
                                            }
                                        }
                                        if (protocol)
                                        {
                                            Console.WriteLine("Матриця після видалення стовпчика:");
                                            ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                        }
                                    }
                                    positive = true;
                                    i = -1;
                                    break;
                                }
                            }
                            if (!positive)
                            {
                                break;
                            }
                            if (!stepResult)
                            {
                                break;
                            }
                        }
                    }
                    if (!positive)
                    {
                        Console.WriteLine("\nНеможливо видалити нульовий рядок: у нульовому рядку вістутні додатні елементи.");
                        break;
                    }
                    if (!stepResult)
                    {
                        Console.WriteLine("\nНеможливо видалити нульовий рядок: відсутні додатні добутки при знаходженні найменьшого невід'ємного добутку елементів результуючого та одинарного стовпчиків.");
                        break;
                    }
                    if (eq)
                    {
                        Console.WriteLine("----------------------------------------\nМатриця після видалення всіх нульових рядків: ");
                        ShowMatrix(matrixMain, rowN + 1, colN + 1);
                    }
                    bool limitResult = true;
                    bool firstOrProtocol = true;
                    bool hasNegative = true;

                    do
                    {
                        if (protocol)
                        {
                            firstOrProtocol = true;
                            Console.WriteLine("----------Пошук опорного розв'язку----------\n");
                        }

                        for (int i = 0; i < rowN; i++)
                        {
                            if (matrixMain[i, colN] < 0)
                            {
                                hasNegative = false;
                                for (int j = 0; j < colN; j++)
                                {
                                    if (matrixMain[i, j] < 0)
                                    {
                                        int processedRow;

                                        // Усунення мінусів в одинарному стовпчику за допомогою МЖВ для пошуку опорного розв'язку
                                        (stepResult, processedRow) = SolutionStepBCD(matrixMain, rowN, colN, i, j);
                                        hasNegative = true;
                                        if (!stepResult)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            if (protocol)
                                            {
                                                Console.WriteLine("Актуальна симплекс-таблиця:");
                                                ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                                Console.WriteLine("Розв'язувальний елемент: " + matrixMain[processedRow, j] + "(рядок " + (processedRow + 1) + ", стовпчик " + (j + 1) + ")");
                                            }

                                            matrixMain = StepMZHV(matrixMain, rowN + 1, colN + 1, processedRow, j);

                                            if (protocol)
                                            {
                                                Console.WriteLine("\nМатриця після  кроку МЖВ:");
                                                ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                            }
                                            Variable tempLeft = left[i];
                                            Variable tempUp = up[j];
                                            left[i] = tempUp;
                                            up[j] = tempLeft;


                                        }

                                        break;
                                    }
                                    if (!stepResult)
                                    {
                                        break;
                                    }
                                    if (!hasNegative)
                                    {
                                        break;
                                    }
                                }

                                if (!stepResult)
                                {
                                    break;
                                }
                                if (!hasNegative)
                                {
                                    break;
                                }
                                i = -1;
                            }
                        }
                        if (!stepResult)
                        {
                            Console.WriteLine("\nОпорний розв'язок не знайдено: відсутні додатні добутки при знаходженні найменьшого невід'ємного добутку елементів результуючого та одинарного стовпчиків.");
                            break;
                        }

                        if (!hasNegative)
                        {
                            Console.WriteLine("\nНемає розв'язків: в одинарному стовпчику є від'ємні елементи при відсутніх від'ємних елементах у відповідних рядках матриці.");
                            break;
                        }

                        if (firstOrProtocol)
                        {
                            Console.WriteLine("----------------------------------------\nМатриця після пошуку опорного розв'язку: ");
                            ShowMatrix(matrixMain, rowN + 1, colN + 1);

                            Console.WriteLine("Опорний розв'язок:");
                            if (!free)
                                ShowSolutionsCD(matrixMain, left, arrayX.Length, colN);
                            else
                            {
                                double[] freeXValues = new double[colN];
                                freeXValues = CalcSolutionsFreeCD(matrixMain, colN, left, arrayX.Length, delRows);
                                for (int i = 0; i < freeXValues.Length; i++)
                                {
                                    Console.Write("X" + (i + 1) + " = " + freeXValues[i]);
                                    if (i < freeXValues.Length - 1)
                                    {
                                        Console.WriteLine("; ");
                                    }
                                    else
                                        Console.WriteLine();
                                }
                            }
                        }

                        if (protocol)
                            Console.WriteLine("\n----------Пошук оптимального розв'язку----------\n");

                        for (int j = 0; j < colN; j++)
                        {
                            if (matrixMain[rowN, j] < 0)
                            {
                                int processedRow;
                                (stepResult, processedRow) = SolutionStepBCD(matrixMain, rowN, colN, rowN, j);

                                hasNegative = true;
                                if (!stepResult)
                                {
                                    break;
                                }
                                else
                                {
                                    if (protocol)
                                    {
                                        Console.WriteLine("Актуальна симплекс-таблиця:");
                                        ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                        Console.WriteLine("Розв'язувальний елемент: " + matrixMain[processedRow, j] + "(рядок " + (processedRow + 1) + ", стовпчик " + (j + 1) + ")");
                                    }
                                    matrixMain = StepMZHV(matrixMain, rowN + 1, colN + 1, processedRow, j);
                                    if (protocol)
                                    {
                                        Console.WriteLine("\nМатриця після  кроку МЖВ:");
                                        ShowMatrix(matrixMain, rowN + 1, colN + 1);
                                    }

                                    Variable tempLeft = left[processedRow];
                                    Variable tempUp = up[j];
                                    left[processedRow] = tempUp;
                                    up[j] = tempLeft;
                                }
                                j = -1;
                            }
                        }
                        if (!stepResult)
                        {
                            Console.WriteLine("\nОптимальний розв'язок не знайдено: відсутні додатні добутки при знаходженні найменьшого невід'ємного добутку елементів результуючого та одинарного стовпчиків.");
                            break;
                        }

                        if (firstOrProtocol)
                        {
                            Console.WriteLine("----------------------------------------\nМатриця після пошуку оптимального розв'язку: ");
                            ShowMatrix(matrixMain, rowN + 1, colN + 1);
                            Console.WriteLine("Оптимальний розв'язок:");
                            if (!free)
                                ShowSolutionsCD(matrixMain, left, arrayX.Length, colN);
                            else
                            {
                                double[] freeXValues = new double[colN];
                                freeXValues = CalcSolutionsFreeCD(matrixMain, colN, left, arrayX.Length, delRows);
                                for (int i = 0; i < freeXValues.Length; i++)
                                {
                                    Console.Write("X" + (i + 1) + " = " + freeXValues[i]);
                                    if (i < freeXValues.Length - 1)
                                    {
                                        Console.WriteLine("; ");
                                    }
                                    else
                                        Console.WriteLine();
                                }
                            }


                            Console.WriteLine();
                            if (func == "2")
                            {
                                Console.WriteLine("min значення цільової функції Z: " + (-1 * Math.Round(matrixMain[rowN, colN], 3)) + "\n");
                            }
                            else
                                Console.WriteLine("max значення цільової функції Z: " + (Math.Round(matrixMain[rowN, colN], 3)) + "\n");
                            if (method == "3")
                                break;
                        }
                        firstOrProtocol = false;

                        // ------------Знаходження цілочисельних розв'язків-----------------

                        int fractXNumb;
                        int rowFractIndex;
                        (fractXNumb, rowFractIndex) = selectfractional(matrixMain, colN, left); // вибір змінної з дробовим розв'язком та її рядка

                        if (rowFractIndex != -1)
                        {
                            limitResult = false;
                            if (protocol)
                            {
                                Console.WriteLine("----------Пошук цілочисельних рішень----------\n" +
                                    "\nМaтриця містить дробну змінну: X"+fractXNumb+" (у рядку номер "+ rowFractIndex+")\n");
                            }
                            string currentLimit;
                            (matrixMain, left, currentLimit, limitResult) = BuildMatrixWithFractLimiter(matrixMain, rowN, colN, left, up, rowFractIndex);

                            if(limitResult)
                            rowN++;
                            else
                            {
                                Console.WriteLine("\nНеможливо створити обмеження для дробової змінної X" + fractXNumb + ": у відповідному рядку всі елементи - нульові.");
                                break;
                            }

                            if (protocol)
                            {
                                Console.WriteLine("Для матриці було створено додаткове обмеження:\n" + currentLimit + "\n\nОновлена матриця:");
                                ShowMatrix(matrixMain, rowN + 1, colN + 1);
                            }
                        }

                        else break;
                    } while (true);

                    if (!stepResult)
                    {
                        break;
                    }
                    if (!hasNegative)
                    {
                        break;
                    }

                    if (method == "3")
                        break;

                    if(!limitResult)
                        break;

                    Console.WriteLine("----------------------------------------\nМатриця після пошуку цілочисельних розв'язків:");
                    ShowMatrix(matrixMain, rowN + 1, colN + 1);
                    Console.WriteLine("Знайдений цілочисельний розв'язок:");
                    ShowSolutionsCD(matrixMain, left, arrayX.Length, colN);

                    Console.WriteLine();
                    if (func == "2")
                    {
                        Console.WriteLine("min значення цільової функції Z: " + (-1 * Math.Round(matrixMain[rowN, colN], 3)) + "\n");
                    }
                    else
                        Console.WriteLine("max значення цільової функції Z: " + (Math.Round(matrixMain[rowN, colN], 3)) + "\n");

                    break;
                    /*  // ------------Двійчасті-----------------

                   // Додання других рівнів для верхнього та лівого рівнів розположення змінних
                   Variable[] arrayV = new Variable[colN];
                   Variable[] arrayU = new Variable[rowN];

                   Variable[] left2 = new Variable[rowN];
                   Variable[] up2 = new Variable[colN];

                   for (int i = 0; i < colN; i++)
                   {
                       arrayV[i] = new Variable(i + 1, 'X');
                       up2[i] = arrayV[i];

                   }
                   for (int i = 0; i < rowN; i++)
                   {
                       arrayU[i] = new Variable(i + 1, 'Y');
                       left2[i] = arrayY[i];
                   }*/
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
                    outputMatrix[i, j] = outputMatrix[i, j] / ars; // Ділимо всі елементи на розв'язувальний елемент
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

    (bool, int) SolutionStepBCD(double[,] matrixMain, int rowN, int colN, int r, int c)
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
                if (current == leastPositive) // У випадку, якщо значення співпадають, обиратися буде, по можливості, елемент у розв'язувальному рядку
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

    (int, double[,]) DeleteColumn(double[,] matrix, int rowN, int colN, int colDel)
    {
        colN--;
        double[,] matrixTemp = new double[rowN, colN];

        for (int r = 0; r < rowN; r++)
        {
            for (int c = 0; c < colN + 1; c++)
            {
                if (c != colDel)
                {
                    if (c < colDel)
                    {
                        matrixTemp[r, c] = matrix[r, c];
                    }
                    else
                    {
                        matrixTemp[r, c - 1] = matrix[r, c];
                    }
                }
            }
        }
        colN--;
        return (colN, matrixTemp);
    }

    void ShowSolutionsB(double[,] matrix, int[] matrixLeft, int xN, int colN)
    {
        for (int i = 1; i <= xN; i++)
        {
            double solution = 0;
            if (matrixLeft.Contains(i))
            {
                int index = Array.IndexOf(matrixLeft, i);
                solution = matrix[index, colN];
                // Console.Write("X" + i + " = " + solution);
                Console.Write("X" + i + " = " + Math.Round(solution, 3));

            }
            else
            {
                Console.Write("X" + i + " = 0");
            }
            if (i < xN)
            {
                Console.Write("; ");
            }
            else
            {
                Console.WriteLine(".");
            }
        }
    }

    (int, double[,]) DeleteRow(double[,] matrix, int rowN, int colN, int rowDel)
    {
        double[,] matrixTemp = new double[rowN, colN + 1];

        for (int r = 0; r < rowN + 1; r++)
        {
            if (r < rowDel)
            {
                for (int c = 0; c < colN + 1; c++)
                {
                    matrixTemp[r, c] = matrix[r, c];
                }
            }
            else if (r > rowDel)
            {
                for (int c = 0; c < colN + 1; c++)
                {
                    matrixTemp[r - 1, c] = matrix[r, c];
                }
            }
        }
        rowN--;
        return (rowN, matrixTemp);
    }

    void ShowSolutionsCD(double[,] matrix, Variable[] matrixLeft, int xN, int colN)
    {
        for (int i = 1; i <= xN; i++)
        {
            bool found = false;
            double solution = 0;
            for (int l = 0; l < matrixLeft.Length; l++)
            {
                if (matrixLeft[l].type == Variable.VariableType.X && matrixLeft[l].index == i)
                {
                    solution = matrix[l, colN];
                    Console.Write("X" + i + " = " + Math.Round(solution, 3));
                    found = true;
                    break;
                }
            }
            if (!found)
                Console.Write("X" + i + " = 0");

            if (i < xN)
            {
                Console.Write("; ");
            }
            else
            {
                Console.WriteLine(".");
            }


        }
    }

    double[] CalcSolutionsFreeCD(double[,] matrixMain, int colN, Variable[] matrixLeft, int xN, Row[] delRows)
    {
        double[] solutionsX = new double[xN];

        Row[] delRowsCopy = delRows;

        for (int r = delRowsCopy.Length - 1; r >= 0; r--) // йдемо з кінця, щоб при знаходженні формули для вільного x-у, всі x-и та y-и, які входять до формули, вже мали свої значення
        {
            double solution = 0;
            if (delRowsCopy[r] != null)
            {
                ElRow[] currentFormula = delRowsCopy[r].GetFormula();
                for (int f = 0; f < currentFormula.Length; f++)
                {
                    double currentSum = 0;
                    // Якщо елемент формули - y, то знаходимо його значення в основній матриці за індексом та множимо на модифікатор
                    if (currentFormula[f].GetXY() == 'Y')
                    {
                        for (int l = 0; l < matrixLeft.Length; l++)
                        {
                            if (matrixLeft[l].index == currentFormula[f].GetIndex())
                            {
                                currentSum = matrixMain[l, colN] * currentFormula[f].GetMod();
                                break;
                            }
                        }
                        solution += currentSum;
                    }
                    //  Якщо елемент формули - x, то знаходимо його значення серед вже обчислених вільних x-ів та множимо на модифікатор
                    else if (currentFormula[f].GetXY() == 'X')
                    {
                        for (int fr = delRowsCopy.Length - 1; fr >= 0; fr--)
                        {
                            if (delRowsCopy[fr].GetIndex() == currentFormula[f].GetIndex())
                            {
                                if (delRowsCopy[fr].GetValue() != double.MinValue)
                                {
                                    currentSum = delRowsCopy[fr].GetValue() * currentFormula[f].GetMod();
                                }
                            }
                        }
                        solution += currentSum;

                    }
                    // Якщо елемент формули - число, то просто додаємо його до розв'язку, так як воно взяте з одинарного рядка
                    else
                    {
                        solution += currentFormula[f].GetMod();
                    }
                }
            }
            // додання всіх елементів формули
            solutionsX[delRowsCopy[r].GetIndex() - 1] = solution;
            delRowsCopy[r].ChangeValue(solutionsX[delRowsCopy[r].GetIndex() - 1]);
        }
        return solutionsX;
    }

    (int, int) selectfractional(double[,] matrix, int colN, Variable[] left)
    {
        double maxFractional = -1;
        int xNumb = -2;
        int row = -1;
        for (int i = 0; i < left.Length; i++)
        {
            if (left[i].type == Variable.VariableType.X)
            {
                double currentFractional = Math.Round(matrix[i, colN], 6) - Math.Floor(Math.Round(matrix[i, colN], 6));
                if (currentFractional > 0 && currentFractional > maxFractional)
                {
                    maxFractional = currentFractional;
                    xNumb = left[i].index;
                    row = i;
                }
            }
        }
        return ((xNumb), row);
    }

    (double[,], Variable[], string, bool) BuildMatrixWithFractLimiter(double[,] matrix, int rowN, int colN, Variable[] left, Variable[] up, int processedRow)
    {
        bool result = false;
        double[,] matrixCopy = new double[rowN + 2, colN + 1];
        int n;
        Variable[] leftCopy = new Variable[left.Length + 1];
        if (left[left.Length - 1].type == Variable.VariableType.S)
            n = left[left.Length - 1].index + 1;
        else
            n = 1;

        for (int i = 0; i < leftCopy.Length; i++)
        {
            if (i < left.Length)
            {
                leftCopy[i] = left[i];
            }
            else
            {
                Variable temp = new Variable(n, 'S');
                leftCopy[i] = temp;
            }
        }

        string limit = "S" + (n) + " = ";


        for (int r = 0; r < rowN + 1; r++)
        {
            for (int c = 0; c < colN + 1; c++)
            {
                if (r < rowN)
                    matrixCopy[r, c] = Math.Round(matrix[r, c], 3);
                else
                {
                    matrixCopy[r + 1, c] = Math.Round(matrix[r, c], 3);

                    double fractionalPart = matrix[processedRow, c] - Math.Floor(matrix[processedRow, c]);
                    if(fractionalPart!=0 && c < colN - 1)
                        result = true;

                    matrixCopy[r, c] = -fractionalPart;
                    limit += (-Math.Round(fractionalPart, 3));
                    if (c < colN - 1)
                    {
                        if (-fractionalPart > 0)
                            limit += "+";
                    }

                }
            }
        }
        return (matrixCopy, leftCopy, limit, result);
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
