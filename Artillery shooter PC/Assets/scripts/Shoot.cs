using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    //public static MoveShot Instance { get; private set; }
    // Use this for initialization
    public MoveShot instance;
    public GameObject target;
    public GameObject soundWave;
    public GameObject shell;
    public Transform sound;
    public AudioSource boom;
    public AudioClip shootSound;
    public Animator anim;
    public float speedX, speedY;
    private float soundSpeed = 0.1f;
    private bool hasShot = false;
    public float waveRadius=0;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    public bool pleaseShoot=false;
    float shotTime=5;
    void Start()
    {
        boom = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per framess
    void Update()
    {
        shotTime += Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (((pleaseShoot)||(Input.GetKeyDown(KeyCode.Space))) && shotTime>5 && hasShot==false && instance.atBase)
        {
            //instance.SetTarget(, Input.mousePosition.y);
            //Vector2 temp = transform.InverseTransformPoint(Input.mousePosition);
            //instance.targetX = temp.x;
            //instance.targetY = temp.y;
            instance.targetX = target.transform.position.x - (target.transform.localScale.x / 2);
            instance.targetY = target.transform.position.y - (target.transform.localScale.y / 2);
            instance.targetHit = false;
            instance.targetSet();
            hasShot = true;
            float vol = Random.Range(volLowRange, volHighRange);
            boom.PlayOneShot(shootSound, vol);
            anim.SetBool("hasShot", true);
            shotTime = 0;
            instance.hasShot = true;
            Quaternion shotRotation = new Quaternion(0, 0, 180, -90);
            Vector3 shotPosition = new Vector3(transform.position.x, transform.position.y, 0);
            GameObject Shell = Instantiate(shell, shotPosition, shotRotation);
            Rigidbody2D temp = Shell.GetComponent<Rigidbody2D>();
            temp.AddForce(Shell.transform.right * Random.Range(200,400));
            pleaseShoot = false;
        }
        speedY = Input.GetAxis("Vertical") / 10;
        speedX = Input.GetAxis("Horizontal") / 10;
        //target.transform.Translate(speedX, speedY, 0);
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pz.z = -1;
        //pz.x = 20;
        target.transform.position = pz;
        if (hasShot && shotTime < 5)
        {
            sound.localScale += new Vector3(soundSpeed, soundSpeed, 0);
            waveRadius = sound.localScale.x * 2.5f;
        }
        else if (shotTime > 5)
        {
            sound.localScale = new Vector3(0,0, 0);
            waveRadius = 0;
            hasShot = false;
        }
        if(shotTime>0.75)
        {
            anim.SetBool("hasShot", false);
        }
        var dir = target.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

    }
    public void shootButton()
    {
        pleaseShoot = true;
    }
}
