using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        private SortedList<int> targets = new SortedList<int>();

        private void Start()
        {
            targets.Add(3);
            targets.Add(5);
            targets.Add(8);
            targets.Add(10);
            targets.Add(12);
            
        Debug.Log(targets.IndexOf(8));
        }
    }
}