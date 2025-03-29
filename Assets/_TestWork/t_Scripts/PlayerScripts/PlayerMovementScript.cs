using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;
    public GameObject inventoryPanel; // Assign in Inspector
    public Camera playerCamera; // Assign your FirstPerson camera
    public float mouseSensitivity = 100f;

    // Inventory integration
    public Inventory inventory; // Reference to Inventory component
    public InventoryUI inventoryUI; // Reference to InventoryUI component
    public float interactionRange = 2f; // Distance to pick up items

    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inventory = GetComponent<Inventory>(); // Assumes Inventory is on player
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ensure inventory and UI are linked
        if (inventoryUI != null && inventory != null)
            inventory.inventoryUI = inventoryUI; // Link Inventory to UI
    }

    void Update()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Horizontal movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f);
        controller.Move(move * speed * Time.deltaTime);

        // Jump (optional)
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Inventory toggle
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            Cursor.lockState = inventoryPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = inventoryPanel.activeSelf;
        }

        // Camera rotation (only if inventory is closed)
        if (!inventoryPanel.activeSelf)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

            // Check for interactable objects (e.g., ingredients)
            if (Input.GetKeyDown(KeyCode.E)) // Press E to interact
            {
                TryInteract();
            }
        }
    }

    void TryInteract()
    {
        // Raycast from camera to detect collectibles
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            CollectibleIngredient collectible = hit.collider.GetComponent<CollectibleIngredient>();
            if (collectible != null)
            {
                Item newItem = new Item(collectible.ingredientName, collectible.icon, collectible.type, collectible.quantity);
                if (inventory.AddItem(newItem))
                {
                    Destroy(collectible.gameObject); // Remove from scene
                    Debug.Log($"Collected {collectible.ingredientName}!");
                }
            }
        }
    }
}