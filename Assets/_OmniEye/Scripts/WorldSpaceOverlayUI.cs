using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceOverlayUI : MonoBehaviour
{
    private const string shaderTestMode = "unity_GUIZTestMode";
    [SerializeField] UnityEngine.Rendering.CompareFunction desiredUIComparison = UnityEngine.Rendering.CompareFunction.Always;
    [SerializeField] Graphic[] uiElementsToApply;
    private Dictionary<Material, Material> materialMapping = new Dictionary<Material, Material>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(var graphic in uiElementsToApply)
        {
            Material material = graphic.materialForRendering;
            if(!materialMapping.TryGetValue(material, out Material materialCopy))
            {
				materialCopy = new Material(material);
                materialMapping.Add(material, materialCopy);
			}
            materialCopy.SetInt(shaderTestMode, (int)desiredUIComparison);
            graphic.material = materialCopy;
        }
    }
}
