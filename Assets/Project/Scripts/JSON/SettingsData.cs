using System.Collections.Generic;
using System;

[Serializable]
public class SettingsData
{
    public int qualityIndex;
    public int fps;
    public int screenModeIndex;
    public float musicVolume;
    public float sfxVolume;

    public bool inverseYAxis;

    public List<int> highscores;
    public List<float> highTimes;
}