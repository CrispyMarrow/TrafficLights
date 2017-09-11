namespace IntersectionSimulator
{
    interface IIntersection
    {
        void Start( );
        void Stop( );
        Direction CurrentDirection { get; }        
    }
}
