using UnityEngine;

namespace Project.Scripts.FractureCrystal
{
    public class ChunkPivotParent : MonoBehaviour
    {
        [SerializeField, Range(0f, 3f)] private float rotateSpeed = 0.4f;

        void Update()
        {
            transform.Rotate(Vector3.up * rotateSpeed, Space.Self);
        }
    }
}
