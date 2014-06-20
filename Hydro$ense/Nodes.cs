using System;
using System.Linq;

namespace HydroSense
{
    class Nodes
    {
        private double[][] X;
        private double[][] Y;
        private double[][] YC;
        public double[][] x { get { return X; } }
        public double[][] y { get { return Y; } }
        public double[][] yc { get { return YC; } }

        public Nodes(double[][] quantity, double[][] cost)
        {
            if (quantity.Length != cost.Length)
            {
                throw new DataMisalignedException("must have a quantity/cost relationship for each node, check inputs");
            }
            X = quantity;
            Y = cost;
            YC = new double[quantity.Length][];

            double val;
            for (int i = 0; i < X.Length; i++)
            {
                if (X[i].Length != Y[i].Length)
                {
                    throw new DataMisalignedException("must have same number of quantities and costs, check inputs");
                }

                val = 0.0;
                YC[i] = new double[X[i].Length];
                for (int j = 0; j < X[i].Length; j++)
                {
                    if (j > 0)
                    {
                        val += ((Y[i][j] + Y[i][j - 1]) / 2.0) * (X[i][j] - X[i][j - 1]);
                    }
                    YC[i][j] = val;
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
