using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
    public class MobileFireButtons : MonoBehaviour
    {
        [SerializeField] private Button _bulletButton;
        [SerializeField] private Button _laserButton;

        public bool BulletPressed { get; private set; }
        public bool LaserPressed { get; private set; }

        private void Awake()
        {
            _bulletButton.onClick.AddListener(() => BulletPressed = true); 
            _laserButton.onClick.AddListener(() => LaserPressed = true); 
        }

        private void LateUpdate()
        {
            BulletPressed = false;
            LaserPressed = false;
        }
    }
}