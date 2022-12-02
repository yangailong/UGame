using UnityEngine;
namespace UGame_Local
{
    [CreateAssetMenu(fileName = "UGame", menuName = "UGame/CfgUGame")]
    public class CfgUGame : ScriptableObject
    {

        /// <summary>
        /// ������Կ.�������ݺ�dll
        /// </summary>
        public string key = null;


        /// <summary>
        /// ILRuntime�Ĵ���ģʽ
        /// </summary>
        public ILRuntimeJITFlags jITFlags = ILRuntimeJITFlags.None;


        /// <summary>
        ///   PDB�ļ��ǵ������ݿ⣬����Ҫ����־����ʾ������кţ�������ṩPDB�ļ����������ڻ��������ڴ棬��ʽ����ʱ�뽫PDBȥ��
        /// </summary>
        public bool usePdb = true;

      
    }


    public enum ILRuntimeJITFlags
    {
        None = 0,

        /// <summary>
        /// ����JITģʽ��ʹ�ø�ģʽ��Ĭ�ϵ�����»ᰴ��ԭʼ�ķ�ʽ���У����÷���������ִ��ʱ���ᱻ���Ϊ��Ҫ��JIT�����ں�̨�߳����JIT������л����Ĵ���ģʽ����
        /// </summary>
        JITOnDemand = 1,

        /// <summary>
        /// ����JITģʽ��ʹ�ø�ģʽʱ�������������õ�˲�伴�ᱻִ��JIT���룬�ڵ�һ��ִ��ʱ��ʹ�üĴ���ģʽ���С�
        /// </summary>
        JITImmediately = 2,

        /// <summary>
        /// ����JITģʽ���÷�����ִ��ʱ��ʼ���Դ�ͳ��ʽִ��
        /// </summary>
        NoJIT = 4,

        /// <summary>
        /// ǿ������ģʽ����ģʽֻ�Է�����Attribute��Ч����ע��ģʽ�ķ����ڱ�����ʱ�������ӷ��������ݴ�С��ǿ�Ʊ�����
        /// </summary>
        ForceInline = 8,



/*        ʹ�ý���
ILRuntime�Ƽ���ʹ��ģʽ��2�֣�

AppDomain���캯��ʱ��ָ��JITģʽ����Ĭ��ʹ�ô�ͳģʽִ�У���������Ҫ�Ż����ܼ������ͷ���ʱ���Ը÷���ָ��JITImmediatelyģʽ
ֱ����AppDomain���캯����ָ��JITOnDemandģʽ
��һ���÷�������ʵ��Ӱ����С��������Ҫ�Ż������������ԱȽϾ�׼�Ŀ���ִ��Ч�����������֪����ʲôʱ��Ӧ��ʹ�ú���ģʽ��Ҳ����ֱ��ʹ��JITOnDemandģʽ����ILRuntime���о�������ģʽ���ڴ������������ܴﵽ���������ƽ��ġ�

*/


    }

}