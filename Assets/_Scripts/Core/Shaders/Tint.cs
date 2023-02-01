using UnityEngine;

namespace InkeepersKeep.Core.Shaders
{
    public class Tint : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Shader _itemActiveShader;
        [SerializeField] private Shader _itemDefaultShader;
        [SerializeField] private float _itemTintStrength = 0.1f;
        [SerializeField] private Color _itemTintColor = new Color(1.0f, 1.0f, 1.0f);

        public void Enable()
        {
            _renderer.material.shader = _itemActiveShader;
            _renderer.material.SetColor("_ItemTintColorOverlay", _itemTintColor);
            _renderer.material.SetFloat("_ItemTintStrength", _itemTintStrength);
        }

        public void Disable() => _renderer.material.shader = _itemDefaultShader;
    }
}
