using Asteroids;
using UnityEngine;
using Zenject;

public class EffectInstaller : MonoInstaller
{
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private int _poolSize;
    public override void InstallBindings()
    {
        var parent = new GameObject("HitEffectsContainer").transform;

        Container.Bind<ParticleHitEffectFactory>()
            .AsSingle()
            .WithArguments(_hitEffect, parent);

        Container.Bind<ParticleHitEffectPool>()
            .AsSingle()
            .WithArguments(_poolSize);
    }
}
