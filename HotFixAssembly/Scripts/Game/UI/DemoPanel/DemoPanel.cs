using UnityEngine;
using UnityEngine.UI;

namespace UGame_Remove
{

    [UILayer(layer: UIPanelLayer.GameUI)]
    public class DemoPanel : UIPanelBase
    {
        public override void OnUIAwake()
        {
            Debug.LogError($"OnUIAwake");

        }

        public override void OnUIStart()
        {
            Debug.LogError($"OnUIStart");
        }

        public override void OnUIEnable()
        {
            Debug.LogError($"OnUIEnable");
            transform.GetMountChind<Button>("m_Mask").onClick.AddListener(OnClickMaskBtn);
        }


        public override void OnUIDisable()
        {
            transform.GetMountChind<Button>("m_Mask").onClick.RemoveAllListeners();
        }


        void OnClickMaskBtn()
        {
            Debug.LogError($"点击.....");
        }




    }
}
