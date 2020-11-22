using System.Collections;
using System.Collections.Generic;
using Scripts.src.Feature.Entities;
using UnityEditor;
using UnityEngine;

public class PreloaderModuleConfigCreator : MonoBehaviour
{
    [MenuItem("Modular framework/Create/Preloader module config")]
    public static void CreateMyAsset()
    {
        PreloadingModuleConfig asset = ScriptableObject.CreateInstance<PreloadingModuleConfig>();

        AssetDatabase.CreateAsset(asset, $"Assets/Resources/{ PreloadingModuleConfig.FILE_NAME }.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
