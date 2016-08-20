using System;
using MathNet.Numerics.Random;
using MathNet.Numerics.Distributions;

namespace ScoopProject_Group3
{
    class MontecarloSim
    {
        //we implement a method that return an array of rates that have the dynamic of the CIR model
        //for dz we use the method of the Normal class
        public static double compute(Libor[] array, int numSim, int H, CirModel param, double r0, double dt, int choose)
        {
            Normal dist = new Normal(); //create an object with mean 0 and 1 as variance
            double dz; //wiener object of the formula of CIR
            double[] prob;

            double tempResult = 0;
            double r;
            double result;

            for (int i = 0; i < numSim; i++)
            {
                prob = SystemRandomSource.Doubles(H, RandomSeed.Robust()); //every time we create a random probability from 0 to 1
                r = r0;
                for (int j = 0; j < H; j++)
                {
                    if (choose == 0) // Euler scheme
                    {
                        dz = dist.InverseCumulativeDistribution(prob[j]) * Math.Sqrt(dt);
                        r = param.getAlpha() * param.getMu() * dt + (1 - param.getAlpha() * dt) * r + param.getSigma() * Math.Sqrt(r * dt) * dz;
                    }
                    else // Milstein scheme
                    {
                        dz = dist.InverseCumulativeDistribution(prob[j]) * Math.Sqrt(dt);
                        r = (r + param.getAlpha() * param.getMu() * dt + param.getSigma() * Math.Sqrt(r * dt) * dz +
                                (Math.Pow(param.getSigma(), 2) * dt * (Math.Pow(dz, 2) - 1)) / 4) / (1 + param.getAlpha() * dt);
                    }  
                }
                tempResult = tempResult + r;
            }
            return result = tempResult / numSim; //we do the average of the rates of the simulations
        }
    }
}