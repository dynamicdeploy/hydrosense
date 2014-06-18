using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    class TracySolver
    {

        internal void Solve(ModelInput m)
        {
            double deltai = 0.005;
            double deltad = 0.01;
            double tolerance = 0.015;
            double[][] quant = CopyArray(m.Q);
            double[][] quantD = CopyArray(m.Q);
            double[][] deltaQ = new double[m.Q.Length][];
            double[][] deltaOFdeltaQ = new double[m.Q.Length][];
            InitializeArrayToZero(deltaQ, m.Q);
            InitializeArrayToZero(deltaOFdeltaQ, m.Q);


            //Console.WriteLine("Initial Guess = " + m.Q.ToArray().ToString());
            double OF = ObjectiveFunction(m, quant);
            //Console.WriteLine("Qbjective Function = " + OF + ": Quantity = " + m.Q.ToArray().ToString());


            // Perform iterative colculations using a while loop construction
            // This is the heart of the non-linear function optimizer
            // Current routines use a simplified first derivative search
            // approach, with a dampening factor to adjust the decision variables
            // during each iteration
            int iter = 0;
            double delta = 1000;
            double dampen = 1;
            while (delta > tolerance && iter < 10000)
            {
                iter++;
                // Compute the numerical derivates for the Objective Function with
                // respect to each decision variable:
                //   The Objective Function is the integral of the marginal demand
                //   functions from zero to the Quantity provided at each demand node minus
                //   the integral of the marginal cost functions from zero to the Quantity
                //   provided from supply node j to demand node i. The DVs are the array 
                //   of quantity from supply node j to demand node i
                for (int i = 0; i < deltaQ.Length; i++)
                {
                    for (int j = 0; j < deltaQ[i].Length; j++)
                    {
                        deltaQ = CopyArray(quant);
                        deltaQ[i][j] = deltaQ[i][j] - deltad;
                        double obj1 = ObjectiveFunction(m, deltaQ);
                        deltaOFdeltaQ[i][j] = (OF - obj1) / deltad;
                    }
                }

                //The partial derivates of the Objective Function with respect to the
                //Quantity provided from supply node j to demand node i are the
                //"direction" that decision variables should be adjusted for the simple
                //non-linear solver routine.  The "distance" the decision variables should
                //be adjusted is assumed to be 1.0.  If the convergence metric (Delta)
                //increases from one iteration to the next, a dampening coefficient will
                //be used to adjust the "distance" to accelerate convergence
                //In addition, the current routine does not place bounds on the decision
                //variables.  Routines need to be added to prevent the QuantS from containing
                //negative values.  Additional routines could be put in place to bound
                //QuantS with maximum values also.
                double sum = 0.0;
                for (int i = 0; i < deltaOFdeltaQ.Length; i++)
                {
                    for (int j = 0; j < deltaOFdeltaQ[i].Length; j++)
                    {
                        sum += Math.Pow(deltaOFdeltaQ[i][j], 2.0);
                    }
                }
                double delta1 = Math.Sqrt(sum);
                if (delta1 > delta)
                {
                    dampen *= delta / delta1;
                }
                for (int i = 0; i < deltaOFdeltaQ.Length; i++)
                {
                    for (int j = 0; j < deltaOFdeltaQ[i].Length; j++)
                    {
                        quant[i][j] += dampen * deltaOFdeltaQ[i][j];
                    }
                }

                // Determine the Quantity delivered to the Demand nodes by adjusting
                // for transportation losses from the supply nodes.
                for (int i = 0; i < quantD.Length; i++)
                {
                    for (int j = 0; j < quantD[i].Length; j++)
                    {
                        quantD[i][j] = m.linkLosses.LinkCostOrLoss(i, j, quantD[i][j]);
                    }
                }

                // Constrain Supply to Demand flows to be positive and not greater than
                // maximum quantity between Supply node j and Demand node i.
                for (int i = 0; i < quant.Length; i++)
                {
                    for (int j = 0; j < quant[i].Length; j++)
                    {
                        double val = quant[i][j];
                        double limit = m.linkLosses.Limit(i, j);
                        if (val < 0)
                            val = 0.0;
                        if (val > limit)
                            val = limit;
                        quant[i][j] = val;
                    }
                }

                // Determine if Demand Node Constraints are violated
                // If they are, redistribute flows proportionally from Supplies
                for (int i = 0; i < quantD.Length; i++)
                {
                    double totalD = SumRow(quantD, i);
                    double limit = m.demandNodes.Limit(i);
                    if (totalD > limit)
                    {
                        double ratio = limit / totalD;
                        for (int j = 0; j < quantD[i].Length; j++)
                        {
                            quantD[i][j] *= ratio;
                        } 
                    }
                }

                // Adjust the Supply Quantities to match the adjusted Demand Quantities
                for (int i = 0; i < quant.Length; i++)
                {
                    for (int j = 0; j < quant[i].Length; j++)
                    {
                        quant[i][j] = m.linkLosses.LossInverse(i, j, quantD[i][j]);
                    }
                }

                // Determine if Supply Node Constraints are violated
                // If they are, redistribute flow proportionally between Demand Nodes
                for (int i = 0; i < quant[0].Length; i++)
                {
                    double totalS = SumColumn(quant, i);
                    double limit = m.supplyNodes.Limit(i);
                    if (totalS > limit)
                    {
                        double ratio = 0.9999 * (limit / totalS);
                        for (int j = 0; j < quant.Length; j++)
                        {
                            quant[j][i] *= ratio;
                        }
                    }
                }

                // Calculate new objective function
                OF = ObjectiveFunction(m, quant);

                // Adjust change in decision variables
                delta = delta1 * dampen;

                // a little output to check against Python code
                Console.WriteLine(string.Format("k = {0}, Delta = {1}, OF = {2}", iter, delta, OF));
                PrintArrayToConsole(quant);
            }
        }


        private double ObjectiveFunction(ModelInput m, double[][] quantities)
        {
            // The ObjectiveFunction is computed by integrating the sum of the
            // difference between the Demand Functions and the Supply Functions.
            // Eventually this routine should be modified to have the marginal
            // supply and demand parameters defined as lists of variables associated
            // with each function. This would allow for a more generic solution
            // of the problem, where different functional forms for the supply and
            // demand relationships could be used without further need to change the
            // arguments passed to this routine, as well as the numerical itegration
            // routine. Alternatively, the supply and demand function parameters could
            // be defined as global variables, so that only the Qs values (DVs) would
            // have to be passed to the routine.
            double rval = 0.0;


            // Calculate total cost of water for each supply node
            for (int i = 0; i < quantities[0].Length; i++)
            {
                rval -= m.supplyNodes.IntegratedCost(i, SumColumn(quantities, i));
            }

            // Calculate total benefits of water for each demand node and the
            // transportation/link cost associated with delivering water from
            // Supply node j to Demand node i
            for (int i = 0; i < quantities.Length; i++)
            {
                double qd = 0.0;
                for (int j = 0; j < quantities[i].Length; j++)
                {
                    qd += m.linkLosses.LinkCostOrLoss(i, j, quantities[i][j]);
                    rval -= m.linkCosts.IntegratedCost(i, j, quantities[i][j]);
                }
                rval += m.demandNodes.IntegratedCost(i, qd);
            }

            return rval;
        }


        private double SumColumn(double[][] Q, int col)
        {
            double rval = 0.0;
            for (int i = 0; i < Q.Length; i++)
            {
                rval += Q[i][col];
            }
            return rval;
        }


        private double SumRow(double[][] Q, int row)
        {
            double rval = 0.0;
            for (int i = 0; i < Q[row].Length; i++)
            {
                rval += Q[row][i];
            }
            return rval;
        }


        private void InitializeArrayToZero(double[][] arr, double[][] pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                arr[i] = new double[pattern[i].Length];
                for (int j = 0; j < pattern[i].Length; j++)
                {
                    arr[i][j] = 0.0;
                }
            }
        }
        

        // taken from here:
        // http://stackoverflow.com/questions/4670720/extremely-fast-way-to-clone-the-values-of-a-jagged-array-into-a-second-array
        private double[][] CopyArray(double[][] source)
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

        private static void PrintArrayToConsole(double[][] quant)
        {
            for (int i = 0; i < quant.Length; i++)
            {
                Console.Write("[");
                for (int j = 0; j < quant[i].Length; j++)
                {
                    Console.Write(quant[i][j]);
                    if (j < quant[i].Length - 1)
                        Console.Write(", ");
                }
                Console.Write("]");
                Console.WriteLine();
            }
        }
    }
}
