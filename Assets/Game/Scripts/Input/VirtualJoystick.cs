using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Asteroids
{
    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private RectTransform _handle;
        [SerializeField] private float _maxRadius = 100f;

        private Vector2 _input = Vector2.zero;

        public float Horizontal => _input.x;
        public float Vertical => _input.y;
        
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform, 
                eventData.position,
                eventData.pressEventCamera,
                out position);
            position = Vector2.ClampMagnitude(position, _maxRadius);
            _handle.anchoredPosition = position;

            _input = position / _maxRadius;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            _input = Vector2.zero;
            _handle.anchoredPosition = Vector2.zero;
        }

    }
}