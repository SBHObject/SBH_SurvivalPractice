using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private AudioSource audioSource;

    public Transform laserPoint;
    public ParticleSystem shotParticle;
    private float SearchRange = 6f;

    [Header("Attack")]
    [SerializeField]
    private int turretDamage;
    [SerializeField]
    private float turretShotRate;
    public LayerMask targetLayer;
    private float lastTimeShot = 0;

    
    private float searchRate = 0.1f;
    private float lastTimeSearch = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(lastTimeSearch + searchRate < Time.time)
        {
            SearchByLaser();
            lastTimeSearch = Time.time;
        }
    }

    private void SearchByLaser()
    {
        Ray ray = new Ray(laserPoint.position, laserPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, SearchRange, targetLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageable character))
            {
                Attack(character);
            }
        }
    }

    private void Attack(IDamageable target)
    {
        if (lastTimeShot + turretShotRate < Time.time)
        {
            audioSource.PlayOneShot(audioSource.clip);
            shotParticle.Play();
            target.TakeDamage(turretDamage);
            lastTimeShot = Time.time;
        }
    }
}
