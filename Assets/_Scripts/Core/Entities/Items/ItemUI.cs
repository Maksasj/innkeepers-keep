using UnityEngine;
using UnityEngine.UIElements;

namespace InkeepersKeep.Core.Entities.Items
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField] private UIDocument _itemHoveringUI;
        private VisualElement _itemInfoUIWindow;
        private Label _itemInfoUILabelName;
        private Label _itemInfoUILabelItemType;
        private Label _itemInfoUILabelDescription;
        private float _itemInfoUIOpacity = 0f;

        [SerializeField] private float _itemInfoUITransitionSpeed = 0.05f;

        public void Awake()
        {
            var rootElement = _itemHoveringUI.rootVisualElement;

            _itemInfoUIWindow = rootElement.Q<VisualElement>("ItemInfoWindow");
            _itemInfoUILabelName = rootElement.Q<Label>("ItemName");
            _itemInfoUILabelItemType = rootElement.Q<Label>("ItemType");
            _itemInfoUILabelDescription = rootElement.Q<Label>("ItemDescription");
        }

        public void ShowItemInfoUI(ItemData data)
        {
            _itemInfoUIWindow.visible = true;
            _itemInfoUIWindow.style.opacity = _itemInfoUIOpacity;

            if (_itemInfoUIOpacity < 1.0f)
                _itemInfoUIOpacity += _itemInfoUITransitionSpeed;

            _itemInfoUILabelName.text = data.Name;
            _itemInfoUILabelItemType.text = data.Type.ToString();
            _itemInfoUILabelDescription.text = data.Description;
        }

        public void HideItemInfoUI()
        {
            _itemInfoUIWindow.style.opacity = _itemInfoUIOpacity;

            if (_itemInfoUIOpacity > 0f) 
                _itemInfoUIOpacity -= _itemInfoUITransitionSpeed;

            if (_itemInfoUIOpacity <= 0)
                _itemInfoUIWindow.visible = false;
        }
    }
}
