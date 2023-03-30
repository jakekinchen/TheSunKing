using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCycler : MonoBehaviour
{
   
    public GameObject[] background;
    int index;
    public GameObject NextButton;
    public GameObject BackButton;
    public GameObject MenuButton;
    public GameObject MenuButton2;

    void Start()
    {
        index = 0;
    }
        

    void Update()
    {
        if(index >= 3)
           index = 3 ; 

        if(index < 0)
           index = 0 ;
        


        if(index == 0)
        {
            background[0].gameObject.SetActive(true);
        }
        
    }

    public void Next()
     {
        index += 1;

        MenuButton2.SetActive(false);
        BackButton.SetActive(true);

        if (index >= background.Length-1)
            {MenuButton.SetActive(true);
            NextButton.SetActive(false);}
    
         for(int i = 0 ; i < background.Length; i++)
         {
            background[i].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
         }
            Debug.Log(index);
     }
    
     public void Previous()
     {

        MenuButton.SetActive(false);
        NextButton.SetActive(true);


          index -= 1;
    if (index < 1)
            {MenuButton2.SetActive(true);
            BackButton.SetActive(false);}
    
         //for(int i = 0 ; i < background.Length; i++
        for(int i = 0 ; i < background.Length; i++)
         {
            background[i].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
         }
            Debug.Log(index);
     }

     public void resetIndex()
     {
         index = 0;

        MenuButton2.SetActive(true);
        BackButton.SetActive(false);

        MenuButton.SetActive(false);
        NextButton.SetActive(true);

         for(int i = 0 ; i < background.Length; i++)
         {
            background[i].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
         }
     }

   
}