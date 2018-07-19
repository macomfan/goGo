using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoModel;


namespace goTest
{

    public class PerformanceCounter
    {
        long freq_ = 0;
        long start_ = 0;
        long stop_ = 0;

        public PerformanceCounter()
        {
            freq_ = System.Diagnostics.Stopwatch.Frequency;
        }

        public void Start()
        {
            start_ = System.Diagnostics.Stopwatch.GetTimestamp();
        }

        public double Count()
        {
            stop_ = System.Diagnostics.Stopwatch.GetTimestamp();
            long count = stop_ - start_;
            start_ = stop_;
            return (double)count / (double)freq_;
        }
    }

    [TestClass]
    public class testPerformance
    {
        [TestMethod]
        public void TestMethod1()
        {
            PerformanceCounter c = new PerformanceCounter();
            c.Start();
            GoLayout layout = new GoLayout();
            bool isBlack = false;
            for (int i = 0; i < GoLayout.SIZE; i++)
            {
                for (int j = 0; j < GoLayout.SIZE; j++)
                {
                    if (isBlack)
                    {
                        layout.SetupDian(new GoCoord(i, j), GoDianType.BLACK);
                    }
                    else
                    {
                        layout.SetupDian(new GoCoord(i, j), GoDianType.WHITE);
                    }
                    isBlack = !isBlack;
                }
            }
            double timestamp1 = c.Count();
            for (int i = 0; i < GoLayout.SIZE; i++)
            {
                for (int j = 0; j < GoLayout.SIZE; j++)
                {
                    layout.SetupDian(new GoCoord(i, j), GoDianType.EMPTY);
                }
            }
            double timestamp2 = c.Count();
            int a = 0;
        }
    }
}
