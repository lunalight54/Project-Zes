using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockData
{
    public static readonly Vector3[] voxelVertices = new Vector3[8] { //in case of confusion check cube image

        new Vector3(0.0f, 0.0f, 0.0f), //vector 0
        new Vector3(1.0f, 0.0f, 0.0f), //vector 1
        new Vector3(1.0f, 1.0f, 0.0f), //vector 2
        new Vector3(0.0f, 1.0f, 0.0f), //vector 3
        new Vector3(0.0f, 0.0f, 1.0f), //vector 4
        new Vector3(1.0f, 0.0f, 1.0f), //vector 5
        new Vector3(1.0f, 1.0f, 1.0f), //vector 6
        new Vector3(0.0f, 1.0f, 1.0f), //vector 7
    };

    public static readonly int[,] blockFaces = new int[6, 6] {

        {0, 3, 1, 1, 3, 2}, // Back Face
		{5, 6, 4, 4, 6, 7}, // Front Face
		{3, 7, 2, 2, 7, 6}, // Top Face
		{1, 5, 0, 0, 5, 4}, // Bottom Face
		{4, 7, 0, 0, 7, 3}, // Left Face
		{1, 2, 5, 5, 2, 6} // Right Face

	};

    public static readonly Vector2[] voxelUvs = new Vector2[6] { // 2d texture vertices coordinates

        new Vector2 (0.0f, 0.0f),
        new Vector2 (0.0f, 1.0f),
        new Vector2 (1.0f, 0.0f),
        new Vector2 (1.0f, 0.0f),
        new Vector2 (0.0f, 1.0f),
        new Vector2 (1.0f, 1.0f)

    };


}
