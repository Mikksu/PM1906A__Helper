using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM1906AHelper.Core
{
    public class FirFilter
    {
        #region Variables

        Queue<double> fir_buffer;

        #endregion

        #region Constructors

        public FirFilter(int Order, double[] Coefficient = null)
        {
            if (Order <= 0)
                throw new ArgumentException("the order must be >=1.", "Order");
            else
                this.Order = Order;

            // initialize the coefficient array.
            if (Coefficient != null)
            {
                this.Coefficient = Coefficient;
            }
            else
            {
                this.Coefficient = new double[Order];

                for (int i = 0; i < Order; i++)
                {
                    this.Coefficient[i] = 1.0 / Order;
                }
            }

            // initialize the FIR buffer.
            fir_buffer = new Queue<double>(Order);
            for (int i = 0; i < Order; i++)
            {
                fir_buffer.Enqueue(0.0);
            }
        }

        #endregion

        #region Properties

        public int Order { get; }

        public double[] Coefficient { get; }

        #endregion

        #region Methods

        public double Filter(double Input)
        {
            fir_buffer.Enqueue(Input);
            if (fir_buffer.Count > Order)
                fir_buffer.Dequeue();

            var x = fir_buffer.ToArray();
            var h = Coefficient.ToArray();

            double y = 0.0;

            for (int i = 0; i < Order; i++)
            {
                y += h[i] * x[Order - i - 1];
            }

            return y;
        }

        #endregion
    }
}
