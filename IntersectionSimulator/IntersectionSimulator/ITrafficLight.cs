using System;
using System.Collections.Generic;

namespace IntersectionSimulator
{
    public interface ITrafficLight
    {
        event Action<ITrafficLight> LightSwitched;
        Direction Direction { get; }
        Dictionary<LightColor, bool> LightsState { get; }
    }
}
