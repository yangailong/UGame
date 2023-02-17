
namespace UGame_Remove
{

    public delegate void UICallback(UIPanelBase panel, params object[] message);


    public delegate void UIAnimCallback(UIPanelBase panel, UICallback callBack, params object[] message);


    public enum UIPanelLayer
    {

        /// <summary>用于显示主界面、设置界面、商城界面等</summary>
        GameUI = 0,

        /// <summary>用于显示固定的UI元素，如固定在屏幕上方的头像、生命值、能量条等</summary>
        Fixed = 1,

        /// <summary>用于显示普通的UI元素，如菜单、文本信息、背包栏等</summary>
        Normal = 2,

        /// <summary>用于显示顶部导航栏，通常包括标题、返回按钮、设置按钮等</summary>
        TopBar = 3,

        /// <summary>用于显示位于普通UI元素上方的UI元素，如弹出窗口、提示框、通知消息等</summary>
        Upper = 4,

        /// <summary>用于显示弹出窗口，如确认框、对话框、奖励弹窗等</summary>
        PopUp = 5,
    }



}

