using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static Sprite ToSprite (this Texture2D texture)
            => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        /// <summary>
        ///     Remove all transparent pixels from a texture, leaving a minimum rect that contains all non-transparent pixels.
        /// </summary>
        /// <param name="source">Texture to be trimmed</param>
        /// <returns>New texture trimmed</returns>
        public static Texture2D TrimTransparentPixels (this Texture2D source)
        {
            var pixels = source.GetPixels();
            var width = source.width;
            var height = source.height;

            int minX = width, maxX = 0;
            int minY = height, maxY = 0;

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                var pixel = pixels[y * width + x];
                if (!(pixel.a > 0.01f)) // Consider non-transparent
                    continue;

                if (x < minX)
                    minX = x;
                if (x > maxX)
                    maxX = x;
                if (y < minY)
                    minY = y;
                if (y > maxY)
                    maxY = y;
            }

            if (minX > maxX || minY > maxY)
            {
                Debug.LogWarning("Texture is fully transparent.");
                return null;
            }

            var croppedWidth = maxX - minX + 1;
            var croppedHeight = maxY - minY + 1;

            var trimmedPixels = source.GetPixels(minX, minY, croppedWidth, croppedHeight);

            var trimmed = new Texture2D(croppedWidth, croppedHeight, source.format, false);
            trimmed.SetPixels(trimmedPixels);
            trimmed.Apply();

            return trimmed;
        }

        public static async UniTask SaveTextureToFile (this Texture2D texture, string filePath, bool overwrite = false,
            bool usePngFormat = true)
        {
            if (texture is null)
            {
                Debug.LogError("Texture is null");
                return;
            }

            if (File.Exists(filePath) && !overwrite)
            {
                Debug.LogWarning(
                    $"File already exists at {filePath}. Enable overwrite if you want to replace the file.");
                return;
            }

            var fileBytes = usePngFormat ? texture.EncodeToPNG() : texture.EncodeToJPG();

            await File.WriteAllBytesAsync(filePath, fileBytes);
        }

        public static async UniTask SaveToProjectFolder (
            this Texture2D texture,
            string relativePath,
            bool overwrite = false,
            bool usePngFormat = true)
        {
            if (!texture)
            {
                Debug.LogError("Texture is null");
                return;
            }

            if (!relativePath.StartsWith("Assets/"))
            {
                Debug.LogError("Invalid path! Use a path that starts with 'Assets/' for saving inside the project.");
                return;
            }

            var fullPath = Path.Combine(Application.dataPath.Replace("Assets", ""), relativePath);

            if (File.Exists(fullPath) && !overwrite)
            {
                Debug.LogWarning($"File already exists at '{fullPath}' and overwrite is disabled.");
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException());

            var fileBytes = usePngFormat ? texture.EncodeToPNG() : texture.EncodeToJPG();
            await File.WriteAllBytesAsync(fullPath, fileBytes);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}