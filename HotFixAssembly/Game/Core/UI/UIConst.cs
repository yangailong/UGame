using System;


namespace UGame_Remove
{
    public delegate void UICallback(UIPanelBase currUI, params object[] param);

    public enum UIPanelLayer
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
    }

}

