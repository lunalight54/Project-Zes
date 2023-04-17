using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "noiseSettings", menuName = "Data/NoiseSettings")]

public class NoiseSettings : ScriptableObject
{
    //change to private and add [serializeField] attribute https://answers.unity.com/questions/1631672/make-variable-visible-in-inspector.html
    public float noiseZoom;
    public int octaves;  //decrreasing this value decrease the detailiness
    public Vector2Int offest;
    public Vector2Int worldOffset;
    public float persistance;  //higher value = more peaks
    public float redistributionModifier;
    public float exponent;  //higher = more platoes, lower = more hills
}