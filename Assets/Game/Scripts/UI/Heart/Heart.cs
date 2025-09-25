using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class Heart : MonoBehaviour
    {
        [SerializeField] private GameObject body;
        public void SetActive(bool value) => body.SetActive(value);

        public class Factory : PlaceholderFactory<Heart> { }
    }
}