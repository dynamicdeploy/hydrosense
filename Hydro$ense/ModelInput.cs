using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    class ModelInput
    {
        private double[][] m_Xs;
        private double[][] m_Ys;
        private double[][] m_Xd;
        private double[][] m_Yd;
        private double[][][] m_Xt;
        private double[][][] m_Yt;
        private double[][][] m_Xtl;
        private double[][][] m_Ytl;
        public Node supplyNodes { get; set; }
        public Node demandNodes { get; set; }
        public LinkCost linkCosts { get; set; }
        public LinkLoss linkLosses { get; set; }
        public double[][] Q { get; set; }
        
        public ModelInput()
        {

        }

        public void ReadFromExcel(string fileName)
        {

        }

        public void ReadHardcoded()
        {
            // marginal supply curves
            m_Xs = new double[2][];
            m_Ys = new double[2][];
            m_Xs[0] = new double[] { 0.0, 2000.0, 4000.0, 5000.0, 7500.0, 10000.0, 20000.0, 30000.0, 50000.0, 65000.0 };
            m_Xs[1] = new double[] { 3.0, 30.61, 3.122, 3.154, 3.234, 3.316, 3.664, 4.050, 4.946, 5.747 };
            m_Ys[0] = new double[] { 0.0, 0.0, 100000.0 };
            m_Ys[1] = new double[] { 1.50, 1.546, 1.617, 1.743, 1.907, 2.025, 2.352, 3.176 };
            supplyNodes = new Node(m_Xs, m_Ys);

            // marginal demand curves
            m_Xd = new double[3][];
            m_Yd = new double[3][];
            m_Xd[0] = new double[] { 0.0, 2000.0, 4000.0, 7500.0, 10000.0, 15000.0, 20000.0, 30000.0, 40000.0 };
            m_Xd[1] = new double[] { 0.0, 3000.0, 7500.0, 12500.0, 20000.0, 30000.0 };
            m_Xd[2] = new double[] { 0.0, 2500.0, 5000.0, 10000.0, 15000.0, 25000.0, 35000.0 };
            m_Yd[0] = new double[] { 15.0, 12.782, 10.892, 8.232, 6.740, 4.518, 3.028, 1.361, 0.611 };
            m_Yd[1] = new double[] { 10.0, 8.607, 6.873, 5.353, 3.679, 2.231 };
            m_Yd[2] = new double[] { 25.0, 19.470, 15.163, 9.197, 5.578, 2.052, 0.755 };
            demandNodes = new Node(m_Xd, m_Yd);

            // transportation costs
            m_Xt = new double[3][][];
            m_Xt[0] = new double[2][];
            m_Xt[1] = new double[2][];
            m_Xt[2] = new double[2][];
            m_Xt[0][0] = new double[] { 0.0, 40000.0 };
            m_Xt[0][1] = new double[] { 0.0, 20000.0 };
            m_Xt[1][0] = new double[] { 0.0, 43000.0 };
            m_Xt[1][1] = new double[] { 0.0, 50000.0 };
            m_Xt[2][0] = new double[] { 0.0, 50000.0 };
            m_Xt[2][1] = new double[] { 0.0, 17500.0 };
            m_Yt = new double[3][][];
            m_Yt[0] = new double[2][];
            m_Yt[1] = new double[2][];
            m_Yt[2] = new double[2][];
            m_Yt[0][0] = new double[] { 1.5, 1.5 };
            m_Yt[0][1] = new double[] { 0.75, 0.75 };
            m_Yt[1][0] = new double[] { 1.25, 1.25 };
            m_Yt[1][1] = new double[] { 100.0, 100.0 };
            m_Yt[2][0] = new double[] { 100.0, 100.0 };
            m_Yt[2][1] = new double[] { 1.3, 1.3 };
            linkCosts = new LinkCost(m_Xt, m_Yt);

            // transportation losses
            m_Xtl = new double[3][][];
            m_Xtl[0] = new double[2][];
            m_Xtl[1] = new double[2][];
            m_Xtl[2] = new double[2][];
            m_Xtl[0][0] = new double[] { 0.0, 5000.0, 10000.0, 15000.0, 20000.0, 30000.0, 40000.0 };
            m_Xtl[0][1] = new double[] { 0.0, 2500.0, 5000.0, 10000.0, 15000.0, 20000.0 };
            m_Xtl[1][0] = new double[] { 0.0, 5000.0, 10000.0, 17500.0, 25000.0, 35000.0, 43000.0 };
            m_Xtl[1][1] = new double[] { 0.0, 50000.0 };
            m_Xtl[2][0] = new double[] { 0.0, 50000.0 };
            m_Xtl[2][1] = new double[] { 0.0, 2500.0, 5000.0, 7500.0, 10000.0, 15000.0, 17500.0 };
            m_Ytl = new double[3][][];
            m_Ytl[0] = new double[2][];
            m_Ytl[1] = new double[2][];
            m_Ytl[2] = new double[2][];
            m_Ytl[0][0] = new double[] { 0.0, 2661.0, 8147.0, 13753.0, 19134.0, 29426.0, 39487.0 };
            m_Ytl[0][1] = new double[] { 0.0, 0.0, 1390.0, 6275.0, 11866.0, 17515.0 };
            m_Ytl[1][0] = new double[] { 0.0, 1467.0, 5821.0, 13959.0, 22448.0, 33443.0, 41917.0 };
            m_Ytl[1][1] = new double[] { 0.0, 0.0 };
            m_Ytl[2][0] = new double[] { 0.0, 0.0 };
            m_Ytl[2][1] = new double[] { 0.0, 303.0, 1717.0, 3707.0, 6071.0, 11403.0, 14209.0 };
            linkLosses = new LinkLoss(m_Xtl, m_Ytl);
            
            // initial guess
            Q = new double[3][];
            Q[0] = new double[] { 10050.64038617956, 11312.969207627437 };
            Q[1] = new double[] { 22028.15601517901, 0.0 };
            Q[2] = new double[] { 0.0, 13684.530792372565 };

        }
    }
}
