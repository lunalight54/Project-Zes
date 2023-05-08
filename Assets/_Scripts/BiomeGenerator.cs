using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    public int waterThreshold = 50;
    public NoiseSettings biomeNoiseSettings;
    public DomainWarping domainWarping;
    public BlockLayerHandler startLayerHandler;

    public List<BlockLayerHandler> additionalLayerHandler;

    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int mapSeedOffset) {
        biomeNoiseSettings.worldOffset = mapSeedOffset;
        int groundPosition = GetSurfaceHeightNoise(data.worldPosition.x +  x, data.worldPosition.z + z, data.chunkHeight);
        for (int y = data.worldPosition.y; y < data.worldPosition.y + data.chunkHeight; y++)
        {
            startLayerHandler.Handle(data, x, y, z, groundPosition, mapSeedOffset);
        }
        foreach(var layer in additionalLayerHandler)
        {
            layer.Handle(data, x, data.worldPosition.y, z, groundPosition, mapSeedOffset);
        }
        return data;
    }

    private int GetSurfaceHeightNoise(int x, int z, int chunkHeight) {
        //float terrainHeight = Noise.OctavePerlin(x, z, biomeNoiseSettings);
        float terrainHeight = domainWarping.GenerateDomainNoise(x, z, biomeNoiseSettings);
        terrainHeight = Noise.Redistribution(terrainHeight, biomeNoiseSettings);
        int surfaceHeight = Noise.RemapValue01ToInt(terrainHeight, 0, chunkHeight);
        return surfaceHeight;
    }
}
