using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{
   public static float[,] GenerateFalloffMap(int size, HeightMapSettings settings)
   {
        size = size * settings.falloffMapSize;
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                float value = 0f;
                float squareValue = 1f;
                float radialValue = 1f;
                if (settings.useSquareFalloff) {
                    float x = (i / (float)size * 2 - 1);
                    float y = (j / (float)size * 2 - 1);
                    
                    value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                    squareValue = Evaluate(value,settings.squareFalloffPersistance);
                }
                if (settings.useRadialFalloff) {
                    float innerRadius = size / (settings.range-settings.innerRadius);
                    float outerRadius = size / (settings.range-settings.outerRadius);
                    radialValue = RadialFallOff(0, innerRadius, outerRadius, i, j, size/2, size/2);
                }
                if (!settings.useSquareFalloff) squareValue = radialValue;
                if (!settings.useRadialFalloff) radialValue = squareValue;
                map[i,j] = (squareValue+radialValue)/2f;
            }
        }

        return map;
   }

   static float Evaluate(float value, float persistance)
   {
        float a = 3;
        float b = a-persistance;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
   }

   static float RadialFallOff(float value, float innerRadius, float outerRadius, int x, int y, float cx, float cy) 
    {
    float dx = cx - x;
    float dy = cy - y;
    float distSqr = dx * dx + dy * dy;
    float iRadSqr = innerRadius * innerRadius;
    float oRadSqr = outerRadius * outerRadius;

    if (distSqr >= oRadSqr) return 1f;
    if (distSqr <= iRadSqr) return value;

    float dist = Mathf.Sqrt(distSqr);
    float t = Mathf.InverseLerp(innerRadius, outerRadius, dist);
    return t;
    }
}
