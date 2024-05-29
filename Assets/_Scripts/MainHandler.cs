using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandler : MonoBehaviour
{
    public CanvasGroup uploadCG;
    public CanvasGroup generateCG;

    private void Start() {
        generateCG.alpha = 0;
        generateCG.gameObject.SetActive(false);
        uploadCG.alpha = 1;

    }

    public void OpenUploadPage(){
        generateCG.alpha = 0;
        generateCG.gameObject.SetActive(false);

        uploadCG.alpha = 0;
        uploadCG.gameObject.SetActive(true);
        LeanTween.alphaCanvas(uploadCG, 1, 0.5f);
    }
    public void OpenGeneratePage(){
        uploadCG.alpha = 0;
        uploadCG.gameObject.SetActive(false);

        generateCG.alpha = 0;
        generateCG.gameObject.SetActive(true);
        LeanTween.alphaCanvas(generateCG, 1, 0.5f);
        
    }
}
