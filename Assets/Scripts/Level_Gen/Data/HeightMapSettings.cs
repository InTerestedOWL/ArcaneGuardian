using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class HeightMapSettings : UpdatableData
{
    public NoiseSettings noiseSettings;

    public float heightMultiplier;

    public AnimationCurve heightCurve;
    public bool useFallOff;

    public float minHeight
    {
        get { return heightMultiplier * heightCurve.Evaluate(0); }
    }

    public float maxHeight
    {
        get { return heightMultiplier * heightCurve.Evaluate(1); }
    }

#if UNITY_EDITOR

    protected override void OnValidate()
    {
        noiseSettings.ValidateValues();

        base.OnValidate();
    }
#endif
}