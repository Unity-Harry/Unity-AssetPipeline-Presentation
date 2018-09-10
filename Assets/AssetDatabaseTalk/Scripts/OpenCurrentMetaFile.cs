using UnityEditor;
using UnityEngine;

public class OpenCurrentMetaFile : MonoBehaviour 
{
    public void OpenMetaFile()
    {
        Object currentObject = Selection.activeObject;
        string currentObjectPath = AssetDatabase.GetAssetPath(currentObject);
        string metadataFile = AssetDatabase.GetTextMetaFilePathFromAssetPath(currentObjectPath);
        EditorUtility.RevealInFinder(metadataFile);
    }
}
