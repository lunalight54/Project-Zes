using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockDataManager : MonoBehaviour
{
    public static float textureOffset = 0.001f;
    public static float tileSizeX, tileSizeY;
    public static Dictionary<BlockType, TextureData> blockTextureDataDictionary = new Dictionary<BlockType, TextureData>();
    public static Dictionary<BlockType, Item> blockDataInfoDictionary = new Dictionary<BlockType, Item>();

    [FormerlySerializedAs("BlockData")] [FormerlySerializedAs("textureData")] public BlockDataSO BlockDataSO;

    private void Awake()
    {
        foreach (var item in BlockDataSO.textureDataList)
        {
            if (blockTextureDataDictionary.ContainsKey(item.blockType) == false)
            { blockTextureDataDictionary.Add(item.blockType, item);
            };
        }
        tileSizeX = BlockDataSO.textureSizeX;
        tileSizeY = BlockDataSO.textureSizeY;
        
        foreach (var item in BlockDataSO.BlockDataInfoList)
        {
            if (blockDataInfoDictionary.ContainsKey(item.blockType) == false)
            { 
                blockDataInfoDictionary.Add(item.blockType, item.item);
                if(item.item != null)
                    Debug.Log("loaded inventory item: " + item.blockType);
            };
        }
    }
}