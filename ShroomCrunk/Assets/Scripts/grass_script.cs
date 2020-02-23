using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grass_script : baseSpawningThing
{
    private InteractionHolder interacty_thingy;

    public void setInteractyThingy(InteractionHolder thingy) {
        interacty_thingy = thingy;
    }
    
    protected override void death()
    {
        Destroy(this.gameObject);
    }

}
    
