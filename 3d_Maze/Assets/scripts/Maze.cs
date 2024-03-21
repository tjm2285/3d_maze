using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

public struct Maze
{
    int2 size;

    public int Length => size.x * size.y;

    public Maze (int2 size)
    {
        this.size = size;
    }

    public int2 IndexToCoordinates(int index)
    {
        int2 coordinates;
        coordinates.y = index / size.x;
        coordinates.x = index - size.x * coordinates.y;

        return coordinates;
    }

    public Vector3 CoordinatesToWorldPosition(int2 coordinates, float Y = 0f) => 
        new Vector3(
        2f * coordinates.x + 1f - size.x,
        2f * coordinates.y + 1f - size.y
        );

    public Vector3 IndexToWorldPosition(int index, float y = 0f) => CoordinatesToWorldPosition(IndexToCoordinates(index),y);
}
