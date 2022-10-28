using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UGame_Remove
{

    [UILayer(layer: UIPanelLayer.GameUI)]
    public class DemoPanel : UIPanelBase, IUIAnimation
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


        public IEnumerator EnterAnim(UICallback callBack = null)
        {
            yield return new WaitForSeconds(2f);

            Debug.LogError($"打开页面开始动画完成....");

            callBack?.Invoke(this);
        }


        public IEnumerator ExitAnim(UICallback callBack = null, params object[] param)
        {
            yield return new WaitForSeconds(2f);

            Debug.LogError($"关闭页面结束动画完成....");

            callBack?.Invoke(this);
        }


        void OnClickMaskBtn()
        {
            Debug.LogError($"点击.....");
        }


    }
}
