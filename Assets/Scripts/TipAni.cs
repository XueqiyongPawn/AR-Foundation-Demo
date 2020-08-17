using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipAni : MonoBehaviour
{
    private Image mImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (mImage == null)
        {
            mImage = this.GetComponent<Image>();
        }
        Color c = mImage.color;
        c.a = 0;
        mImage.color = c;
        StartCoroutine(StartAni());
    }

    private IEnumerator StartAni()
    {
        while (mImage.color.a < 0.98f)
        {
            Color c = mImage.color;
            c.a += 1/60f;
            mImage.color = c;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
