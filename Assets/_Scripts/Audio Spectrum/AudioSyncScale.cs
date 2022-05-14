using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncer
{
    [Header("Scale values")]
    public Vector3 beatScale;
    public Vector3 restScale;

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (IsBeat) return;

        // Unscale object when not in beat
        transform.localScale = Vector3.Lerp(transform.localScale, restScale, RestTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("Scale");
        StartCoroutine("Scale", beatScale);

    }

    private IEnumerator Scale(Vector3 _target)
    {
        Vector3 curr = transform.localScale;
        Vector3 initScale = curr;

        float timer = 0;

        // While object is not at target -> scale
        while (curr != _target)
        {
            // Lerp scale
            curr = Vector3.Lerp(initScale, _target, timer / TimeToBeat);

            // Update timer
            timer += Time.deltaTime;

            // Update scale
            transform.localScale = curr;

            yield return null;
        }

        // Stop beat
        IsBeat = false;
    }

}
