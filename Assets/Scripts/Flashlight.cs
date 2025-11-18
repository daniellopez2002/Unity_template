using UnityEngine;

public class FlashLight: Collectable
{

    [Header("FlashLight settings")]
    [SerializeField]
    public bool IsOn = false;
    [SerializeField]
    private GameObject _light;

    private void Start()
    {
        SwitchLight();
    }
    public void SwitchLight()
    {
        IsOn = !IsOn;
        _light.SetActive(IsOn);
    }
}