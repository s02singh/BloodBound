using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class LightningController : MonoBehaviour
{

    [SerializeField] private AudioClip thunderSound;

    [SerializeField] private float radius;

    private ParticleSystem lightning;
    private AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        if (!lightning.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void Init(float start_x, float start_z, float height)
    {
        lightning = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = thunderSound;
        
        ChangeLocation(start_x, start_z);
        ChangeHeight(height);        
        Startlightning();
    }

    public void Startlightning()
    {
        lightning.Play();
        audioSource.Play();
    }

    public void Stoplightning()
    {
        lightning.Stop();
        audioSource.Stop();
    }

    public void ChangeLocation(float x, float z)
    {
        transform.position = new Vector3(x, transform.position.y, z);
    }

    public void ChangeHeight(float height)
    {
        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        var sh = lightning.shape;
        sh.scale = new Vector3(sh.scale.x, sh.scale.y, height * 0.033f);

        /**
         * The 0.033 factor was determined by tested at PositionY=1
         * and then changing the scaleZ while observing the cone's spread from the side view
         * And repeating the same for PositionY=2, PositionY=3, etc.
         * ScaleZ ~= 0.033 * PositionY  ,  where radius is measured in Unity units
         */
    }

    public void ChangeRadius(float radius)
    {
        var sh = lightning.shape;
        sh.angle = radius / 1.9f;

        /**
         * The 1.9 factor was determined by tested at Height=1, ScaleZ=0.033
         * and then changing the angle while observing the cone's spread from the top view
         * Angle ~= 1.9 * radius  ,  where radius is measured in Unity units
         */
    }
}
