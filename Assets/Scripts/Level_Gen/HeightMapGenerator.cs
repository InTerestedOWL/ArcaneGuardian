using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, Vector2 sampleCenter,
        Vector2 coord, float numVertsPerLine, bool generateFallOffMap, float[,] fallOffMap, int chunkcountDivided)
    {
        float[,] values = Noise.GenerateNoiseMap(width, height, settings.noiseSettings, sampleCenter);

        AnimationCurve heightCurve_threadsave = new AnimationCurve(settings.heightCurve.keys);


        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for (int i = 0; i < width; i++)
        {
            int xOnFallOffMap = (int)(i + (((coord.x * (numVertsPerLine)) - (numVertsPerLine) / 2)) +
                                      ((chunkcountDivided * (numVertsPerLine)) - (numVertsPerLine) / 2) + numVertsPerLine);
            if (coord.x < 0 || (coord.x == 0 && i < numVertsPerLine / 2))
            {
                xOnFallOffMap = (int)(i + (((coord.x * (numVertsPerLine)) + (numVertsPerLine) / 2)) +
                                      ((chunkcountDivided * (numVertsPerLine)) - (numVertsPerLine) / 2));
            }

            if (xOnFallOffMap > 5 * numVertsPerLine || xOnFallOffMap < 0)
            {
                Debug.Log("x" + xOnFallOffMap);
            }

            for (int j = 0; j < height; j++)
            {
                int yOnFallOffMap = (int)(j + ((coord.y * numVertsPerLine - numVertsPerLine / 2) -
                                          (chunkcountDivided * numVertsPerLine - numVertsPerLine / 2))* (coord.y -chunkcountDivided));
                if (coord.y < 0 || (coord.y == 0))
                {
                    yOnFallOffMap = (int)(j + (((coord.y *-1) * numVertsPerLine + numVertsPerLine / 2) +
                                          (chunkcountDivided * numVertsPerLine + numVertsPerLine / 2)  - numVertsPerLine));
                }

                if (yOnFallOffMap > 5 * numVertsPerLine || yOnFallOffMap < 0)
                {
                    Debug.Log("y: " + yOnFallOffMap + " coord: " + coord.y + " j:" + j);
                }

                /*if (coord.x == 0f && coord.y == 2f)
                {
                    Debug.Log(numVertsPerLine + "-" + xOnFallOffMap + " , " + yOnFallOffMap + "->" +
                              fallOffMap[xOnFallOffMap, yOnFallOffMap]);
                }*/

                values[i, j] *=
                    heightCurve_threadsave.Evaluate((values[i, j] -
                                                    (generateFallOffMap
                                                        ? fallOffMap[xOnFallOffMap, yOnFallOffMap]
                                                        : 0))) * settings.heightMultiplier;

                if (values[i, j] > maxValue)
                {
                    maxValue = values[i, j];
                }

                if (values[i, j] < minValue)
                {
                    minValue = values[i, j];
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

    public HeightMap(float[,] values, float minValue, float maxValue)
    {
        this.values = values;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}