using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grass_script : baseSpawningThing
{
    private InteractionHolder interacty_thingy;

    public void setInteractyThingy(InteractionHolder thingy) {
        interacty_thingy = thingy;
        addToInteractyThingy();
    }

    private void addToInteractyThingy()
    {
        interacty_thingy.addGrassToThing(this.gameObject);
    }

    private void removeFromInteractyThingy()
    {
        interacty_thingy.removeGrassFromThing(this.gameObject);
    }

    protected override void death()
    {
        removeFromInteractyThingy();
        Destroy(this.gameObject);
    }

}
    
