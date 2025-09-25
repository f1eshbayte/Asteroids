using UnityEngine;

namespace Asteroids
{
    public interface IShipInput
    {
        float Rotation { get; }
        float Thrust { get; }
        bool FireBullet { get; }
        bool FireLaser { get; }
    }

    public class DesktopShipInput : IShipInput
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        
        public float Rotation => Input.GetAxis(Horizontal);
        public float Thrust => Mathf.Max(0, Input.GetAxis(Vertical));
        public bool FireBullet => Input.GetKeyDown(KeyCode.Space);
        public bool FireLaser => Input.GetKeyDown(KeyCode.LeftControl);
    }

    public class MobileShipInput : IShipInput
    {
        private VirtualJoystick _joystick;
        private MobileFireButtons _buttons;

        public MobileShipInput(VirtualJoystick joystick, MobileFireButtons buttons)
        {
            _joystick = joystick;
            _buttons = buttons;
        }

        public float Rotation => _joystick.Horizontal;
        public float Thrust => Mathf.Max(0, _joystick.Vertical);
        public bool FireBullet => _buttons.BulletPressed;
        public bool FireLaser => _buttons.LaserPressed;
    }
}