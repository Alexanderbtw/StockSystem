using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class Box
    {
        private int id;
        public int Id 
        { 
            get { return id; } 
            init { id = value; }
        }

        private double x_len;
        public double XLen 
        { 
            get { return x_len; }
            init { x_len = value; } 
        }

        private double z_len;
        public double ZLen 
        {
            get { return z_len; }
            init { z_len = value; }
        }

        private double y_len;
        public double YLen 
        { 
            get { return y_len; } 
            init { y_len = value; }
        }

        public double Volume 
        { 
            get { return Math.Round(x_len * y_len * z_len, 2); } 
        }

        private double weight;
        public double Weight 
        { 
            get { return weight; }
            init { weight = value; } 
        }

        private DateOnly end_date;
        public DateOnly EndDate 
        { 
            get { return end_date; } 
            init { end_date = value; }
        }

        [JsonConstructor]
        public Box(int id, double x_len, double z_len, double y_len, double weight, DateOnly prod_date)
        {
            this.id = id;
            this.x_len = x_len;
            this.y_len = y_len;
            this.z_len = z_len;
            this.weight = weight;
            this.end_date = prod_date.AddDays(100);
        }
    }
}
