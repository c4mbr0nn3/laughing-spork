using System;

namespace ScoopProject_Group3
{
    class Finder
    {
        /*this is a recursive method that use the binary search apllied with the DataTime
          object which find a specific date and return the number of the position in the array
          of the respective date*/
        public static int find(Libor[] array, DateTime date, int min, int max)
        {
            if (max < min)
            {
                return -1;
            }
            int mid = (max + min) / 2;

            //if the date in mid position is equal to the date we give as input it turns 0 an so we founded our date
            if (DateTime.Compare(array[mid].getDate(), date) == 0)
            {
                return mid;
            }

            //if the date in mid position happens after than our input it tourns >0 and we redo the finder method
            if (DateTime.Compare(array[mid].getDate(), date) > 0)
            {
                return find(array, date, min, mid - 1);
            }

            //the last case is if the date in mid happens before our input we redo the finder method
            return find(array, date, mid + 1, max);
        }
    }
}
