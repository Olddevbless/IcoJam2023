using CommonComponents;
using CommonComponents.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : Damagable,PlayerInput.IPlayerActions
{
    [SerializeField] float playerAcceleration;
    [SerializeField] float playerMaxSpeed;
    //[SerializeField] float playerJumpPower;
    [SerializeField] float playerDodgePower;
    [SerializeField] Vector2 mousePos;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Vector2 movement;


    [Header("UI")]
    [SerializeField] GameObject[] shieldPartsUI;
    [SerializeField] GameObject[] playerHPUI;
    //[SerializeField] GameObject torso;
    public Vector3 mouseToGroundPoint;
    public Vector2 currentMoveVector;
    Vector3 dir;
    /*[Header("Jump")]
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpBufferTime;
    [SerializeField] float coyoteTimeCounter;
    [SerializeField] float jumpBufferCounter;
    [SerializeField] bool isGrounded;*/
    Animator playerAnimator;
    [Header("Attacking")]
    PlayerAttack playerAttack;
    public int shieldSize;
    [SerializeField] GameObject[] shieldParts;
    public bool shieldIsRaised;
    Rigidbody playerRB;
    public bool isDead;


    #region InputSetup
    private PlayerInput _controls;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _dodgeAction;
    private InputAction _swapWeaponsAction;
    private InputAction _secondaryAction;
    private InputAction _specialAction;
    private InputAction _activateAction;
    private InputAction _pauseAction;
    private InputAction _primaryAction;

    #endregion

    void Awake()
    {
        CacheControls();
        playerAnimator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        EnableControls();
            
    }

    private void OnDisable()
    {
        DisableControls();
            
    }
    private void EnableControls()
    {
        _controls.Enable();
        _controls.Player.Enable();
            
    }

    private void DisableControls()
    {
        _controls.Disable();
        _controls.Player.Disable();
            
    }
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        shieldSize = 0;
        playerAttack = GetComponent<PlayerAttack>();
        

    }

    // Update is called once per frame
    void Update()
    {
        CheckHP();
        DetermineShieldSize();
        
        /*if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (playerRB.velocity.y < 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }*/

    }
    private void FixedUpdate()
    {
        PlayerMove();
        if (playerRB.velocity.sqrMagnitude>playerMaxSpeed)
        {
            playerRB.velocity *= 0.75f;
        }
        PlayerAim();
    }
    private void PlayerMove()
    {
        Quaternion rotation = Quaternion.Euler(0, 45.0f,0 );
        var vector3movement = new Vector3(movement.x, 0, movement.y);
        vector3movement = rotation * vector3movement;
        playerRB.AddForce(vector3movement*playerAcceleration);
        playerAnimator.SetBool("isRunning", true);
        if(movement ==Vector2.zero)
        {
            playerRB.velocity *= 0.75f;
            playerAnimator.SetBool("isRunning", false);
        }
    }
    private void PlayerAim()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 50));
        RaycastHit mouseRayHit;
        if (Physics.Raycast(mouseRay, out mouseRayHit, Mathf.Infinity))
        {
            mouseToGroundPoint = mouseRayHit.point;
            dir = mouseToGroundPoint - this.transform.position;
            dir.y = 0;
            this.transform.forward = dir;
            //torso.transform.forward = dir;
        }
    }
    public void OnAction(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();

    }

    public void OnDoPause(InputAction.CallbackContext context)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.PauseGame();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    
        movement = context.ReadValue<Vector2>();
            

    }

   
    public void OnPrimary(InputAction.CallbackContext context)
    {
        
        playerAttack.Attack();
        /*{
            playerAnimator.SetBool("Hit",true);
            if (noOfClicks == 1)
            {
                playerAnimator.SetBool("hit1", true);
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

            if (noOfClicks >= 2 && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit1"))
            {
                playerAnimator.SetBool("hit1", false);
                playerAnimator.SetBool("hit2", true);
            }
            if (noOfClicks >= 3 && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit2"))
            {
                playerAnimator.SetBool("hit2", false);
                playerAnimator.SetBool("hit3", true);
            }
        }*/
       
        

    }
    
    
    public void OnSecondary(InputAction.CallbackContext context)
    {
       
        if (context.interaction is HoldInteraction )
        {
            
            if (shieldSize>0)
            {
                
                shieldIsRaised = true;
                RaiseShield(shieldIsRaised);
            }
            else
            {
                shieldIsRaised = false;
                
            }
        }
        
    }
    public void OnSecondaryCancel(InputAction.CallbackContext obj) { shieldIsRaised = false;}

    public void OnSwapWeapon(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }


    public void CheckHP()
    {
        if (currentHP <= 0)
        {
            playerRB.constraints = RigidbodyConstraints.FreezeAll;
            playerAnimator.Play("Death",0,0);
            isDead = true;
            DisableControls();
            Destroy(this.gameObject, 5f);
        }
        if (currentHP == 1)
        {
            playerHPUI[0].SetActive(true);
            playerHPUI[1].SetActive(false);
            playerHPUI[2].SetActive(false);
        }
        if (currentHP == 2)
        {
            playerHPUI[0].SetActive(true);
            playerHPUI[1].SetActive(true);
            playerHPUI[2].SetActive(false);
        }
        if (currentHP == 3)
        {
            playerHPUI[0].SetActive(true);
            playerHPUI[1].SetActive(true);
            playerHPUI[2].SetActive(true);
        }
    }
    public void GrowShield()
    {
        if (shieldSize<3)
        {
            shieldSize++;
            
        }      
    }
    public void RaiseShield(bool isShieldRaised)
    {
        
        if (isShieldRaised)
        {
            playerAnimator.SetBool("isBlocking", true);
        }
        else
        {
            playerAnimator.SetBool("isBlocking", false);
        }
        
    }
    public void BreakShield()
    {
        if (shieldSize>0)
        {

            var shieldpartRB =shieldParts[Random.Range(0, shieldSize)].AddComponent<Rigidbody>();
            shieldpartRB.AddForce(new Vector3(Random.Range(1, 2), Random.Range(1, 2), Random.Range(1, 2)), ForceMode.Impulse);
            shieldSize--;
        }
    }
    
    public void DetermineShieldSize()
    {
        if (shieldSize == 0)
        {
            foreach (GameObject shieldpartUI in shieldPartsUI)
            {
                shieldpartUI.SetActive(false);
            }
            foreach (GameObject shieldpart in shieldParts)
            {

                shieldpart.SetActive(false);
            }
        }
        if (shieldSize == 1)
        {
            shieldPartsUI[0].SetActive(true);
            shieldParts[0].SetActive(true);
            shieldPartsUI[1].SetActive(false);
            shieldParts[1].SetActive(false);
            shieldPartsUI[2].SetActive(false);
            shieldParts[2].SetActive(false);
        }
        if (shieldSize == 2)
        {
            shieldPartsUI[0].SetActive(true);
            shieldParts[0].SetActive(true);
            shieldPartsUI[1].SetActive(true);
            shieldParts[1].SetActive(true);
            shieldPartsUI[2].SetActive(false);
            shieldParts[2].SetActive(false);
        }
        if (shieldSize == 3)
        {
            shieldPartsUI[0].SetActive(true);
            shieldParts[0].SetActive(true);
            shieldPartsUI[1].SetActive(true);
            shieldParts[1].SetActive(true);
            shieldPartsUI[2].SetActive(true);
            shieldParts[2].SetActive(true);
        }
    }
    
    private void CacheControls()
    {

        _controls = new PlayerInput();
        _moveAction = _controls.Player.Move;
        _lookAction = _controls.Player.Look;
        _primaryAction = _controls.Player.Primary;
        _dodgeAction = _controls.Player.Dodge;
        _secondaryAction = _controls.Player.Secondary;
        _specialAction =_controls.Player.Special;
        _swapWeaponsAction = _controls.Player.SwapWeapon;
        _activateAction = _controls.Player.Action;
        _pauseAction = _controls.Player.DoPause;
        _dodgeAction.performed += OnDodge;
        _moveAction.performed += OnMove;
        _lookAction.performed += OnLook;
        _primaryAction.started += OnPrimary;
        //_primaryAction.canceled += OnPrimaryCancel;
        _secondaryAction.started += OnSecondary;
        _secondaryAction.canceled += OnSecondaryCancel;
        _specialAction.started += OnSpecial;
        _swapWeaponsAction.started += OnSwapWeapon;
        _activateAction.performed += OnAction;
        //_pauseAction = _controls.Player.DoPause;
        _pauseAction.performed += OnDoPause;
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (shieldSize>0)
        {
            playerAttack.ThrowShield();
        }
        
    }

    // Start is called before the first frame update

}
