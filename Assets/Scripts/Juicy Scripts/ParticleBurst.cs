using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBurst : MonoBehaviour
{

    ParticleSystem myParticles;

    void Start(){
        //getting a reference to the particle system
        //and turning it off
        myParticles = GetComponent<ParticleSystem>();
        var emission = myParticles.emission;
        emission.enabled = false;
    }

    public void Burst(){
        var emission = myParticles.emission;
        emission.enabled = true;
        myParticles.Play();
    }

  

}
