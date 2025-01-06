using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sign : MonoBehaviour
{
	public List<string> lines;
	int index = -1;
	bool hovered = false;

	string lastline = "";

	TextMeshProUGUI text;
	RectTransform rectTransform;

	float lerper = 1;
	void Start() {
		text = GetComponentInChildren<TextMeshProUGUI>();
		rectTransform = GetComponentsInChildren<RectTransform>()[1];
	}
	private void Update() {
		lerper = Mathf.Lerp(lerper,hovered && index != -1 ? 1f : 0f,Time.deltaTime * 10f);
		lastline = index != -1 ? lines[index] : lastline;

		if(Input.GetMouseButtonDown(0) && hovered) {
			index += 1;

			if(index >= lines.Count) {
				index = -1;
			}
		}

		if(!hovered) index = -1;

		text.text = lastline;
		rectTransform.localScale = Vector3.one * 0.01f * lerper;
	}
	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.GetComponent<Grabber>() != null) {
			hovered = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if(collision.gameObject.GetComponent<Grabber>() != null) {
			hovered = false;
		}
	}

}
