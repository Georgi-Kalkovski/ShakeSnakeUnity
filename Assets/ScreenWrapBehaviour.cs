using UnityEngine;
using System.Collections;
using System;

public class ScreenWrapBehaviour : MonoBehaviour
{
	public bool advancedWrapping = true;

	Renderer[] renderers;

	bool isWrappingX = false;
	bool isWrappingY = false;

	Transform[] ghosts = new Transform[8];

	float screenWidth;
	float screenHeight;

	void Start()
	{
		renderers = GetComponentsInChildren<Renderer>();

		var cam = Camera.main;

		var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
		var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

		screenWidth = screenTopRight.x - screenBottomLeft.x;
		screenHeight = screenTopRight.y - screenBottomLeft.y;

		if (advancedWrapping)
		{
			CreateGhostSnakes();
		}
	}
	void Update()
	{
		if (advancedWrapping)
		{
			AdvancedScreenWrap();
		}
		else
		{
			ScreenWrap();
		}
	}

	void ScreenWrap()
	{
		foreach (var renderer in renderers)
		{
			if (renderer.isVisible)
			{
				isWrappingX = false;
				isWrappingY = false;
				return;
			}
		}
		if (isWrappingX && isWrappingY)
		{
			return;
		}

		var cam = Camera.main;
		var newPosition = transform.position;

		var viewportPosition = cam.WorldToViewportPoint(transform.position);

		if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
		{
			newPosition.x = -newPosition.x;

			isWrappingX = true;
		}
		if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
		{
			newPosition.y = -newPosition.y;

			isWrappingY = true;
		}
		transform.position = newPosition;
	}

	void AdvancedScreenWrap()
	{
		var isVisible = false;
		foreach (var renderer in renderers)
		{
			if (renderer.isVisible)
			{
				isVisible = true;
				break;
			}
		}

		if (!isVisible)
		{
			SwapSnakes();
		}
	}

	void CreateGhostSnakes()
	{
		for (int i = 0; i < 8; i++)
		{
			ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;

			DestroyImmediate(ghosts[i].GetComponent<ScreenWrapBehaviour>());
		}

		PositionGhostSnakes();
	}

	void PositionGhostSnakes()
	{
		var ghostPosition = transform.position;

		ghostPosition.x = transform.position.x + screenWidth;
		ghostPosition.y = transform.position.y;
		ghosts[0].position = ghostPosition;

		// Bottom-right
		ghostPosition.x = transform.position.x + screenWidth;
		ghostPosition.y = transform.position.y - screenHeight;
		ghosts[1].position = ghostPosition;

		// Bottom
		ghostPosition.x = transform.position.x;
		ghostPosition.y = transform.position.y - screenHeight;
		ghosts[2].position = ghostPosition;

		// Bottom-left
		ghostPosition.x = transform.position.x - screenWidth;
		ghostPosition.y = transform.position.y - screenHeight;
		ghosts[3].position = ghostPosition;

		// Left
		ghostPosition.x = transform.position.x - screenWidth;
		ghostPosition.y = transform.position.y;
		ghosts[4].position = ghostPosition;

		// Top-left
		ghostPosition.x = transform.position.x - screenWidth;
		ghostPosition.y = transform.position.y + screenHeight;
		ghosts[5].position = ghostPosition;

		// Top
		ghostPosition.x = transform.position.x;
		ghostPosition.y = transform.position.y + screenHeight;
		ghosts[6].position = ghostPosition;

		// Top-right
		ghostPosition.x = transform.position.x + screenWidth;
		ghostPosition.y = transform.position.y + screenHeight;
		ghosts[7].position = ghostPosition;

		//Debug.Log(String.Format("\n Width: " + ghostPosition.x.ToString() + " Height: " + ghostPosition.y.ToString()));

		for (int i = 0; i < 8; i++)
		{
			ghosts[i].rotation = transform.rotation;
		}
	}

	void SwapSnakes()
	{
		foreach (var ghost in ghosts)
		{
			if (ghost.position.x < screenWidth && ghost.position.x > -screenWidth &&
				ghost.position.y < screenHeight && ghost.position.y > -screenHeight)
			{
				transform.position = ghost.position;
				
				break;
			}
		}

		PositionGhostSnakes();
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(20, 20, 160, 48), "Simple Wrapping"))
		{
			SwitchToSimpleWrapping();
		}

		if (GUI.Button(new Rect(200, 20, 160, 48), "Advanced Wrapping"))
		{
			SwitchToAdvancedWrapping();
		}
	}

	void SwitchToSimpleWrapping()
	{
		advancedWrapping = false;

		foreach (var ghost in ghosts)
		{
			Destroy(ghost.gameObject);
		}
	}

	void SwitchToAdvancedWrapping()
	{
		advancedWrapping = true;

		CreateGhostSnakes();
	}
}