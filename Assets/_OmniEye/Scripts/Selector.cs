using UnityEngine;

public class Selector : MonoBehaviour
{
	public bool isLaserEyeSelector;
	public bool isLazyFollowSelector;
	public float timeToActivate = 3f;

	private GameObject LaserPrompt;
	private GameObject LaserSelected;
	private GameObject LazyPrompt;
	private GameObject LazySelected;

	public void OnSelectorEnter()
	{

	}
	public void SelectorAvtivated(out bool heatUpSelected, out bool lazyFollowSelected, Selector prevActivatedSelector)
	{
		heatUpSelected = isLaserEyeSelector;
		lazyFollowSelected = isLazyFollowSelector;

		LaserPrompt = GetChildByName(gameObject, "LaserPrompt");
		LaserSelected = GetChildByName(gameObject, "LaserSelected");
		LazyPrompt = GetChildByName(gameObject, "LazyPrompt");
		LazySelected = GetChildByName(gameObject, "LazySelected");

		gameObject.GetComponent<Selector>().SetTextAsSelected();
		if (prevActivatedSelector != null && prevActivatedSelector != gameObject.GetComponent<Selector>())
		{
			prevActivatedSelector.SetTextAsPrompt();
		}
	}

	public void SelectorExit()
	{

	}

	private void SetTextAsPrompt()
	{
		if (isLaserEyeSelector)
		{
			LaserPrompt.SetActive(true);
			LaserSelected.SetActive(false);
		}
		else if (isLazyFollowSelector)
		{
			LazyPrompt.SetActive(true);
			LazySelected.SetActive(false);
		}
	}

	private void SetTextAsSelected()
	{
		if (isLaserEyeSelector)
		{
			LaserPrompt.SetActive(false);
			LaserSelected.SetActive(true);
		}
		else if (isLazyFollowSelector)
		{
			LazyPrompt.SetActive(false);
			LazySelected.SetActive(true);
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
