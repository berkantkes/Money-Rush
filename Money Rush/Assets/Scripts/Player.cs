using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private List<Transform> spawnPos;
    [SerializeField] private GameObject[] pickups = new GameObject[14];
    [SerializeField] private Text moneyText;
    [SerializeField] private Text lastMoneyText;
    [SerializeField] private GameObject quarterCent;
    [SerializeField] private GameObject halfCent;
    [SerializeField] private GameObject fiveCent;
    [SerializeField] private GameObject gameOverCanvas;
    


    private int pickupsNo = 1;
    private int spawnPosNo = 1;
    private float ySpeed = 0;
    private float zSpeed = 3;
    private float xSpeed = 3;
    private float money = 0.50f;

    public bool gameContinue = true;

    private void Start()
    {
        pickups[0].transform.position = spawnPos[0].transform.position;
        gameContinue = true;
    }

    private void FixedUpdate()
    {
        if (gameContinue)
        {
            Vector3 direction = Vector3.up * ySpeed + Vector3.forward * zSpeed + Vector3.right * xSpeed * dynamicJoystick.Horizontal;
            rb.velocity = direction * speed * Time.fixedDeltaTime;
        }
        
        if (transform.position.x > 3.75f)
        {
            transform.position = new Vector3(3.75f, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -3.75f)
        {
            transform.position = new Vector3(-3.75f, transform.position.y, transform.position.z);
        }
        for (int i = 0; i < pickupsNo; i++)
        {
            if(pickups[i].tag == "50cent")
            {
                pickups[i].transform.position = spawnPos[i].transform.position;
            }

            if (pickups[i].tag == "25cent")
            {
                pickups[i].transform.position = spawnPos[i].transform.position + new Vector3(0, -0.2f, 0);
            }

            if (pickups[i].tag == "5cent")
            {
                pickups[i].transform.position = spawnPos[i].transform.position + new Vector3(0, -0.275f, 0);
            }

        }

        if (!gameContinue)
        {
            rb.velocity = new Vector3(0, 0, 0);
            for(int i = 0; i < pickupsNo; i++)
            {
                pickups[i].transform.SetParent(null);
                pickupsNo--;
            }
        }

        Vector3 velocityrb = rb.velocity;
        float velocityrbx = velocityrb.x;


        if (velocityrbx < 0)
        {
            for (int i = 0; i < pickupsNo; i++)
            {
                pickups[i].GetComponent<JoystickPlayerExample>().TurnLeft();
            }
        }
        if (velocityrbx == 0)
        {
            for (int i = 0; i < pickupsNo; i++)
            {
                pickups[i].GetComponent<JoystickPlayerExample>().Idle();
            }
        }
        if (velocityrbx > 0)
        {
            for (int i = 0; i < pickupsNo; i++)
            {
                pickups[i].GetComponent<JoystickPlayerExample>().TurnRight();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "50cent" || collision.gameObject.tag == "25cent" || collision.gameObject.tag == "5cent")
        {
            if (collision.gameObject.tag == "50cent")
            {
                CreateHalfCent();
                Destroy(collision.gameObject);
                //RaiseMoney(0.50f);
            }
            if (collision.gameObject.tag == "25cent")
            {
                CreateQuarterCent();
                Destroy(collision.gameObject);
                //RaiseMoney(0.25f);
            }
            if (collision.gameObject.tag == "5cent")
            {
                CreateFiveCent();
                Destroy(collision.gameObject);
                //RaiseMoney(0.05f);
            }

        }

        if (collision.gameObject.tag == "MultipleWall")
        {

            float value = collision.GetComponent<ColorWallIncrease>().value;
            float netValue = (value - 1) * money;
            float newHalfCent = Mathf.Floor((netValue / 0.5f));
            for (int i = 0; i < newHalfCent; i++)
            {
                CreateHalfCent();
            }

            float mod = netValue % 0.5f;
            float newQuarterCent = Mathf.Floor(mod / 0.25f);
            for (int i = 0; i < newQuarterCent; i++)
            {
                CreateQuarterCent();
            }

            float mod2 = netValue % 0.25f;
            float newFiveCent = Mathf.Floor(mod2 / 0.05f);
            for (int i = 0; i < newFiveCent; i++)
            {
                CreateFiveCent();
            }

        }

        if(collision.gameObject.tag == "ColorWallSubtraction")
        {
            float decreaseValue = collision.GetComponent<ColorWallIncrease>().value;
            float netMoney = money - decreaseValue;

            while (money != netMoney)
            {
                pickupsNo--;
                spawnPosNo--;
                if (pickups[pickupsNo].tag == "50cent")
                {
                    DecreaseMoney(0.50f);
                }
                if (pickups[pickupsNo].tag == "25cent")
                {
                    DecreaseMoney(0.25f);
                }
                if (pickups[pickupsNo].tag == "5cent")
                {
                    DecreaseMoney(0.05f);
                }

                Destroy(pickups[pickupsNo]);

                if(money < netMoney)
                {
                    float netValue = netMoney - money;
                    float newHalfCent = Mathf.Floor((netValue / 0.5f));
                    for (int i = 0; i < newHalfCent; i++)
                    {
                        CreateHalfCent();
                    }

                    float mod = netValue % 0.5f;
                    float newQuarterCent = Mathf.Floor(mod / 0.25f);
                    for (int i = 0; i < newQuarterCent; i++)
                    {
                        CreateQuarterCent();
                    }
                    float mod2 = netValue % 0.25f;
                    float newFiveCent = Mathf.Floor(mod2 / 0.05f);
                    for (int i = 0; i < newFiveCent; i++)
                    {
                        CreateFiveCent();
                    }
                }
            }

        }

        if (collision.gameObject.tag == "ColorWall")
        {
            float value = collision.GetComponent<ColorWallIncrease>().value;
            float newHalfCent = Mathf.Floor((value / 0.5f));
            for (int i = 0; i < newHalfCent; i++)
            {
                CreateHalfCent();
            }

            float mod = value % 0.5f;
            float newQuarterCent = Mathf.Floor(mod / 0.25f);
            for (int i = 0; i < newQuarterCent; i++)
            {
                CreateQuarterCent();
            }

            float mod2 = value % 0.25f;
            float newFiveCent = Mathf.Floor(mod2 / 0.05f);
            for (int i = 0; i < newFiveCent; i++)
            {
                CreateFiveCent();
            }

        }

        if(collision.gameObject.tag == "DivisionWall")
        {
            
            float newMoney = money;
            float value = collision.GetComponent<ColorWallIncrease>().value;
            float netMoney = (Mathf.Round((money / value) * 100f)) / 100f;
            double netMoneyd = (double)netMoney;
            Debug.Log("netmoney : "+netMoney);
            
            for(int k = 0; k < 20; k++)
            {
                if ((double)money != netMoneyd)
                {
                    /*
                    money = 0;
                    spawnPosNo = 0;
                    pickupsNo = 0;
                    float netValue = netMoney - money;
                    float newHalfCent = Mathf.Floor(netValue / 0.5f);
                    for (int i = 0; i < newHalfCent; i++)
                    {
                        CreateHalfCent();
                    }

                    float mod = netValue % 0.5f;
                    float newQuarterCent = Mathf.Floor(mod / 0.25f);
                    for (int i = 0; i < newQuarterCent; i++)
                    {
                        CreateQuarterCent();
                    }

                    float mod2 = netValue % 0.25f;
                    float newFiveCent = Mathf.Floor(mod2 / 0.05f);
                    for (int i = 0; i < newFiveCent; i++)
                    {
                        CreateFiveCent();
                    }
                    */
                    
                    pickupsNo--;
                    spawnPosNo--;
                    if (pickups[pickupsNo].tag == "50cent")
                    {
                        DecreaseMoney(0.50f);
                    }
                    if (pickups[pickupsNo].tag == "25cent")
                    {
                        DecreaseMoney(0.25f);
                    }
                    if (pickups[pickupsNo].tag == "5cent")
                    {
                        DecreaseMoney(0.05f);
                    }

                    Destroy(pickups[pickupsNo]);

                    if ((double)money < netMoneyd)
                    {
                        float netValue = netMoney - money;
                        float newHalfCent = Mathf.Floor(netValue / 0.5f);
                        for (int i = 0; i < newHalfCent; i++)
                        {
                            CreateHalfCent();
                        }

                        float mod = netValue % 0.5f;
                        float newQuarterCent = Mathf.Floor(mod / 0.25f);
                        for (int i = 0; i < newQuarterCent; i++)
                        {
                            CreateQuarterCent();
                        }

                        float mod2 = netValue % 0.25f;
                        float newFiveCent = Mathf.Floor(mod2 / 0.05f);
                        for (int i = 0; i < newFiveCent; i++)
                        {
                            CreateFiveCent();
                        }
                    }
                    Debug.Log(money);
                    
                }
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FinishLine")
        {
            gameContinue = false;
            gameOverCanvas.SetActive(true);
            money = Mathf.Round(money * 100f) / 100f;
            lastMoneyText.text = money + "$";
        }
        
    }


    public void RaiseMoney(float amount)
    {
        money += amount;
        money = Mathf.Round(money * 100f) / 100f;
        moneyText.text = money + "$";
    }

    public void DecreaseMoney(float amount)
    {
        money -= amount;
        money = Mathf.Round(money * 100f) / 100f;
        moneyText.text = money + "$";
    }

    private void CreateHalfCent()
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, 90);
        pickups[pickupsNo] = Instantiate(halfCent, spawnPos[spawnPosNo].position, newRotation);
        pickups[pickupsNo].transform.parent = this.transform;
        pickupsNo++;
        spawnPosNo++;
        money += 0.50f;
        money = Mathf.Round(money * 100f) / 100f;
        moneyText.text = money + "$";
    }

    private void CreateQuarterCent()
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, 90);
        pickups[pickupsNo] = Instantiate(quarterCent, spawnPos[spawnPosNo].position + new Vector3(0, -0.2f, 0), newRotation);
        pickups[pickupsNo].transform.parent = this.transform;
        pickupsNo++;
        spawnPosNo++;
        money += 0.25f;
        money = Mathf.Round(money * 100f) / 100f;
        moneyText.text = money + "$"; ;
        
    }

    private void CreateFiveCent()
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, 90);
        pickups[pickupsNo] = Instantiate(fiveCent, spawnPos[spawnPosNo].position + new Vector3(0, -0.275f, 0), newRotation);
        pickups[pickupsNo].transform.parent = this.transform;
        pickupsNo++;
        spawnPosNo++;
        money += 0.05f;
        money = Mathf.Round(money * 100f) / 100f;
        moneyText.text = money + "$";
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Scene");
    }
}
