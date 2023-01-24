using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private RoomTrigger _roomTrigger;

    private Coroutine _corutine;
    private bool _isPlaying;
    private float _maxValue = 1f;
    private float _minValue = 0;
    private float changeRate = 0.3f;

    private void OnEnable()
    {
        _roomTrigger.Entered += OnEntered;
        _roomTrigger.Left += OnLeft;
    }

    private void OnDisable()
    {
        _roomTrigger.Entered -= OnEntered;
        _roomTrigger.Left -= OnLeft;
    }

    private void OnEntered()
    {

        if (_isPlaying == false)
        {
            _audioSource.Play();
            _isPlaying = true;
        }

        SetCurrentValue(_maxValue);
    }

    private void OnLeft()
    {
        SetCurrentValue(_minValue);
    }

    private void SetCurrentValue(float targetVolume)
    {
        if (_corutine != null)
        {
            StopCoroutine(_corutine);
        }
        _corutine = StartCoroutine(ChangeVolume(targetVolume));
    }

    private IEnumerator ChangeVolume(float targetVolume)
    {

        while (_audioSource.volume != targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, changeRate * Time.deltaTime);

            if (_audioSource.volume == _minValue)
            {
                _audioSource.Stop();
                _isPlaying = false;
            }

            yield return null;
        }
    }
}
