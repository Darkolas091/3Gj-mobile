using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionToUse;
    [SerializeField] private float speed;

    


    void Start()
    {
        
    }

   
    void Update()
    {
        Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveDirection.x, 0f, moveDirection.y);
        move = transform.TransformDirection(move);
        transform.Translate(move * speed * Time.deltaTime);
    }
}
