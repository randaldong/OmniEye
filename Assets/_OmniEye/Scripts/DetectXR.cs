using UnityEngine;
using UnityEngine.XR.Management;

public class DetectXR : MonoBehaviour
{
	public bool startInVR = true;
    public GameObject xrOrigin;
    public GameObject desktopTester;

    void Start()
    {
		if (startInVR)
		{
			var xrSettings = XRGeneralSettings.Instance;
			if (xrSettings == null)
			{
				Debug.Log("XRGeneralSettings is Null!");
				return;
			}

			var xrManager = xrSettings.Manager;
			if (xrManager == null)
			{
				Debug.Log("XRManager is Null!");
				return;
			}

			var xrLoader = xrManager.activeLoader;
			if (xrLoader == null)
			{
				Debug.Log("XRLoader is Null!");
				UseDesktop();
				return;
			}

			Debug.Log("XR Headset Connected!");
			UseXR();
		}
		else
		{
			UseDesktop();
		}
        
		void UseDesktop()
		{
			xrOrigin.SetActive(false);
			desktopTester.SetActive(true);
		}

		void UseXR()
		{
			xrOrigin.SetActive(true);
			desktopTester.SetActive(false);
		}
	}
}
