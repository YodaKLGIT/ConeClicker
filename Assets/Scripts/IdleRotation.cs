using UnityEngine;
using DG.Tweening;

public class idleRotation : MonoBehaviour
{
    private Tween _idleRotation;

    private void Start()
    {
        // Start idle rotation animation when the game starts
        _idleRotation = transform.DORotate(new Vector3(0, 360, 0), 5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1); // Infinite looping
    }

    public void PauseIdle()
    {
        _idleRotation.Pause(); // Pause idle rotation
    }

    public void ResumeIdle()
    {
        _idleRotation.Play(); // Resume idle rotation
    }
}
