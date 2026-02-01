using UnityEngine;
using System.Collections;

public class LoopingTimedSpikes : MonoBehaviour
{
    [Header("Distinct Spike")]
    [SerializeField] private float _periodDelay = 0f;

    [Header("Shared Properties")]
    [SerializeField] private Transform _spikes;
    [SerializeField] private float _periodDuration = 5f;
    [SerializeField] private float _deployDuration = 0.2f;
    [SerializeField] private float _presentedDuration = 1f;
    [SerializeField] private float _retractDuration = 0.75f;
    [SerializeField] private float _retractedHeight = -1.25f;
    [SerializeField] private float _deployedHeight = 1f;

    private Coroutine _loopRoutine;

    [SerializeField]
    private ParticleSystem particleSystem;

    private void Start()
    {
        ResetTrap();
        StartCoroutine(SetInitialDelay());
    }

    private IEnumerator SetInitialDelay()
    {
        yield return new WaitForSeconds(_periodDelay + _periodDuration);
        _loopRoutine = StartCoroutine(TrapLoop());
    }

    private IEnumerator TrapLoop()
    {
        //yield return new WaitForSeconds(_periodDuration);

        while (true)
        {
            yield return Deploy();
            yield return new WaitForSeconds(_presentedDuration);
            yield return Retract();
            yield return new WaitForSeconds(_periodDuration);
        }
    }

    private IEnumerator Deploy()
    {
        yield return MoveSpikes(
            _retractedHeight,
            _deployedHeight,
            _deployDuration);
        
        particleSystem.Emit(Random.Range(10,30));
    }

    private IEnumerator Retract()
    {
        yield return MoveSpikes(
            _deployedHeight,
            _retractedHeight,
            _retractDuration);
    }

    private IEnumerator MoveSpikes(float fromY, float toY, float duration)
    {
        float t = 0f;
        Vector3 pos = _spikes.position;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            pos.y = Mathf.Lerp(fromY, toY, t);
            _spikes.position = pos;
            yield return null;
        }

        pos.y = toY;
        _spikes.position = pos;
    }

    private void ResetTrap()
    {
        Vector3 pos = _spikes.position;
        pos.y = _retractedHeight;
        _spikes.position = pos;
    }
}
