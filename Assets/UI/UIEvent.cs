
public delegate void UICallBack(UIBase currUI, params object[] objs);

public delegate void UIAnimCallBack(UIBase UIbase, UICallBack callBack, params object[] objs);

public enum UIType
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
}