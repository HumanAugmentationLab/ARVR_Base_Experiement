using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Assets.LSL4Unity.Scripts;

public class SphereController : MonoBehaviour {

	public LSLIntMarkerStream markerStream;
	public GameObject TestObject;

	public float StateTime;
	public float BlinkTime;
	public float blinkChance;

	private float StateClock = 0;
	private float BlinkClock = 0;

	private int State = 0;


	// Use this for initialization
	void Start () {
		Assert.IsNotNull(markerStream, "You forgot to assign the reference to a marker stream implementation!");
	}
	
	// Update is called once per frame
	void Update () {
		StateClock += Time.deltaTime;

		if (State == 2) {
			BlinkClock += Time.deltaTime;
			if (BlinkClock > BlinkTime) {
				BlinkClock -= BlinkTime;
				TestObject.SetActive (!TestObject.activeInHierarchy);
			}
		}

		if (StateClock > StateTime) {
			StateClock -= StateTime;
			switch (State) {
			case 0: //Off
				State = Random.value < blinkChance ? 2 : 1;
				TestObject.SetActive (true);
				break;
			case 1: //On
				State = 0;
				TestObject.SetActive (false);
				break;
			case 2: //Blink
				State = 0;
				TestObject.SetActive (false);
				break;
			}
			markerStream.Write(State);
		}
	}
}
