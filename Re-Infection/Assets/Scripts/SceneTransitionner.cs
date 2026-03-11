using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionner : MonoBehaviour
{
    [SerializeField] Image transitioniAnim;
    [SerializeField] Transform transitionTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        StartCoroutine(LoadAsyncScene(name));
    }

    // ローディング処理
    IEnumerator LoadAsyncScene(string name)
    {
        Image obj = Instantiate(transitioniAnim, transitionTransform);
        obj.rectTransform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(1.0f);

        AsyncOperation ope = SceneManager.LoadSceneAsync(name);

        while (!ope.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        Animator animator = obj.gameObject.GetComponent<Animator>();
        animator.SetTrigger("End");

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
