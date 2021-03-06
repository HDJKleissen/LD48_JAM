using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public Vector3 Velocity;
    public PlayerDisguise Disguise = PlayerDisguise.None;
    public GameObject ClothesChangeCloudPrefab;

    public PlayerAnimationController animationController;
    public CircleCollider2D FeetCollider;
    public Animator ClothesChangeCloudAnimator;
    public PlayerCamera playerCamera;

    public Area CurrentArea = null;

    public List<Interactable> interactablesInRange = new List<Interactable>();

    GameObject spawnedCloud;

    public bool HasRecordsRoomKey = false;
    public bool playerFrozen = false;
    public bool Disguising {
        get {
            return spawnedCloud != null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        Interactable closestToMouse = null;
        float closestDistanceToMouse = float.PositiveInfinity;

        foreach (Interactable interactable in interactablesInRange)
        {
            if (!interactable.PlayerInRange)
            {
                interactable.PlayerInRange = true;
            }
            interactable.ClosestToMouse = false;

            if (interactable.IsHovered())
            {
                if (interactable.MouseDistance < closestDistanceToMouse)
                {
                    closestDistanceToMouse = interactable.MouseDistance;
                    closestToMouse = interactable;
                }
            }
        }

        if (closestToMouse != null)
        {
            closestToMouse.ClosestToMouse = true;
        }

        //if (Input.GetButtonDown("Interact"))
        //{
        //    if (interactablesInRange.Count > 0)
        //    {
        //        Interactable interactable = interactablesInRange[0];

        //        interactable.Interact();
        //    }
        //}

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Disguising || playerFrozen)
        {
            animationController.UpdateAnimator(Vector3.zero);
            return;
        }
        Velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * MoveSpeed * Time.fixedDeltaTime;

        animationController.UpdateAnimator(Velocity);

        transform.position += Velocity;
        if (CurrentArea != null)
        {
            if (CurrentArea?.FloorHeight != (int)transform.position.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, CurrentArea.FloorHeight);
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interactable" && collision.isTrigger)
        {
            Interactable other = collision.GetComponent<Interactable>();
            if (other != null)
            {
                interactablesInRange.Insert(0, collision.GetComponent<Interactable>());
            }
            else
            {
                Debug.LogWarning("Object " + collision.name + " has the interactable tag but no Interactable component! Please fix!");
            }
        }
        else if(collision.tag == "CameraBounds")
        {
            playerCamera.exactlyFollowing = false;
            playerCamera.lerpFollowing = false;
            playerCamera.startedRefollowing= true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable" && collision.isTrigger)
        {
            Interactable other = collision.GetComponent<Interactable>();
            if (other != null)
            {
                if (interactablesInRange.Contains(other))
                {
                    other.PlayerInRange = false;
                    interactablesInRange.Remove(collision.GetComponent<Interactable>());
                }
            }
            else
            {
                Debug.LogWarning("Object " + collision.name + " has the interactable tag but no Interactable component! Please fix!");
            }
        }
        else if (collision.tag == "CameraBounds")
        {
            playerCamera.startedRefollowing = false;
        }
    }

    public void ChangeDisguise(PlayerDisguise disguise)
    {
        spawnedCloud = Instantiate(ClothesChangeCloudPrefab, transform);
        Disguise = disguise;
        animationController.ChangeDisguise(disguise);
        //needs wrapping in an if statement to check that the new disguise is not the same as the previous one
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/ClothesSwap", gameObject);
    }
}

public enum PlayerDisguise
{
    None,
    Cactus,
    Man,
    ManBald,
    ManTie,
    Woman,
    WomanBlack,
    WomanDress,
    Security
}