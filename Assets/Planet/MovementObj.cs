using UnityEngine;

namespace Planet
{
    public class MovementObj : MonoBehaviour
    {
        public int orbitStartPercent; //TODO between 0 and 99, not implemented
        public float orbitalRadius; // min 7, max 56

        public float currentRotDegree;
        
        public GameObject[] satellites;
    }
}