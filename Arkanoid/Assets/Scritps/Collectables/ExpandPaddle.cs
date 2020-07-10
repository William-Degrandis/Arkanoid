public class ExpandPaddle : Collectable
{
    public float NewWidth = 2.5f;
    protected override void ApplyEffect()
    {
        if(PaddleController._instance != null && !PaddleController._instance.PaddleIsTrasforming)
        {
            PaddleController._instance.StartWidthBuff(NewWidth);
        }
    }
}
