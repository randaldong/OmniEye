using UnityEngine;
using UnityEngine.UI;

public class GazeInteractor : MonoBehaviour
{
	[Header("Ray Settings")]
	[SerializeField] private bool showDebugRay = true;
	[SerializeField] private float _maxDetectionDistance = 10.0f;
	[SerializeField] private float _minDetectionDistance = 0.1f;
	[SerializeField] private LayerMask _gazeInteractableLayer;
	[SerializeField] private LayerMask _portalLayer;
	[SerializeField] private LayerMask _selectorLayer;


	[Header("Reticle Settings")]
	[SerializeField] GameObject gazeAim;
	[SerializeField] Image gazeLoadImage;
	[SerializeField] Image gazeWizardImage;
	[SerializeField] Image portalActivateImage;
	[SerializeField] Image teleportingImage;
	[SerializeField] Image selectorActivateImage;


	private Ray _ray;
	private RaycastHit _hitBrickInfo, _hitPortalInfo, _hitSelectorInfo;
	private GazeInteractable _curInteractable, _prevInteractable;
	private Portal _curPortal, _prevPortal;
	private Selector _curSelector, _prevSelector, _prevActivatedSelector;
	private float _gazeTime = 0;
	private float _portalTime = 0;
	private float _selectorTime = 0;
	private bool heatUpSelected;
	private bool lazyFollowSelected;

	private Quaternion attachOrient;
	private float distance = 0;
	private Vector3 attachPos;
	
	private void FixedUpdate()
	{
		_ray = new Ray(transform.position, transform.forward);

		/*--------------------------------- Ray & GazeInteractable ---------------------------------*/
		if (Physics.Raycast(_ray, out _hitBrickInfo, _maxDetectionDistance, _gazeInteractableLayer))
		{
			SetRayDisplay(showDebugRay, _minDetectionDistance, _ray, _hitBrickInfo.distance, new Color(0.9f, 0.5f, 1.0f, 0.8f));
			//Debug.Log("Casted ray hits: " + _hitInfo.transform.name);
			_curInteractable = _hitBrickInfo.transform.GetComponent<GazeInteractable>();
			if (_curInteractable != null && _curInteractable != _prevInteractable) // gaze enter
			{
				//Debug.Log(_hitInfo.transform.gameObject.name);
				_curInteractable.heatUpSelected = heatUpSelected;
				_curInteractable.lazyFollowSelected = lazyFollowSelected;
				_curInteractable.GazeEnter();
				if (_prevInteractable != null) {
					OnGazeExit();
				}
				_prevInteractable = _curInteractable;
			}
			else if (_curInteractable != null && _curInteractable == _prevInteractable) // keep gazing
			{
				_gazeTime += Time.deltaTime;
				if (heatUpSelected || lazyFollowSelected)
				{
					gazeLoadImage.fillAmount = _gazeTime / _curInteractable.timeToActivate;
				}

				if (_gazeTime >= _curInteractable.timeToActivate)
				{
					if (distance == 0)
					{
						distance = _hitBrickInfo.distance;
						attachOrient = _curInteractable.transform.rotation;
					}
					OnGazeActivated();
				}
			}
		}
		else
		{
			SetRayDisplay(showDebugRay, _minDetectionDistance, _ray, _maxDetectionDistance, new Color(0.5f, 0.5f, 1.0f, 0.5f));
			if (_prevInteractable != null)
			{
				OnGazeExit();
			}
		}

		/*--------------------------------- Ray & Portal ---------------------------------*/
		if (Physics.Raycast(_ray, out _hitPortalInfo, _maxDetectionDistance, _portalLayer))
		{
			_curPortal = _hitPortalInfo.transform.GetComponent<Portal>();
			if (_curPortal != null && _curPortal != _prevPortal) // upon look at portal
			{
				_curPortal.OnPortalEnter();
				if (_prevPortal != null)
				{
					OnPortalExit();
				}
				_prevPortal = _curPortal;
			}
			else if (_curPortal != null && _curPortal == _prevPortal) // start activate
			{
				_portalTime += Time.fixedDeltaTime;
				portalActivateImage.fillAmount = _portalTime / _curPortal.timeToActivate;

				if (_portalTime >= _curPortal.timeToActivate)
				{
					OnPortalActivated();
				}
			}

		}
		else
		{
			if (_prevPortal != null)
			{
				OnPortalExit();
			}
		}

		/*--------------------------------- Ray & Selector ---------------------------------*/
		if (Physics.Raycast(_ray, out _hitSelectorInfo, _maxDetectionDistance, _selectorLayer))
		{
			_curSelector = _hitSelectorInfo.transform.GetComponent<Selector>();
			if (_curSelector != null && _curSelector != _prevSelector) // upon select
			{
				_curSelector.OnSelectorEnter();
				if (_prevSelector != null)  
				{
					OnSelectorExit();
				}
				_prevSelector = _curSelector;
			}
			else if (_curSelector != null && _curSelector == _prevSelector) // start activate
			{
				_selectorTime += Time.fixedDeltaTime;
				selectorActivateImage.fillAmount = _selectorTime / _curSelector.timeToActivate;

				if (_selectorTime >= _curSelector.timeToActivate)
				{
					OnSelectorActivated();
					if (_prevActivatedSelector != _curSelector)
					{
						_prevActivatedSelector = _curSelector;
					}
				}
			}

		}
		else
		{
			if (_prevSelector != null)
			{
				OnSelectorExit();
			}
		}
	}

	/*--------------------------------- Ray & GazeInteractable ---------------------------------*/
	void SetRayDisplay(bool showRay, float _minDetectionDistance, Ray ray, float hitDistance, Color color)
	{
		if (showRay && hitDistance > _minDetectionDistance)
		{
			Debug.DrawRay(ray.origin, ray.direction * hitDistance, color);
		}
	}

	private void OnGazeActivated()
	{
		
		_curInteractable.heatUpSelected = heatUpSelected;
		_curInteractable.lazyFollowSelected= lazyFollowSelected;
		gazeLoadImage.fillAmount = 0;

		if (lazyFollowSelected)
		{
			attachPos = transform.position + distance * transform.forward;
		}
		Debug.Log(attachPos);
		_curInteractable.GazeAvtivated(_gazeTime - _curInteractable.timeToActivate, attachPos, attachOrient);

		if (heatUpSelected)
		{
			gazeWizardImage.fillAmount = (_gazeTime - _curInteractable.timeToActivate) / _curInteractable.timeToHeatUp;
		}
		if (_gazeTime > _curInteractable.timeToActivate + _curInteractable.timeToHeatUp)
		{
			gazeWizardImage.fillAmount = 0;
		}
	}

	private void OnGazeExit()
	{
		_prevInteractable.GazeExit();
		_prevInteractable = null;
		_gazeTime = 0;
		gazeLoadImage.fillAmount = 0f;
		gazeWizardImage.fillAmount = 0f;
	}

	/*--------------------------------- Ray & Portal ---------------------------------*/
	private void OnPortalActivated()
	{
		portalActivateImage.fillAmount = 0;
		_curPortal.PortalAvtivated(_portalTime - _curPortal.timeToActivate);
		teleportingImage.fillAmount = (_portalTime - _curPortal.timeToActivate) / _curPortal.timeToTeleport;
		if (_portalTime > _curPortal.timeToActivate + _curPortal.timeToTeleport)
		{
			teleportingImage.fillAmount = 0;
		}
	}

	private void OnPortalExit()
	{
		_prevPortal.PortalExit();
		_prevPortal = null;
		_portalTime = 0;
		portalActivateImage.fillAmount = 0f;
		teleportingImage.fillAmount = 0f;
	}

	/*--------------------------------- Ray & Selector ---------------------------------*/
	private void OnSelectorActivated()
	{
		selectorActivateImage.fillAmount = 0;
		_curSelector.SelectorAvtivated(out heatUpSelected, out lazyFollowSelected, _prevActivatedSelector);
	}

	private void OnSelectorExit()
	{
		_prevSelector.SelectorExit();
		_prevSelector = null;
		_selectorTime = 0;
		selectorActivateImage.fillAmount = 0f;
	}
}