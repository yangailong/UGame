using UnityEngine;

public class UILayerMgr : MonoBehaviour
{

    [SerializeField] private UIRootData uiRootData=default;

    private void Awake()
    {
        if (uiRootData.m_Canvas == null)
        {
            Debug.LogError($"UILayerMgr :Root is null!");
        }

        if (uiRootData.m_camera == null)
        {
            Debug.LogError($"UILayerMgr :Camera is null!");
        }

        if (uiRootData.m_One == null)
        {
            Debug.LogError($"UILayerMgr :One is null! ");
        }

        if (uiRootData.m_Two == null)
        {
            Debug.LogError($"UILayerMgr :Two is null!");
        }

        if (uiRootData.m_Three == null)
        {
            Debug.LogError($"UILayerMgr :Three is null!");
        }

        if (uiRootData.m_Four == null)
        {
            Debug.LogError($"UILayerMgr :Four is null!");
        }
    }


    public void SetLayer(UIBase ui)
    {
        ui.transform.SetParent(GetLayer(ui.m_UIType));
        ui.name = ui.GetType().Name;
        RectTransform rt = ui.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;
        rt.sizeDelta = Vector2.zero;


        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector3.one;

        rt.sizeDelta = Vector2.zero;
        rt.transform.localPosition = new Vector3(0, 0, 0);
        rt.anchoredPosition3D = new Vector3(0, 0, 0);
        rt.SetAsLastSibling();
    }


    public RectTransform GetLayer(UIType type)
    {
        switch (type)
        {
            case UIType.One: return uiRootData.m_One;
            case UIType.Two: return uiRootData.m_Two;
            case UIType.Three: return uiRootData.m_Three;
            case UIType.Four: return uiRootData.m_Four;
        }
        return null;
    }

}


[System.Serializable]
public struct UIRootData
{
    public Canvas m_Canvas;
    public Camera m_camera;
    public RectTransform m_One;
    public RectTransform m_Two;
    public RectTransform m_Three;
    public RectTransform m_Four;
}
