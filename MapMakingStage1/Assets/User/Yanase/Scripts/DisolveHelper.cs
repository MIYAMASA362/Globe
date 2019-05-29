
using UnityEngine;

public class DisolveHelper : MonoBehaviour
{
    
    public Material mat;
    public float amount;
    public float smooth;

    private bool fade;
    private bool unfade;
    [Header("go")]
    public GameObject[] characters;


    public float timer;
    public bool disolve;

    private bool end;

    public DisolveHelper thisS;
    //------------------------------------


    void Update(){
        if(!end)
        {

        if(disolve){
            timer += Time.deltaTime;

             if(timer > .2f && timer < .3f){
                fade = true;
                unfade = false;
            }

             if(timer > 1.1f && timer < 1.2f){
                characters[0].SetActive(false);
                characters[1].SetActive(true);
            }

             if(timer > 1.5f && timer < 1.7f){
                fade = false;
                unfade = true;
            }
            if(timer > 3)
            {
                end = true;
                Destroy(thisS.gameObject);

            }
        }//Disolve end

        mat.SetFloat("_DissolveAmount",amount);

        if(fade)
        {
            unfade = false;
            if(amount < .98f){
                amount += Time.deltaTime * smooth;
            }else{
                amount = 1;
                fade = false;
            }
        }
        else if(unfade)
        {
            fade = false;
            if(amount > .02f){
                amount -= Time.deltaTime * smooth;
            }else{
                amount = 0;
                unfade = false;
            }

        }
        }
    }//Update end 
}
