using System;

namespace ScoopProject_Group3
{
    /*we create an object called Libor with the 
  date parameter and the corrispondent value in that day*/
    class Libor
    {
        private DateTime Date;
        private double Value;

        public void setDate(DateTime date)
        {
            this.Date = date;
        }
        public DateTime getDate()
        {
            return this.Date;
        }
        public void setValue(double value)
        {
            this.Value = value;
        }
        public double getValue()
        {
            return this.Value;
        }
    }
}
