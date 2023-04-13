using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GazeInteractable : MonoBehaviour
{
	[Header("General Settings", order = 0)]
	[SerializeField] private Color selectionOutlineColor = new Color(0.5f, 0.5f, 1.0f);
	[SerializeField] private float selectionOutlineWidth = 6.0f;
	[SerializeField] private float _resumeDelay;

	[Header("HeatUp Settings", order = 0)]
	public bool heatUpSelected = true;
	[SerializeField] private float heatUpSpeed = 0.01f;
	[SerializeField] private Material lavaMaterial;




	private Material originalMaterial;
	private void Start()
	{
		originalMaterial = gameObject.GetComponent<MeshRenderer>().material;
	}

	public void GazeEnter()
	{
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

	public void GazeAvtivated()
	{
		HeatUp(heatUpSelected);
	}

	public void GazeExit()
	{
		gameObject.GetComponent<MeshRenderer>().material = originalMaterial;
		gameObject.GetComponent<Outline>().enabled = false;
	}

	private void HeatUp(bool heatUpSelected)
	{
		if (heatUpSelected)
		{

			gameObject.GetComponent<MeshRenderer>().material = lavaMaterial;


		}
	}
}


