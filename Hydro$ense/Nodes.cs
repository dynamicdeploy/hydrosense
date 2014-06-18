using System;
using System.Linq;

namespace HydroSense
{
    class Node
    {
        public double[][] x { get; set; }
        public double[][] y { get; set; }
        public double[][] yc { get; set; }

        public Node(double[][] quantity, double[][] cost)
        {
            if (quantity.Length != cost.Length)
            {
                throw new DataMisalignedException("must have a quantity/cost relationship for each node, check inputs");
            }
            x = quantity;
            y = cost;
            yc = new double[quantity.Length][];

            double val;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i].Length != y[i].Length)
                {
                    throw new DataMisalignedException("must have same number of quantities and costs, check inputs");
                }

                val = 0.0;
                yc[i] = new double[x[i].Length];
                for (int j = 0; j < x[i].Length; j++)
                {
                    if (j > 0)
                    {
                        val += ((y[i][j] + y[i][j - 1]) / 2.0) * (x[i][j] - x[i][j - 1]);
                    }
                    yc[i][j] = val;
                }
            }
        }

        /// <summary>
        /// Get upper limit of the quantity that can delivered to a node
        /// </summary>
        /// <param name="n">node number</param>
        /// <returns></returns>
        public double Limit(int n)
        {
            return x[n].Last();
        }

        /// <summary>
        /// Get marginal cost
        /// </summary>
        /// <param name="n">node number</param>
        /// <param name="q">quantity</param>
        /// <returns></returns>
        public double MarginalCost(int n, double q)
        {
            return Util.CalculateCost(x[n], y[n], q);
        }

        /// <summary>
        /// Get integrated supply cost
        /// </summary>
        /// <param name="n">node number</param>
        /// <param name="q">quantity</param>
        /// <returns></returns>
        public double IntegratedCost(int n, double q)
        {
            return Util.CalculateCost(x[n], yc[n], q);
        }

    }
}
