using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolService
{
    public partial class TestThread : Form
    {
        public TestThread()
        {
            InitializeComponent();
        }
        private void PerformLongWork()
        {
            //this simulates some long work
            Thread.Sleep(5000);
            //do some long work here
        }
        private void ClearResultLabel()
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            ClearResultLabel();
            long degreeofParallelism = 10;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            long lowerbound = 0;
            long upperBound = 0;
            List<Task<long>> tasks = new List<Task<long>>();
            long countOfNumbersToBeAddedByOneTask = 100000; //1 lakh
            for (int spawnedThreadNumber = 1; spawnedThreadNumber <= degreeofParallelism; spawnedThreadNumber++)
            {
                lowerbound = ++upperBound;
                upperBound = countOfNumbersToBeAddedByOneTask * spawnedThreadNumber;
                //copying the values to be passed to task in local variables to avoid closure variable
                //issue. You can safely ignore this concept for now to avoid a detour. For now you
                //can assume I've done bad programming by creating two new local variables unnecessarily.
                var lowerLimit = lowerbound;
                var upperLimit = upperBound;
                
                tasks.Add(Task.Run(() => AddNumbersBetweenLimits(lowerLimit, upperLimit)));

            }

            Task.WhenAll(tasks).ContinueWith(task => CreateFinalSum(tasks));
            stopwatch.Stop();
            MessageBox.Show("time taken to do sum operation (in miliseconds) : " + stopwatch.ElapsedMilliseconds);
        }

        private void TestThread_Load(object sender, EventArgs e)
        {

        }


        private static void CreateFinalSum(List<Task<long>> tasks)
        {
            var finalValue = tasks.Sum(task => task.Result);
            
            MessageBox.Show("Sum is : " + finalValue);
        }

        private static long AddNumbersBetweenLimits(long lowerLimitInclusive, long upperLimitInclusive)
        {
            long sumTotal = 0;
            for (long i = lowerLimitInclusive; i <= upperLimitInclusive; i++)
            {
                sumTotal += i;
            }

            return sumTotal;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearResultLabel();
            long degreeofParallelism = 10;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            long lowerbound = 0;
            long upperBound = 0;
            //List<Task<long>> tasks = new List<Task<long>>();
            List<Thread> lst = new List<Thread>();

            long countOfNumbersToBeAddedByOneTask = 100000; //1 lakh
            long Sum = 0;
            for (int spawnedThreadNumber = 1; spawnedThreadNumber <= degreeofParallelism; spawnedThreadNumber++)
            {
                lowerbound = ++upperBound;
                upperBound = countOfNumbersToBeAddedByOneTask * spawnedThreadNumber;
                //copying the values to be passed to task in local variables to avoid closure variable
                //issue. You can safely ignore this concept for now to avoid a detour. For now you
                //can assume I've done bad programming by creating two new local variables unnecessarily.
                var lowerLimit = lowerbound;
                var upperLimit = upperBound;
                Sum += AddNumbersBetweenLimits(lowerbound, upperBound);
            }

            
            stopwatch.Stop();
            MessageBox.Show("time taken to do sum operation (in miliseconds) : " + stopwatch.ElapsedMilliseconds
                + Environment.NewLine + 
                " Sum is " + Sum.ToString ());

        }
    }
}
