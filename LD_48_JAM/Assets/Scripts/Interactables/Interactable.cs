using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    public SpriteRenderer hoverOutline, objectSprite;
    public UnityEvent OnInteract;
    Animator animator;
    public float MouseInteractRadius, outlineScale = 1.2f;
    public GameObject electricityLines;
    public bool PlayerInRange = false;
    public bool ClosestToMouse = false;

    void Awake()
    {
        if (OnInteract == null)
            OnInteract = new UnityEvent();
        animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            objectSprite = animator.GetComponent<SpriteRenderer>();
        }

        if(objectSprite == null)
        {
            objectSprite = GetComponent<SpriteRenderer>();
        }

        hoverOutline.sprite = objectSprite.sprite;
    }

    private void Update()
    {
        if (PlayerInRange)
        {
            if (electricityLines != null)
            {
                electricityLines.SetActive(true);
            }
            objectSprite.color = new Color(0.807f, 0.537f, 0.184f);
        }
        else
        {
            if (electricityLines != null)
            {
                electricityLines.SetActive(false);
            }
            objectSprite.color = Color.white;
        }
        if (IsHovered() && PlayerInRange)
        {
            if (ClosestToMouse)
            {
                if (!hoverOutline.gameObject.activeInHierarchy)
                {
                    hoverOutline.gameObject.SetActive(true);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Interact();
                }
            }
        }
        else if (hoverOutline.gameObject.activeInHierarchy)
        {
            hoverOutline.gameObject.SetActive(false);
        }

        if (objectSprite != null)
        {
            hoverOutline.sprite = objectSprite.sprite;
            hoverOutline.flipX = objectSprite.flipX;

        }
    }

    void OnValidate()
    {
        if(hoverOutline != null)
        {
            hoverOutline.color = new Color(0.807f,0.537f,0.184f);
            hoverOutline.transform.localScale = new Vector3(outlineScale, outlineScale, 1);
            hoverOutline.maskInteraction = SpriteMaskInteraction.None;
        }
        else
        {
            Debug.LogError("Interactable object " + name + " does not have a hover outline object attached.");
        }

        gameObject.tag = "Interactable";

        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                return;
            }
        }
        Debug.LogError("Interactable object " + name + " does not have a trigger collider attached.");
    }

    public bool IsHovered()
    {
        return MouseDistance < MouseInteractRadius;
    }

    public float MouseDistance {
        get {
            return Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        }
    }

    public void Interact()
    {
        OnInteract.Invoke();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}