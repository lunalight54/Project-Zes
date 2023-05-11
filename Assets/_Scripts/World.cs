using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class World : MonoBehaviour
{
    public int mapSizeInChunks = 6;
    public int chunkSize = 16, chunkHeight = 100;
    public int chunkDrawingRange = 8;

    public GameObject chunkPrefab;

    public TerrainGenerator terrainGenerator;
    public Vector2Int mapSeedOffset;

    public Dictionary<Vector3Int, ChunkData> chunkDataDictionary = new Dictionary<Vector3Int, ChunkData>();
    public Dictionary<Vector3Int, ChunkRenderer> chunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>();

    public UnityEvent OnWorldCreated, OnNewChunksGenerated;

    public WorldData worldData { get; set; }

    private void Awake()
    {
        worldData = new WorldData
        {
            chunkHeight = this.chunkHeight,
            chunkSize = this.chunkSize,
            chunkDataDictionary = new Dictionary<Vector3Int, ChunkData>(),
            chunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>()
        };
        GenerateWorld(Vector3Int.zero);
    }

    public void GenerateWorld()
    {
        GenerateWorld(Vector3Int.zero);
    }

    private void GenerateWorld(Vector3Int position)
    {

        WorldGenerationData worldGenerationData= GetPositionsThatPlayersSees(position);

        foreach (Vector3Int pos in worldGenerationData.chunkPositionsToRemove)
        {
            WorldDataHelper.RemoveChunk(this, pos);
        }

        foreach (Vector3Int pos in worldGenerationData.chunkDataToRemove)
        {
            WorldDataHelper.RemoveChunkData(this, pos);
        }

        foreach (var pos in worldGenerationData.chunkDataPositionsToCreate)
        {
            ChunkData data = new ChunkData(chunkSize, chunkHeight, this, pos);
            ChunkData newData = terrainGenerator.GenerateChunkData(data, mapSeedOffset);
            worldData.chunkDataDictionary.Add(pos, newData);
        }

        foreach (var pos in worldGenerationData.chunkPositionsToCreate)
        {
            ChunkData data = worldData.chunkDataDictionary[pos];
            MeshData meshdata = Chunk.GetChunkMeshData(data);
            GameObject chunkObject = Instantiate(chunkPrefab, data.worldPosition, Quaternion.identity);
            ChunkRenderer chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
            worldData.chunkDictionary.Add(data.worldPosition, chunkRenderer);
            chunkRenderer.InitializeChunk(data);
            chunkRenderer.UpdateChunk(meshdata);
        }

        OnWorldCreated?.Invoke();
    }

    internal bool SetBlock(RaycastHit hit, BlockType blockType, bool toPlace)
    {
        ChunkRenderer chunk = hit.collider.GetComponent<ChunkRenderer>();
        if (chunk == null)
            return false;
        Vector3Int pos = GetBlockPos(hit, toPlace);

        WorldDataHelper.SetBlock(chunk.ChunkData.worldReference, pos, blockType);
        chunk.ModifiedByThePlayer = true;

        if (Chunk.IsOnEdge(chunk.ChunkData, pos))
        {
            List<ChunkData> neighbourDataList = Chunk.GetEdgeNeighbourChunk(chunk.ChunkData, pos);
            foreach (ChunkData neighbourData in neighbourDataList)
            {
                //neighbourData.modifiedByThePlayer = true;
                ChunkRenderer chunkToUpdate = WorldDataHelper.GetChunk(neighbourData.worldReference, neighbourData.worldPosition);
                if (chunkToUpdate != null)
                    chunkToUpdate.UpdateChunk();
            }
        }

        chunk.UpdateChunk();
        return true;
    }

    private Vector3Int GetBlockPos(RaycastHit hit, bool toPlace)
    {
       
            Vector3 pos = new Vector3(
                GetBlockPositionIn(hit.point.x, hit.normal.x, toPlace),
                GetBlockPositionIn(hit.point.y, hit.normal.y, toPlace),
                GetBlockPositionIn(hit.point.z, hit.normal.z, toPlace)
            );

            return Vector3Int.RoundToInt(pos);


    }

    private float GetBlockPositionIn(float pos, float normal, bool toPlace)
    {
        if (Mathf.Abs(pos % 1) == 0.5f)
        {
            if (toPlace) {
                pos += (normal / 2);

            }
            else {
                pos -= (normal / 2);
            }
        }

        return (float)pos;
    }
    

    internal void RemoveChunk(ChunkRenderer chunk)
    {
        chunk.gameObject.SetActive(false);
    }

    private WorldGenerationData GetPositionsThatPlayersSees(Vector3Int playerPosition)
    {
        List<Vector3Int> allChunkPositionsNeeded = WorldDataHelper.GetChunkPositionsAroundPlayer(this, playerPosition);
        List<Vector3Int> allChunkDataPositionsNeeded = WorldDataHelper.GetDataPositionsAroundPlayer(this, playerPosition);

        List<Vector3Int> chunkPositionsToCreate = WorldDataHelper.SelectPositionsToCreate(worldData, allChunkPositionsNeeded, playerPosition);
        List<Vector3Int> chunkDataPositionsToCreate = WorldDataHelper.SelectDataPositionsToCreate(worldData, allChunkDataPositionsNeeded, playerPosition);

        List<Vector3Int> chunkPositionToRemove = WorldDataHelper.GetUnnededChunks(worldData, allChunkPositionsNeeded);
        List<Vector3Int> chunkDataToRemove = WorldDataHelper.GetUnnededData(worldData, allChunkDataPositionsNeeded);

        WorldGenerationData data = new WorldGenerationData
        {
            chunkPositionsToCreate = chunkPositionsToCreate,
            chunkDataPositionsToCreate = chunkDataPositionsToCreate,
            chunkPositionsToRemove = chunkPositionToRemove,
            chunkDataToRemove = chunkDataToRemove
        };
        return data;
    }

    internal void LoadAdditionalChunkRequest(GameObject player)
    {
        Debug.Log("Load More Chunks");
        GenerateWorld(Vector3Int.RoundToInt(player.transform.position));
        OnNewChunksGenerated?.Invoke();
    }

    internal BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        Vector3Int pos = Chunk.ChunkPositionFromBlockCoords(this, x, y, z);
        ChunkData containerChunk = null;

        worldData.chunkDataDictionary.TryGetValue(pos, out containerChunk);

        if (containerChunk == null)
            return BlockType.Nothing;
        Vector3Int blockInCHunkCoordinates = Chunk.GetBlockInChunkCoordinates(containerChunk, new Vector3Int(x, y, z));
        return Chunk.GetBlockFromChunkCoordinates(containerChunk, blockInCHunkCoordinates);
    }

    public struct WorldGenerationData
    {
        public List<Vector3Int> chunkPositionsToCreate;
        public List<Vector3Int> chunkDataPositionsToCreate;
        public List<Vector3Int> chunkPositionsToRemove;
        public List<Vector3Int> chunkDataToRemove;
    }

    public struct WorldData
    {
        public Dictionary<Vector3Int, ChunkData> chunkDataDictionary;
        public Dictionary<Vector3Int, ChunkRenderer> chunkDictionary;
        public int chunkSize;
        public int chunkHeight;
    }
}