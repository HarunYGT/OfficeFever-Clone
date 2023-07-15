using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Vector3 direction;
    private Camera Cam;
    [SerializeField] private float playerSpeed;
    private Animator playerAnim;

    [SerializeField] private List<Transform> papers = new List<Transform>();
    [SerializeField] private Transform papersPlace;
    private float YAxis,delay;
    public TextMeshProUGUI moneyCounter;

    public static PlayerController Instance;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(this);
        }
        else{
            Instance = this;
        }
    }
    void Start()
    {
        Cam = Camera.main;
        playerAnim = GetComponent<Animator>();

        papers.Add(papersPlace);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            Plane plane = new Plane(Vector3.up,transform.position);
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

            if(plane.Raycast(ray, out var distance)){
                direction = ray.GetPoint(distance);
            }

            transform.position = Vector3.MoveTowards(transform.position,new Vector3(direction.x,0f,direction.z),
                playerSpeed * Time.deltaTime);

            var offset = direction - transform.position;
            
            if(offset.magnitude > 1f){
                 transform.LookAt(direction);    
            }
           
        }
        if(Input.GetMouseButtonDown(0)){
            if(papers.Count > 1){
                playerAnim.SetBool("isRunwithCarry",true);
                playerAnim.SetBool("isCarrying",true);
            }
            else{
                playerAnim.SetBool("isRunning",true);
            }
        }
        else if (Input.GetMouseButtonUp(0)){
            playerAnim.SetBool("isRunning",false);
            if(papers.Count >1){
                playerAnim.SetBool("isRunwithCarry",false);
                playerAnim.SetBool("isCarrying",true);
            }
            else{
                playerAnim.SetBool("isRunning", false);
            }
        }

        if(papers.Count >1){
            for (int i = 1; i < papers.Count; i++)
            {
                var firstPaper = papers.ElementAt(i-1);
                var secondPaper = papers.ElementAt(i);

                secondPaper.position = new Vector3(Mathf.Lerp(secondPaper.position.x,firstPaper.position.x,Time.deltaTime*15f),
                Mathf.Lerp(secondPaper.position.y,firstPaper.position.y + 0.03f, Time.deltaTime*15f),firstPaper.position.z);
            }
        }




        if(Physics.Raycast(transform.position,transform.forward,out var hit,1f)){
            Debug.DrawRay(transform.position,transform.forward *1f,Color.green);
            if(hit.collider.CompareTag("Table") && papers.Count <21){
                if(hit.collider.transform.childCount > 2)
                {
                    var papper = hit.collider.transform.GetChild(1);
                    papper.rotation = Quaternion.Euler(papper.rotation.x,Random.Range(0f,180f),papper.rotation.z);
                    papers.Add(papper);
                    papper.parent = null;

                    if(hit.collider.transform.parent.GetComponent<Printer>().CountPapers>1)
                        hit.collider.transform.parent.GetComponent<Printer>().CountPapers--;

                    if(hit.collider.transform.parent.GetComponent<Printer>().YAxis>0f)
                        hit.collider.transform.parent.GetComponent<Printer>().YAxis -= 0.03f;

                    playerAnim.SetBool("isCarrying", true);
                    playerAnim.SetBool("isRunning", false);
                }
            }
            if(hit.collider.CompareTag("PP") && papers.Count > 1){
                var WorkDesk = hit.collider.transform;
                if(WorkDesk.childCount > 0){
                    YAxis = WorkDesk.GetChild(WorkDesk.childCount - 1).position.y;
                }
                else{
                    YAxis = WorkDesk.position.y;
                }
                for(var index = papers.Count - 1; index >= 1;index--){
                    papers[index].DOJump(new Vector3(WorkDesk.position.x,YAxis,WorkDesk.position.z),2f,1,0.2f)
                    .SetDelay(delay).SetEase(Ease.Flash);

                    papers.ElementAt(index).parent = WorkDesk;
                    papers.RemoveAt(index);

                    YAxis += 0.04f;
                    delay += 0.02f; 
                }

                WorkDesk.parent.GetChild(WorkDesk.parent.childCount -1 ).GetComponent<Renderer>().enabled = false;
            }
            if(papers.Count <=1){
                playerAnim.SetBool("isCarrying",false);
                playerAnim.SetBool("isRunwithCarry",false);
                playerAnim.SetBool("idle",true);
            }
        }   
        else{
            Debug.DrawRay(transform.position,transform.forward *1f,Color.red);
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("PP")){
            other.gameObject.GetComponent<Workdesk>().Work();
        }
        if(other.CompareTag("Dollar")){
            Destroy(other.gameObject);

            PlayerPrefs.SetInt("dollar",PlayerPrefs.GetInt("dollar") +5);
            moneyCounter.text = "$"+ PlayerPrefs.GetInt("dollar");
        }
    }


    private void OnTriggerExit(Collider other){
        if(other.CompareTag("PP")){
            playerAnim.SetBool("isCarrying",false);
            playerAnim.SetBool("isRunwithCarry",false);
            playerAnim.SetBool("idle",false);
            playerAnim.SetBool("isRunning",true);
            delay = 0f;
        }
        
        if(other.CompareTag("Table")){
            if(papers.Count >1){
                playerAnim.SetBool("isCarrying",false);
                playerAnim.SetBool("isRunwithCarry",true);
            }
            else{
                playerAnim.SetBool("isRunning",true);
            }
        }
    }
}
