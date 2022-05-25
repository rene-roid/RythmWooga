using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using System;

public class DiscordController : MonoBehaviour
{

    [Header("Rich Presence")]
    public static long clientID = 977875383460454450;
    public string State;
    public string Details;

    public static long StartTimestamp = -1;

    public string LargeImageKey;
    public string LargeImageText;
    
    public string SmallImageKey;
    public string SmallImageText;

    [Header("Discord")]
    public static Discord.Discord discord;
    private ActivityManager _activityManager;
    private Discord.Activity _activity;
    private bool _isConnected = false;

    private void Awake()
    {
        // Dont destroy on load
        DontDestroyOnLoad(this);
        // Check if there is already an instance of this object and if there is then destroy the others gameobject
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (StartTimestamp == -1) StartTimestamp = ToUnixTime();
    }
    private void CheckDiscord()
    {
        // Check if the Discord application is running
        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
        {
            if (p.ToString() == "System.Diagnostics.Process (Discord)")
            {
                Debug.Log("Discord is running");
                _isConnected = true;
                break;
            }
        }
    }

    private void Start()
    {
        CheckDiscord();
        
        if (_isConnected)
        {
            discord = new Discord.Discord(clientID, (System.UInt64)Discord.CreateFlags.Default);
            _activityManager = discord.GetActivityManager();

            UserActivity();
        }
    }

    private IEnumerator UpdatePresence()
    {
        while (true)
        {
            if (_isConnected)
            {
                // Get actual scene index
                int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                
                if (sceneIndex == 0)
                {
                    UserActivity();
                } else if (sceneIndex == 2)
                {
                    _activity.State = "Fighting DIO";
                    _activity.Details = "In game";

                    //_activity.Timestamps.Start = StartTimestamp;

                    _activity.Assets.LargeImage = "dio";
                    //_activity.Assets.LargeText = LargeImageText;

                    //_activity.Assets.SmallImage = SmallImageKey;
                    //_activity.Assets.SmallText = SmallImageText;

                    _activityManager.UpdateActivity(_activity, (res) => {
                        if (res == Discord.Result.Ok)
                        {
                            Debug.Log("Actualizado correctamente");
                        }
                    });
                }
                

            } else
            {
                discord.Dispose();
            }
            yield return new WaitForSeconds(5);
        }
    }

    private void UserActivity()
    {
        _activity.State = State;
        _activity.Details = Details;

        _activity.Timestamps.Start = StartTimestamp;

        _activity.Assets.LargeImage = LargeImageKey;
        _activity.Assets.LargeText = LargeImageText;
        
        _activity.Assets.SmallImage = SmallImageKey;
        _activity.Assets.SmallText = SmallImageText;

        //_activity.Assets.LargeText = LargeImageText;

        _activityManager.UpdateActivity(_activity, (res) => {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Actualizado correctamente");
            }
        });
    }

    private long ToUnixTime()
    {
        DateTime date = System.DateTime.UtcNow;
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return (long)(date - epoch).TotalSeconds;
    }

    void Update()
    {
        if (!_isConnected) return;
        discord.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        // Stop discord client
        discord.Dispose();
    }
}