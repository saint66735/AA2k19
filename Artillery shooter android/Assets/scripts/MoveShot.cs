using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveShot : MonoBehaviour
{
    float distance = 0;
    public float baseSpeed = 10f;
    public float flyTime=0,predictedFlyTime;
    public float targetY=0, targetX=0;
    public float playerY, playerX;
    public float distanceY, distanceX;
    public bool targetHit=true;
    public float targetcenterX, targetcenterY, shotcenterX, shotcenterY;
    float speedX=0, speedY=0;
    float blowDistance;
    float blowPower=1;
    float playerCenterX, playerCenterY;
    public bool atBase=true;
    float waitAnimation=0;
    private Transform currentTarget;
    public bool hasShot;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    public GameObject player;
    public GameObject shot;
    public GameObject flash;
    public Rigidbody2D shotbody;
    public GameObject target;
    public Animator anim;
    public GameObject targetDecoy;
    public GameObject BloodPile;
    public GameObject Hole;
    public GameLogic gameLogic;
    public AudioSource boom;
    GameObject decoy;
    public AudioClip explosionSound;
    // Use this for initialization
    void Start () {
        shotbody = gameObject.GetComponent<Rigidbody2D>();
        distance = Mathf.Sqrt(Mathf.Pow(targetY - playerY, 2) + Mathf.Pow(targetX - playerX, 2));
        predictedFlyTime = Mathf.Sqrt(distance);
        anim = GetComponent<Animator>();
        boom = GetComponent<AudioSource>();
        baseSpeed = 5f;
    }
	
	// Update is called once per frame
	void Update () {
        waitAnimation -= Time.deltaTime;
        if (waitAnimation < 0.95) flash.SetActive(false);
        if (waitAnimation < 0.9) anim.SetBool("targetHit", false);
    }
    void FixedUpdate()
    {
        //playerY = player.transform.position.y;
        //playerX = player.transform.position.x;
        shotcenterX = shot.transform.position.x - shot.transform.localScale.x / 2;
        shotcenterY = shot.transform.position.y - shot.transform.localScale.y / 2;
        if (targetHit == false && hasShot == true)
        {
            //shotbody.AddForce(transform.up * speedX);
            //shotbody.AddForce(transform.right * speedY * -1);
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, baseSpeed * Time.deltaTime);
            atBase = false;
        }
        if (Mathf.Abs(Mathf.Sqrt(Mathf.Pow(shotcenterX-targetcenterX,2) + Mathf.Pow(shotcenterY - targetcenterY, 2))) < target.transform.localScale.x)
        {
            targetHit = true;
        }
        if (targetHit == true && atBase == false)
        {
            flash.SetActive(true);
            anim.SetBool("targetHit", true);
            float vol = Random.Range(volLowRange, volHighRange);
            boom.PlayOneShot(explosionSound, vol);
            waitAnimation = 1;
            //shotbody.AddForce(shotbody.velocity * -10);
            hasShot = false;
            Destroy(decoy);

        }
        if(shotbody.velocity.x==0 && shotbody.velocity.y == 0 && targetHit == true && atBase == false)
        {
            targetHit = false;
            Quaternion shotRotation = new Quaternion(0, 0, 90, 0);
            Vector3 shotPosition = new Vector3(shot.transform.position.x, shot.transform.position.y, 0);
            GameObject ExplosionHole = Instantiate(Hole, shotPosition, shotRotation);
            
            GameObject[] enemyList;
            enemyList = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemyList.Length; i++)
            {
                blowDistance = Mathf.Sqrt(Mathf.Pow(enemyList[i].transform.position.y - transform.position.y, 2) + Mathf.Pow(enemyList[i].transform.position.x - transform.position.x, 2));
                if (blowDistance < 4)
                {
                    Destroy(enemyList[i]);
                    Quaternion enemyRotation = new Quaternion(0, 0, 90, 0);
                    Vector3 enemyPosition = new Vector3(enemyList[i].transform.position.x, enemyList[i].transform.position.y, 0);
                    GameObject enemyBlood = Instantiate(BloodPile, enemyPosition, enemyRotation);
                    gameLogic.kills++;
                    gameLogic.killsForGold++;
                }
                else if (blowDistance < 5)
                {
                    Rigidbody2D blowRigid = enemyList[i].GetComponent<Rigidbody2D>();
                    //Vector2 toVector = enemyList[i].transform.position - transform.position;
                    //float angleToTarget = Vector2.Angle(transform.up, toVector);
                    blowRigid.AddForce(enemyList[i].transform.up * -1 * blowPower);
                }
            }
            enemyList = GameObject.FindGameObjectsWithTag("Friendly");
            for (int i = 0; i < enemyList.Length; i++)
            {
                blowDistance = Mathf.Sqrt(Mathf.Pow(enemyList[i].transform.position.y - transform.position.y, 2) + Mathf.Pow(enemyList[i].transform.position.x - transform.position.x, 2));
                if (blowDistance < 4)
                {
                    Destroy(enemyList[i]);
                    Quaternion enemyRotation = new Quaternion(0, 0, 90, 0);
                    Vector3 enemyPosition = new Vector3(enemyList[i].transform.position.x, enemyList[i].transform.position.y, 0);
                    GameObject enemyBlood = Instantiate(BloodPile, enemyPosition, enemyRotation);
                    gameLogic.friendlyKills++;
                }
                else if (blowDistance < 5)
                {
                    Rigidbody2D blowRigid = enemyList[i].GetComponent<Rigidbody2D>();
                    //Vector2 toVector = enemyList[i].transform.position - transform.position;
                    //float angleToTarget = Vector2.Angle(transform.up, toVector);
                    blowRigid.AddForce(enemyList[i].transform.up * -1 * blowPower);
                }
            }
            shot.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0.1f);
            atBase = true;
        }
        if (!atBase)
        {
            var dir = decoy.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
        
    }
    public void targetSet()
    {
        gameLogic.currentAmmo--;
        Quaternion enemyRotation = new Quaternion(0, 0, 0, 0);
        Vector3 enemyPosition = new Vector3(target.transform.position.x, target.transform.position.y, -0.2f);
        decoy = Instantiate(targetDecoy, enemyPosition, enemyRotation);
        currentTarget = decoy.transform;
        targetcenterX = targetX - target.transform.localScale.x / 2;
        targetcenterY = targetY - target.transform.localScale.y / 2;
        //playerCenterX = playerX - player.transform.localScale.x / 2;
        //playerCenterY = playerY - player.transform.localScale.y / 2;
        //distanceX = Mathf.Abs(targetcenterX - playerCenterX);
        //distanceY = Mathf.Abs(targetcenterY - playerCenterY);
        //if (targetX < playerX) distanceX *= -1;
        //if (targetY < playerY) distanceY *= -1;
        //speedX = distanceX * baseSpeed /10;
        //speedY = distanceY * baseSpeed /10;
    
    }
   
}

