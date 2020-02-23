using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class ShrineScript : MonoBehaviour
{
    public string[] monologue;
    public TextMeshPro text;
    public Transform shrineCamPos;
    public float camMoveTime;
    private Camera mainCam;
    private Vector3 initialCamPos;
    private Quaternion initialCamRot;
    private PlayerMover playerMover;
    public int lettersScrolledPerSecond = 5;
    private CinemachineBrain cinemachineBrain;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    public Animator canvasAnimator;
    public Animator shrineAnimator;

	bool seen = false;

    private void Awake()
    {
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        mainCam = Camera.main;
        text.text = "";
    }

	private void Start()
	{
		playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
	}

	public void getTriggered()
    {
		if (!seen)
		{
			StartCoroutine(shrineEvent());
		}

		RechargeSpores(true);
    }

	public void Leave()
	{
		RechargeSpores(false);
	}


	private IEnumerator shrineEvent()
    {
		seen = true;
		yield return StartCoroutine(moveCamera());
        playAnimationsForShrineStuff();
        yield return StartCoroutine(playText());
        endAnimationsForShrineStuff();
        yield return StartCoroutine(moveCameraBack());
        yield return null;
    }

    private void endAnimationsForShrineStuff()
    {
        shrineAnimator.SetBool("Cinematico", false);
        canvasAnimator.SetBool("CinematicoUI", false);
    }
    private void playAnimationsForShrineStuff()
    {
        shrineAnimator.SetBool("Cinematico", true);
        canvasAnimator.SetBool("CinematicoUI", true);
    }
    private IEnumerator moveCamera()
    {
		var preventables = playerMover.GetComponents<IPreventable>();
		foreach (var preventable in preventables)
		{
			preventable.StartPrevent();
		}

		if (cinemachineVirtualCamera != null)
        {
            cinemachineVirtualCamera.enabled = false;
            cinemachineBrain.enabled = false;
        }
        float timePassed = 0f;
        initialCamPos = mainCam.gameObject.transform.position;
        initialCamRot = mainCam.gameObject.transform.rotation;
        while (timePassed < camMoveTime)
        {
            mainCam.gameObject.transform.position = Vector3.Lerp(initialCamPos, shrineCamPos.position, timePassed / camMoveTime);
            mainCam.gameObject.transform.rotation = Quaternion.Lerp(initialCamRot, shrineCamPos.rotation, timePassed / camMoveTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
        mainCam.gameObject.transform.position = shrineCamPos.position;
        mainCam.gameObject.transform.rotation = shrineCamPos.rotation;
        yield return null;
    }

    private IEnumerator moveCameraBack()
    {
        float timePassed = 0f;
        while (timePassed < camMoveTime)
        {
            mainCam.gameObject.transform.position = Vector3.Lerp(shrineCamPos.position, initialCamPos, timePassed / camMoveTime);
            mainCam.gameObject.transform.rotation = Quaternion.Lerp(shrineCamPos.rotation, initialCamRot, timePassed / camMoveTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
        mainCam.gameObject.transform.position = initialCamPos;
        mainCam.gameObject.transform.rotation = initialCamRot;
		var preventables = playerMover.GetComponents<IPreventable>();
		foreach (var preventable in preventables)
		{
			preventable.StopPrevent();
		}
		if (cinemachineVirtualCamera != null)
        {
            cinemachineBrain.enabled = true;
            cinemachineVirtualCamera.enabled = true;
        }
        yield return null;
    }

    private IEnumerator playText()
    {
        text.enabled = true;
        text.text = "";
        int monologueIndex = 0;
        while(monologueIndex < monologue.Length){
            string currentBlurb = monologue[monologueIndex];
            int stringIndex = 0;
            float timePassed = 0;
            float timePerLetter = 1 / (float)lettersScrolledPerSecond;
            while (stringIndex < currentBlurb.Length)
            {
                timePassed += Time.deltaTime;
                int lettersPassed = Mathf.FloorToInt(timePassed / timePerLetter);
                if(lettersPassed > 0)
                {
                    timePassed -= lettersPassed * timePerLetter;
                    stringIndex += lettersPassed;
                    text.text = currentBlurb.Substring(0, stringIndex);
                }
                yield return null;
            }
            yield return StartCoroutine(waitForInput());
            monologueIndex++;
        }
        yield return null;

    }

    private IEnumerator waitForInput()
    {
        /*bool inputGiven = false;
        while (!inputGiven)
        {
            inputGiven = Input.anyKey;
            yield return null;
        }*/
        yield return new WaitForSeconds(1f);
    }

	private void RechargeSpores(bool recharging)
	{
		if (recharging)
		{
			playerMover.GetComponentInChildren<WaitForSpores>().Event_DipAndWait(transform);
		}
		else
		{
			playerMover.GetComponentInChildren<WaitForSpores>().EndWait();

		}
	}
}
