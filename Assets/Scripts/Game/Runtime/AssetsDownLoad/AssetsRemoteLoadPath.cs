using UnityEngine;

namespace AbeatsGame
{

    /// <summary>资源下载路径</summary>

    public class AssetsRemoteLoadPath
    {
        private static string path = $"https://testflutteraab.s3.ap-southeast-1.amazonaws.com/Assets";


        //{AbeatsGame.AssetsRemoteLoadPath.Path}/[BuildTarget]
        public static string Path
        {
            get
            {
                return $"{path}/{Application.version}";
            }
            set
            {
                path = value;
            }
        }
    }
}
