using UnityEngine;
using System.Collections;

public class Mic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var Source = GetComponent<AudioSource>();
		Source.clip = Microphone.Start("Rift Audio", true, 1, 48000);
		Source.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
