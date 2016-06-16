using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace EventEx01Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Using a delegate for Publish-Subscribe");
            var child = new Publisher();
            var mom = new Subscriber("Mom");
            var dad = new Subscriber("Dad");
            mom.StartMonitoring(child);
            dad.StartMonitoring(child);

            child.DoTask("Do Homework", 3);

            // This is bad, code outside publisher
            // should not have unlimited access to 
            // delegate! Otherwise, code outside of
            // the publishing class can mess with
            // the delegate's invocation list.
            child.ReportProgress = null;

            // Mom and Dad don't get notified,
            // because delegate was reset
            child.DoTask("Write Essay", 4);

            Console.WriteLine("Press <Enter> to quit the program:");
            Console.ReadLine();
        }
    }

    public class Subscriber
    {
        string _name;

        public Subscriber(string name)
        {
            _name = name;
        }

        public void StartMonitoring(Publisher pub)
        {
            pub.ReportProgress += LogInfo;
        }

        public void StopMonitoring(Publisher pub)
        {
            pub.ReportProgress -= LogInfo;
        }

        public void LogInfo(string message)
        {
            Console.WriteLine("Subscriber " + _name
                + " received message " + message);
        }
    }

    public class Publisher
    {
        // Expose a delegate as a public property,
        // So code accessing an instane of the 
        // publisher can specify the delegate.
        public Action<string> ReportProgress
        {
            get; set;
        }
        //public event Action<string> ReportProgress = null;

        public void DoTask(string taskName, int numberOfSteps)
        {
            if (ReportProgress != null)
            {
                ReportProgress("Starting Task " + taskName);
            }
            for (var step = 1; step <= numberOfSteps; step++)
            {
                Thread.Sleep(1000);
                if (ReportProgress != null)
                {
                    ReportProgress("Completed Step " + step);
                }
            }
            if (ReportProgress != null)
            {
                ReportProgress("Finished Task " + taskName);
            }
        }

    }
}
