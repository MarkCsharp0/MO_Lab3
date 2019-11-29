using System;
using System.Collections.Generic;

namespace Lab3
{

    class Program
    {

        //Вычисление градиента численным  методом
        public static List<decimal> Gradient(func f, List<decimal> x, decimal h)
        {
            var first = new decimal[x.Count];
            var second = new decimal[x.Count];
            // decimal h = 0.0001m;
            var grad = new List<decimal>();
            for (int i = 0; i < x.Count; i++)
            {
                x.CopyTo(first);
                x.CopyTo(second);
                first[i] += h;
                second[i] -= h;
                grad.Add((f(first) - f(second)) / (2 * h));
            }
            return grad;
        }

        // Длина градиента
        public static decimal GradientLength(List<decimal> x)
        {

            decimal len = 0;
            for (int i = 0; i < x.Count; i++)
            {
                len += (x[i] * x[i]);
            }
            len = (decimal)Math.Sqrt((double)len);
            return len;

        }

        // Вычисляем матрицу алгебраических дополнений
        public static decimal[,] Allied(decimal[,] matrix, int size)
        {
            var newMatrix = new decimal[size, size];
            int i, j;
            for (i = 0; i < size; i++)
            {
                for (j = 0; j < size; j++)
                {
                    newMatrix[i, j] = ((i + j) % 2 != 0 ? -1 : 1) * Determinant(PreMinor(matrix, i, j, size), size - 1);
                }
            }
            return newMatrix;

        }

        // Возвращает матрицу size - 1 порядка, из которой вычеркнули строку с индексом row и столбец с индексом col
        public static decimal[,] PreMinor(decimal[,] matrix, int row, int col, int size)
        {

            var newMatrix = new decimal[size - 1, size - 1];
            int i, j, iN, jn;
            for (i = 0, iN = 0; i < size; i++)
            {
                if (i != row)
                {
                    for (j = 0, jn = 0; j < size; j++)
                    {
                        if (j != col)
                        {
                            if (jn >= (size - 1))
                            {
                                continue;
                            }
                            else
                            {
                                newMatrix[iN, jn] = matrix[i, j];
                                jn++;
                            }
                        }
                    }
                    iN++;
                }
            }
            return newMatrix;
        }
        // Вычисляет определитель матрицы
        public static decimal Determinant(decimal[,] matrix, int size)
        {
            if (size == 1)
            {
                return matrix[0, 0];
            }

            decimal determ = 0;
            for (int i = 0; i < size; i++)
            {
                decimal el = (i % 2 == 0) ? matrix[0, i] : -matrix[0, i];
                determ += el * Determinant(PreMinor(matrix, 0, i, size), size - 1);
            }
            return determ;
        }

        // Вычисляет и возвращает транспонированную матрицу
        public static decimal[,] Transpose(decimal[,] matrix, int size)
        {
            var newMatrix = (decimal[,])matrix.Clone();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    newMatrix[j, i] = matrix[i, j];
                }
            }
            return newMatrix;
        }

        // Умножает матрицу на число
        public static decimal[,] Multiply(decimal numb, decimal[,] matrix, int size)
        {
            var newMatrix = (decimal[,])matrix.Clone();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    newMatrix[i, j] *= numb;
                }
            }
            return newMatrix;
        }

        // Численно вычисляет матрицу Гессе(матрицу двойных производных)
        public static decimal[,] ComputeHessian(func f, List<decimal> x, decimal step)
        {
            var matrix = new decimal[x.Count, x.Count];
            var varCount = x.Count;
            var fPlus = new decimal[varCount];
            var args = new decimal[varCount];
            x.CopyTo(args);
            var curFValue = f(args); //значение функции в текущей точке 
            for (int i = 0; i < varCount; i++)
            {

                x.CopyTo(args);
                args[i] += step;
                fPlus[i] = f(args);
            }

            for (int i = 0; i < varCount; i++)
                for (int j = 0; j < varCount; j++)
                {
                    x.CopyTo(args);
                    args[i] += step;
                    args[j] += step;
                    decimal value = f(args);
                    matrix[i, j] = (value - fPlus[i] - fPlus[j] + curFValue) / (step * step);
                }


            return matrix;

        }

        // Вычисляем матрицу Гессе аналитическим методом
        public static decimal[,] GetHessianMatrix(List<decimal> x)
        {
            var array = new decimal[x.Count];
            x.CopyTo(array);
            var matrix = new decimal[,] { {FuncDerivative1(array), FuncDerivative3(array) },
                {FuncDerivative3(array), FuncDerivative2(array) } };
            return matrix;

        }
        
        public static decimal Func1(decimal[] x)
        {
            return (x[0] * x[0] + x[1] - 11) * (x[0] * x[0] + x[1] - 11) + (x[0] + x[1] * x[1] - 7) * (x[0] + x[1] * x[1] - 7);
        }

        public static decimal Func2(decimal[] x)
        {
            return (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]) + 100 * (1 - x[0]) * (1 - x[0]);
        }
        public static decimal Func6(decimal[] x)
        {
            return 100 * (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]) + (1 - x[0]) * (1 - x[0]);
        }
        /* public static decimal func2(decimal[] x)
         {
             return x[0] * x[0] * ((decimal)Math.Sqrt((double)x[1]));
         }

         public static decimal func3(decimal[] x)
         {
             return 2 * x[0] * x[0] * x[0] - 2 * x[0] * x[1] + x[1] * x[1];
         }
         */
        // Производная по x
        public static decimal FuncDerivative4(decimal[] x)
        {
            return 4 * x[0] * (x[0] * x[0] + x[1] - 11) + 2 * x[0] + 2 * x[1] * x[1] - 14;

        }
        // Производная по y
        public static decimal FuncDerivative5(decimal[] x)
        {
            return 2 * x[0] * x[0] + 4 * x[1] * (x[0] + x[1] * x[1] - 7) + 2 * x[1] - 22;

        }

        // Двойная производная по x
        public static decimal FuncDerivative1(decimal[] x)
        {
            return 12 * x[0] * x[0] + 4 * x[1] - 42;
        }
        // Двойная производная по y
        public static decimal FuncDerivative2(decimal[] x)
        {
            return 4 * x[0] + 12 * x[1] * x[1] - 26;
        }

        // Смешанная производная
        public static decimal FuncDerivative3(decimal[] x)
        {
            return 4 * x[0] + 4 * x[1];
        }

        public delegate decimal func(decimal[] x);

        // Вычисляем вектор шага для метода Ньютона(произведение определителя обратной матрицы Гессе на градиент)
        public static List<decimal> GetStep(List<decimal> gradient, decimal[,] matrix)
        {
            var determ = FindDeterminantOfInverseMatrix(matrix, gradient.Count);
            var step = new List<decimal>(gradient.ToArray());
            for (int i = 0; i < step.Count; i++)
            {
                step[i] *= determ;
            }
            return step;
        }

        // На вход получаем матрицу Гессе, находим для нее обратную матрицу и её определитель
        public static decimal FindDeterminantOfInverseMatrix(decimal[,] matrix, int size)
        {
            var determ = Determinant(matrix, size);
            var alMatrix = Allied(matrix, size);
            var InverseMatrix = Multiply(1 / determ, alMatrix, size);
            var result = Determinant(InverseMatrix, size);
            return result;

        }


        public static void Main(string[] args)
        {
            var x_k = new List<decimal>();
            func f = Func1;
            var type = "Численный";
            var eps = 0.1m;
            x_k.Add(100.0m);
            x_k.Add(100.0m);
            var start = new List<decimal>(x_k.ToArray());
            decimal len_k;
            var cnt = 0;
            do
            {

                /*  if (x_k1.Count != 0)
                  {
                      x_k = new List<decimal>(x_k1.ToArray());
                  }*/
                List<decimal> gradient;
                decimal[,] matrix;
                if (type.Equals("Численный"))
                {
                    gradient = Gradient(f, x_k, eps);
                    matrix = ComputeHessian(f, x_k, eps);
                } else
                {
                    gradient = new List<decimal>();
                    gradient.Add(FuncDerivative4(x_k.ToArray()));
                    gradient.Add(FuncDerivative5(x_k.ToArray()));
                    matrix = GetHessianMatrix(x_k);
                }

                len_k = GradientLength(gradient);
                var step = GetStep(gradient, matrix);
                for (int i = 0; i < gradient.Count; i++)
                {
                    x_k[i] -= step[i];
                }
                
                cnt++;
                Console.WriteLine(len_k);
            } while (Math.Abs(len_k) > eps);

            var ans = new decimal[x_k.Count];
            x_k.CopyTo(ans);
            Console.WriteLine("Точка старта:" + start[0] + " " + start[1]);
            Console.WriteLine("Точность: " + eps);
            Console.WriteLine("Количество итераций: " + cnt);
            Console.WriteLine("Способ вычисления производной: " + type);
            Console.WriteLine("Итоговый модуль градиента: " + len_k);
            Console.WriteLine("Найденная точка минимума: " + x_k[0] + " " + x_k[1]);
            Console.WriteLine("Значение минимума: " + f(ans));
        }
    }
}