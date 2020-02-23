using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
	public void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			EndTheGame();
		}
	}

	[SerializeField] string gotoScene;
	public void EndTheGame()
	{
		SceneManager.LoadScene(gotoScene);
	}
}
