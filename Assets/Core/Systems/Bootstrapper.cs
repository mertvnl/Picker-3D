using System.Threading.Tasks;
using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static async void Boot()
    {
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("UI")));

        await Task.Delay(3000);

        LevelSystem.Instance.LoadMainLevel();
    }
}
