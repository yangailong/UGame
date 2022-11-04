
namespace UGame_Remove
{
    public delegate void UICallback(UIPanelBase panel, params object[] param);

    public delegate void UIAnimCallback(UIPanelBase panel, UICallback callBack, params object[] param);


    public enum UIPanelLayer
    {

        GameUI = 0,

        Fixed = 1,

        Normal = 2,
        
        TopBar = 3,

        Upper = 4,

        PopUp = 5,
    }



}

