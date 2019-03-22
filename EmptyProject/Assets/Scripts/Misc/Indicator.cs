using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Indicator : MonoBehaviour
{

    public bool ShowIndicatorGizmos = true;
    public RectTransform thisRect;
    public RectTransform indicatorRect;
    public RectTransform indicatorImageRect;
    public Text distancetxt;
    public Text remainingTimetxt;

    public Transform playerPosition;
    public Animator warningAnim;
    public bool showWarning;

    public Vector3 closetsPoint;
    int dist;


    public void ShowWarning(bool show)
    {
        showWarning = show;
        SetDirection();
        warningAnim.SetBool("Show", showWarning);
        indicatorRect.gameObject.SetActive(showWarning);
        distancetxt.gameObject.SetActive(showWarning);
        GetClosetPoint();
    }

    Quaternion LookCorrection;
    Vector3 targetDirection;
    Quaternion targetRot;
    private void SetDirection()
    {
        LookCorrection = Quaternion.LookRotation(Vector3.up, -Vector3.back);
        targetDirection = closetsPoint - playerPosition.position;
        targetDirection.z = 0;
        
        targetRot = Quaternion.LookRotation(targetDirection, Vector3.forward) * LookCorrection;
        indicatorRect.rotation = targetRot;
    }

    private void LateUpdate()
    {
        GetClosetPoint();

        if(showWarning)
        {
            dist = Mathf.RoundToInt(Vector3.Distance(playerPosition.position, closetsPoint));
            distancetxt.text = dist + "M";
            distancetxt.transform.position = Vector3.Lerp(thisRect.position, indicatorImageRect.position, .75f);


            LookCorrection = Quaternion.LookRotation(Vector3.up, -Vector3.back);
            targetDirection = closetsPoint - playerPosition.position;
            targetDirection.z = 0; // prevents the

            // this check prevents lookRotation is zero log if you try to feed Vector3.zero to look rotation
            if(targetDirection.sqrMagnitude >= float.Epsilon)
            {
                targetRot = Quaternion.LookRotation(targetDirection, Vector3.forward) * LookCorrection;
                indicatorRect.rotation = Quaternion.RotateTowards(indicatorRect.rotation, targetRot, Time.deltaTime * 1000);
            }
        }
    }
    public void SetTimeRemainingText(float timeRemaing)
    {
        remainingTimetxt.text = timeRemaing.ToString("00.0") + "s";
    }

    void GetClosetPoint()
    {
        if(playerPosition!=null)
        {
            closetsPoint = GameManager.Instance.playerAreaBounds.ClosestPoint(playerPosition.position);
            closetsPoint = Vector3.Lerp(closetsPoint, Vector3.zero, .01f);
        }
    }
    
    private void OnDrawGizmos()
    {
        if(!ShowIndicatorGizmos)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(closetsPoint, 1);
    }
}