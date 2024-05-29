using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonImgID : MonoBehaviour
{
    public static SingletonImgID instance{get; private set;}
    public string ReturnedImgID{get; private set;}

    private void Awake() {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
    
    public void SetImgID(string data){
        ReturnedImgID = data;
    }

}
