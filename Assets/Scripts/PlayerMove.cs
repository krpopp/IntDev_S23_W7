using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    //for tuning
    public float speed;
    public float raycastDist;
    public float jumpForce;

    //for particles
    ParticleBurst burstParticles;
    ParticleTrail trailParticles;
    bool hasHit = false;

    //for shape animation
    Squash squashAnim;
    Stretch strechAnim;

    //for enemy collision animation
    EnemyHit enemyHit;

    //for UI
    public TMP_Text livesText;
    public TMP_Text scoreText;

    //gameplay
    int lives = 3;
    int score = 0;

    //physics
    Rigidbody2D myBody;
    float moveX;
    bool jump = false;
    bool onGround = false;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        FindJuicyComponents();
        ResetUI();
    }

    //IGNORE this is something i'm doing for the in class activity
    void FindJuicyComponents()
    {
        burstParticles = GetComponentInChildren<ParticleBurst>();
        trailParticles = GetComponentInChildren<ParticleTrail>();
        squashAnim = GetComponentInChildren<Squash>();
        strechAnim = GetComponentInChildren<Stretch>();
        enemyHit = GetComponentInChildren<EnemyHit>();
    }

    // Update is called once per frame
    void Update()
    {
        //inputs
        VerticalMovement();
        HorizontalMovement();
    }

    void FixedUpdate(){
        HandleParticles();
        HandleAnimations();

        //horizontal velocity
        myBody.velocity = new Vector3(moveX, myBody.velocity.y);    

        //vertical velocity aka jumping
        if(jump){
            myBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

        //ground check
        //also: trigger the hasHit bool if this is the first time we've hit the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDist);
        if(hit.collider != null && hit.transform.tag == "Ground"){
            if(!onGround){
                hasHit = true;
            } else{
                hasHit = false;
            }
            onGround = true;
        } else{
            onGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.tag == "Enemy")
        {
            //if we hit an enemy
            //decrease out life count, reflect that in the UI
            //restart the game if we're dead :(
            lives--;
            ResetUI();
            if (lives > 0)
            {
                HandleEnemyCollision();
            }
            else
            {
                SceneManager.LoadScene(0);
            }
            
        } else if(collision.gameObject.transform.tag == "Collect")
        {
            //if we hit a collectable
            //remove that collectable, increase the score and make the UI reflect that
            Destroy(collision.gameObject);
            score++;
            ResetUI();
        }
    }

    //updates UI
    void ResetUI()
    {
        livesText.text = "Lives: " + lives.ToString();
        scoreText.text = "Score: " + score.ToString();
    }

    void HorizontalMovement(){
        moveX = (Input.GetAxis("Horizontal") * speed) * Time.fixedDeltaTime;
    }

    void VerticalMovement(){
        if(Input.GetKeyDown(KeyCode.Space) && onGround){
            jump = true;
        }
    }

    //for all particle effects
    void HandleParticles(){
        //if you include dust jumping particles and we just hit the ground
        //run those particles
        if(burstParticles != null && hasHit){
            burstParticles.Burst();
        }
        //if you include the dust running particles and the player is moving on the ground
        //run the trail particles; otherwise, turn them off
        if(trailParticles != null){
            if(myBody.velocity.x != 0 && onGround){
                trailParticles.StartTrail();
            } else{
                trailParticles.StopTrail();
            }
        }
    }

    //for juicy shape animations
    void HandleAnimations()
    {
        //if you include the squash animation and we just hit the ground
        //play the squash animation 
        if(squashAnim != null && hasHit)
        {
            squashAnim.TriggerSquash();
        }
        //if you include the strech animation and we just jumped
        //play the stretch animation
        if(strechAnim != null && jump)
        {
            strechAnim.TriggerStrech();
        }
    }

    //for juicy enemy collisions
    void HandleEnemyCollision()
    {
        //if you include the enemy hit juice
        //run the code for when we hit an enemy
        if (enemyHit != null)
        {
            enemyHit.HitEnemy();
        }
    }
    
}
