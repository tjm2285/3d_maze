using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
public class Game : MonoBehaviour
{
    [SerializeField]
    MazeVisualization visualization;

    [SerializeField]
    int2 mazeSize = int2(20, 20);

    Maze maze;

    private void Awake()
    {
        maze = new Maze(mazeSize);
        visualization.Visualize(maze);
    }

    void OnDestroy()
    {
        maze.Dispose();
    }
}
