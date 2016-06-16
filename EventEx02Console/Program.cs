using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEx02Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Events Example 2:");
            Console.WriteLine("Adopting some .Net Conventions");

            Tweetee janet = new Tweetee("Janet");
            Tweetee trevor = new Tweetee("Trevor");
            Tweetee marcy = new Tweetee("Marcy");

            Tweeter britney = new Tweeter("Britney");
            Tweeter justin = new Tweeter("Justin");
            Tweeter shaq = new Tweeter("Shaq");

            marcy.OptIn(britney);
            janet.OptIn(britney);
            marcy.OptIn(justin);
            trevor.OptIn(justin);
            trevor.OptIn(shaq);

            britney.ComposeTweet("Oh my God!");
            justin.ComposeTweet("I'll be at Brixton on Jan 11th");
            shaq.ComposeTweet("One more year");

            Console.WriteLine("\r\nCan subscribe multiple times:");
            marcy.OptIn(britney);
            britney.ComposeTweet("Oops, I did it again!");

            Console.WriteLine("\r\nOpting out:");
            marcy.OptOut(justin);
            justin.ComposeTweet("It's not mine.");

            Console.WriteLine("\r\nIf no one is listening...");
            trevor.OptOut(shaq);
            shaq.ComposeTweet("Buy my new rap album");

            Console.WriteLine("Press <Enter> to quit the program:");
            Console.ReadLine();
        }
    }

    public class Tweeter
    {
        // Expose an event that subscribers can 
        // listen for to get notified of a tweet.
        public event EventHandler<TweetEventArgs> Tweet;

        string _name;
        public string Name
        {
            get { return _name; }
        }

        public Tweeter(string name)
        {
            _name = name;
        }

        public void ComposeTweet(string message)
        {
            if (Tweet != null)
            {
                Tweet(this, new TweetEventArgs(message, DateTime.Now));
            }
        }
    }

    public class TweetEventArgs : EventArgs
    {
        DateTime _tweetTime;
        string _message;

        public TweetEventArgs(string message, DateTime tweetTime)
        {
            _message = message;
            _tweetTime = tweetTime;
        }

        public string Message
        {
            get { return _message; }
        }

        public DateTime TweetTime
        {
            get { return _tweetTime; }
        }
    }

    public class Tweetee
    {
        string _name;

        public Tweetee(string name)
        {
            _name = name;
        }

        public void OptIn(Tweeter tweeter)
        {
            tweeter.Tweet += LogInfo;
        }

        public void OptOut(Tweeter tweeter)
        {
            tweeter.Tweet -= LogInfo;
        }

        public void LogInfo(object sender, TweetEventArgs e)
        {
            Console.WriteLine("\r\n" + _name
                + " received message \"" + e.Message + "\""
                + "\r\ndated " + e.TweetTime
                + "\r\nfrom " + ((Tweeter)sender).Name);
        }
    }
}
