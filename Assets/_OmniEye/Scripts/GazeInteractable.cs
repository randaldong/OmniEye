using System;
using System.Collections;
using System.Collections.Generic;
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


	private Material originalMaterial;
	private Vector3 originalPosition;
	private GameObject attachToObject;
	private float attachDistance;

	public void GazeEnter(GameObject targetObject)
	{
		originalMaterial = new Material(gameObject.GetComponent<MeshRenderer>().material);
		originalPosition = gameObject.transform.position;
		attachToObject = targetObject;
		attachDistance = Vector3.Distance(targetObject.transform.position, originalPosition);

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

	public void GazeAvtivated(float activateTime)
	{
		HeatUp(activateTime);
		LazyFollow();
	}

	public void GazeExit()
	{
		gameObject.GetComponent<MeshRenderer>().material = originalMaterial;
		gameObject.GetComponent<Outline>().enabled = false;
		transform.position = originalPosition;
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

	private void LazyFollow()
	{
		if (lazyFollowSelected)
		{
			gameObject.GetComponent<Rigidbody>().useGravity = false;
			transform.position = attachToObject.transform.position + attachToObject.transform.forward * attachDistance;
		}
	}

}


