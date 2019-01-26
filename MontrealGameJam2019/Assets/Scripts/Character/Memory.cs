using System.Collections;
using System.Collections.Generic;

public class Memory
{
    private float maxTime;
    private float leftTime;
    private bool lost;

    public Memory(float time)
    {
        maxTime = time;
        leftTime = time;
        lost = false;
    }

    public void Lose(float amt)
    {
        leftTime -= amt;
        
        // check if the memory is completely lost
        if (leftTime <= 0)
        {
            lost = true;
        }
    }

    public bool IsLost()
    {
        return lost;
    }

    public float GetLeftTime()
    {
        return leftTime;
    }

    public void Recover()
    {
        leftTime = maxTime;
    }
}
