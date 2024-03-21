using UnityEngine;

[CreateAssetMenu]
public class MazeVisualization : ScriptableObject
{
    [SerializeField]
    MazeCellObject end, straight, corner, tJunction, xJunction;

    public void Visualize(Maze maze)
    {
        for (int i = 0; i < maze.Length; i++)
        {
            MazeCellObject instance = xJunction.GetInstance();
            instance.transform.localPosition = maze.IndexToWorldPosition(i);
        }
    }
}
