using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Playables.FrameData;

public class FloatingText : MonoBehaviour
{
    private float scale = 0.5f;
    private float evaluationTime = 0f;
    [SerializeField] private float scaleRate = 0.5f;
    [SerializeField] private float moveRate = 0.5f;
    [SerializeField] private AnimationCurve curve;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        evaluationTime += Time.deltaTime * scaleRate;

        float curveValue = curve.Evaluate(evaluationTime);

        scale = curveValue;

        transform.localScale = Vector3.one * scale;
        transform.localPosition += Vector3.up * Time.deltaTime * moveRate;

        if (scale <= 0)
        {
            Destroy(gameObject);
        }

    }
}
