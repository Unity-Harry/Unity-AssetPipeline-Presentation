using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Fill in information about the currently selected asset based upon a very simple markup
public class TextToCurrentAssetInfo : MonoBehaviour
{
	public bool AllowSceneAssets;
	private string m_TextTemplate = string.Empty;
	private Object m_LastObject;
	private Text m_Text;

	// The following will be found and replaced within the text template
	
	// The name of the currently selected object
	private const string kName = "{Name}";
	
	// The asset name on disk
	private const string kFullName = "{FullName}";
	
	// Asset GUID
	private const string kGUID = "{GUID}";
	
	// C# Type of the object
	private const string kType = "{Type}";
	
	// Asset Type (Native, Foreign)"
	private const string kAssetType = "{AssetType}";
	
	// The type of importer for the asset
	private const string kImporter = "{Importer}";
	
	// Object LocalID
	private const string kLocalID = "{LocalID}";
	
	// Object InstanceID
	private const string kInstanceID = "{InstanceID}";
	
 	// Path of the assets .meta file
	private const string kMetaFilePath = "{MetaFilePath}";
	
	// Path of the assets binary file within Library/metadata
	private const string kLibraryMetaFilePath = "{LibraryMetaPath}";
	
	// A list of objects created by import of the Asset
	private const string kAssetObjects = "{Objects}";
	
	// Number of objects created by import of the Asset
	private const string kNumAssetObjects = "{NumObjects}";
	
	// A list of Assets this Asset is dependant on
	private const string kAssetDependancies = "{Dependancies}";
	
	// Number of dependancies the Asset has
	private const string kNumAssetDependancies = "{NumDependancies}";
	
	// The max number of dependancies we'll list
	private const int 	 kMaxDependancies = 5;
	
	// The max number of objects we'll list
	private const int 	 kMaxObjects = 6;

	private void Start()
	{
		m_Text = GetComponent<Text>();
		Debug.Assert(m_Text);

		m_TextTemplate = m_Text.text;
	}

	private void Update ()
	{
		Object currentObject = Selection.activeObject;
		string currentObjectPath = AssetDatabase.GetAssetPath(currentObject);
		bool isSceneAsset = string.IsNullOrEmpty(currentObjectPath);

		if (currentObject && 
		    currentObject != m_LastObject &&
		    (!isSceneAsset || AllowSceneAssets))
		{
			m_Text.text = CreateStringFromTemplate(m_TextTemplate, currentObject, currentObjectPath);
		}
		else if (!currentObject || isSceneAsset && !AllowSceneAssets)
		{
			m_Text.text = string.Empty;
		}
		
		m_LastObject = currentObject;
	}

	private static string CreateStringFromTemplate(string template, Object currentObject, string currentObjectPath)
	{
		string result = template;

		string guid;
		int localID;
		if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(currentObject, out guid, out localID))
		{
			result = result.Replace(kName, currentObject.name);
			result = result.Replace(kFullName, Path.GetFileName(currentObjectPath));
			result = result.Replace(kGUID, guid);
			result = result.Replace(kLocalID, localID.ToString());
			result = result.Replace(kAssetType, AssetDatabase.IsForeignAsset(currentObject) ? "Foreign" : "Native");
			result = result.Replace(kImporter, AssetImporter.GetAtPath(currentObjectPath).GetType().Name);
			result = result.Replace(kMetaFilePath, AssetDatabase.GetTextMetaFilePathFromAssetPath(currentObjectPath));
			result = result.Replace(kLibraryMetaFilePath, AssetExplorerUtility.GetShortMetadataFilePathFromAssetGuid(guid));

			if (result.Contains(kAssetObjects))
			{
				// LoadAllAssetsAtPath is not possible for scenes
				if(!currentObjectPath.EndsWith(".unity"))
				{
					string objectListString = string.Empty;

					var objects = AssetDatabase.LoadAllAssetsAtPath(currentObjectPath);
					for (var i = 0; i < objects.Length; i++)
					{
						Object obj = objects[i];

						if (i == kMaxObjects)
						{
							objectListString += "<color=yellow>...</color>";
							break;
						}
						
						string objString =
							CreateStringFromTemplate("Name: <color=yellow>{Name}</color> - Type: <color=yellow>{Type}</color>\n", obj, currentObjectPath);
						if (!AssetDatabase.IsMainAsset(obj))
							objString = objString.Insert(0, "    ");

						objectListString += objString;
					}

					result = result.Replace(kAssetObjects, objectListString);
				}
				else
					result = result.Replace(kAssetObjects, "<color=red>Accessing Objects within Scene is not allowed</color>");
			}

			if (result.Contains(kNumAssetObjects))
			{
				if (currentObjectPath.EndsWith(".unity"))
					result = result.Replace(kNumAssetObjects, "1");
				else
					result = result.Replace(kNumAssetObjects, AssetDatabase.LoadAllAssetsAtPath(currentObjectPath).Length.ToString());
			}

			var dependancies = AssetDatabase.GetDependencies(currentObjectPath, false);
			result = result.Replace(kNumAssetDependancies, dependancies.Length.ToString());

			if (result.Contains(kAssetDependancies))
			{
				string dependancyListString = string.Empty;

				for (int i = 0; i < dependancies.Length; ++i)
				{
					var depPath = dependancies[i];

					if (i == kMaxDependancies)
					{
						dependancyListString += "<color=yellow>...</color>";
						break;
					}
					
					var depObj = AssetDatabase.LoadMainAssetAtPath(depPath);
					
					dependancyListString +=
						CreateStringFromTemplate("Name: <color=yellow>{Name}</color> - Type: <color=yellow>{Type}</color>\n", depObj, depPath);
				}
				
				result = result.Replace(kAssetDependancies, dependancyListString);
			}
		}
			
		// We can check these on scene objects
		result = result.Replace(kType, currentObject.GetType().Name);
		result = result.Replace(kInstanceID, currentObject.GetInstanceID().ToString());

		return result;
	}


}
