using System;
using System.Collections.Generic;
using MVVM;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class ShipHealthView : MonoBehaviour
    {
        [NonSerialized] [Data("Health")] public ReactiveProperty<int> HealthProperty = new();
        [NonSerialized] [Data("MaxHealth")] public ReactiveProperty<int> MaxHealthProperty = new();

        [SerializeField] private Transform _heartsContainer;
        [Inject] private Heart.Factory _factory;

        private readonly List<Heart> _hearts = new();

        private void Awake()
        {
            MaxHealthProperty
                .Subscribe(CreateHearts)
                .AddTo(this);

            HealthProperty
                .Subscribe(SetHealth)
                .AddTo(this);
        }

        private void OnEnable()
        {
            if (_hearts.Count == 0 && MaxHealthProperty.Value > 0)
            {
                CreateHearts(MaxHealthProperty.Value);
            }
            if (_hearts.Count > 0)
            {
                SetHealth(HealthProperty.Value);
            }
        }

        private void CreateHearts(int maxHealth)
        {
            foreach (var heart in _hearts)
                Destroy(heart.gameObject);
            _hearts.Clear();

            for (int i = 0; i < maxHealth; i++)
            {
                var heart = _factory.Create();
                heart.transform.SetParent(_heartsContainer, false);
                _hearts.Add(heart);
            }

            SetHealth(HealthProperty.Value);
        }

        public void SetHealth(int value)
        {
            if (_hearts.Count < value)
            {
                int target = Mathf.Max(value, MaxHealthProperty.Value);
                if (target > 0)
                {
                    CreateHearts(target);
                }
            }
            for (int i = 0; i < _hearts.Count; i++)
                _hearts[i].SetActive(i < value);
        }
    }
}