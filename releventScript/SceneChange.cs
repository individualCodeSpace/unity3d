using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
   public void Selected()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
