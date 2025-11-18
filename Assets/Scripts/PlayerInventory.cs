using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerInventory : MonoBehaviour
{
    public List<Collectable> Objects;

    [Header("Inventory settings")]
    [SerializeField] private int _inventorySize = 6;
    [SerializeField] private GameObject _inventoryContainer;

    public Collectable CurrentItem = null;
    private int _currentIndex = 0;

    private void Start()
    {
        ClampInventorySize();
        SetItem();
    }

    private void ClampInventorySize()
    {
        if (Objects.Count > _inventorySize)
            Objects = Objects.GetRange(0, _inventorySize);
    }

    private void SetItem()
    {
        if (Objects.Count == 0)
        {
            CurrentItem = null;
            _currentIndex = 0;
            Debug.Log("[PlayerInventory] Inventory empty");
            return;
        }
        _currentIndex = Mathf.Clamp(_currentIndex, 0, Objects.Count - 1);

        CurrentItem = Objects[_currentIndex];
        CurrentItem.gameObject.SetActive(true);

        foreach(Collectable c in Objects)
        {
            if (c == CurrentItem) continue;
            c.gameObject.SetActive(false);
        }

        Debug.Log($"[PlayerInventory] Selected item: {CurrentItem?.name ?? "None"} (index {_currentIndex})");
    }

    public void AddItem(Collectable item)
    {
        Objects.Add(item);

        item.EnableRigidBody(false);

        item.gameObject.SetActive(false);
        item.transform.SetParent(_inventoryContainer.transform);

        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        SetItem();
    }
    public void NextItem()
    {
        if (Objects.Count == 0) return;

        _currentIndex = (_currentIndex + 1) % Objects.Count;
        SetItem();
    }

    public void PrevItem()
    {
        if (Objects.Count == 0) return;

        _currentIndex--;
        if (_currentIndex < 0) _currentIndex = Objects.Count - 1;

        SetItem();
    }

    public void DropCurrentItem()
    {
        if (CurrentItem == null) return;

        CurrentItem.Drop(PlayerController.Instance.transform);

        Objects.Remove(CurrentItem);

        if (_currentIndex >= Objects.Count)
            _currentIndex = Objects.Count - 1;

        SetItem();
    }
}
