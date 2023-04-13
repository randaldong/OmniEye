using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class GazeInteractor : MonoBehaviour
{
	[Header("Custom Settings")]
	public GameObject gazeReticleGameObject;
	public float timeToActivate = 2.0f;
	public bool showRay = true;
	public GameObject gazeReticle;

	[SerializeField] private float _maxDetectionDistance = 10.0f;
	[SerializeField] private float _minDetectionDistance = 0.1f;
	[SerializeField] private LayerMask _gazeInteractableLayer;


	private Ray _ray;
	private RaycastHit _hitInfo;
	private GazeInteractable _curInteractable, _prevInteractable;
	private float _gazeTime = 0;
	private bool _gazeHasBeenActivated = false;

	private void FixedUpdate()
	{
		_ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(_ray, out _hitInfo, _maxDetectionDistance, _gazeInteractableLayer))
		{
			SetRayDisplay(showRay, _minDetectionDistance, _ray, _hitInfo.distance, new Color(0.9f, 0.5f, 1.0f, 0.8f));
			//Debug.Log("Casted ray hits: " + _hitInfo.transform.name);
			_curInteractable = _hitInfo.transform.GetComponent<GazeInteractable>();
			if (_curInteractable != null && _curInteractable != _prevInteractable) // gaze enter
			{
				//Debug.Log(_hitInfo.transform.gameObject.name);
				_curInteractable.GazeEnter();
				if (_prevInteractable != null) {
					OnGazeExit();
				}
				_prevInteractable = _curInteractable;
			}
			else if (_curInteractable != null && _curInteractable == _prevInteractable) // keep gazing
			{
				_gazeTime += Time.fixedDeltaTime;
				if (_gazeTime >= timeToActivate && _gazeHasBeenActivated == false)
				{
					_curInteractable.GazeAvtivated();
					_gazeHasBeenActivated = true;
				}
			}
		}
		else
		{
			SetRayDisplay(showRay, _minDetectionDistance, _ray, _maxDetectionDistance, new Color(0.5f, 0.5f, 1.0f, 0.5f));
			if (_prevInteractable != null)
			{
				OnGazeExit();
			}
		}
	}

	void SetRayDisplay(bool showRay, float _minDetectionDistance, Ray ray, float hitDistance, Color color)
	{
		if (showRay && hitDistance > _minDetectionDistance)
		{
			Debug.DrawRay(ray.origin, ray.direction * hitDistance, color);
		}
	}

	private void OnGazeExit()
	{
		_prevInteractable.GazeExit();
		_prevInteractable = null;
		_gazeTime = 0;
		_gazeHasBeenActivated = false;
	}
}