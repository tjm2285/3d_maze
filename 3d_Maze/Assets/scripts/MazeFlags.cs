[System.Flags]
public enum MazeFlags
{
    Empty = 0,

    PassageN = 0b0001,
    PassageE = 0b0010,
    PassageS = 0b0100,
    PassageW = 0b1000,

    PassageAll = 0b1111
}
