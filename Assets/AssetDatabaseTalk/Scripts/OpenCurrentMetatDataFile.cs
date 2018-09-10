using UnityEditor;
using UnityEngine;

public class OpenCurrentMetatDataFile : MonoBehaviour 
{
	public void OpenMetaDataFile()
	{
		Object currentObject = Selection.activeObject;
		string currentObjectPath = AssetDatabase.GetAssetPath(currentObject);
		string guid;
		int localID;
		if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(currentObject, out guid, out localID))
		{
			string metadataPath = AssetExplorerUtility.GetMetadataFilePathFromAssetGuid(guid);
			EditorUtility.RevealInFinder(metadataPath);
		}
	}
}
