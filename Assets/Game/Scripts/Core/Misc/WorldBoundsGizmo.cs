using UnityEngine;
using Zenject;


namespace Asteroids
{
    [ExecuteAlways]
    public class WorldBoundsGizmo : MonoBehaviour
    {
        [Inject] private PhysicsWorld _world;
        [SerializeField] private Color _color = Color.green;

        private void OnDrawGizmos()
        {
            if (_world == null) return;

            Gizmos.color = _color;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(_world.Width, _world.Height, 0));
        }
    }
}