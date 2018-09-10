using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Colour the first two characters of the guid and library/meta path to show the relationship
public class LibraryMetadataColoriser : MonoBehaviour 
{
	private Text m_Text;

	private void Start () {
		m_Text = GetComponent<Text>();
		Debug.Assert(m_Text);
	}

	private void LateUpdate()
	{
		if (!string.IsNullOrEmpty(m_Text.text))
		{
			Object currentObject = Selection.activeObject;
			
			string guid;
			int localID;

			if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(currentObject, out guid, out localID))
			{
				string metaStart = guid.Substring(0, 2);
				string result = m_Text.text;

				result = result.Replace($"<color=yellow>{metaStart}", $"<color=yellow><b><color=red>{metaStart}</color></b>");
				result = result.Replace($"/{metaStart}/", $"/<b><color=red>{metaStart}</color></b>/");

				m_Text.text = result;
			}
		}
	}
}
