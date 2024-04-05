using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    Color color = Color.white;

    [SerializeField, Min(0f)]
    float speed = 1f;

    Maze maze;

    int targetIndex;

    Vector3 targetPosition;

    private void Awake()
    {
        GetComponent<Light>().color = color;
        GetComponent<MeshRenderer>().material.color = color;
        ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
        main.startColor = color;
    }

    public void StartNewGame(Maze maze, int2 coordinates)
    {
        this.maze = maze;
        targetIndex = maze.CoordinatesToIndex(coordinates);
        targetPosition = transform.localPosition =
            maze.CoordinatesToWorldPosition(coordinates, transform.localPosition.y);
    }

    void Sniff(ref (int, float) trail, NativeArray<float> scent, int indexOffset)
    {
        int sniffIndex = targetIndex + indexOffset;
        float detectedScent = scent[sniffIndex];
        if (detectedScent > trail.Item2)
        {
            trail = (sniffIndex, detectedScent);
        }
    }

    bool TryFindNewTarget(NativeArray<float> scent)
    {
        MazeFlags cell = maze[targetIndex];
        (int, float) trail = (0, 0f);

        if (cell.Has(MazeFlags.PassageNE))
        {
            Sniff(ref trail, scent, maze.StepN + maze.StepE);
        }
        if (cell.Has(MazeFlags.PassageNW))
        {
            Sniff(ref trail, scent, maze.StepN + maze.StepW);
        }
        if (cell.Has(MazeFlags.PassageSE))
        {
            Sniff(ref trail, scent, maze.StepS + maze.StepE);
        }
        if (cell.Has(MazeFlags.PassageSW))
        {
            Sniff(ref trail, scent, maze.StepS + maze.StepW);
        }
        if (cell.Has(MazeFlags.PassageE))
        {
            Sniff(ref trail, scent, maze.StepE);
        }
        if (cell.Has(MazeFlags.PassageW))
        {
            Sniff(ref trail, scent, maze.StepW);
        }
        if (cell.Has(MazeFlags.PassageN))
        {
            Sniff(ref trail, scent, maze.StepN);
        }
        if (cell.Has(MazeFlags.PassageS))
        {
            Sniff(ref trail, scent, maze.StepS);
        }

        if (trail.Item2 > 0f)
        {
            targetIndex = trail.Item1;
            targetPosition = maze.IndexToWorldPosition(trail.Item1, targetPosition.y);
            return true;
        }
        return false;
    }

    public Vector3 Move(NativeArray<float> scent)
    {
        Vector3 position = transform.localPosition;
        Vector3 targetVector = targetPosition - position;
        float targetDistance = targetVector.magnitude;
        float movement = speed * Time.deltaTime;

        while (movement > targetDistance)
        {
            position = targetPosition;
            if (TryFindNewTarget(scent))
            {
                movement -= targetDistance;
                targetVector = targetPosition - position;
                targetDistance = targetVector.magnitude;
            }
            else
            {
                return transform.localPosition = position;
            }
        }

        return transform.localPosition =
            position + targetVector * (movement / targetDistance);
    }
}
