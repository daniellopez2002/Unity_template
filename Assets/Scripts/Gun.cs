using UnityEngine;

public class Gun : Collectable
{
    [Header("Gun settings")]
    [SerializeField] public int CurrentAmmo = 0;
    [SerializeField] private int _maxAmmo = 8;

    public void Shoot()
    {
        if (CurrentAmmo == 0) return;

    }
}
