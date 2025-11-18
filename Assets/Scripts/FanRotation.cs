using UnityEngine;

public class FanRotation : MonoBehaviour
{
    [SerializeField] public float RotationSpeed = 300f;
    [SerializeField] private bool isOn = true;

    void Update()
    {
        if (!isOn) return;

        transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime, Space.Self);
    }
}
