using TMPro;
using MVVM;
using UnityEngine;

namespace Asteroids
{
    public class RewardView : MonoBehaviour
    {
        [Data("Reward")] [SerializeField] public TMP_Text RewardText;
    }
}