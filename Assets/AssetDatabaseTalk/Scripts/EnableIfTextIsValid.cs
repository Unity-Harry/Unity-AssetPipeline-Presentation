using UnityEngine;
using UnityEngine.UI;

// Move text behind background when there's no valid info (When no Asset is selected)
public class EnableIfTextIsValid : MonoBehaviour
{
	public Text TextToWatch;
	private int m_InitialSiblingIndex = 0;
	
	private void Start () 
	{
		Debug.Assert(TextToWatch);

		m_InitialSiblingIndex = transform.GetSiblingIndex();
	}
	
	private void Update () 
	{
		if (!string.IsNullOrEmpty(TextToWatch.text))
			transform.SetSiblingIndex(m_InitialSiblingIndex);
		else
			transform.SetAsFirstSibling();
	}
}
