//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace UGame.Remove {

	public partial class CfgRemoteLang : ScriptableObject {

		[SerializeField]
		private CfgRemoteLangItem[] _Items;
		private CfgRemoteLangItem[] items { get { return _Items; } }

		public CfgRemoteLangItem Get(string id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				CfgRemoteLangItem item = _Items[index];
				if (item.id == id) { return item; }
				if (string.Compare(id, item.id) < 0) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class CfgRemoteLangItem {

		[SerializeField]
		private string _Id;
		public string id { get { return _Id; } }

		[SerializeField]
		private string _CN;
		public string CN { get { return _CN; } }

		[SerializeField]
		private string _EN;
		public string EN { get { return _EN; } }

		[SerializeField]
		private string _TC;
		public string TC { get { return _TC; } }

		[SerializeField]
		private string _DE;
		public string DE { get { return _DE; } }

		[SerializeField]
		private string _ES;
		public string ES { get { return _ES; } }

		[SerializeField]
		private string _FR;
		public string FR { get { return _FR; } }

		public override string ToString() {
			return string.Format("[CfgRemoteLangItem]{{id:{0}, CN:{1}, EN:{2}, TC:{3}, DE:{4}, ES:{5}, FR:{6}}}",
				id, CN, EN, TC, DE, ES, FR);
		}

	}

}
