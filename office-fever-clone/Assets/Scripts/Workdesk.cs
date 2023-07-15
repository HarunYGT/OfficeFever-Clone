using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Workdesk : MonoBehaviour
{
    [SerializeField] Transform DollarsPlace;
    [SerializeField] GameObject Dollar;
    private float YAxis;
    private IEnumerator makeMoneyIE;

    public Animator female_anim;

    private void Start(){
        makeMoneyIE = MakeMoney();
    }
    public void Work(){
        female_anim.SetBool("work",true);

        InvokeRepeating("DOSubmitPapers",2f,1f);  

        StartCoroutine(makeMoneyIE);
    }

    private IEnumerator MakeMoney(){
        var counter = 0;
        var DollarPlaceIndex= 0;

        yield return new WaitForSecondsRealtime(2);

        while(counter < transform.childCount){
            GameObject NewDollar = Instantiate(Dollar,new Vector3(DollarsPlace.GetChild(DollarPlaceIndex).position.x,YAxis,
            DollarsPlace.GetChild(DollarPlaceIndex).position.z),DollarsPlace.GetChild(DollarPlaceIndex).rotation);

            NewDollar.transform.DOScale(new Vector3(0.4f,0.4f,0.8f),0.5f).SetEase(Ease.OutElastic);

            if(DollarPlaceIndex < DollarsPlace.childCount-1){
                DollarPlaceIndex++;
            } else{
                DollarPlaceIndex =0;
                YAxis +=0.5f; 
            }  
            yield return new WaitForSecondsRealtime(3f);
        }

    }

    void DOSubmitPapers(){
        if(transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount-1).gameObject,1f);
        }
        else{
            female_anim.SetBool("work",false);

            var Desk = transform.parent;
            Desk.GetChild(Desk.childCount-1).GetComponent<Renderer>().enabled = true;
            
            StopCoroutine(makeMoneyIE);
        }
    }
}
