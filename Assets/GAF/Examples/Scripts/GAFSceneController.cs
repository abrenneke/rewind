using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GAFSceneController : MonoBehaviour 
{
	public void RunDemoScene(string _SceneName)
	{
		SceneManager.LoadScene(_SceneName);
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (SceneManager.GetActiveScene().name != "Main")
				SceneManager.LoadScene("Main");
		}
	}
}
