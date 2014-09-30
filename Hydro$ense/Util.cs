using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    static class Util
    {
        public static double CalculateCost(double[] quantities, double[] costs, double quantity)
        {
            if (quantity < 0)
                return 0.0;

            int i = 0;
            while (quantities[i] < quantity && i < quantities.Length - 1)
            {
                i++;
            }

            /* if first value in quantities is greater than quantity, use the
             * first two quantities and costs. ASSUMES at least two values. */
            if (i == 0)
                i++;

            /* don't divide by zero */
            if (quantities[i] == 0 && quantities[i - 1] == 0)
                return 0.0;

            double slope = (costs[i] - costs[i - 1]) / (quantities[i] - quantities[i - 1]);
            return costs[i - 1] + (quantity - quantities[i - 1]) * slope;
        }

        public static void WriteMatrixToLog(StringBuilder log, double[][] quant)
        {
            for (int i = 0; i < quant.Length; i++)
            {
                log.Append("[");
                for (int j = 0; j < quant[i].Length; j++)
                {
                    log.Append(quant[i][j]);
                    if (j < quant[i].Length - 1)
                        log.Append(", ");
                }
                log.Append("]");
                log.AppendLine();
            }
        }

        public static void WriteArrayToLog(StringBuilder log, double[][] quant)
        {
            log.Append("[");
            for (int i = 0; i < quant.Length; i++)
            {
                for (int j = 0; j < quant[i].Length; j++)
                {
                    log.Append(quant[i][j]);
                    if (i != quant.Length - 1 || j != quant[i].Length - 1)
                        log.Append(", ");
                }
            }
            log.AppendLine("]");
        }

        /* taken from here:
         * http://stackoverflow.com/questions/4670720/extremely-fast-way-to-clone-the-values-of-a-jagged-array-into-a-second-array
         */
        public static double[][] CopyArray(double[][] source)
        {
            double[][] rval = new double[source.Length][];

            for (int i = 0; i < source.Length; i++)
            {
                double[] row = new double[source[i].Length];

                Array.Copy(source[i], row, source[i].Length);
                rval[i] = row;
            }

            return rval;

        }

        public static double SumColumn(double[][] Q, int col)
        {
            double rval = 0.0;
            for (int i = 0; i < Q.Length; i++)
            {
                rval += Q[i][col];
            }
            return rval;
        }

        public static double SumRow(double[][] Q, int row)
        {
            double rval = 0.0;
            for (int i = 0; i < Q[row].Length; i++)
            {
                rval += Q[row][i];
            }
            return rval;
        }

        public static void InitializeArrayToZero(double[][] arr, int numRows, int numColumns)
        {
            for (int i = 0; i < numRows; i++)
            {
                arr[i] = new double[numColumns];
                for (int j = 0; j < numColumns; j++)
                {
                    arr[i][j] = 0.0;
                }
            }
        }

        public static void WriteResultsToLog(StringBuilder log, double OF, double[][] quantS, double[][] quantD, int iter, double delta)
        {
            log.AppendLine(string.Format("k = {0}, Delta = {1}, OF = {2}", iter, delta, OF));
            log.Append("Supply = ");
            Util.WriteArrayToLog(log, quantS);
            log.Append("Demand = ");
            Util.WriteArrayToLog(log, quantD);
        }

        public static T[] ToFlat<T>(this T[][] jagged)
        {
            return jagged.SelectMany(innerInner => innerInner).ToArray();
        }

        public static T[] ToFlat<T>(this T[][][] jagged)
        {
            return jagged.SelectMany(inner => inner.SelectMany(innerInner => innerInner)).ToArray();
        }

        /* taken from here:
         * http://www.rkinteractive.com/blogs/SoftwareDevelopment/post/2013/05/07/Algorithms-In-C-LUP-Decomposition.aspx
         * 
         * Perform LUP decomposition on a matrix A.
         * We implement the code to compute LU "in place" in the matrix A.
         * In order to make some of the calculations more straight forward and to 
         * match Cormen's et al. pseudocode the matrix A should have its first row 
         * and first columns to be all 0.
         * 
         * Reference: Thomas H. Cormen, Charles E. Leiserson, Ronald L. Rivest, and Clifford Stein. 
         * Introduction To Algorithms Third Edition. The MIT Press, 2009. */
        public static int[] LUPDecomposition(ref double[][] A)
        {
            int n = A.Length - 1;

            /* pi represents the permutation matrix.  We implement it as an array
             * whose value indicates which column the 1 would appear.  We use it to avoid 
             * dividing by zero or small numbers. */
            int[] pi = new int[n + 1];
            double p = 0;
            int kp = 0;
            int pik = 0;
            int pikp = 0;
            double aki = 0;
            double akpi = 0;

            /* Initialize the permutation matrix, will be the identity matrix */
            for (int j = 0; j <= n; j++)
            {
                pi[j] = j;
            }

            for (int k = 0; k <= n; k++)
            {
                /* In finding the permutation matrix p that avoids dividing by zero
                 * we take a slightly different approach.  For numerical stability
                 * We find the element with the largest 
                 * absolute value of those in the current first column (column k).  If all elements in
                 * the current first column are zero then the matrix is singluar and throw an
                 * error. */
                p = 0;
                for (int i = k; i <= n; i++)
                {
                    if (Math.Abs(A[i][k]) > p)
                    {
                        p = Math.Abs(A[i][k]);
                        kp = i;
                    }
                }
                if (p == 0)
                {
                    throw new Exception("singular matrix");
                }

                /* These lines update the pivot array (which represents the pivot matrix)
                 * by exchanging pi[k] and pi[kp]. */
                pik = pi[k];
                pikp = pi[kp];
                pi[k] = pikp;
                pi[kp] = pik;

                /* Exchange rows k and kpi as determined by the pivot */
                for (int i = 0; i <= n; i++)
                {
                    aki = A[k][i];
                    akpi = A[kp][i];
                    A[k][i] = akpi;
                    A[kp][i] = aki;
                }

                /* Compute the Schur complement */
                for (int i = k + 1; i <= n; i++)
                {
                    A[i][k] = A[i][k] / A[k][k];
                    for (int j = k + 1; j <= n; j++)
                    {
                        A[i][j] = A[i][j] - (A[i][k] * A[k][j]);
                    }
                }
            }
            return pi;
        }

        /* taken from here:
         * http://www.rkinteractive.com/blogs/SoftwareDevelopment/post/2013/05/14/Algorithms-In-C-Solving-A-System-Of-Linear-Equations.aspx
         * 
         * Given A and b solve for x using LUP Decomposition.
         * "A" will become LU, a n+1xm+1 matrix where the first row and columns are zero.
         * This is for ease of computation and consistency with Cormen et al.
         * pseudocode.
         * The pi array represents the permutation matrix.
         * 
         * Reference: Thomas H. Cormen, Charles E. Leiserson, Ronald L. Rivest, and Clifford Stein. 
         * Introduction To Algorithms Third Edition. The MIT Press, 2009. */
        public static double[] LUPSolve(ref double[][] A, double[] b)
        {
            int[] pi = LUPDecomposition(ref A);

            int n = A.Length - 1;
            double[] x = new double[n + 1];
            double[] y = new double[n + 1];
            double suml = 0;
            double sumu = 0;
            double lij = 0;

            /* Solve for y using formward substitution */
            for (int i = 0; i <= n; i++)
            {
                suml = 0;
                for (int j = 0; j <= i - 1; j++)
                {
                    /* Since we've taken L and U as a singular matrix as an input
                     * the value for L at index i and j will be 1 when i equals j, not LU[i][j], since
                     * the diagonal values are all 1 for L. */
                    if (i == j)
                    {
                        lij = 1;
                    }
                    else
                    {
                        lij = A[i][j];
                    }
                    suml = suml + (lij * y[j]);
                }
                y[i] = b[pi[i]] - suml;
            }

            /* Solve for x by using back substitution */
            for (int i = n; i >= 0; i--)
            {
                sumu = 0;
                for (int j = i + 1; j <= n; j++)
                {
                    sumu = sumu + (A[i][j] * x[j]);
                }
                x[i] = (y[i] - sumu) / A[i][i];
            }
            return x;
        }

        [Obsolete("Not quite working correctly, use LUPSolve instead.", true)]
        private static double[] LinearEquationSolver(int N, double[][] C, double[] y)
        {
            /* N is the number of demand and supply nodes
             * y contains size values, and is the right hand vector
             * for the linear system of equations [C]{x} = <y> */
            double[] rval = new double[N];
            double[] ys = new double[N];
            int k = 0;
            int MaxIt = 100;
            double maxdel = 1.0;
            double DelLimit = 0.0000001;
            
            /* Solve the system using an iterative approach by computing
             * each x value as a 2x2 matrix solution */
            while (k < MaxIt && maxdel > DelLimit)
            {
                maxdel = 0.0;
                for (int i = 0; i < N - 1; i++)
                {
                    double sum0 = y[i];
                    double sum1 = y[i + 1];
                    for (int j = 0; j < i; j++)
                    {
                        sum0 -= C[i][j] * rval[j];
                        sum1 -= C[i + 1][j] * rval[j];
                    }
                    for (int j = i + 2; j < N; j++)
                    {
                        sum0 -= C[i][j] * rval[j];
                        sum1 -= C[i + 1][j] * rval[j];
                    }

                    double num = C[i + 1][i + 1] * sum0 - C[i][i + 1] * sum1;
                    double den = C[i + 1][i + 1] * C[i][i] - C[i][i + 1] * C[i + 1][i];
                    double xnew = num / den;
                    double delx = xnew - rval[i];
                    rval[i] = xnew;
                    if (Math.Abs(delx) > maxdel)
                        maxdel = Math.Abs(delx);

                    sum0 = y[N - 2];
                    sum1 = y[N - 1];
                    for (int j = 0; j < N - 2; j++)
                    {
                        sum0 -= C[N - 2][j] * rval[j];
                        sum1 -= C[N - 1][j] * rval[j];
                    }

                    num = C[N - 1][N - 2] * sum0 - C[N - 2][N - 2] * sum1;
                    den = C[N - 1][N - 2] * C[N - 2][N - 1] - C[N - 2][N - 2] * C[N - 1][N - 1];
                    xnew = num / den;
                    delx = xnew - rval[N - 1];
                    rval[N - 1] = xnew;
                    if (Math.Abs(delx) > maxdel)
                        maxdel = Math.Abs(delx);
                    if (maxdel < DelLimit)
                        k = MaxIt;
                    k += 1;
                }
            }
            return rval;
        }
    }
}
