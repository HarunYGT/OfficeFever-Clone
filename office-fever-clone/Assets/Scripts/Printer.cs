using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Printer : MonoBehaviour
{
    [SerializeField] private Transform[] PapersPlace = new Transform[10]; 
    [SerializeField] private GameObject paper;
    public float PaperDeliveryTime,YAxis;
    float scaleFactor = 1f;
    public float CountPapers;

    void Start()
    {
        for (int i = 0; i < PapersPlace.Length; i++)
        {
            PapersPlace[i] = transform.GetChild(1).GetChild(i);
        }

        StartCoroutine(PrintPaper(PaperDeliveryTime));
    }
    public IEnumerator PrintPaper(float Time){
        var pp_index = 0;

        while (CountPapers <100){
            GameObject NewPaper = Instantiate(paper,new Vector3(transform.position.x,-3f,transform.position.z),
            Quaternion.identity,transform.GetChild(0));
            NewPaper.transform.localScale = paper.transform.localScale * scaleFactor;

            NewPaper.transform.DOJump(new Vector3(PapersPlace[pp_index].position.x, PapersPlace[pp_index].position.y + YAxis,
            PapersPlace[pp_index].position.z), 2f, 1, 0.5f).SetEase(Ease.OutQuad);
            if(pp_index <9){
            pp_index++;
            }
            else{
                pp_index =0;
                YAxis += 0.03f;
            }
            yield return new WaitForSecondsRealtime(Time);
        }
        

    }
}
