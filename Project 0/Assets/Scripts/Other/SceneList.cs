using Vexe.Runtime.Types;
using UnityEditor;

public class SceneList : BaseScriptableObject
{
    public string[] sceneNames;
    
    [MenuItem("Scenes Name/Save Scenes Names")]
    private static void SaveScenesNames()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        SceneList list = (SceneList)AssetDatabase.LoadAssetAtPath("Assets/SceneList.asset", typeof(SceneList));

        if (list == null)
        {
            list = CreateInstance<SceneList>();
            AssetDatabase.CreateAsset(list, "Assets/SceneList.asset");
        }

        list.sceneNames = new string[scenes.Length];
        for (int i = 0; i < scenes.Length; ++i)
        {
            list.sceneNames[i] = scenes[i].path;
        }

        AssetDatabase.SaveAssets();
    }
}
