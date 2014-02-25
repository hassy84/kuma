using UnityEngine;
using System.Collections;

public class ParticleGather : MonoBehaviour {

	public ParticleSystem particleSystem;
	public Transform target;
	public float gatherSpeed = 1.0f;
	public float delay = 0.5f;

	private float startTime = 0f;

	// Use this for initialization
	void Start () {
		if (particleSystem == null) {
			particleSystem = gameObject.GetComponent<ParticleSystem>() as ParticleSystem;
		}
	}

	void OnEnable () {
		startTime = Time.time + delay;
	}

	// Update is called once per frame
	void Update () {
		if (startTime > Time.time) {
			return;
		}

		// extract the particles
		ParticleSystem.Particle[] theParticles = new ParticleSystem.Particle[particleSystem.particleCount];
		particleSystem.GetParticles(theParticles);

		for (var i=0; i<theParticles.Length; i++) {
			theParticles[i].position *= 0.999f;
//            theParticles[i].position = theParticles[i].position.normalized
//                                       * Mathf.Max(1f, theParticles[i].position.magnitude);
		}

		// copy them back to the particle system
		particleSystem.SetParticles(theParticles, particleSystem.particleCount);
	}
}
