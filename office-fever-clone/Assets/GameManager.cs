using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {
        float startAngle = 15f;
        float endAngle = -15f;
        float duration = 1f;

        targetTransform.localEulerAngles = new Vector3(0f, 0f, startAngle); // Başlangıç açısını ayarla

        targetTransform.DOLocalRotate(new Vector3(0f, 0f, endAngle), duration) // Hedef açıya dön
            .SetLoops(-1, LoopType.Yoyo) // Sonsuz dönüş
            .SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        if(targetTransform.GetComponent<Renderer>().enabled == false){
            DOTween.Kill(targetTransform);
        }
    }
}
 