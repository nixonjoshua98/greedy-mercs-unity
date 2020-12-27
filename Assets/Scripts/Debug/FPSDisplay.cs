﻿using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 100);

		style.alignment = TextAnchor.UpperLeft;
		style.normal.textColor = Color.white;
		style.fontSize = h * 2 / 100;

		float fps = 1.0f / deltaTime;
		float ms = deltaTime * 1000.0f;

		string text = string.Format("{0:0.0} ms ({1:0.} fps)", ms, fps);

		GUI.Label(rect, text, style);
	}
}