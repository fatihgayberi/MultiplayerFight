using UnityEngine;

namespace Wonnasmith
{
    public static class PlayerPrefsManager
    {
        private const string playerNameKey = "PlayerName";


        /// <summary>
        /// Player ismini setlemek için kullanilir
        /// </summary>
        /// <param name="playerName"></param>
        public static void SetPlayerName(this string playerName)
        {
            if (playerName.IsStringNullOrWhiteSpace())
            {
                return;
            }

            PlayerPrefs.SetString(playerNameKey, playerName);
        }


        /// <summary>
        /// Player ismini getlemek için kullanilir
        /// </summary>
        /// <param name="playerName"></param>
        public static string GetPlayerName()
        {
            if (PlayerPrefs.GetString(playerNameKey).IsStringNullOrWhiteSpace())
            {
                SetPlayerName("Player_" + System.Guid.NewGuid().ToString());
            }

            return PlayerPrefs.GetString(playerNameKey);
        }
    }
}