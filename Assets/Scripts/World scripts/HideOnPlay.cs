using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    public bool hideOnPlay;

    // Start is called before the first frame update
    void Start()
    {
        if (hideOnPlay){
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
