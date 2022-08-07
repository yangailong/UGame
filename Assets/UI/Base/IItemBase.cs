using UnityEngine;

public interface IItemBase
{
    void SetData();
    void Refresh();
    void Refresh<T>(T data);
    //void Update();
    //void Destory();
   // void Clear();
}
