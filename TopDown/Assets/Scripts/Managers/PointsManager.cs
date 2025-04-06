using UnityEngine;

namespace Managers
{
    public class PointsManager : MonoBehaviour
    {
        public static PointsManager Instance;
        public int points;
    
        public delegate void OnPointsChanged(int points);
        public static event OnPointsChanged PointsChanged;
    
        private void Awake()
        {
            if(Instance == null)Instance = this;
            else Destroy(gameObject);
        }

        public void AddPoints()
        {
            points++;
            PointsChanged?.Invoke(points);
        }
    }
}
