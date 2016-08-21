using System;
using System.Globalization;
using System.IO;

namespace ScoopProject_Group3
{
    class Loader
    {
        //we create a method that count how many objects we have to manage(the number of rates taking in the web)
        public static int CountEntries(string filePath)
        {
            int count = 0;

            try
            {
                StreamReader reader = new StreamReader(filePath); //class StramReader to reader something in a specific file in a specific path
                // get rid of header entry
                reader.ReadLine();
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(line)) //if the line have no characters goes out of cicle
                    {
                        string[] record = line.Split(','); //we go on the next "word" using the split
                        count++;
                    }
                }
                reader.Close();
            }
            catch (FileNotFoundException e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("\n The file could not be read! \n");
                Console.WriteLine(e.Message);
            }
            return count;
        }

        //method that load the data from a specific path
        public static void loadTicks(string filePath, Libor[] array)
        {
            try
            {
                int i = 0;
                double tickValue;
                StreamReader reader = new StreamReader(filePath);
                // get rid of header entry
                reader.ReadLine();
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(line))
                    {
                        string[] record = line.Split(',');
                        DateTime tickDate = DateTime.ParseExact(record[0], "yyyy-MM-dd", null);
                        array[i].setDate(tickDate);
                        if (double.TryParse(record[1], NumberStyles.Any, CultureInfo.InvariantCulture, out tickValue))
                        {
                            array[i].setValue(tickValue);
                        }
                        else
                        {
                            array[i].setValue(-10);
                        }
                        i++;
                    }
                }
                reader.Close();
            }
            catch (FileNotFoundException e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("\n The file could not be read! \n");
                Console.WriteLine(e.Message);
            }

        }

        public static void fillMissing(Libor[] array, double dt)
        {
            CirModel tempModel = new CirModel();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].getValue() == -10)
                {
                    tempModel.estimate(array, 60, i - 1, dt);
                    array[i].setValue(MontecarloSim.compute(array, 100, 1, tempModel, array[i - 1].getValue(), dt, 0));
                }
            }

        }
    }
}
