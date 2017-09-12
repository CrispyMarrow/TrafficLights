using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IntersectionSimulator
{
    public enum LightColor
    {
        None = 0,
        Red = 1,
        Yellow = 2,
        Green = 3
    }

    public class TrafficLight : ITrafficLight
    {
        public event Action<ITrafficLight> LightSwitched;
        public Direction Direction { get; private set; }
        public Dictionary<LightColor, bool> LightsState
        {
            // Creating new Dictionary will prevent modifying of LightsState
            // by any outside users of this class.
            // Note: it currently works because both Key and Value are primitive types.
            // If either will be changed to reference type,
            // make sure to create here deep copy or switch to other implementation at all.
            get { return new Dictionary<LightColor, bool>( _lights ); }
        } 

        public TrafficLight( IDirectionSwitcher directionSwitcher, int intervalOfYellow, Direction direction )
        {
            directionSwitcher.DirectionSwitched += OnDirectionSwitched;
            _timer = new Timer( SwitchToYellow );

            _intervalOfYellow = intervalOfYellow;
            Direction = direction;

            _lights = new Dictionary<LightColor, bool>
            {
                { LightColor.Red, false },
                { LightColor.Yellow, false },
                { LightColor.Green, false }
            };
        }

        private readonly Timer _timer;
        private readonly int _intervalOfYellow;
        private readonly Dictionary<LightColor, bool> _lights;

        private void OnDirectionSwitched( Direction direction, int intervalOfNewDirection )
        {
            if ( direction == Direction )
            {
                SwitchToGreen( intervalOfNewDirection );
            }
            else
            {
                SwitchToRed();
            }
        }

        private void SwitchToRed()
        {
            _lights[LightColor.Red] = true;
            _lights[LightColor.Yellow] = false;
            _lights[LightColor.Green] = false;

            FireSwitchEvent();
        }

        private void SwitchToGreen( int intervalOfGreen )
        {
            _lights[LightColor.Red] = false;
            _lights[LightColor.Yellow] = false;
            _lights[LightColor.Green] = true;

            var startYellow = Math.Max( intervalOfGreen - _intervalOfYellow, 0 );
            _timer.Change( startYellow, Timeout.Infinite );

            FireSwitchEvent();
        }

        private void SwitchToYellow( object state )
        {
            _lights[LightColor.Yellow] = true;
            FireSwitchEvent();
        }

        private void FireSwitchEvent( )
        {
            var handler = LightSwitched;
            if ( handler != null ) { handler( this ); }
        }
    }
}
