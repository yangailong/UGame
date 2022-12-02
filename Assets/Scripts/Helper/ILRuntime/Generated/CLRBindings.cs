using System;
using System.Collections.Generic;
using System.Reflection;
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = ILRuntime.Other.UncheckedList<object>;
#endif
namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

//will auto register in unity
#if UNITY_5_3_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static private void RegisterBindingAction()
        {
            ILRuntime.Runtime.CLRBinding.CLRBindingUtils.RegisterBindingAction(Initialize);
        }

        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2> s_UnityEngine_Vector2_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3> s_UnityEngine_Vector3_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion> s_UnityEngine_Quaternion_Binding_Binder = null;

        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            Google_Protobuf_ProtoPreconditions_Binding.Register(app);
            System_String_Binding.Register(app);
            Google_Protobuf_CodedOutputStream_Binding.Register(app);
            Google_Protobuf_CodedInputStream_Binding.Register(app);
            Google_Protobuf_MessageParser_1_IMessageAdapt_Binding_Adaptor_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            UGame_Local_AssetsMapperConst_Binding.Register(app);
            UnityEngine_WaitForEndOfFrame_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            System_Array_Binding.Register(app);
            System_NotSupportedException_Binding.Register(app);
            UnityEngine_TextAsset_Binding.Register(app);
            System_ArgumentNullException_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_String_Binding.Register(app);
            System_Char_Binding.Register(app);
            System_ArgumentException_Binding.Register(app);
            System_Collections_Generic_List_1_String_Binding.Register(app);
            System_Collections_IEnumerable_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_KeyCollection_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_Dictionary_2_Int32_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_Dictionary_2_Int32_ILTypeInstance_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_List_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_List_1_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_List_1_ILTypeInstance_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_Dictionary_2_Int32_ILTypeInstance_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_Dictionary_2_Int32_ILTypeInstance_Binding_KeyCollection_Binding_Enumerator_Binding.Register(app);
            UnityEngine_AudioSource_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_List_1_ILTypeInstance_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_List_1_ILTypeInstance_Binding_KeyCollection_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_List_1_GameObject_Binding.Register(app);
            System_Collections_Generic_List_1_GameObject_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_Dictionary_2_Int32_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_GameObject_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_List_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_GameObject_List_1_ILTypeInstance_Binding.Register(app);
            UnityEngine_Mathf_Binding.Register(app);
            UnityEngine_UI_Button_Binding.Register(app);
            UnityEngine_Events_UnityEvent_Binding.Register(app);
            System_Collections_Generic_Queue_1_ILTypeInstance_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Reflection_MemberInfo_Binding.Register(app);
            System_Single_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ScriptableObject_Binding.Register(app);
            System_Reflection_Assembly_Binding.Register(app);
            System_Linq_Enumerable_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding.Register(app);
            UnityEngine_MonoBehaviour_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            System_Action_1_Single_Binding.Register(app);
            System_Action_Binding.Register(app);
            UnityEngine_WaitForSeconds_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Enum_ILTypeInstance_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Threading_Interlocked_Binding.Register(app);
            WebSocket4Net_WebSocket_Binding.Register(app);
            System_MethodAccessException_Binding.Register(app);
            UnityEngine_JsonUtility_Binding.Register(app);
            System_EventHandler_Binding.Register(app);
            System_EventHandler_1_ErrorEventArgs_Binding.Register(app);
            System_EventHandler_1_MessageReceivedEventArgs_Binding.Register(app);
            System_EventHandler_1_DataReceivedEventArgs_Binding.Register(app);
            WebSocket4Net_DataReceivedEventArgs_Binding.Register(app);
            System_IO_MemoryStream_Binding.Register(app);
            System_IO_BinaryReader_Binding.Register(app);
            System_IO_Stream_Binding.Register(app);
            System_MissingMemberException_Binding.Register(app);
            System_IO_BinaryWriter_Binding.Register(app);
            Google_Protobuf_MessageExtensions_Binding.Register(app);
            UnityEngine_Networking_UnityWebRequest_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_String_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_String_Binding.Register(app);
            UnityEngine_Networking_DownloadHandler_Binding.Register(app);
            System_Text_Encoding_Binding.Register(app);
            UnityEngine_Networking_UploadHandlerRaw_Binding.Register(app);
            System_Action_2_Int32_IMessageAdapt_Binding_Adaptor_Binding.Register(app);
            System_Activator_Binding.Register(app);
            UnityEngine_Pool_ObjectPool_1_Component_Binding.Register(app);
            UGame_Local_UGame_Binding.Register(app);
            UGame_Local_CfgUGame_Binding.Register(app);
            UnityEngine_PlayerPrefs_Binding.Register(app);
            UGame_Local_CryptoManager_Binding.Register(app);
            System_Exception_Binding.Register(app);
            UnityEngine_AddressableAssets_Addressables_Binding.Register(app);
            UnityEngine_ResourceManagement_AsyncOperations_AsyncOperationHandle_1_TextAsset_Binding.Register(app);
            System_Action_1_TextAsset_Binding.Register(app);
            UnityEngine_ResourceManagement_AsyncOperations_AsyncOperationHandle_1_GameObject_Binding.Register(app);
            System_Action_1_GameObject_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Object_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Object_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_Object_Binding.Register(app);
            System_Action_1_ScriptableObject_Binding.Register(app);
            System_IO_Path_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_MonoBehaviourAdapter_Binding_Adaptor_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_RectTransform_Binding.Register(app);
            System_Action_1_MonoBehaviourAdapter_Binding_Adaptor_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_MonoBehaviourAdapter_Binding_Adaptor_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_MonoBehaviourAdapter_Binding_Adaptor_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            System_Enum_Binding.Register(app);
            System_ApplicationException_Binding.Register(app);
            UnityEngine_Application_Binding.Register(app);
            System_Reflection_MethodBase_Binding.Register(app);
            UnityEngine_Events_UnityEventBase_Binding.Register(app);
            UGame_Local_MountObject_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector2));
            s_UnityEngine_Vector2_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector3));
            s_UnityEngine_Vector3_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Quaternion));
            s_UnityEngine_Quaternion_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion>;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            s_UnityEngine_Vector2_Binding_Binder = null;
            s_UnityEngine_Vector3_Binding_Binder = null;
            s_UnityEngine_Quaternion_Binding_Binder = null;
        }
    }
}
