using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class healthScript : MonoBehaviour
{
    public float maxHealth = 100, health;
    public Slider slider;
    public bool isGettingHurt;

    #region Singleton of health script 
    public static healthScript playerHealth = null;

    void Awake()
    {
        if(playerHealth==null)
            playerHealth = this;
        else if(playerHealth!=null)
            Debug.Log(playerHealth.gameObject.name);
    }
    #endregion

    public delegate void whenInfectedEvent(float health);
    public static whenInfectedEvent whenInfected;
    public delegate void whenDoneInfectedEvent();
    public static whenDoneInfectedEvent whenDoneInfected;
    private bool isHealing;

    void Start()
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        health = maxHealth;
        gameManager.sceneUnloader+=unload;
        isHealing = false;
    }

    void Update()
    {
        if(health <= 0)
            StartCoroutine(death());

        if(isGettingHurt || isHealing)
            whenInfected?.Invoke(health);

        if(Input.GetKey(KeyCode.L))
            doDamage(50);

        if(Input.GetKeyUp(KeyCode.L))
            isGettingHurt = false;

        if(Input.GetKeyUp(KeyCode.R))
            health = 100;

        if(!isGettingHurt && !isHealing)
            whenDoneInfected?.Invoke();
    }

    public IEnumerator healer(float healAmount)
    {
        float startHealth = health;
        for(;;)
        {
            if(health < startHealth + healAmount && health<100)
            {
                float tempHeal = 10 * Time.fixedDeltaTime;
                health += tempHeal;
                isHealing = true;
            }
            else
            {
                isHealing = false;
                yield break;
            }
            yield return null;
        }
    }

    public float doDamage(float dps)
    {
        if(health>0)
        {
            health -= dps * Time.deltaTime;
            isGettingHurt = true;
        }
        else
            resetSliderSize();

        Mathf.Clamp(health,0,maxHealth);
        return health;
    }

    public void resetSliderSize()
    {
        isGettingHurt = false;
    }

    IEnumerator death()
    {
        // gameManager.sceneUnloader?.Invoke();
        // GetComponent<CartController>().enabled = false;
        yield return new WaitForSeconds(1);
        health = 100;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1,LoadSceneMode.Single);
        // SceneManager.LoadScene(0,LoadSceneMode.Single);
    }

    // removing static objects from scene as they dont get deleted automatically and we have to delete them manually
    void unload()
    {
        whenDoneInfected = null;
        whenInfected = null;
        playerHealth = null;
        gameManager.sceneUnloader = null;
    }
}
