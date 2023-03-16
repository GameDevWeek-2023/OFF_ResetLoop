
using Interaction;

public class BeerInteraction : SimpleItemPickupInteraction
{
    protected override void Start()
    {
        base.Start();

        if (WorldState.Instance.HasKeyEventHappend(WorldState.KeyEvent.BEER_TAKEN))
        {
            Destroy(gameObject);
        }
    }

    public override void SpecificMouseDownBehaviour()
    {
        if (clickable)
        {
            GameEvents.Instance.OnKeyEvent(WorldState.KeyEvent.BEER_TAKEN);
            AddToInventory(item);
            Destroy(gameObject);
        }
    }

    public void ActivateBeer()
    {
        clickable = true;
    }

    public void DeactivateBeer()
    {
        clickable = false;
    }


}
