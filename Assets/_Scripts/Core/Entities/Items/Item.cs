using UnityEngine;
using UnityEngine.UIElements;

namespace InkeepersKeep.Core.Entities.Items
{
    public class Item : MonoBehaviour, IGrabbable, IColorTintable, IHoverable
    {   
        /* Needed for color tint */
        [SerializeField] private Renderer   _renderer;
        [SerializeField] private Shader     _itemActiveShader;
        [SerializeField] private Shader     _itemDefaultShader;
        [SerializeField] private float      _itemTintStrength = 0.1f;
        [SerializeField] private Color      _itemTintColor = new Color(1.0f, 1.0f, 1.0f);

        /* Needed for item hovering ui and description */
        [SerializeField] private UIDocument _itemHoveringUI;
        private VisualElement               _itemInfoUIWindow;
        private Label                       _itemInfoUILabelName;
        private Label                       _itemInfoUILabelItemType;
        private Label                       _itemInfoUILabelDescription;
        private float                       _itemInfoUIOpacity = 0;
        [SerializeField] private float      _itemInfoUITransitionSpeed = 0.05f;

        /* Physics */
        [SerializeField] private Rigidbody  _rigidbody;
        [SerializeField] private float      _dropForce = 5f;
        [SerializeField] private float      _dropMinDistance = 0.2f;
            
        /* Item grabbing */
        [SerializeField] private float      _interpolationSpeed = 10f;
        private Transform                   _grabPoint;
        private bool                        _isDragging = false;

        /* General Item information */
        [SerializeField] private string     _itemName;
        [SerializeField] private ItemType   _itemType;
        [SerializeField] private string     _itemDescription;

        /* Item connector thing */
        public bool _isConnectedToConnector;
        public bool _isCollidingWithItemConnector;
        public ItemConnector _itemConnector;

        public void Start()
        {
            var rootElement = _itemHoveringUI.rootVisualElement;

            _itemInfoUIWindow = rootElement.Q<VisualElement>("ItemInfoWindow");
            _itemInfoUILabelName = rootElement.Q<Label>("ItemName");
            _itemInfoUILabelItemType = rootElement.Q<Label>("ItemType");
            _itemInfoUILabelDescription = rootElement.Q<Label>("ItemDescription");            
        }

        private void FixedUpdate()
        {
            if (_grabPoint == null)
                return;

            Vector3 newPosition = Vector3.Lerp(transform.position, _grabPoint.position, Time.deltaTime * _interpolationSpeed);
            _rigidbody.MovePosition(newPosition);
        }

        public void Grab(Transform grabPoint)
        {
            if(_isConnectedToConnector && _itemConnector != null)
            {
                _itemConnector.Detach();
            }

            _grabPoint = grabPoint;
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            ChangeTint();
            _isDragging = true;
        }

        public void Drop()
        {
            if(_isCollidingWithItemConnector == true)
            {
                _itemConnector.Attach(GetComponent<Item>());
                _isConnectedToConnector = true;
            } else
            {
                _rigidbody.useGravity = true;
                _rigidbody.constraints = RigidbodyConstraints.None;

                if (Vector3.Distance(transform.position, _grabPoint.position) > _dropMinDistance)
                {
                    Vector3 moveDirection = (_grabPoint.position - transform.position);
                    _rigidbody.velocity = moveDirection * _dropForce;
                }
                else
                {
                    _rigidbody.velocity = Vector3.zero;
                }
            }

            _grabPoint = null;

            _isDragging = false;
            ReturnDefaultTint();
        }

        public void Hover()
        {
            if (!_isDragging)
                ShowItemInfoUI();
            else
                UnShowItemInfoUI();

            /* Lets change item color tint */
            ChangeTint();
        }

        public void Unhover()
        {
            /* Item info ui */
            UnShowItemInfoUI();

            if (_isDragging) return;

            /* Lets change item color tint*/
            ReturnDefaultTint();
        }

        private void ShowItemInfoUI()
        {
            /* Item info ui */
            _itemInfoUIWindow.visible = true;
            _itemInfoUIWindow.style.opacity = _itemInfoUIOpacity;

            if (_itemInfoUIOpacity < 1.0f) _itemInfoUIOpacity += _itemInfoUITransitionSpeed;

            _itemInfoUILabelName.text = _itemName;
            _itemInfoUILabelItemType.text = _itemType.ToString();
            _itemInfoUILabelDescription.text = _itemDescription;
        }

        private void UnShowItemInfoUI()
        {
            _itemInfoUIWindow.style.opacity = _itemInfoUIOpacity;
            if (_itemInfoUIOpacity > 0.0f) _itemInfoUIOpacity -= _itemInfoUITransitionSpeed;
            if (_itemInfoUIOpacity <= 0)
            {
                _itemInfoUIWindow.visible = false;
            }
        }

        public void ChangeTint()
        {
            _renderer.material.shader = _itemActiveShader;
            _renderer.material.SetColor("_ItemTintColorOverlay", _itemTintColor);
            _renderer.material.SetFloat("_ItemTintStrength", _itemTintStrength);
        }

        public void ReturnDefaultTint() => _renderer.material.shader = _itemDefaultShader;
    }
}
