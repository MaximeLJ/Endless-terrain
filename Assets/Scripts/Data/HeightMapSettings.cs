using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class HeightMapSettings : UpdatableData
{
    public NoiseSettings noiseSettings;
    public const int rangeRadius = 10;
    [HideInInspector]
    public int range = 10;

    public bool useSquareFalloff;
    public bool useRadialFalloff;
    [Range(1,5)]
    public int falloffMapSize = 1;
    [Range(0,3)]
    public float squareFalloffPersistance;
    [Range(0,rangeRadius)]
    public float innerRadius;
    [Range(0,rangeRadius)]
    public float outerRadius;

    public float heightMultiplier;
    public AnimationCurve heightCurve;

    public float minHeight {
        get {
            return heightMultiplier * heightCurve.Evaluate(0);
        }
    }

    public float maxHeight {
        get {
            return heightMultiplier * heightCurve.Evaluate(1);
        }
    }

    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        noiseSettings.ValidateValues();
        base.OnValidate();
    }
    #endif
}
