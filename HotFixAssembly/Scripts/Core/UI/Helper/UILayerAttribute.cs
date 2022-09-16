using System;

namespace UGame_Remove
{

    public class UILayerAttribute : Attribute
    {
        public UIPanelLayer layer { get; }

        public UILayerAttribute(UIPanelLayer layer)
        {
            this.layer = layer;
        }

    }
}
