using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Represents block types avaliable in ganerated world
public enum BlockType
{
    Nothing, ///< represent empty block
    Air, ///< represents air block
    Grass_Dirt, ///< represents grass block
    Dirt, ///< represents dirt block
    Grass_Stone, ///< represents air block
    Stone, ///< represents stone block
    TreeTrunk, ///< represents treetrunk block
    TreeLeafesTransparent, ///< represents transparent tree leafes block
    TreeLeafsSolid, ///< represents tree leafs block
    Water, ///< represents water block
    Sand ///< represents sand block
}