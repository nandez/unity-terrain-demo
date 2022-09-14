using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
    /// <summary>
    /// Este script esta basado en el WaterBasic de los StandardAssets.
    /// </summary>
    //[ExecuteInEditMode]
    public class WaterController : MonoBehaviour
    {
        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (!renderer)
                return;

            var material = renderer.sharedMaterial;
            if (!material)
                return;

            var waveSpeed = material.GetVector("WaveSpeed");
            float waveScale = material.GetFloat("_WaveScale");
            float t = Time.time / 20.0f;

            var offset4 = waveSpeed * (t * waveScale);
            var offsetClamped = new Vector4(Mathf.Repeat(offset4.x, 1.0f), Mathf.Repeat(offset4.y, 1.0f),
            Mathf.Repeat(offset4.z, 1.0f), Mathf.Repeat(offset4.w, 1.0f));
            material.SetVector("_WaveOffset", offsetClamped);
        }
    }
}
