using System;
using System.Collections.Generic;

namespace UGame_Local_Editor
{
    public class ExcelToScriptableObjectImpl
    {
        public List<ExcelToScriptableObjectSetting> excel_settings = null;

        public ExcelToScriptableObjectImpl()
        {
            excel_settings = new List<ExcelToScriptableObjectSetting>();
        }


        public void AddNewExcel()
        {
            excel_settings.Add(new ExcelToScriptableObjectSetting());
        }
    }

    [System.Serializable]
    public class ExcelToScriptableObjectSetting
    {
        public string excel_name;
        public string script_directory = "Assets";
        public string asset_directory = "Assets";
        public string name_space;
        public bool use_hash_string = false;
        public bool hide_asset_properties = true;
        public bool generate_get_method_if_possible = true;
        public bool key_to_multi_values = false;
        public bool use_public_items_getter = false;
        public bool compress_color_into_int = true;
        public bool treat_unknown_types_as_enum = false;
        public bool generate_tostring_method = true;

        [System.NonSerialized]
        public bool dir_manual_edit;

        [System.NonSerialized]
        public int script_dir_index;

        [System.NonSerialized]
        public int asset_dir_index;

        public void UpdateDirIndices(string[] folders)
        {
            script_dir_index = -1;
            asset_dir_index = -1;
            for (int i = 0, imax = folders.Length; i < imax; i++)
            {
                string folder = folders[i];
                if (script_directory == folder) { script_dir_index = i; }
                if (asset_directory == folder) { asset_dir_index = i; }
            }
        }
    }

}
