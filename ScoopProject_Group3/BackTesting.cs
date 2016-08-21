using System;

namespace ScoopProject_Group3
{
    class BackTesting
    {
        //we implement a method that return an array with the new rates using MC method
        public static double[] compute(Libor[] array, int H, int numSim, int days, int hist, DateTime start, int max, double dt, int choose)
        {
            int startPos = Finder.find(array, start, 0, max);
            double[] interestR = new double[days];
            double[] sigma = new double[days];
            CirModel modelParameters = new CirModel();
            double r;
            try
            {
                for (int i = 0; i < days; i++)
                {
                    modelParameters.estimate(array, hist, startPos + i, dt);

                    sigma[i] = modelParameters.getSigma();

                    //this is a condition to smooth the predict value if they go out of a certain range
                    if (i > 1 && (sigma[i] > 1.2 * sigma[i - 1] || sigma[i] < 0.8 * sigma[i - 1]))
                    {
                        sigma[i] = (sigma[i - 2] + sigma[i - 1]) / (double)2;
                        modelParameters.setSigma(sigma[i]);
                    }

                    r = MontecarloSim.compute(array, numSim, H, modelParameters, array[startPos + i].getValue(), dt, choose);

                    interestR[i] = r;

                    //another smooth condition
                    if (i > 1 && (interestR[i] > 1.5 * interestR[i - 1] || interestR[i] < 0.5 * interestR[i - 1]))
                    {
                        interestR[i] = (interestR[i - 2] + interestR[i - 1]) / (double)2;
                        modelParameters.setSigma(interestR[i]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Reduce numbers of simulated days you choose or change start position!");
            }
            return interestR;
        }
    }
}
