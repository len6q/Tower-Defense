using System;

[Serializable]
public class EnemyConfig
{
    public Enemy Prefab;

    [FloatRangeSlider(.5f, 2f)] public FloatRange Scale = new FloatRange(1f);
    [FloatRangeSlider(-.4f, .4f)] public FloatRange PathOffset = new FloatRange(0f);
    [FloatRangeSlider(.2f, 5f)] public FloatRange Speed = new FloatRange(1f);
    [FloatRangeSlider(10f, 1000f)] public FloatRange Health = new FloatRange(100f);    
}

public enum EnemyType
{
    BoximonCyclopes,
    BoximonFiery,
    Spider
}