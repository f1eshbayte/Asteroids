using MVVM;
using TMPro;
using UnityEngine;

namespace Asteroids
{
    public class LaserView : MonoBehaviour
    {
        [Data("CurrentShots")] [SerializeField] public TMP_Text CurrentShots;
        [Data("RechargeTimer")] [SerializeField] public TMP_Text RechargeTimer;
    }
}