﻿using UnityEngine;

namespace DPTeam
{
    public static class Utility
    {
        public static float EaseOutCubic(float start, float end, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
        
            if (start > end)
            {
                ratio = 1 - ratio;
                return CubicBendDownFunc(end, start, ratio);
            }
            else
            {
                ratio--;
                return CubicBendUpFunc(start, end, ratio);
            }
        }
        
        private static float CubicBendUpFunc(float min, float max, float arg)
        {
            max -= min;
            return max * (arg * arg * arg + 1) + min;
        }
    
        private static float CubicBendDownFunc(float min, float max, float arg)
        {
            max -= min;
            return max * (arg * arg * arg) + min;
        }
        
        public static int GetMask(params int[] layerNumbers)
        {
            if (layerNumbers == null)
                throw new System.ArgumentNullException(nameof (layerNumbers));
            int mask = 0;
            foreach (int layer in layerNumbers)
            {
                if (layer != -1)
                    mask |= 1 << layer;
            }
            return mask;
        }

        public static bool IsLayerInLayerMask(LayerMask layerMask, int layer) => (layerMask & layer) == layer;
    }
}