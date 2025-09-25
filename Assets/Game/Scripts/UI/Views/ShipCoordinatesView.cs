using UnityEngine;
using TMPro;
using MVVM;

namespace Asteroids
{
    public class ShipCoordinatesView : MonoBehaviour
    {
        [Data("Position")] [SerializeField] public TMP_Text PositionText;
        [Data("Rotation")] [SerializeField] public TMP_Text RotationText;
        [Data("Velocity")] [SerializeField] public TMP_Text VelocityText;
    }
}