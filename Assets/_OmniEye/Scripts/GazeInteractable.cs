using UnityEngine;

public class GazeInteractable : MonoBehaviour
{
	[Header("Selection Settings", order = 0)]
	[SerializeField] private Color selectionOutlineColor = new Color(0.5f, 0.5f, 1.0f);
	[SerializeField] private float selectionOutlineWidth = 6.0f;
	public float timeToActivate = 1.5f;
	[SerializeField] private float resumeDelay;

	[Header("HeatUp Settings", order = 1)]
	public bool heatUpSelected = false;
	public float timeToHeatUp = 3.0f;
	[SerializeField] private Material lavaMaterial;

	[Header("Lazy Follow Settings", order = 2)]
	public bool lazyFollowSelected = false;
	[Tooltip("The distance to where the object finally stays : the distance to the object's original position")]
	public float lazyFollowDistanceRatio = 1.0f;


	private Material originalMaterial;

	public void GazeEnter()
	{
		originalMaterial = new Material(gameObject.GetComponent<MeshRenderer>().material);

		if (gameObject.GetComponent<Outline>() != null)
		{
			gameObject.GetComponent<Outline>().enabled = true;
		}
		else
		{
			Outline outline = gameObject.AddComponent<Outline>();
			outline.enabled = true;
			gameObject.GetComponent<Outline>().OutlineColor = selectionOutlineColor;
			gameObject.GetComponent<Outline>().OutlineWidth = selectionOutlineWidth;
		}
	}

	public void GazeAvtivated(float activateTime, Vector3 attachPos, Quaternion attachOrient)
	{
		HeatUp(activateTime);
		LazyFollow(attachPos, attachOrient);
	}

	public void GazeExit()
	{
		gameObject.GetComponent<MeshRenderer>().material = originalMaterial;
		gameObject.GetComponent<Outline>().enabled = false;
	}

	private void HeatUp(float activateTime)
	{
		if (heatUpSelected)
		{
			if (activateTime < timeToHeatUp)
			{
				gameObject.GetComponent<MeshRenderer>().material.Lerp(originalMaterial, lavaMaterial, activateTime / timeToHeatUp);
			}
			else
			{
				gameObject.GetComponent<MeshRenderer>().material = lavaMaterial;
			}
		}
	}

	private void LazyFollow(Vector3 attachPos, Quaternion attachOrient)
	{
		if (lazyFollowSelected)
		{
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
			gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			transform.position = attachPos;
			transform.rotation = attachOrient;
		}
	}

}


