using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField]
    MazeVisualization visualization;

    [SerializeField]
    int2 mazeSize = int2(20, 20);

    [SerializeField, Tooltip("Use zero for random seed.")]
    int seed;

    [SerializeField, Range(0f, 1f)]
    float pickLastProbability = 0.5f;

    Maze maze;

    private void Awake()
    {
        maze = new Maze(mazeSize);

        new GenerateMazeJob
        {
            maze = maze,
            seed = seed != 0 ? seed : Random.Range(1, int.MaxValue),
            pickLastProbability = pickLastProbability
        }.Schedule().Complete();

        visualization.Visualize(maze);
    }

    void OnDestroy()
    {
        maze.Dispose();
    }
}
