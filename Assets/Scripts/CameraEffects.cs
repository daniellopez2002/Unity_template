using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour
{
    [SerializeField]
    private Volume _volume;

    private Vignette _vignette;
    private MotionBlur _motionBlur;
    private ColorAdjustments _colorAdjustments;

    [Header("Effect bools")]
    public bool TiredEffect = false;
    public bool MotionEffect = false;
    public bool ScareEffect = false;

    [Header("Run Settings")]
    public float runBlur = 0.3f;
    public float runSaturationBoost = 10f;

    [Header("Tired Settings")]
    public float TiredDuration = 1.0f;
    public float TiredVignetteMin = 0.2f;
    public float TiredVignetteMax = 0.45f;
    public float TiredBreathSpeed = 3.5f;
    public float TiredDesaturation = -40f;

    private float tiredTimer = 0f;


    private void Awake()
    {
        _volume.profile.TryGet(out _vignette);
        _volume.profile.TryGet(out _motionBlur);
        _volume.profile.TryGet(out _colorAdjustments);
    }

    private void Update()
    {
        if (TiredEffect) HandleTiredEffect();
        if (MotionEffect) HandleMotionBlurEffect();
        if (ScareEffect) HandleScareEffect();
    }

    private void HandleMotionBlurEffect()
    {

    }

    private void HandleTiredEffect()
    {
        tiredTimer += Time.deltaTime;

        // Oscilación tipo respiración
        float t = (Mathf.Sin(Time.time * TiredBreathSpeed) + 1f) / 2f;
        float vignetteValue = Mathf.Lerp(TiredVignetteMin, TiredVignetteMax, t);
        _vignette.active = true;
        _vignette.intensity.Override(vignetteValue);
        //_colorAdjustments.saturation.Override(
        //    Mathf.Lerp(_colorAdjustments.saturation.value, TiredDesaturation, Time.deltaTime * 3f)
        //);

        if (tiredTimer >= TiredDuration)
            ResetTired();
    }

    public void TriggerTiredEffect()
    {
        tiredTimer = 0f;
        TiredEffect = true;
    }

    private void ResetTired()
    {
        TiredEffect = false;

        // Volver a valores normales
        _vignette.intensity.Override(0f);
        _colorAdjustments.saturation.Override(0f);
    }

    private void HandleScareEffect()
    {

    }
}
