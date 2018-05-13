using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject friendly1;
    public GameObject friendly2;
    public GameObject friendly3;
    public GameObject friendly4;
    public GameObject tile1;
    public GameObject tile2;
    public GameObject tile3;
    public GameObject tile4;
    public GameObject tile5;
    public GameObject tile6;
    public GameObject tile7;
    public GameObject tile8;
    public Sprite dirt1;
    public Sprite dirt2;
    public Sprite dirt3;
    public Sprite dirt4;
    public Sprite dirt5;
    public Sprite dirt6;
    public Sprite dirt7;
    public Sprite dirt8;
    public AudioSource music;
    public Canvas shop;
    public MoveShot shotScript;
    public Text waveText;
    public Text killText;
    public Text goldText;
    public Text goldText2;
    public Text ammoText;
    public Text textPriceAmmo;
    public Text textPriceFriendly;
    public Text textPriceSpeed;
    public float friendlyKills = 0;
    public float waveDifficultyPerLevel = 2;
    public float currentWave = 0;
    public float Basedifficulty = 3;
    public float amountToKill = 1;
    public float gold = 0;
    public float difficulty = 1;
    public float kills = 0;
    public float ammoPerWave = 10;
    public float extraAmmo = 1;
    public float currentAmmo = 10;
    public float killsForGold = 0;
    float spawnIntervalEnemy = 4f;
    float timeLeftEnemy = 0;
    float timeLeftFriendly = 0;
    float spawnIntervalFriendly = 8f;
    float maxDecals = 40;
    float friendlySpawnRate = 2;
    float priceSpeed = 40, priceFriendly = 40, priceAmmo = 30;
    // your textures to combine
    // !! after importing as sprite change to advance mode and enable read and write property !!
    public Sprite[] textures;
    // just to see on editor nothing to add from editor
    public Texture2D atlas;
    public Material testMaterial;
    public SpriteRenderer testSpriteRenderer;
    int textureWidthCounter = 0;
    int width, height;

    Quaternion enemyRotation;
    Vector3 enemyPosition;
    Quaternion friendlyRotation;
    Vector3 friendlyPosition;
    // Use this for initialization
    void Start()
    {
        textures = new Sprite[100];
        float rand = Random.Range(1, 50);
        for (int i = 0; i < 100; i++)
        {
            if (rand >= 1 && rand < 2)
            {
                textures[i] = dirt1;
            }
            else if (rand >= 2 && rand < 3)
            {
                textures[i] = dirt2;
            }
            else if (rand >= 3 && rand < 4)
            {
                textures[i] = dirt3;
            }
            else if (rand >= 4 && rand < 5)
            {
                textures[i] = dirt4;
            }
            else if (rand >= 5 && rand < 6)
            {
                textures[i] = dirt5;
            }
            else if (rand >= 6 && rand < 7)
            {
                textures[i] = dirt6;
            }
            else if (rand >= 7 && rand < 8)
            {
                textures[i] = dirt7;
            }
            else if (rand >= 8)
            {
                textures[i] = dirt8;
            }
        }
        //generateTerrain();
        
        /*
        textures[1] = dirt2;
        textures[2] = dirt3;
        textures[3] = dirt4;
        textures[4] = dirt5;
        textures[5] = dirt6;
        textures[6] = dirt7;
        textures[7] = dirt8;
        */
        // determine your size from sprites
        width = 0;
        height = 0;
        foreach (Sprite t in textures)
        {
            width += t.texture.width;
            // determine the height
            if (t.texture.height > height) height = t.texture.height;
        }

        // make your new texture
        atlas = new Texture2D(width, height, TextureFormat.RGBA32, false);
        // loop through your textures
        for (int i = 0; i < textures.Length; i++)
        {
            int y = 0;
            while (y < atlas.height)
            {
                int x = 0;
                while (x < textures[i].texture.width)
                {
                    if (y < textures[i].texture.height)
                    {
                        // fill your texture
                        atlas.SetPixel(x + textureWidthCounter, y, textures[i].texture.GetPixel(x, y));
                    }
                    else
                    {
                        // add transparency
                        atlas.SetPixel(x + textureWidthCounter, y, new Color(0f, 0f, 0f, 0f));
                    }
                    ++x;
                }
                ++y;
            }
            atlas.Apply();
            textureWidthCounter += textures[i].texture.width;
        }
        // for normal renderers
        if (testMaterial != null) testMaterial.mainTexture = atlas;
        // for sprite renderer just make  a sprite from texture
        Sprite s = Sprite.Create(atlas, new Rect(0f, 0f, atlas.width, atlas.height), new Vector2(0.5f, 0.5f));
        testSpriteRenderer.sprite = s;
        // add your polygon collider
        testSpriteRenderer.gameObject.AddComponent<PolygonCollider2D>();

    shop.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeftEnemy += Time.deltaTime;
        timeLeftFriendly += Time.deltaTime;
        if (timeLeftEnemy > spawnIntervalEnemy)
        {
            enemyRotation = new Quaternion(0, 0, 90, 90);
            enemyPosition = new Vector3(Random.Range(25, 30), Random.Range(-15, 14), -0.1f);
            float rand = Random.Range(1, 4);
            if (rand >= 1 && rand < 2) { GameObject enemyClone = Instantiate(enemy1, enemyPosition, enemyRotation); }
            else if (rand >= 2 && rand < 3) { GameObject enemyClone = Instantiate(enemy2, enemyPosition, enemyRotation); }
            else if (rand >= 3 && rand < 4) { GameObject enemyClone = Instantiate(enemy3, enemyPosition, enemyRotation); }
            timeLeftEnemy = 0;
        }
        if (timeLeftFriendly > spawnIntervalFriendly)
        {
            friendlyRotation = new Quaternion(0, 0, 90, 90);
            friendlyPosition = new Vector3(Random.Range(-15, -9), Random.Range(-15, 14), -0.1f);
            float rand = Random.Range(1, 5);
            if (rand >= 1 && rand < 2) { GameObject friendlyClone = Instantiate(friendly1, friendlyPosition, friendlyRotation); }
            else if (rand >= 2 && rand < 3) { GameObject friendlyClone = Instantiate(friendly2, friendlyPosition, friendlyRotation); }
            else if (rand >= 3 && rand < 4) { GameObject friendlyClone = Instantiate(friendly3, friendlyPosition, friendlyRotation); }
            else if (rand >= 4 && rand < 5) { GameObject friendlyClone = Instantiate(friendly4, friendlyPosition, friendlyRotation); }

            timeLeftFriendly = 0;
        }
        GameObject[] decalList;
        decalList = GameObject.FindGameObjectsWithTag("Decal");
        if (decalList.Length > maxDecals)
        {
            //decalList[0].SetActive(false);
            Destroy(decalList[0]);
        }
        killText.text = "Kills:" + kills;

        goldText2.text = goldText.text;
        goldText.text = "" + Mathf.Round(gold);
        if (kills >= amountToKill)
        {
            nextWave();
        }
        if (currentAmmo <= 0)
        {
            Application.UnloadLevel("Test");
            Application.LoadLevel("MainMenu");
            music.Stop();
        }
        ammoText.text = "" + currentAmmo;
        if (killsForGold > 0)
        {
            gold += killsForGold * 10;
            killsForGold = 0;
        }
    }
    void generateTerrain()
    {
        int limit = 0;
        for (float i = -35; i < 45; i += 1.28f)
        {
            for (float d = -20; d < 25; d += 1.28f)
            {
                float rand = Random.Range(1, 50);
                enemyRotation = new Quaternion(0, 0, 90, 0);
                enemyPosition = new Vector3(i, d, 1f);
                if (rand >= 1 && rand < 2)
                {
                    GameObject tile = Instantiate(tile1, enemyPosition, enemyRotation);
                }
                else if (rand >= 2 && rand < 3)
                {
                    GameObject tile = Instantiate(tile2, enemyPosition, enemyRotation);
                }
                else if (rand >= 3 && rand < 4)
                {
                    GameObject tile = Instantiate(tile3, enemyPosition, enemyRotation);
                }
                else if (rand >= 4 && rand < 5)
                {
                    GameObject tile = Instantiate(tile4, enemyPosition, enemyRotation);
                }
                else if (rand >= 5 && rand < 6)
                {
                    GameObject tile = Instantiate(tile5, enemyPosition, enemyRotation);
                }
                else if (rand >= 6 && rand < 7)
                {
                    GameObject tile = Instantiate(tile6, enemyPosition, enemyRotation);
                }
                else if (rand >= 7 && rand < 8)
                {
                    GameObject tile = Instantiate(tile7, enemyPosition, enemyRotation);
                }
                else if (rand >= 8)
                {
                    GameObject tile = Instantiate(tile8, enemyPosition, enemyRotation);
                }

                limit++;
                if (limit > 4000) return;
            }
        }
    }
    void nextWave()
    {
        currentWave++;
        //gold += kills * 10;
        gold += currentWave * 10;
        difficulty = currentWave * waveDifficultyPerLevel + Basedifficulty;
        amountToKill = difficulty;
        waveText.text = "Wave:" + currentWave;
        kills = 0;
        spawnIntervalEnemy = 25 / difficulty;
        ammoPerWave += extraAmmo;
        currentAmmo = ammoPerWave;

        if (friendlyKills > 0) friendlyKills--;
    }
    public void addAmmo()
    {
        if (gold >= priceAmmo)
        {
            ammoPerWave += 3;
            gold -= priceAmmo;
            priceAmmo *= 1.2f;
            textPriceAmmo.text = "" + Mathf.Round(priceAmmo);
        }
    }
    public void addFriendly()
    {
        if (gold >= priceFriendly)
        {
            friendlySpawnRate++;
            spawnIntervalFriendly = 16 / friendlySpawnRate;
            gold -= priceFriendly;
            priceFriendly *= 1.3f;
            textPriceFriendly.text = "" + Mathf.Round(priceFriendly);
        }
    }
    public void addSpeed()
    {
        if (gold >= priceSpeed)
        {
            shotScript.baseSpeed += 1.0f;
            gold -= priceSpeed;
            priceSpeed *= 1.3f;

            textPriceSpeed.text = "" + Mathf.Round(priceSpeed);
        }
    }
}
