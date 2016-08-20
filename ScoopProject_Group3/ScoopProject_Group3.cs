using System;
using System.IO;
using System.Windows.Forms;
using PlotCharts;

namespace ScoopProject_Group3
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            string path = "data_12M.csv";

            int ticksCount = Loader.CountEntries(path);
            Libor[] liborTicks = new Libor[ticksCount];
            for (int i = 0; i < ticksCount; i++)
            {
                liborTicks[i] = new Libor();
            }
            Loader.loadTicks(path, liborTicks);

            int hist;
            int H;
            int days;
            int numSim;
            string timeString;
            int datePos;
            DateTime time;
            bool res;

            Console.WriteLine("Enter start date for calibration: (yyyy-mm-dd)");
            do
            {
                timeString = Console.ReadLine(); // Read string from console
                time = DateTime.ParseExact(timeString, "yyyy-MM-dd", null);
                datePos = Finder.find(liborTicks, time, 0, ticksCount - 1);
                if (datePos == -1)
                {
                    Console.WriteLine("Date not found. Enter different date!");
                }
            } while (datePos == -1);

            Console.WriteLine("Enter historical days to calibrate the model:");
            do
            {
                string line1 = Console.ReadLine();
                res = int.TryParse(line1, out hist);
                if (res == false)
                {
                    Console.WriteLine("Must be an integer! Try again.");
                }
            }
            while (res != true);

            Console.WriteLine("Enter days ahead period:");
            do
            {
                string line2 = Console.ReadLine(); // Read string from console
                res = int.TryParse(line2, out H);
                if (res == false)
                {
                    Console.WriteLine("Must be an integer! Try again.");
                }
            } while (res != true);

            Console.WriteLine("Enter trading days you want to estimate:");
            do
            {
                string line3 = Console.ReadLine(); // Read string from console
                res = int.TryParse(line3, out days);
                if (res == false)
                {
                    Console.WriteLine("Must be an integer! Try again.");
                }
            } while (res != true);

            Console.WriteLine("Montecarlo simulation:");
            do
            {
                string line4 = Console.ReadLine(); // Read string from console
                res = int.TryParse(line4, out numSim);
                if (res == false)
                {
                    Console.WriteLine("Must be an integer! Try again.");
                }
            } while (res != true);

            //choose if you want the eulero or milstein method
            Console.WriteLine("[0]=Eulero scheme\t[1]=Milstein scheme");
            string line5 = Console.ReadLine(); // Read string from console
            int choose;
            int.TryParse(line5, out choose);

            double dt = 1 / (255 * 0.75 + 256 * 0.25); //time discretization

            Loader.fillMissing(liborTicks, dt);

            double[] backtest = BackTesting.compute(liborTicks, H, numSim, days, hist, time, ticksCount - 1, dt, choose);

            Convergence.compute(liborTicks, hist, time, ticksCount - 1, dt, 300, H);
           
            // output results printed on .txt files
            string name = "cir_calibration_result.txt";

            StreamWriter writer = new StreamWriter(name);
            writer.WriteLine("DATE \t ESTIMATED value \t ACTUAL value");
            for (int i = 0; i < backtest.Length; i++)
            {
                writer.WriteLine(liborTicks[datePos + H + i].getDate().ToString("dd/MM/yyyy")+ "\t"+backtest[i]+"\t"+ liborTicks[datePos + H + i].getValue());
            }
            writer.Close();

            Console.WriteLine("Calibration DONE! cir.txt and hist.txt have been created so you can compare the results.");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ResultPlot());

            Console.Write("Press ENTER to continue");
            Console.ReadLine();
        }
    }
}