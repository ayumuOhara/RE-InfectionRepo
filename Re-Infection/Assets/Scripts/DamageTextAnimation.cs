using UnityEngine;

public class DamageTextAnimation : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos = new Vector3(0, 70.0f, 0);
    float animDuration = 0.5f;
    float startTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.localPosition;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var journeyFraction = (Time.time - startTime) / animDuration;
        transform.localPosition = Vector3.Lerp(startPos, startPos + endPos, journeyFraction);

        if(journeyFraction > 1)
            Destroy(gameObject);
    }
}
