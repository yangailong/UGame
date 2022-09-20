using UGame_Local;
using UnityEngine;
using UnityEngine.InputSystem;
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

        inputActions.GamePlay.Jump.performed += Jump_performed;
        inputActions.GamePlay.Movement.performed += Movement_performed;
    }


    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.GamePlay.Jump.performed -= Jump_performed;
        inputActions.GamePlay.Movement.performed -= Movement_performed;
    }



    private void Movement_performed(InputAction.CallbackContext obj)
    {
        Debug.Log($"Movement performed  {obj.ReadValue<Vector2>()}  {obj.ReadValueAsObject()}");
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        Debug.Log($"Jump performed :{obj.ReadValueAsButton()}");
    }



}

