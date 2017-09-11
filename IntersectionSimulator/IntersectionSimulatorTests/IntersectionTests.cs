using System.Threading;
using IntersectionSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntersectionSimulatorTests
{
    [TestClass]
    public class IntersectionTests
    {
        [TestMethod]
        public void Constructor_doesNotStartAutomatically()
        {
            var changed = new ManualResetEvent(false);

            //2 seconds
            var intervalOfDirection = 2 * 1000;
            var intersection = new Intersection(intervalOfDirection);

            intersection.DirectionSwitched += ( directions, i ) => changed.Set();

            //Adding +10 ms to be on safe side of any small delays.
            Assert.IsFalse( changed.WaitOne( intervalOfDirection + 10 ) );
        }

        [TestMethod]
        public void Start_waitForInterval_SwitchedDirection()
        {
            var changed = new ManualResetEvent(false);

            //2 seconds
            var intervalOfDirection = 2 * 1000;
            var intersection = new Intersection(intervalOfDirection);

            intersection.DirectionSwitched += (directions, i) => changed.Set();
            intersection.Start();

            //Adding +10 ms to be on safe side of any small delays.
            Assert.IsTrue(changed.WaitOne(intervalOfDirection + 10));
        }

        [TestMethod]
        public void Start_waitForInterval_SwitchedDirectionIntoOpposite()
        {
            var changed = new ManualResetEvent(false);

            //2 seconds
            var intervalOfDirection = 2 * 1000;
            var intersection = new Intersection(intervalOfDirection);
            var wasDirection = intersection.CurrentDirection;
            var newDirection = wasDirection;

            intersection.DirectionSwitched += (directions, i) => newDirection = intersection.CurrentDirection;
            intersection.Start();

            //Adding +10 ms to be on safe side of any small delays.
            changed.WaitOne( intervalOfDirection + 10 );
            Assert.AreNotEqual( wasDirection, newDirection );
            
        }

        [TestMethod]
        public void Stop_waitForInterval_NotSwitchedDirection()
        {
            var changed = new ManualResetEvent(false);

            //2 seconds
            var intervalOfDirection = 2 * 1000;
            var intersection = new Intersection(intervalOfDirection);

            intersection.DirectionSwitched += (directions, i) => changed.Set();
            intersection.Start();
            intersection.Stop();

            //Adding +10 ms to be on safe side of any small delays.
            Assert.IsFalse(changed.WaitOne(intervalOfDirection + 10));
        }
    }
}
