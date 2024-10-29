using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour, IInteractable
{
    public Transform launchPosition;
    private float launchTimer = 1f;
    [SerializeField]
    private float launchPower = 1000f;
    private AudioSource audioSource;

    public GameObject launchEffect;
    public Transform launchPos;

    private Collider thisCollider;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        thisCollider = GetComponent<Collider>();
    }

    public string GetInteractPrompt()
    {
        string str = "[E]\n¹ß»ç";
        return str;
    }

    public void OnInteracte()
    {
        CharacterManager.Instance.Player.controller.StopMove();
        CharacterManager.Instance.Player.transform.position = launchPosition.position;

        StartCoroutine(Launch());
    }

    private IEnumerator Launch()
    {
        thisCollider.enabled = false;
        yield return new WaitForSeconds(launchTimer);
        audioSource.PlayOneShot(audioSource.clip);
        GameObject effect = Instantiate(launchEffect, launchPos.position, Quaternion.identity);
        Destroy(effect, 2f);

        CharacterManager.Instance.Player.controller.canMove = true;
        CharacterManager.Instance.Player.controller.LaunchPlayer(launchPower, launchPosition.up);

        yield return new WaitForSeconds(launchTimer);
        thisCollider.enabled = true;
    }
}
