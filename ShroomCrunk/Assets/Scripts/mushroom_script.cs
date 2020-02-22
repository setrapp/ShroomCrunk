using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushroom_script : MonoBehaviour
{
    public float growTime = 2f;
    public float lifeTime = 30f;
    public void growUp()
    {
        float growStartScale = .001f;
        transform.localScale = new Vector3(.001f, .001f, .001f);
        StartCoroutine(growroutine(growStartScale));
        StartCoroutine(die());
    }

    private IEnumerator die()
    {
        lifeTime = lifeTime + Random.Range(lifeTime - 3f, lifeTime + 3f);
        Debug.Log("dying");
        float timeAlive = 0f;
        while(timeAlive < lifeTime)
        {
            timeAlive += Time.deltaTime;
            yield return null;
        }
        float timeDying = 0f;
        float initialScale = transform.localScale.x;
        float scale = initialScale;
        while (timeDying < growTime)
        {
            timeDying += Time.deltaTime;
            scale = Mathf.Lerp(initialScale, Mathf.Epsilon, timeDying/growTime);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        Destroy(this.gameObject);
        yield return null;

    }
    private IEnumerator growroutine(float startScale)
    {
        Debug.Log("growing");
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
    }
}
