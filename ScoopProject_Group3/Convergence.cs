using System;
using System.IO;

namespace ScoopProject_Group3
{
    class Convergence
    {
        public static void compute(Libor[] array, int hist, DateTime start, int max, double dt, int numSim, int H)
        {
            double euler;
            double eulError;
            double milstein;
            double milError;

            int startPos = Finder.find(array, start, 0, max);
            CirModel modelParameters = new CirModel();
            modelParameters.estimate(array, hist, startPos, dt);
            double r0 = array[startPos].getValue();

            string name = "convergence.txt";
            StreamWriter writer2 = new StreamWriter(name);
            writer2.WriteLine("EULERO scheme \t MILSTEIN scheme \t EULERO error \t MILSTEIN error");
            for (int i=0; i<numSim; i++)
            {
                euler = MontecarloSim.compute(array, i + 1, H, modelParameters, r0, dt, 0); // Eulero;
                eulError = Math.Abs(r0 - euler);
                milstein = MontecarloSim.compute(array, i + 1, H, modelParameters, r0, dt, 1); // Milstein
                milError = Math.Abs(r0 - milstein);

                writer2.WriteLine(euler + "\t" + milstein + "\t" + eulError + "\t" + milError);
            }
            writer2.Close();
        }
    }
}
