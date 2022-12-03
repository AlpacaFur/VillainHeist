using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMirror : MonoBehaviour
{
    [SerializeField] private GameObject[] mirrors;
    private Renderer[] mirrorRenderers;
    // Start is called before the first frame update
    void Start()
    {
        mirrorRenderers = new Renderer[mirrors.Length];
        for (int i = 0; i < mirrorRenderers.Length; i++)
        {
            mirrorRenderers[i] = mirrors[i].GetComponent<Renderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        hideMirror();
    }

    private void hideMirror()
    {
        for (int i = 0; i < mirrorRenderers.Length; i++)
        {
            mirrors[i].SetActive(mirrorRenderers[i].isVisible);
        }
    }
}
