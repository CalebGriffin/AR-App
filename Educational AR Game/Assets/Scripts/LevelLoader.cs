using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject whiteImage; // The image that will be faded on top of the scene

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the object is enabled, if the current scene is not the menu scene then do the animation otherwise, hide the white image
    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "MenuScene")
        {
            whiteImage.SetActive(true);
            whiteImage.GetComponent<Animator>().Play("In");
        }
        else
        {
            whiteImage.SetActive(false);
        }
    }

    // Call the coroutine to transition to the next scene, takes the scene name as a parameter
    public void LoadLevelFunc(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    // Coroutine that sets the transition image to true and then plays the animation, waits for 1 second and then loads the next scene, takes the scene name as a parameter
    public IEnumerator LoadLevel(string sceneName)
    {
        whiteImage.SetActive(true);
        whiteImage.GetComponent<Animator>().Play("Out");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
