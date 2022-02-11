using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject whiteImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void LoadLevelFunc(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    public IEnumerator LoadLevel(string sceneName)
    {
        whiteImage.SetActive(true);
        whiteImage.GetComponent<Animator>().Play("Out");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
