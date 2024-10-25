using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour, IInteractable
{
    public Transform launchPosition;
    private float launchTimer = 1f;
    [SerializeField]
    private float launchPower = 1000f;

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
        yield return new WaitForSeconds(launchTimer);

        CharacterManager.Instance.Player.controller.canMove = true;
        CharacterManager.Instance.Player.controller.LaunchPlayer(launchPower, launchPosition.up);
    }
}
