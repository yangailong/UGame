using UnityEngine;
public class ClassBinding : MonoBehaviour
{
    [SerializeField]
    private string m_ClassName = string.Empty;

    private void Awake()
    {
        //gameObject.AddComponent<binderTest>();

        //Type type = Type.GetType(m_ClassName);

       // Debug.Log($"name:{type.Name}");

        //gameObject.AddComponent(type);
    }


    private void Start()
    {
        
    }


}
