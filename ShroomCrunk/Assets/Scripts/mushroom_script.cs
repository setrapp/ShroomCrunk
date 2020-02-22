using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushroom_script : MonoBehaviour
{
    private float growTime = 2f;
    public void growUp()
    {
        float growStartScale = .001f;
        transform.localScale = new Vector3(.001f, .001f, .001f);
        StartCoroutine(growroutine(growStartScale));
    }

    private IEnumerator growroutine(float startScale)
    {
        float scale = startScale;
        float timePassed = 0f;
        float finalScale = Random.Range(.5f, 2f);
        while (scale < finalScale - .01f)
        {
            timePassed += Time.deltaTime;
            scale = Mathf.Lerp(startScale, 1.0f, timePassed);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        this.enabled = false;
        yield return null;
    }
}
