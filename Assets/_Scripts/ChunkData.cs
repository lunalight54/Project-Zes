using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public BlockType[] blocks;  //change to properties
    public int chunkSize = 16;
    public int chunkHeight = 100;
    public World worldReference;
    public Vector3Int worldPosition;

    public bool modifiedByThePlayer = false;

    public ChunkData(int chunkSize, int chunkHeight, World world, Vector3Int worldPosition)
    {
        this.chunkSize = chunkSize;
        this.chunkHeight = chunkHeight;
        this.worldReference = world;
        this.worldPosition = worldPosition;
        blocks = new BlockType[chunkSize * chunkHeight * chunkSize];
    }
}
