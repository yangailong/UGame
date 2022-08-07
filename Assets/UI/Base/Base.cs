using UnityEngine;

public class Base : MonoBehaviour
{

    /// <summary>UI第一次加载时调用</summary>
    public virtual void OnUIStart() { }

    /// <summary>UI每次open时调用</summary>
    public virtual void OnUIEnable() { SetActive(true); }

    /// <summary>UI每次close时调用</summary>
    public virtual void OnUIDisable() { SetActive(false); }

    /// <summary>手动调用</summary>
    public virtual void OnUIRefresh() { }

    /// <summary>UI每次删除时调用</summary>
    public virtual void OnUIDestroy() { }

    public void SetActive(bool active)
    {
        if (active != gameObject.activeSelf)
        {
            gameObject.SetActive(active);
        }
    }

}
