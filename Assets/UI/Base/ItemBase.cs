using UnityEngine;

public abstract class ItemBase : MonoBehaviour, IItemBase
{
    public virtual bool IsActive
    {
        set
        {
            if (gameObject.activeInHierarchy != value)
            {
                gameObject.SetActive(value);
            }
        }
        get { return gameObject.activeInHierarchy; }
    }

    private RectTransform RT = null;

    protected RectTransform RectTrans
    {
        get
        {
            if (RT == null)
            {
                RT = GetComponent<RectTransform>();
            }
            return RT;
        }
    }

    public void SetData() { }

    public void Refresh() { }


    public virtual void Refresh<T>(T data) { if (data == null) return; }
 

}
