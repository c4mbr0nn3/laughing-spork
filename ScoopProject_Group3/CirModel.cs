using System;

namespace ScoopProject_Group3
{
    /*We implement a class where we calculate the parameter of the given array rates take from the web
      which are used to estimate the future rate we will estimate*/
    class CirModel
    {
        private double Alpha;
        private double Mu;
        private double Sigma;

        public void setAlpha(double Value)
        {
            this.Alpha = Value;
        }

        public double getAlpha()
        {
            return this.Alpha;
        }

        public void setMu(double Value)
        {
            this.Mu = Value;
        }

        public double getMu()
        {
            return this.Mu;
        }

        public void setSigma(double Value)
        {
            this.Sigma = Value;
        }

        public double getSigma()
        {
            return this.Sigma;
        }

        //method to calculate all the parameter in the OLS method that we use for the MC simulation
        public void estimate(Libor[] array, int hist, int startPos, double dt)
        {
            double a = 0;
            double b = 0;
            double c = 0;
            double d = 0;
            double s = 0;
            double h;
            try
            {
                for (int i = 0; i < hist - 1; i++)
                {
                    a = a + array[startPos - i].getValue();
                    b = b + array[startPos - i - 1].getValue();
                    c = c + 1 / (array[startPos - i - 1].getValue());
                    d = d + (array[startPos - i].getValue()) / (array[startPos - i - 1].getValue());
                }
                this.Alpha = (Math.Pow(hist, 2) - 2 * hist + 1 + a * c - b * c - (hist - 1) * d) / ((Math.Pow(hist, 2) - 2 * hist + 1 - b * c) * dt);
                this.Mu = ((hist - 1) * a - d * b) / (Math.Pow(hist, 2) - 2 * hist + 1 + a * c - b * c - (hist - 1) * d);

                for (int i = 0; i < hist - 1; i++)
                {
                    h = (array[startPos - i].getValue() - array[startPos - i - 1].getValue() - Mu + Alpha * array[startPos - i - 1].getValue()) / (Math.Sqrt(array[startPos - i - 1].getValue()));
                    s = s + Math.Pow(h, 2);
                }

                this.Sigma = Math.Sqrt(s / (hist - 2));
            }
            catch (Exception e)
            {
                Console.WriteLine("Reduce number of historical data you choose or change start position!");
            }

        }
    }
}
