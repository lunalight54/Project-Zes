using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// data class containg information about game chunk
///
/// chukt is a 3 dimentional cuboidal portion of the world
public class ChunkData
{
    /// 1 dimentional array with blocks' types
    ///
    /// 1 dim array is used to represent block in 3 dims to make code run faster, size of array equals chunkSize * chunkSize * chunkHeight
    public BlockType[] blocks;

    /// vertical dimensions of chunk
    public int chunkSize = 16;

    /// height of the chunk
    public int chunkHeight = 100;

    ///reference to World class
    ///
    /// necessary for neighbouring chunk updates
    public World worldReference;

    /// absolute world position vector
    ///
    /// used to convert betweend absolute and local (for chunk) positions of blocks
    public Vector3Int worldPosition;

    /// flag indicating if chunk was modified by player
    public bool modifiedByThePlayer = false;


    /// constructor
    /// 
    /// @param chynkSize
    /// @param chunkHeight
    /// @param world
    /// @param worldPosition
    public ChunkData(int chunkSize, int chunkHeight, World world, Vector3Int worldPosition)
    {
        this.chunkSize = chunkSize;
        this.chunkHeight = chunkHeight;
        this.worldReference = world;
        this.worldPosition = worldPosition;
        blocks = new BlockType[chunkSize * chunkHeight * chunkSize];
    }
}
