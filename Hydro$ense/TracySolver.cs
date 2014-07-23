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
            double deltai = 0.005;
            double deltad = 0.01;
            double tolerance = 0.015;
            double[][] quant = Util.CopyArray(mi.Q);
            double[][] quantD = Util.CopyArray(mi.Q);
            double[][] delQ = Util.CopyArray(mi.Q);
            double[][] delQ1 = Util.CopyArray(mi.Q);
            double[][] delQ2 = Util.CopyArray(mi.Q);
            double[][] delQ12 = Util.CopyArray(mi.Q);
            double[][] delOFdelQ = new double[mi.Q.Length][];
            double[][] delOFdelQ2 = new double[mi.Q.Length][];
            double[][] del2OFdelQ2 = new double[mi.Q.Length][];
            Util.InitializeArrayToZero(delOFdelQ, mi.Q);
            Util.InitializeArrayToZero(delOFdelQ2, mi.Q);
            Util.InitializeArrayToZero(del2OFdelQ2, mi.Q);


            Console.WriteLine("Initial Guess:");
            Util.PrintArrayToConsole(quant);
            double OF = ObjectiveFunction(mi, quant);


            // Perform iterative colculations using a while loop construction
            // This is the heart of the non-linear function optimizer
            // Current routines use a simplified first derivative search
            // approach, with a dampening factor to adjust the decision variables
            // during each iteration
            int iter = 0;
            int maxIter = 500;
            double delta = 1000;
            double dampen = 1;
            while (delta > tolerance && iter < maxIter)
            {
                iter++;
                // Compute the numerical derivates for the Objective Function with
                // respect to each decision variable:
                //   The Objective Function is the integral of the marginal demand
                //   functions from zero to the Quantity provided at each demand node minus
                //   the integral of the marginal cost functions from zero to the Quantity
                //   provided from supply node j to demand node i. The DVs are the array 
                //   of quantity from supply node j to demand node i
                double OF1;
                double OF12;
                double OF2;
                for (int i = 0; i < quant.Length; i++)
                {
                    for (int j = 0; j < quant[i].Length; j++)
                    {
                        delQ1[i][j] = delQ[i][j] - deltad;
                        delQ12[i][j] = delQ[i][j] - deltad;
                        OF1 = ObjectiveFunction(mi, delQ1);
                        delOFdelQ[i][j] = (OF - OF1) / deltad;

                        for (int k = 0; k < i; k++)
                        {
                            delQ2[i][k] -= deltad;
                            delQ12[i][k] -= deltad;
                            OF2 = ObjectiveFunction(mi, delQ2);
                            OF12 = ObjectiveFunction(mi, delQ12);
                            delOFdelQ2[i][k] = (OF2 - OF12) / deltad;
                            del2OFdelQ2[i][k] = (delOFdelQ[i][j] - delOFdelQ2[i][k]) / deltad;
                            delQ2[i][k] = quant[i][k];
                            delQ12[i][k] = quant[i][k];
                        }

                        for (int ii = 0; ii < quant.Length; ii++)
                        {
                            for (int jj = 0; jj < quant[ii].Length; jj++)
                            {
                                if (ii == 0 & jj == 0)
                                    continue;
                                delQ2[ii][jj] -= deltad;
                                delQ12[ii][jj] -= deltad;
                                OF2 = ObjectiveFunction(mi, delQ2);
                                OF12 = ObjectiveFunction(mi, delQ12);
                                delOFdelQ2[ii][jj] = (OF2 - OF12) / deltad;
                                del2OFdelQ2[ii][jj] = (delOFdelQ[i][j] - delOFdelQ2[i][jj]) / deltad;
                                delQ2[ii][jj] = quant[ii][jj];
                                delQ12[ii][jj] = quant[ii][jj];
                            }
                        }

                        // set the second derivative on the diagonal
                        delQ1[i][j] += 2 * deltad;
                        OF2 = ObjectiveFunction(mi, delQ1);
                        delOFdelQ2[i][j] = (OF2 - OF) / deltad;
                        del2OFdelQ2[i][j] = (delOFdelQ2[i][j] - delOFdelQ[i][j]) / deltad;

                        // use the Marquard Algorithm to condition the matrix
                        del2OFdelQ2[i][j] += Math.Exp((iter - maxIter) * deltad);
                        delQ2[i][j] = quant[i][j];
                        delQ1[i][j] = quant[i][j];
                        delQ12[i][j] = quant[i][j];
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
                for (int i = 0; i < delOFdelQ.Length; i++)
                {
                    for (int j = 0; j < delOFdelQ[i].Length; j++)
                    {
                        sum += Math.Pow(delOFdelQ[i][j], 2.0);
                    }
                }
                double delta1 = Math.Sqrt(sum);
                if (delta1 > delta)
                {
                    dampen *= delta / delta1;
                }
                for (int i = 0; i < delOFdelQ.Length; i++)
                {
                    for (int j = 0; j < delOFdelQ[i].Length; j++)
                    {
                        quant[i][j] += dampen * delOFdelQ[i][j];
                    }
                }

                // Determine the Quantity delivered to the Demand nodes by adjusting
                // for transportation losses from the supply nodes.
                for (int i = 0; i < quantD.Length; i++)
                {
                    for (int j = 0; j < quantD[i].Length; j++)
                    {
                        quantD[i][j] = mi.linkLosses.LinkCostOrLoss(i, j, quantD[i][j]);
                    }
                }

                // Constrain Supply to Demand flows to be positive and not greater than
                // maximum quantity between Supply node j and Demand node i.
                for (int i = 0; i < quant.Length; i++)
                {
                    for (int j = 0; j < quant[i].Length; j++)
                    {
                        double val = quant[i][j];
                        double limit = mi.linkLosses.Limit(i, j);
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
                    double totalD = Util.SumRow(quantD, i);
                    double limit = mi.demandNodes.Limit(i);
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
                        quant[i][j] = mi.linkLosses.LossInverse(i, j, quantD[i][j]);
                    }
                }

                // Determine if Supply Node Constraints are violated
                // If they are, redistribute flow proportionally between Demand Nodes
                for (int i = 0; i < quant[0].Length; i++)
                {
                    double totalS = Util.SumColumn(quant, i);
                    double limit = mi.supplyNodes.Limit(i);
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
                OF = ObjectiveFunction(mi, quant);

                // Adjust change in decision variables
                delta = delta1 * dampen;

                // a little output to check against Python code
                Console.WriteLine(string.Format("k = {0}, Delta = {1}, OF = {2}", iter, delta, OF));
                Util.PrintArrayToConsole(quant);
            }
            mi.Q = quant;
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
                rval -= m.supplyNodes.IntegratedCost(i, Util.SumColumn(quantities, i));
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

    }
}
