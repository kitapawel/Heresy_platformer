using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    Rigidbody2D myRigidBody2D;
    const int ACTOR_LAYER = 22;
    const int ACTORNONCOLLIDABLE_LAYER = 23;

    [SerializeField]
    ScriptableObject scriptableObject;
    [SerializeField] 
    private float rotateSpeed = 20f;
    bool isMousePressed = false;
    bool isPlayerInRange = false;

    //TODO only if in range

    private void OnMouseDown()
    {
        if (isPlayerInRange)
        {
            isMousePressed = true;
            myRigidBody2D.isKinematic = true;
        }
    }
    private void OnMouseUp()
    {
        if (isPlayerInRange)
        {
            isMousePressed = false;
            myRigidBody2D.isKinematic = false;
        }
    }
    private void OnMouseOver()
    {
        if (isPlayerInRange)
        {            
            PickObject();
        }
    }


    private void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        var boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        boxCollider2D.size = new Vector2(2f, 2f);
        boxCollider2D.offset = new Vector2(0, 0);
    }
    void Update()
    {
        CarryItem();
    }

    private void CarryItem()
    {
        if (isMousePressed && isPlayerInRange /*&& myRigidBody2D.velocity.x == 0 */&& myRigidBody2D.velocity.y == 0)
        {
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPosition;
            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, rotateSpeed * Input.GetAxis("Mouse ScrollWheel")));
            //TODO disable collision with player          
        }
        else
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    void PickObject()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            if (scriptableObject)
            {
                if (FindObjectOfType<PlayerInput>().GetComponent<InventorySystem>().isInventoryFull() == false)
                {
                    //the whole EquipItem operation needs to be completed, otherwise Destroy() operation might
                    //get omitted, which will lead to duplication
                    FindObjectOfType<PlayerInput>().GetComponent<InventorySystem>().EquipItem(scriptableObject);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventory is full. Try to free up some space.");
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ControlInput>())
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ControlInput>())
        {
            isPlayerInRange = false;
        }
    }
}
