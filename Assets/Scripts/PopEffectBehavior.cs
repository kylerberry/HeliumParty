using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopEffectBehavior : MonoBehaviour
{
    ParticleSystem bang;
    ParticleSystem bang2;
    ParticleSystem bang3;
    ParticleSystem cloud;
    ParticleSystem particles;
    ParticleSystem popText;

    void Start()
    {
        bang = transform.Find("PopBang").GetComponent<ParticleSystem>();
        popText = bang.transform.Find("PopText").GetComponent<ParticleSystem>();
        bang2 = bang.transform.Find("PopBang2").GetComponent<ParticleSystem>();
        bang3 = bang.transform.Find("PopBang3").GetComponent<ParticleSystem>();
        cloud = bang.transform.Find("PopCloud").GetComponent<ParticleSystem>();
        particles = bang.transform.Find("PopParticles").GetComponent<ParticleSystem>();
    }

    public bool IsAlive()
    {
        return bang.IsAlive() &&
            bang2.IsAlive() &&
            bang3.IsAlive() &&
            cloud.IsAlive() &&
            particles.IsAlive() &&
            popText.IsAlive();
    }

    public void Explode(float radius)
    {
        // @todo implement pop size based on radius
        bang.Play();
        bang2.Play();
        bang3.Play();
        cloud.Play();
        particles.Play();
        popText.Play();
    }
}
