using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntersectionSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntersectionSimulatorTests
{
    [TestClass]
    public class TrafficLightTests
    {
        // 4 sec
        const int IntervalOfDirection = 4 * 1000;
        // 1 sec
        const int IntervalOfYellow = 1 * 1000;
        private Intersection NewIntersection( )
        {
            var intersection = new Intersection(IntervalOfDirection);
            return intersection;
        }

        private TrafficLight NewTrafficLight( Intersection intersection)
        {
            var light = new TrafficLight(intersection, IntervalOfYellow, Direction.NS);
            return light;
        }       

        [TestMethod]
        public void DirectionSwitched_sameDirectionIsOn_GreenIsOnOtherAreOff2()
        {
            var intersection = NewIntersection();
            var trafficLight = NewTrafficLight(intersection);
            var switched = new ManualResetEvent(false);
            // full 2 switches and +1 sec to be sure
            var timeToWait = IntervalOfDirection * 2 + 1;

            intersection.DirectionSwitched += (direction, i) =>
            {
                if (direction == trafficLight.Direction) switched.Set();
            };

            intersection.Start();
            switched.WaitOne(timeToWait);
            intersection.Stop();

            Assert.IsFalse(trafficLight.LightsState[LightColor.Red]);
            Assert.IsFalse(trafficLight.LightsState[LightColor.Yellow]);
            Assert.IsTrue(trafficLight.LightsState[LightColor.Green]);
        }

        [TestMethod]
        public void DirectionSwitched_oppositeDirectionIsOn_RedIsOnOtherAreOff( )
        {
            var intersection = new IntersectionStub( IntervalOfDirection );
            var trafficLight = new TrafficLight( intersection, IntervalOfYellow, Direction.EW );

            intersection.SwitchToNS();

            Assert.IsTrue( trafficLight.LightsState[LightColor.Red] );
            Assert.IsFalse( trafficLight.LightsState[LightColor.Yellow] );
            Assert.IsFalse( trafficLight.LightsState[LightColor.Green] );
        }

        [TestMethod]
        public void DirectionSwitched_sameDirectionIsOn_GreenIsOnOtherAreOff()
        {
            var intersection = new IntersectionStub( IntervalOfDirection );
            var trafficLight = new TrafficLight( intersection, IntervalOfYellow, Direction.EW );

            intersection.SwitchToEW();

            Assert.IsFalse(trafficLight.LightsState[LightColor.Red]);
            Assert.IsFalse(trafficLight.LightsState[LightColor.Yellow]);
            Assert.IsTrue(trafficLight.LightsState[LightColor.Green]);
        }

        [TestMethod]
        public void DirectionSwitched_sameDirectionIsOn_YellowSwitchesOnInRightTime( )
        {
            // Doing -10 ms to be on safe side of any small delays.
            var timeStillOff = IntervalOfDirection - IntervalOfYellow - 10;

            var intersection = new IntersectionStub( IntervalOfDirection );
            var trafficLight = new TrafficLight( intersection, IntervalOfYellow, Direction.EW );

            intersection.SwitchToEW();

            Thread.Sleep( timeStillOff );
            Assert.IsFalse( trafficLight.LightsState[LightColor.Yellow] );

            // Compensating 10 ms we subtracted from  timeStillOff and adding 10 ms for any lags.
            Thread.Sleep( 20 );
            Assert.IsTrue( trafficLight.LightsState[LightColor.Yellow] );
        }

        [TestMethod]
        public void DirectionSwitched_sameDirectionIsOn_YellowSwitchesOffInRightTime()
        {
            // Doing -10 ms to be on safe side of any small delays.
            var timeStillOn = IntervalOfDirection - 10;

            var intersection = new IntersectionStub( IntervalOfDirection );
            var trafficLight = new TrafficLight( intersection, IntervalOfYellow, Direction.EW );

            intersection.SwitchToEW();

            Thread.Sleep( timeStillOn );
            Assert.IsTrue( trafficLight.LightsState[LightColor.Yellow] );
            
            // in reality this would happen automatically 
            intersection.SwitchToNS();
            Assert.IsFalse(trafficLight.LightsState[LightColor.Yellow]);
        }

        private class IntersectionStub : IDirectionSwitcher
        {
            public event Action<Direction, int> DirectionSwitched;

            private int _intervalOfDirection;
            public IntersectionStub( int interval)
            {
                _intervalOfDirection = interval;
            }

            public void SwitchToNS( )
            {
                var handler = DirectionSwitched;
                if ( handler != null ) handler( Direction.NS, _intervalOfDirection );
            }

            public void SwitchToEW()
            {
                var handler = DirectionSwitched;
                if (handler != null) handler(Direction.EW, _intervalOfDirection);
            }
        }
    }
}
