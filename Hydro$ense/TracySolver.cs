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
            double[][] deltaQ = new double[m.Q.Length][];
            double[][] deltaOFdeltaQ = new double[m.Q.Length][];

            Console.WriteLine("Initial Guess = " + m.Q.ToArray().ToString());
            double OF = ObjectiveFunction(m.supplyNodes, m.demandNodes, m.linkCosts, m.linkLosses, m.Q);
            Console.WriteLine("Qbjective Function = " + OF + ": Quantity = " + m.Q.ToArray().ToString());
            
            
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
                // Compute the numerical derivates for the Objective Function with
                // respect to each decision variable:
                // The Objective Function is the integral of the marginal demand
                // functions from zero to the Quantity provided at each demand node minus
                // the integral of the marginal cost functions from zero to the Quantity
                // provided from supply node j to demand node i
                // The DVs are the array of quantity from supply node j to
                // demand node i
                int numNodes = m.Q.Length * m.Q[0].Length;
                for (int i = 0; i < numNodes; i++)
                {

                }
            }


        }

        private double ObjectiveFunction(Node supplyNodes, Node demandNodes, LinkCost linkCosts, LinkLoss linkLosses, double[][] Q)
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
            for (int i = 0; i < Q[0].Length; i++)
            {
                rval -= supplyNodes.IntegratedCost(i, SumColumn(Q, i));
            }

            // Calculate total benefits of water for each demand node and the
            // transportation/link cost associated with delivering water from
            // Supply node j to Demand node i
            for (int i = 0; i < Q.Length; i++)
            {
                double qd = 0.0;
                for (int j = 0; j < Q[i].Length; j++)
                {
                    qd += linkCosts.LinkCostOrLoss(i, j, Q[i][j]);
                    rval -= linkCosts.IntegratedCost(i, j, Q[i][j]);
                }
                rval += demandNodes.IntegratedCost(i, qd);
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
    }
}
