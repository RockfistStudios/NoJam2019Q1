using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPieChart : MonoBehaviour
{
    public Camera mycamera;

    public float lerpSpeed = 2;

    public Image slice01;
    public Image slice02;
    public Image slice03;

    public Image slice01Text;
    public Image slice02Text;
    public Image slice03Text;

    public float one;
    public float two;
    public float three;

    private void Update()
    {
        slice01.fillAmount = Mathf.Lerp(slice01.fillAmount, one, Time.deltaTime * lerpSpeed);
        slice02.fillAmount = Mathf.Lerp(slice02.fillAmount, two, Time.deltaTime * lerpSpeed);
        slice03.fillAmount = Mathf.Lerp(slice03.fillAmount, three, Time.deltaTime * lerpSpeed);


        slice02.rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(360, 0, slice01.fillAmount));
        slice03.rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(360, 0, slice01.fillAmount + slice02.fillAmount));


      


        slice02Text.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        slice03Text.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void UpdatePieChart(int _one, int _two, int _three)
    {
        one = (float)_one / (float)(_one+ _two+ _three);
        two = (float)_two / (float)(_one + _two + _three);
        three = (float)_three / (float)(_one + _two + _three);

        if(one < .1f)
        {
            slice01Text.gameObject.SetActive(false);
        }
        else
        {
            slice01Text.gameObject.SetActive(true);
        }

        if(two < .1f)
        {
            slice02Text.gameObject.SetActive(false);
        }
        else
        {
            slice02Text.gameObject.SetActive(true);
        }

        if(three < .1f)
        {
            slice03Text.gameObject.SetActive(false);
        }
        else
        {
            slice03Text.gameObject.SetActive(true);
        }
    }
}