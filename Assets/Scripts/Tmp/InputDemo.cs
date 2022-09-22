using UGame_Local;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
/// <summary> 说明</summary>
public class InputDemo : MonoBehaviour
{
    //https://www.bilibili.com/video/BV1xT4y1L7Cj/?vd_source=a2f3e46462dde0fcb4a0ba38c29651d8

    InputActions inputActions = null;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();

        inputActions.GamePlay.Jump.performed += Jump_performed;
        inputActions.GamePlay.Movement.performed += Movement_performed;
        Touch.onFingerDown += Touch_onFingerDown;
        Touch.onFingerMove += Touch_onFingerMove;
    }



    private void OnDisable()
    {

        inputActions.GamePlay.Jump.performed -= Jump_performed;
        inputActions.GamePlay.Movement.performed -= Movement_performed;
        Touch.onFingerDown -= Touch_onFingerDown;
        Touch.onFingerMove -= Touch_onFingerMove;

        inputActions.Disable();
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }



    private void Movement_performed(InputAction.CallbackContext obj)
    {
        Debug.Log($"Movement performed  {obj.ReadValue<Vector2>()}  {obj.ReadValueAsObject()}");
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        Debug.Log($"Jump performed :{obj.ReadValueAsButton()}");
    }


    private void Touch_onFingerDown(Finger obj)
    {
        Debug.Log($"{obj.screenPosition}");
    }

    private void Touch_onFingerMove(Finger obj)
    {
        Debug.Log($"{obj.screenPosition}");
    }

    private void Update()
    {
        Debug.Log($"{Touch.activeTouches}");

        foreach (Touch touch in Touch.activeTouches)
        {
            Debug.Log($"{touch.phase == UnityEngine.InputSystem.TouchPhase.Began}");
        }
    }
}

