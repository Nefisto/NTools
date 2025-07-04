﻿using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static void DestroyChildren (this Transform transform)
        {
            foreach (Transform child in transform)
                Object.Destroy(child.gameObject);
        }
        
        public static void DisableChildren (this Transform transform)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
    }
}