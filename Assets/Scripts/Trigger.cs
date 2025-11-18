using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
    [Header("Actions On Trigger Enter")]
    [SerializeField]
    private UnityEvent _onTriggerEnter;

    bool _isActivated = false;
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_isActivated) return;

        _isActivated = true;
        _onTriggerEnter.Invoke();
    }

}
