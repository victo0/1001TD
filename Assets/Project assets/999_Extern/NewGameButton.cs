using UnityEngine;
using System.Collections;

public class NewGameButton : MonoBehaviour
{
	public void ChangeToScene (int sceneToChangeTo) 
	{
		Application.LoadLevel (sceneToChangeTo);
	}
}