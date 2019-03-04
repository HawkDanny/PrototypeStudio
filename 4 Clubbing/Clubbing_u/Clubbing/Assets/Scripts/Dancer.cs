using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dancer : MonoBehaviour
{
    [Header("Cosmetics")]
    public Sprite shoeLeft;
    public Sprite shoeRight;
    public Sprite shoeLeft_alt;
    public Sprite shoeRight_alt;
    public Color shoeColor;
    private Color shoeColorTransparent;

    [Header("Dancing")]
    public float stepLength;
    public float stepDistance;
    public bool leftFirst; //Step to the left first

    //Shoe information
    private GameObject shoeLeftGO;
    private GameObject shoeRightGO;
    private Renderer shoeLeftRenderer;
    private Renderer shoeRightRenderer;

    private void Awake()
    {
        //Get the shoe gameobjects
        shoeLeftGO = this.transform.GetChild(0).gameObject;
        shoeRightGO = this.transform.GetChild(1).gameObject;
        //Get the shoe renderers
        shoeLeftRenderer = shoeLeftGO.GetComponent<Renderer>();
        shoeRightRenderer = shoeRightGO.GetComponent<Renderer>();

        //Randomly set the shoe sprites
        if (Random.Range(0, 2) < 1)
        {
            (shoeLeftRenderer as SpriteRenderer).sprite = shoeLeft;
            (shoeRightRenderer as SpriteRenderer).sprite = shoeRight;
        }
        else
        {
            (shoeLeftRenderer as SpriteRenderer).sprite = shoeLeft_alt;
            (shoeRightRenderer as SpriteRenderer).sprite = shoeRight_alt;
        }
        //Set the shoe colors
        shoeColorTransparent = new Color(shoeColor.r, shoeColor.g, shoeColor.b, 0f);
        shoeLeftRenderer.material.color = shoeColorTransparent;
        shoeRightRenderer.material.color = shoeColorTransparent;
    }

    private void Start()
    {
        //Dance();
    }

    private void Dance()
    {
        //StartCoroutine("LeftStep");
    }

    public void LeftStepDown()
    {
        //float asdasd = (stepLength / 2) + Random.Range(-0.05f, 0.05f);
        this.transform.Translate(this.transform.right * Random.Range(-0.1f, 0.1f));
        shoeLeftRenderer.material.DOColor(shoeColor, (stepLength / 2) + Random.Range(-0.05f, 0.05f)).SetEase(Ease.InExpo);
    }

    public void LeftStepUp()
    {
        shoeLeftRenderer.material.DOColor(shoeColorTransparent, (stepLength / 4) + Random.Range(-0.05f, 0.05f)).SetEase(Ease.InExpo);
    }

    public void RightStepDown()
    {
        this.transform.Translate(this.transform.right * Random.Range(-0.1f, 0.1f));
        shoeRightRenderer.material.DOColor(shoeColor, (stepLength / 2) + Random.Range(-0.05f, 0.05f)).SetEase(Ease.InExpo);
    }

    public void RightStepUp()
    {
        shoeRightRenderer.material.DOColor(shoeColorTransparent, (stepLength / 4) + Random.Range(-0.05f, 0.05f)).SetEase(Ease.InExpo);
    }

    /*
    private IEnumerator LeftStep()
    {
        this.transform.Translate(this.transform.right * Random.Range(-0.1f, 0.1f));
        shoeLeftRenderer.material.DOColor(shoeColor, stepLength / 2).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(stepLength);
        shoeLeftRenderer.material.DOColor(shoeColorTransparent, stepLength / 4).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(stepLength / 6);
        StartCoroutine("RightStep");
    }

    private IEnumerator RightStep()
    {
        this.transform.Translate(this.transform.right * Random.Range(-0.1f, 0.1f));
        shoeRightRenderer.material.DOColor(shoeColor, stepLength / 2).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(stepLength);
        shoeRightRenderer.material.DOColor(shoeColorTransparent, stepLength / 4).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(stepLength / 6);
        StartCoroutine("LeftStep");
    }*/
}
