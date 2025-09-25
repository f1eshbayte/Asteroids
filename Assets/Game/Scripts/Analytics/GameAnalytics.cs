using Firebase.Analytics;
using UnityEngine;

namespace Asteroids
{
    public class GameAnalytics : MonoBehaviour
    {
        private void Start()
        {
            FirebaseAnalytics.LogEvent("Start App");
        }
    }
}