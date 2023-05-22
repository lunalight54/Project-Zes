using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ItemContainer inventory;
    private PlayerCamera playerCamera;

    public float interactionRaylenght = 5;
    public LayerMask groundMask;
    public bool fly = false;
    public Animator animator;
    bool isWaiting = false;
    public World world;
    public InventoryPanel inventoryPanel;


    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        world = FindObjectOfType<World>();
        inventoryPanel = FindObjectOfType<InventoryPanel>();
        inventoryPanel.gameObject.SetActive(false);
    }
    private void Start()
    {
        playerInput.OnMouseClick += HandleMouseClick;
        playerInput.OnFly += HandleFlyClick;
        playerInput.OnRightMouseClick += HandleRightMouseClick;
        playerInput.OnInventoryClick += HandleInventoryClick;
    }
    private void HandleFlyClick()
    {
        fly = !fly;
    }
    private void Update()
    {
        if (fly)
        {
            animator.SetFloat("speed", 0);
            animator.SetBool("isGrounded", false);
            animator.ResetTrigger("jump");
            playerMovement.Fly(playerInput.MovementInput, playerInput.IsJumping, playerInput.RunningPressed);
        }
        else
        {
            animator.SetBool("isGrounded", playerMovement.IsGrounded);
            if (playerMovement.IsGrounded && playerInput.IsJumping && isWaiting == false)
            {
                animator.SetTrigger("jump");
                isWaiting = true;
                StopAllCoroutines();
                StartCoroutine(ResetWaiting());
            }
            animator.SetFloat("speed", playerInput.MovementInput.magnitude);
            playerMovement.HandleGravity(playerInput.IsJumping);
            playerMovement.Walk(playerInput.MovementInput, playerInput.RunningPressed);
        }
    }
    IEnumerator ResetWaiting()
    {
        yield return new WaitForSeconds(0.1f);
        animator.ResetTrigger("jump");
        isWaiting = false;
    }
    private void HandleMouseClick()
    {
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionRaylenght, groundMask))
        {
            
            ChunkRenderer chunk = hit.collider.GetComponent<ChunkRenderer>();
            Vector3Int pos = world.GetBlockPos(hit, false);
            var blockToAdd = WorldDataHelper.GetBlock(chunk.ChunkData.worldReference, pos);
            //Debug.Log(block);
            ModifyTerrain(hit, BlockType.Air,false);
            //find object type and pass
            Item itemToAdd = BlockDataManager.blockDataInfoDictionary[blockToAdd];
                inventory.Add(itemToAdd, 1);
        }
    }
    private void HandleRightMouseClick()
    {
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionRaylenght, groundMask))
        {
            ModifyTerrain(hit, BlockType.Dirt, true);
            //inventoryPanel.
        }
        
    }

    private void ModifyTerrain(RaycastHit hit, BlockType blockType, bool toPlace)
    {
        world.SetBlock(hit, blockType, toPlace);
    }

    private void HandleInventoryClick()
    {

        inventoryPanel.gameObject.SetActive(!inventoryPanel.isActiveAndEnabled);
        if (inventoryPanel.isActiveAndEnabled)
        {
            playerCamera.blocked = true;
            playerMovement.blocked = true;
        }
        else
        {
            playerMovement.blocked = false;
            playerCamera.blocked = false;
        }
    }
}
