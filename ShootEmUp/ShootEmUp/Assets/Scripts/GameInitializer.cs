using UnityEngine;

namespace DefaultNamespace
{
    public class GameInitializer : MonoBehaviour
    {

        void Start()
        {
            ObjectPool.Initialize();
        }
        
    }
}