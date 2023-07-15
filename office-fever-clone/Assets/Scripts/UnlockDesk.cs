using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnlockDesk : MonoBehaviour
{
    [SerializeField] private GameObject unlockProgressObj;
    [SerializeField] private GameObject newDesk;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI dollarAmount;
    [SerializeField] private int deskPrice,deskRemainPrice;
    [SerializeField] private float progressValue;
    public NavMeshSurface buildNavMesh; 
   
    void Start()
    {
        dollarAmount.text = "$" + deskPrice.ToString();
        deskRemainPrice = deskPrice;
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") && PlayerPrefs.GetInt("dollar") > 0){
            progressValue =Mathf.Abs( 1f- CalculateMoney()/deskPrice);

            if(PlayerPrefs.GetInt("dollar") >= deskPrice){
                
                PlayerPrefs.SetInt("dollar", PlayerPrefs.GetInt("dollar") - deskRemainPrice);

                deskRemainPrice = 0;
            }
            else
            {
                deskRemainPrice -= PlayerPrefs.GetInt("dollar");
                PlayerPrefs.SetInt("dollar",0);
            }
            progressBar.fillAmount = progressValue;

            PlayerController.Instance.moneyCounter.text ="$" + PlayerPrefs.GetInt("dollar").ToString(); 
            dollarAmount.text = "$" + deskRemainPrice.ToString();
 
            if(deskRemainPrice==0){
                GameObject NewDesk = Instantiate(newDesk, new Vector3(transform.position.x,0f,transform.position.z),
                Quaternion.Euler(0,180,0));

                NewDesk.transform.DOScale(1.1f,1f).SetEase(Ease.OutElastic);
                NewDesk.transform.DOScale(1f,1f).SetDelay(1.1f).SetEase(Ease.OutElastic);
                unlockProgressObj.SetActive(false);

                buildNavMesh.BuildNavMesh();
            }
        }
    }

    private float CalculateMoney(){
        return deskRemainPrice - PlayerPrefs.GetInt("dollar");
    }
}
