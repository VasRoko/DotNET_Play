using System;

namespace CSVProj
{
    public class CarStatisctics
    {
        public CarStatisctics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;

        }
        public int Total { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public int Count { get; set; }
        public double Avg { get; set; }

        public CarStatisctics Accumulate(Car c)
        {
            Count += 1;
            Total += c.Comb;
            Max = Math.Max(Max, c.Comb);
            Min = Math.Min(Min, c.Comb);

            return this;
        }

        public CarStatisctics Compute()
        {
            Avg = Total / Count;
            return this;
        }
    }
}
