using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inspection: MonoBehaviour
{
    [Header("HUD references")]
    [SerializeField] private TMP_Text _infotext;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _rejectButton;

    [Header("SFX")]
    [SerializeField] private AudioClip _sfxButtonClick;

    private void Start()
    {
        _acceptButton.onClick.AddListener(() => Accept());
        _rejectButton.onClick.AddListener(() => Reject());
    }

    public void SetText(string text)
    {
        _infotext.text = text;
    }

    private void Reject()
    {
        InspectionManager.Instance.Reject();
        AudioManager.Instance.PlaySFX(_sfxButtonClick);
    }

    private void Accept()
    {
        InspectionManager.Instance.ConfirmPickup();
        AudioManager.Instance.PlaySFX(_sfxButtonClick);
    }
}