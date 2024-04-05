using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

    [SerializeField, Range(0f, 1f)]
    float openDeadEndProbability = 0.5f;

    [SerializeField, Range(0f, 1f)]
    float openArbitraryProbability = 0.5f;

    [SerializeField]
    Player player;

    [SerializeField]
    Agent[] agents;

    Maze maze;
    Scent scent;

    private void Awake()
    {
        maze = new Maze(mazeSize);
        scent = new Scent(maze);
        if (seed != 0)
        {
            Random.InitState(seed);
        }

        player.StartNewGame(new Vector3(1f, 0f, 1f));

        new FindDiagonalPassagesJob
    {
        maze = maze
    }.ScheduleParallel(
        maze.Length, maze.SizeEW, new GenerateMazeJob
        {
            maze = maze,
            seed = seed != 0 ? seed : Random.Range(1, int.MaxValue),
            pickLastProbability = pickLastProbability,
            openDeadEndProbability = openDeadEndProbability,
            openArbitraryProbability = openArbitraryProbability
        }.Schedule()
    ).Complete();

        visualization.Visualize(maze);

        for (int i = 0; i < agents.Length; i++)
        {
            var coordinates =
                int2(Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y));
            agents[i].StartNewGame(maze, coordinates);
        }
    }
    void Update()
    {
        NativeArray<float> currentScent = scent.Disperse(maze, player.Move());
        for (int i = 0; i < agents.Length; i++)
        {
            agents[i].Move(currentScent);
        }
    }
    void OnDestroy()
    {
        maze.Dispose();
        scent.Dispose();
    }
}
