using UnityEngine;

public class Base : MonoBehaviour
{

    /// <summary>UI��һ�μ���ʱ����</summary>
    public virtual void OnUIStart() { }

    /// <summary>UIÿ��openʱ����</summary>
    public virtual void OnUIEnable() { SetActive(true); }

    /// <summary>UIÿ��closeʱ����</summary>
    public virtual void OnUIDisable() { SetActive(false); }

    /// <summary>�ֶ�����</summary>
    public virtual void OnUIRefresh() { }

    /// <summary>UIÿ��ɾ��ʱ����</summary>
    public virtual void OnUIDestroy() { }

    public void SetActive(bool active)
    {
        if (active != gameObject.activeSelf)
        {
            gameObject.SetActive(active);
        }
    }

}
