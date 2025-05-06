using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTrigger : MonoBehaviour
{

	public ParticleSystem stepR;
	public ParticleSystem stepL;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void StepRight()
    {
        stepR.Play();
    }

	void StepLeft(){
stepL.Play();
	}
}
