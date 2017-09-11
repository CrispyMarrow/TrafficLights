using System;

namespace IntersectionSimulator
{
    public interface IDirectionSwitcher
    {
        // Shows new direction and how long it will be on
        event Action<Direction, int> DirectionSwitched;
    }
}
