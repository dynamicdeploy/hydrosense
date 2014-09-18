using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    class TracySolver
    {

        internal void Solve(ModelInput mi)
        {
            int iter = 0;
            int maxIter = 5000;
            double delta = 1000;
            double deltad = 0.01;
            double tolerance = 0.015;
            int numDemandNodes = mi.Q.Length;
            int numSupplyNodes = mi.Q[0].Length;
            int numNodes = numDemandNodes * numSupplyNodes;
            double[][] quantS = Util.CopyArray(mi.Q);
            double[][] quantD = Util.CopyArray(mi.Q);
            double[][] delQ1 = Util.CopyArray(mi.Q);
            double[][] delQ2 = Util.CopyArray(mi.Q);
            double[][] delQ12 = Util.CopyArray(mi.Q);
            double[] delOFdelQ = new double[numNodes];
            double[] delOFdelQ2 = new double[numNodes];
            double[][] dQW = Util.CopyArray(mi.Q);
            double[][] del2OFdelQ2 = new double[numNodes][];
            Util.InitializeArrayToZero(del2OFdelQ2, numNodes, numNodes);

            /* Initial Guess of Quantity delivered to the Demand nodes
             * by adjusting for transportation losses from the supply nodes.*/
            for (int i = 0; i < quantD.Length; i++)
            {
                for (int j = 0; j < quantD[i].Length; j++)
                {
                    quantD[i][j] = mi.linkLosses.LinkCostOrLoss(i, j, quantS[i][j]);
                }
            }
            Console.WriteLine("Initial Guess:");
            double OF = ObjectiveFunction(mi, quantS, quantD);
            Util.PrintResultsToConsole(OF, quantS, quantD, iter, delta);

            /* Perform iterative colculations using a while loop construction
             * This is the heart of the non-linear function optimizer
             * Current routines use a simplified first derivative search
             * approach, with a dampening factor to adjust the decision variables
             * during each iteration */
            double OF1;
            double OF12;
            double OF2;
            while (delta > tolerance && iter < maxIter)
            {
                iter++;
                OF = ObjectiveFunction(mi, quantS, quantD);
                
                delQ1 = Util.CopyArray(quantS);
                delQ2 = Util.CopyArray(quantS);
                delQ12 = Util.CopyArray(quantS);

                /* Compute the numerical derivates for the Objective Function with
                 * respect to each decision variable:
                 *   The Objective Function is the integral of the marginal demand
                 *   functions from zero to the Quantity provided at each demand node minus
                 *   the integral of the marginal cost functions from zero to the Quantity
                 *   provided from supply node j to demand node i. The DVs are the array 
                 *   of quantity from supply node j to demand node i */
                for (int i = 0; i < quantS.Length; i++)
                {
                    for (int j = 0; j < quantS[i].Length; j++)
                    {
                        delQ1[i][j] -= deltad;
                        delQ12[i][j] -= deltad;
                        OF1 = ObjectiveFunction(mi, delQ1, quantD);
                        delOFdelQ[i * numSupplyNodes + j] = (OF - OF1) / deltad;

                        for (int ki = 0; ki < i; ki++)
                        {
                            for (int kj = 0; kj < j; kj++)
                            {
                                delQ2[ki][kj] -= deltad;
                                delQ12[ki][kj] -= deltad;
                                OF2 = ObjectiveFunction(mi, delQ2, quantD);
                                OF12 = ObjectiveFunction(mi, delQ12, quantD);
                                delOFdelQ2[ki * numSupplyNodes + kj] = (OF2 - OF12) / deltad;
                                del2OFdelQ2[i * numSupplyNodes + j][ki * numSupplyNodes + kj] = (delOFdelQ[i * numSupplyNodes + j] - delOFdelQ2[ki * numSupplyNodes + kj]) / deltad;
                                delQ2[ki][kj] = quantS[ki][kj];
                                delQ12[ki][kj] = quantS[ki][kj];
                            }
                            for (int kj = j + 1; kj < numSupplyNodes; kj++)
                            {
                                delQ2[ki][kj] -= deltad;
                                delQ12[ki][kj] -= deltad;
                                OF2 = ObjectiveFunction(mi, delQ2, quantD);
                                OF12 = ObjectiveFunction(mi, delQ12, quantD);
                                delOFdelQ2[ki * numSupplyNodes + kj] = (OF2 - OF12) / deltad;
                                del2OFdelQ2[i * numSupplyNodes + j][ki * numSupplyNodes + kj] = (delOFdelQ[i * numSupplyNodes + j] - delOFdelQ2[ki * numSupplyNodes + kj]) / deltad;
                                delQ2[ki][kj] = quantS[ki][kj];
                                delQ12[ki][kj] = quantS[ki][kj];
                            }
                        }
                        for (int ki = i + 1; ki < numDemandNodes; ki++)
                        {
                            for (int kj = 0; kj < j; kj++)
                            {
                                delQ2[ki][kj] -= deltad;
                                delQ12[ki][kj] -= deltad;
                                OF2 = ObjectiveFunction(mi, delQ2, quantD);
                                OF12 = ObjectiveFunction(mi, delQ12, quantD);
                                delOFdelQ2[ki * numSupplyNodes + kj] = (OF2 - OF12) / deltad;
                                del2OFdelQ2[i * numSupplyNodes + j][ki * numSupplyNodes + kj] = (delOFdelQ[i * numSupplyNodes + j] - delOFdelQ2[ki * numSupplyNodes + kj]) / deltad;
                                delQ2[ki][kj] = quantS[ki][kj];
                                delQ12[ki][kj] = quantS[ki][kj];
                            }
                            for (int kj = j + 1; kj < numSupplyNodes; kj++)
                            {
                                delQ2[ki][kj] -= deltad;
                                delQ12[ki][kj] -= deltad;
                                OF2 = ObjectiveFunction(mi, delQ2, quantD);
                                OF12 = ObjectiveFunction(mi, delQ12, quantD);
                                delOFdelQ2[ki * numSupplyNodes + kj] = (OF2 - OF12) / deltad;
                                del2OFdelQ2[i * numSupplyNodes + j][ki * numSupplyNodes + kj] = (delOFdelQ[i * numSupplyNodes + j] - delOFdelQ2[ki * numSupplyNodes + kj]) / deltad;
                                delQ2[ki][kj] = quantS[ki][kj];
                                delQ12[ki][kj] = quantS[ki][kj];
                            }
                        }

                        /* set the second derivative on the diagonal */
                        delQ1[i][j] += 2 * deltad;
                        OF2 = ObjectiveFunction(mi, delQ1, quantD);
                        delOFdelQ2[i * numSupplyNodes + j] = (OF2 - OF) / deltad;
                        del2OFdelQ2[i * numSupplyNodes + j][i * numSupplyNodes + j] = (delOFdelQ2[i * numSupplyNodes + j] - delOFdelQ[i * numSupplyNodes + j]) / deltad;

                        /* use the Marquard Algorithm to condition the matrix */
                        del2OFdelQ2[i * numSupplyNodes + j][i * numSupplyNodes + j] += Math.Exp((iter - 500) * deltad);
                        delQ2[i][j] = quantS[i][j];
                        delQ1[i][j] = quantS[i][j];
                        delQ12[i][j] = quantS[i][j];
                    }

                }

                /*  Determine the change in quantity from Demand Node i to Supply Node j 
                 *  by solving the linear set of equations [d2OFdQ2]{dQ} = {dOFdQ}
                 *  using the LinearEquationSolver Routine */
                double[] dQ = LinearEquationSolver(numNodes, del2OFdelQ2, delOFdelQ);
                for (int i = 0; i < quantS.Length; i++)
                {
                    for (int j = 0; j < quantS[i].Length; j++)
                    {
                        quantS[i][j] += dQ[i * numSupplyNodes + j];
                        dQW[i][j] = dQ[i * numSupplyNodes + j];
                    }
                }

                /* The partial derivates of the Objective Function with respect to the
                 * Quantity provided from supply node j to demand node i are the
                 * "direction" that decision variables should be adjusted for the simple
                 * non-linear solver routine.  The "distance" the decision variables should
                 * be adjusted is assumed to be 1.0. */
                double sum = 0.0;
                for (int i = 0; i < dQ.Length; i++)
                {
                    sum += dQ[i] * dQ[i];
                }

                /* Adjust change in decision variables */
                delta = Math.Sqrt(sum);

                /* Adjust quantities to make sure they are all within
                 * the supply, demand, and transportation constraints */
                CheckConstraints(mi, quantS, quantD);

                /* Calculate new objective function */
                OF = ObjectiveFunction(mi, quantS, quantD);

                /* a little output to check against Python code */
                Util.PrintResultsToConsole(OF, quantS, quantD, iter, delta);
            }
            mi.Q = quantS;
        }

        /// <summary>
        /// Adjust quantities to make sure they are all within the 
        /// supply, demand, and transportation constraints
        /// </summary>
        /// <param name="mi">Model input data</param>
        /// <param name="quantS">Supply quantities</param>
        /// <param name="quantD">Demand quantities</param>
        private void CheckConstraints(ModelInput mi, double[][] quantS, double[][] quantD)
        {
            /* Determine the Quantity delivered to the Demand nodes by adjusting
             * for transportation losses from the supply nodes. */
            for (int i = 0; i < quantD.Length; i++)
            {
                for (int j = 0; j < quantD[i].Length; j++)
                {
                    quantD[i][j] = mi.linkLosses.LinkCostOrLoss(i, j, quantS[i][j]);
                }
            }

            /* Constrain Supply to Demand flows to be positive and not greater than
             * maximum quantity between Supply node j and Demand node i. */
            for (int i = 0; i < quantS.Length; i++)
            {
                for (int j = 0; j < quantS[i].Length; j++)
                {
                    if (quantS[i][j] < mi.linkLosses.LowerLimit(i, j))
                    {
                        quantS[i][j] = mi.linkLosses.LowerLimit(i, j);
                    }
                    if (quantS[i][j] > mi.linkLosses.UpperLimit(i, j))
                    {
                        quantS[i][j] = mi.linkLosses.UpperLimit(i, j);
                    }
                }
            }

            /* Determine the Quantity delivered to the Demand nodes by adjusting
             * for transportation losses from the supply nodes after constraining
             * supply flows */
            for (int i = 0; i < quantS.Length; i++)
            {
                for (int j = 0; j < quantS[i].Length; j++)
                {
                    quantD[i][j] = mi.linkLosses.LinkCostOrLoss(i, j, quantS[i][j]);
                }
            }

            /* Determine if Demand Node Constraints are violated
             * If they are, redistribute flows proportionally from Supplies */
            for (int i = 0; i < quantD.Length; i++)
            {
                double totalD = Util.SumRow(quantD, i);
                double limit = mi.demandNodes.UpperLimit(i);
                if (totalD > limit)
                {
                    double ratio = limit / totalD;
                    for (int j = 0; j < quantD[i].Length; j++)
                    {
                        quantD[i][j] *= ratio;
                    }
                }
            }

            /* Adjust the Supply Quantities to match the adjusted Demand Quantities */
            for (int i = 0; i < quantS.Length; i++)
            {
                for (int j = 0; j < quantS[i].Length; j++)
                {
                    quantS[i][j] = mi.linkLosses.LossInverse(i, j, quantD[i][j]);
                }
            }

            /* Determine if Supply Node Constraints are violated
             * If they are, redistribute flow proportionally between Demand Nodes */
            for (int i = 0; i < quantS[0].Length; i++)
            {
                double totalS = Util.SumColumn(quantS, i);
                double limit = mi.supplyNodes.UpperLimit(i);
                if (totalS > limit)
                {
                    double ratio = 0.9999 * (limit / totalS);
                    for (int j = 0; j < quantS.Length; j++)
                    {
                        quantS[j][i] *= ratio;
                    }
                }
            }
        }


        private double ObjectiveFunction(ModelInput m, double[][] quantS, double[][] quantD)
        {
            /* The ObjectiveFunction is computed by integrating the sum of the
             * difference between the Demand Functions and the Supply Functions.
             * Eventually this routine should be modified to have the marginal
             * supply and demand parameters defined as lists of variables associated
             * with each function. This would allow for a more generic solution
             * of the problem, where different functional forms for the supply and
             * demand relationships could be used without further need to change the
             * arguments passed to this routine, as well as the numerical itegration
             * routine. Alternatively, the supply and demand function parameters could
             * be defined as global variables, so that only the Qs values (DVs) would
             * have to be passed to the routine. */
            double rval = 0.0;

            /* Adjust quantities to make sure they are all within
             * the supply, demand, and transportation constraints */
            CheckConstraints(m, quantS, quantD);

            /* Calculate total cost of water for each supply node */
            for (int i = 0; i < quantS[0].Length; i++)
            {
                rval -= m.supplyNodes.IntegratedCost(i, Util.SumColumn(quantS, i));
            }

            /* Calculate total benefits of water for each demand node and the
             * transportation/link cost associated with delivering water from
             * Supply node j to Demand node i */
            for (int i = 0; i < quantS.Length; i++)
            {
                double qd = 0.0;
                for (int j = 0; j < quantS[i].Length; j++)
                {
                    qd += m.linkLosses.LinkCostOrLoss(i, j, quantS[i][j]);
                    rval -= m.linkCosts.IntegratedCost(i, j, quantS[i][j]);
                }
                rval += m.demandNodes.IntegratedCost(i, qd);
            }

            return rval;
        }

        private double[] LinearEquationSolver(int N, double[][] C, double[] y)
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
            /*
             *  Solve the system using an iterative approach by computing
             *  Each x value as a 2X2 matrix solution
             */
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


