using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text tickText;
    public World world;
    public Text playPauseText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayPauseClicked()
    {
        if (!world.playing)
        {
            playPauseText.text = "Pause";
            world.playing = true;
        } else
        {
            playPauseText.text = "Play";
            world.playing = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        tickText.text = "Tick: " + world.tick;
    }
}
