using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeInteractable : MonoBehaviour
{
	[Header("General Settings", order = 0)]
	[SerializeField] private Color selectionOutlineColor = new Color(0.5f, 0.5f, 1.0f);
	[SerializeField] private float selectionOutlineWidth = 6.0f;
	public float timeToActivate = 1.5f;
	[SerializeField] private float resumeDelay;

	[Header("HeatUp Settings", order = 0)]
	public bool heatUpSelected = true;
	public float timeToHeatUp = 3.0f;
	[SerializeField] private Material lavaMaterial;



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

	public void GazeAvtivated(float activateTime)
	{
		HeatUp(heatUpSelected, activateTime);
	}

	public void GazeExit()
	{
		gameObject.GetComponent<MeshRenderer>().material = originalMaterial;
		gameObject.GetComponent<Outline>().enabled = false;
	}

	private void HeatUp(bool heatUpSelected, float activateTime)
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
}


