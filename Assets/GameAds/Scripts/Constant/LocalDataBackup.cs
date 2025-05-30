using gameAds.Constant;
using System.IO;
using UnityEngine;

public class LocalDataBackup
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "playerData.json");

    public static void SaveData()
    {
        PlayerData data = new PlayerData
        {
            coins = PlayerPrefs.GetInt(GameAdsConstant.TotalCoins),
            balance = PlayerPrefs.GetInt(GameAdsConstant.Balance),
            name = PlayerPrefs.GetString(GameAdsConstant.UserName),
            email = PlayerPrefs.GetString(GameAdsConstant.UserName)
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
    }

    public static void LoadData()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            PlayerPrefs.SetInt(GameAdsConstant.TotalCoins, data.coins);
            PlayerPrefs.SetInt(GameAdsConstant.Balance, data.balance);
            PlayerPrefs.SetString(GameAdsConstant.UserName, data.name);
            PlayerPrefs.SetString(GameAdsConstant.UserEmail, data.email);
        }
    }

    [System.Serializable]
    private class PlayerData
    {
        public int coins;
        public int balance;
        public string name;
        public string email;
    }
}