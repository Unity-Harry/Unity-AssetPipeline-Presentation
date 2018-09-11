using UnityEditor;
using UnityEngine;

public class OpenCurrentMetatDataFile : MonoBehaviour 
{
	public void OpenMetaDataFile()
	{
		Object currentObject = Selection.activeObject;
		string currentObjectPath = AssetDatabase.GetAssetPath(currentObject);
		string guid;
		
#if UNITY_2018_2
		long localID;
#else
		int localID;
#endif
		if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(currentObject, out guid, out localID))
		{
			string metadataPath = AssetExplorerUtility.GetMetadataFilePathFromAssetGuid(guid);
			EditorUtility.RevealInFinder(metadataPath);
		}
	}
}
