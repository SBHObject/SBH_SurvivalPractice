using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;
    public LineRenderer laser;

    public Transform laserPoint;
    public ParticleSystem shotParticle;
    private float searchRange = 20f;

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
        animator = GetComponent<Animator>();

        laser.SetPosition(1, new Vector3(0, 0, searchRange));
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

        if (Physics.Raycast(ray, out hit, searchRange, targetLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageable character))
            {
                float laserLength = Vector3.Distance(laserPoint.position, hit.point);
                LaserLengthChange(laserLength);
                Attack(character);
            }
        }
        else
        {
            LaserLengthChange(searchRange);
        }
    }

    private void Attack(IDamageable target)
    {
        if (lastTimeShot + turretShotRate < Time.time)
        {
            audioSource.PlayOneShot(audioSource.clip);
            animator.SetTrigger("Attack");
            shotParticle.Play();
            target.TakeDamage(turretDamage);
            lastTimeShot = Time.time;
        }
    }

    private void LaserLengthChange(float length)
    {
        laser.SetPosition(1, new Vector3(0, 0, length));
    }
}
