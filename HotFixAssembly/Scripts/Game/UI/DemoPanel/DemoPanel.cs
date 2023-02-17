﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UGame_Remove
{

    [UILayer(layer: UIPanelLayer.GameUI)]
    public class DemoPanel : UIPanelBase, IUIEnterAnimation, IUIExitAnimation
    {

        public override void OnUIAwake()
        {
            //Debug.LogError($"OnUIAwake");
        }


        public override void OnUIStart()
        {
            //Debug.LogError($"OnUIStart");
        }


        public override void OnUIEnable()
        {
            //Debug.LogError($"OnUIEnable");
            transform.GetMountChind<Button>("m_Mask").onClick.AddListener(OnClickMaskBtn);
            transform.GetMountChind<Button>("m_CloseBtn").onClick.AddListener(OnClickMaskBtn);
        }



        public override void OnUIDisable()
        {
            transform.GetMountChind<Button>("m_Mask").onClick.RemoveAllListeners();
        }


        public IEnumerator EnterAnim()
        {
            yield return new WaitForSeconds(2f);

            Debug.LogError($"打开页面开始动画完成....");
           
        }


        public IEnumerator ExitAnim()
        {
            yield return new WaitForSeconds(2f);

            Debug.LogError($"关闭页面结束动画完成....");
        }




        void OnClickMaskBtn()
        {
            Debug.LogError($"点击.....");

            //NetProxy.Instance.C2SMessage();

        }





    }
}
