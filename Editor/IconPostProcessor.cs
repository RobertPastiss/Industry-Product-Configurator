using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IndustryCSE.Tool.ProductConfigurator.ScriptableObjects;
using UnityEngine;
using UnityEditor;

namespace IndustryCSE.Tool.ProductConfigurator.Editor
{
    public class IconPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var asset in importedAssets)
            {
                if (!asset.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) continue;
                if (Directory.GetParent(Directory.GetParent(asset).ToString()).ToString() !=
                    EditorCore.ConfigurationIconPath) continue;
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(asset);
                string fileName = Path.GetFileNameWithoutExtension(asset);
                string[] split = fileName.Split(" - ");
                string id = split[1];
                foreach (var configurationAsset in AssetDatabase.FindAssets($"t:{nameof(ConfigurationOption)}"))
                {
                    var configurationOption = AssetDatabase.LoadAssetAtPath<ConfigurationOption>(
                        AssetDatabase.GUIDToAssetPath(configurationAsset));
                    if (!string.Equals(configurationOption.UniqueIdString, id)) continue;
                    configurationOption.icon = texture;
                    EditorUtility.SetDirty(configurationOption);
                }
            }
        }
    }
}
