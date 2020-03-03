using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetector : Activator
{
    private int nbLightSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            updateNbLightSource(+1);
        }else{
            Debug.Log(collision.gameObject.layer);
        }        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Light"))
        {
            updateNbLightSource(-1);
        }
    }

    public void On()
    {
        if (TryActivate != null)
        {
            active = true;
            TryActivate();            
        }        
    }

    public void Off(){
        if (TryDeactivate != null)
        {
            active = false;
            TryDeactivate();            
        }
    }

    void updateNbLightSource(int i)
    {
        nbLightSource += i;

        if(nbLightSource == 1)
            On();
        if(nbLightSource == 0)
        {
            Off();
        }
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}
