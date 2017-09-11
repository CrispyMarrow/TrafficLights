using System;
using System.Threading;

namespace IntersectionSimulator
{
    class Program
    {
        // Use TimeCoefficient to manipulate time scale in application;
        
        // Use this for honest 30 mintues simulation
        public const int TimeCoefficient = 1000;
        
        // Use this for speeded up simulation
        //public const int TimeCoefficient = 10;
        
        // Interval of switching directions NS -> EW -> NS and so on.
        public const int IntervalOfDirection = 5 * 60 * TimeCoefficient;
        
        // How long will yellow be shown, during switch Green -> Red.
        public const int IntervalOfYellow = 30 * TimeCoefficient;
        
        // How long to wait before stoping simulation
        public const int TimeToSimulate = 30 * 60 * TimeCoefficient;

        static void Main(string[] args)
        {
            Console.WriteLine("Simulator: set of traffic lights on intersection.");
            Console.WriteLine("Output is in following format:");
            Console.WriteLine("[ timestamp ] : Traffic light direction [ Red Yellow Green ]");
            Console.WriteLine("For each color:\n'*' - color is on\n'o' - color is off\n");

            var intersection = new Intersection( IntervalOfDirection );
            intersection.DirectionSwitched += ( direction, i ) => Console.WriteLine();

            var nsLight = new TrafficLight( intersection, IntervalOfYellow, Direction.NS );
            nsLight.LightSwitched += OnLightSwitch;

            var ewLight = new TrafficLight( intersection, IntervalOfYellow, Direction.EW );
            ewLight.LightSwitched += OnLightSwitch;

            intersection.Start();
            _startTime = DateTime.UtcNow;
            Thread.Sleep( TimeToSimulate );

            intersection.Stop();

            Console.ReadKey();
        }

        // Is used to display time in console output
        private static DateTime _startTime;
        private static void OnLightSwitch( ITrafficLight trafficLight )
        {
            var timestamp = (int)(DateTime.UtcNow - _startTime).TotalSeconds;

            Console.Write("[ {0:D3} ] : {1} [", timestamp, trafficLight.Direction);
            Console.Write( trafficLight.LightsState[LightColor.Red] ? " *" : " o" );
            Console.Write(trafficLight.LightsState[LightColor.Yellow] ? " *" : " o");
            Console.Write(trafficLight.LightsState[LightColor.Green] ? " *" : " o");
            Console.WriteLine(" ]");
        }
    }
}
