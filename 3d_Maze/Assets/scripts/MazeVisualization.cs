using UnityEngine;

[CreateAssetMenu]
public class MazeVisualization : ScriptableObject
{
    static Quaternion[] rotations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };

    [SerializeField]
    MazeCellObject deadEnd, straight, corner, tJunction, xJunction;

    public void Visualize(Maze maze)
    {
        for (int i = 0; i < maze.Length; i++)
        {
            (MazeCellObject, int) prefabWithRotation = GetPrefab(maze[i]);
            MazeCellObject instance = prefabWithRotation.Item1.GetInstance();
            instance.transform.SetPositionAndRotation(
                maze.IndexToWorldPosition(i), rotations[prefabWithRotation.Item2]
            );
        }
    }

    (MazeCellObject, int) GetPrefab(MazeFlags flags) => flags switch
    {
        MazeFlags.PassageN => (deadEnd, 0),
        MazeFlags.PassageE => (deadEnd, 1),
        MazeFlags.PassageS => (deadEnd, 2),
        MazeFlags.PassageW => (deadEnd, 3),

        MazeFlags.PassageN | MazeFlags.PassageS => (straight, 0),
        MazeFlags.PassageE | MazeFlags.PassageW => (straight, 1),

        MazeFlags.PassageN | MazeFlags.PassageE => (corner, 0),
        MazeFlags.PassageE | MazeFlags.PassageS => (corner, 1),
        MazeFlags.PassageS | MazeFlags.PassageW => (corner, 2),
        MazeFlags.PassageW | MazeFlags.PassageN => (corner, 3),

        MazeFlags.PassageAll & ~MazeFlags.PassageW => (tJunction, 0),
        MazeFlags.PassageAll & ~MazeFlags.PassageN => (tJunction, 1),
        MazeFlags.PassageAll & ~MazeFlags.PassageE => (tJunction, 2),
        MazeFlags.PassageAll & ~MazeFlags.PassageS => (tJunction, 3),

        _ => (xJunction, 0)
    };
}
