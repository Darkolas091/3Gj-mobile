using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionToUse;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator animator; 

    void Start()
    {
        
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveDirection.x, 0f, moveDirection.y);
        move = transform.TransformDirection(move);

        transform.Translate(move * speed * Time.deltaTime, Space.World);

        float moveAmount = moveDirection.magnitude;
        animator.SetFloat("Speed", moveAmount);
    }
}