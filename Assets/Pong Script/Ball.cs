using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField]
    Vector2 Speed;
    [SerializeField]
    Vector2 resetpos;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public bool GameStart;

    public GameObject Paddle1;
    public GameObject Paddle2;
    public UI_Manager UI;
    public string LastHit;
    public string WhoServed;
    public ParticleSystem BallParticle;
    public SpriteRenderer sr;
    GameInputAction Input;
    
    private void Awake()
    {
        Input = new GameInputAction();
        WhoServed = "P1 Served";
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Input.Player.Start.performed += StartTheGame;
        Input.Player.Enable();
    }

    private void OnDisable()
    {
        Input.Player.Start.performed -= StartTheGame;
        Input.Player.Disable();
    }

    public IEnumerator ResetBall()
    {
        BallParticle.Play(); //Play the particle
        sr.enabled = false; //turn sprite off
        yield return new WaitForSeconds(2f);
        sr.enabled = true; //Turn back the renderer
        transform.position = new Vector2(resetpos.x, resetpos.y); //reset the position
    }

    public void ActivateSpeedPU(float magnitude)
    {
        rb.velocity *= magnitude;
    }

    public void StartTheGame(InputAction.CallbackContext context)
    {
        if(GameStart == false)
        {
            GameStart = true;
            Debug.Log("Game started");
            if(WhoServed == "P1 Served") rb.velocity = Speed;
            else if(WhoServed == "P2 Served") rb.velocity = -Speed;
            UI.StartGame();
        }  
        else
        {
            //Apabila menekan key space lagi, tidak akan berubah dan menyatakan
            //permainan sudah dimulai
            Debug.Log("Game already started");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Is starting " + GameStart);
        //Get rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == Paddle1)
        {
            LastHit = Paddle1.GetComponent<PaddleControl>().PaddleName;
            Debug.Log("Hit " + LastHit);
        } 
        else if(collision.gameObject == Paddle2)
        {
            LastHit = Paddle2.GetComponent<PaddleControl>().PaddleName;
            Debug.Log("Hit " + LastHit);
        }
    }
}
