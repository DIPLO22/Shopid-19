using UnityEngine.SceneManagement;
using UnityEngine;

public class ThankYouScreenScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.X))
        {
            SceneManager.LoadScene(0);
        }
        
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
