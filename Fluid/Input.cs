namespace Fluid
{
    public enum Input : uint
    {
        Nothing = 0,
        HoldLeft = 1 << 0,
        HoldUp = 1 << 1,
        HoldRight = 1 << 2,
        HoldDown = 1 << 3,
        HoldSpace = 1 << 4,
        ReleaseLeft = 1 << 5,
        ReleaseUp = 1 << 6,
        ReleaseRight = 1 << 7,
        ReleaseDown = 1 << 8,
        ReleaseSpace = 1 << 9

    }
}
