using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
	public bool isLaserEyeSelector;
	public bool isLazyFollowSelector;
	public float timeToActivate = 3f;

	public void OnSelectorEnter()
	{
		SetTextAsPrompt(true);
	}
	public void SelectorAvtivated(out bool heatUpSelected, out bool lazyFollowSelected)
	{
		SetTextAsPrompt(false);
		heatUpSelected = isLaserEyeSelector;
		lazyFollowSelected = isLazyFollowSelector;
		Debug.Log(heatUpSelected);
	}

	public void SelectorExit()
	{

	}

	private void SetTextAsPrompt(bool isPrompt)
	{
		if (isLaserEyeSelector)
		{
			GameObject LaserPrompt = GetChildByName(gameObject, "LaserPrompt");
			LaserPrompt.SetActive(isPrompt);

			GameObject LaserSelected = GetChildByName(gameObject, "LaserSelected");
			LaserSelected.SetActive(!isPrompt);
		}
		else if(isLazyFollowSelector)
		{
			GameObject LazyPrompt = GetChildByName(gameObject, "LazyPrompt");
			LazyPrompt.SetActive(isPrompt);

			GameObject LazySelected = GetChildByName(gameObject, "LazySelected");
			LazySelected.SetActive(!isPrompt);
		}
	}

	private GameObject GetChildByName(GameObject obj, string name)
	{
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			if (obj.transform.GetChild(i).name == name)
			{
				return obj.transform.GetChild(i).gameObject;
			}
			GameObject tmp = GetChildByName(transform.GetChild(i).gameObject, name);
			if (tmp != null)
			{
				return tmp;
			}
		}
		return null;
	}
}
