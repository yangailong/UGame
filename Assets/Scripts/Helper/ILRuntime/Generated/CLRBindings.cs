using System;
using System.Collections.Generic;
using System.Reflection;

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

        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            UnityEngine_Debug_Binding.Register(app);
            UGame_Local_AssetsMappingConst_Binding.Register(app);
            System_String_Binding.Register(app);
            UnityEngine_ResourceManagement_AsyncOperations_AsyncOperationHandle_1_SceneInstance_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            UnityEngine_TextAsset_Binding.Register(app);
            System_Action_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_String_Binding.Register(app);
            System_Collections_Generic_List_1_String_Binding.Register(app);
            System_Collections_IEnumerable_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Char_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ScriptableObject_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Reflection_Assembly_Binding.Register(app);
            System_Linq_Enumerable_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding.Register(app);
            System_Reflection_MemberInfo_Binding.Register(app);
            System_Text_Encoding_Binding.Register(app);
            System_Security_Cryptography_Aes_Binding.Register(app);
            System_Security_Cryptography_SymmetricAlgorithm_Binding.Register(app);
            System_Security_Cryptography_ICryptoTransform_Binding.Register(app);
            System_Convert_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Enum_ILTypeInstance_Binding.Register(app);
            System_Object_Binding.Register(app);
            UnityEngine_PlayerPrefs_Binding.Register(app);
            System_Exception_Binding.Register(app);
            UnityEngine_AddressableAssets_Addressables_Binding.Register(app);
            UnityEngine_ResourceManagement_Util_ComponentSingleton_1_MonoBehaviourRuntime_Binding.Register(app);
            UnityEngine_MonoBehaviour_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Object_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_MonoBehaviourAdapter_Binding_Adaptor_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_RectTransform_Binding.Register(app);
            System_Action_1_MonoBehaviourAdapter_Binding_Adaptor_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_MonoBehaviourAdapter_Binding_Adaptor_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_MonoBehaviourAdapter_Binding_Adaptor_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            System_Array_Binding.Register(app);
            System_Attribute_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            System_Enum_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            System_NotSupportedException_Binding.Register(app);
            UnityEngine_UI_CanvasScaler_Binding.Register(app);
            UnityEngine_Screen_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);
            UnityEngine_WaitForSeconds_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector2));
            s_UnityEngine_Vector2_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector3));
            s_UnityEngine_Vector3_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3>;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            s_UnityEngine_Vector2_Binding_Binder = null;
            s_UnityEngine_Vector3_Binding_Binder = null;
        }
    }
}
