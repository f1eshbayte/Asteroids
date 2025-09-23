using UnityEngine;
using Zenject;

namespace Asteroids
{
    public class ConfigsInstaller : MonoInstaller
    {
        [SerializeField] private string _shipFile = "ship.json";
        [SerializeField] private string _asteroidsFile = "asteroids.json";
        [SerializeField] private string _ufoFile = "ufo.json";
        [SerializeField] private string _worldMapFile = "worldMap.json";

        public override void InstallBindings()
        {
            BindShipConfig();
            BindAsteroidConfig();
            BindUfoConfig();
            BindWorldMapConfig();
        }

        private void BindWorldMapConfig()
        {
            var worldMapConfig = ConfigLoader.LoadConfig<WorldMapConfig>(_worldMapFile);
            Container.Bind<WorldMapConfig>().FromInstance(worldMapConfig).AsSingle();
        }

        private void BindUfoConfig()
        {
            var ufoConfig = ConfigLoader.LoadConfig<UfoConfig>(_ufoFile);
            Container.Bind<UfoConfig>().FromInstance(ufoConfig).AsSingle();
        }

        private void BindAsteroidConfig()
        {
            var asteroidsConfig = ConfigLoader.LoadConfig<AsteroidsConfig>(_asteroidsFile);
            Container.Bind<AsteroidsConfig>().FromInstance(asteroidsConfig).AsSingle();
        }

        private void BindShipConfig()
        {
            var shipConfig = ConfigLoader.LoadConfig<ShipConfig>(_shipFile);
            Container.Bind<ShipConfig>().FromInstance(shipConfig).AsSingle();
        }
    }

}