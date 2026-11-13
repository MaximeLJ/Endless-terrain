using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings heightMapSettings, MeshSettings meshSettings, Vector2 sampleCentre)
    {
        float[,] values = Noise.GenerateNoiseMap(width, height, heightMapSettings.noiseSettings, sampleCentre);

        AnimationCurve heightCurve_threadsafe = new AnimationCurve(heightMapSettings.heightCurve.keys);

        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                values[i,j] *= heightCurve_threadsafe.Evaluate(values[i,j]) * heightMapSettings.heightMultiplier;

                if (values[i,j] > maxValue) {
                    maxValue = values[i,j];
                }
                if (values[i,j] < minValue) {
                    minValue = values[i,j];
                }
            }
        }

        if (heightMapSettings.useSquareFalloff || heightMapSettings.useRadialFalloff) {
            float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(meshSettings.numVertsPerLine, heightMapSettings);
            int offsetX = meshSettings.numVertsPerLine*(int)(sampleCentre.x/(meshSettings.numVertsPerLine-3));
            int offsetY = meshSettings.numVertsPerLine*(int)(heightMapSettings.falloffMapSize-(sampleCentre.y/(meshSettings.numVertsPerLine-3))-1);
            if (offsetX==1) offsetX=0; 
            if (offsetY==1) offsetY=0;
            int waterLandX = (sampleCentre.x <= meshSettings.numVertsPerLine*meshSettings.meshScale*(heightMapSettings.falloffMapSize-1) && sampleCentre.x >= 0)?1:0;
            int waterLandY = (sampleCentre.y <= meshSettings.numVertsPerLine*meshSettings.meshScale*(heightMapSettings.falloffMapSize-1) && sampleCentre.y >= 0)?1:0;
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    values[i,j] -= falloffMap[(i+offsetX)*waterLandX,(j+offsetY)*waterLandY]*values[i,j];
                }
            }
        }

        return new HeightMap(values, minValue, maxValue);
    }
}

public struct HeightMap
{
    public readonly float[,] values;
    public readonly float minValue;
    public readonly float maxValue;

    public HeightMap (float[,] values, float minValue, float maxValue)
    {
        this.values = values;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}