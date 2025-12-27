using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Services.Auth;
using Models;

namespace Storage
{
    public static class TokenStore
    {
        private static readonly string FilePath = "tokens.json";

        // chatId → AuthData
        private static Dictionary<long, AuthData> _tokens = new Dictionary<long, AuthData>();

        private static readonly object _lock = new object();

        static TokenStore()
        {
            Load();
        }

        // Tokenlarni saqlash (chatId bo‘yicha)
        public static void Set(long chatId, string? accessToken, string? refreshToken)
        {
            lock (_lock)
            {
                _tokens[chatId] = new AuthData
                {
                    token = accessToken,
                    refresh_token = refreshToken
                };

                Save();
            }
        }

        // Token olish
        public static AuthData? Get(long chatId)
        {
            lock (_lock)
            {
                return _tokens.ContainsKey(chatId) ? _tokens[chatId] : null;
            }
        }

        // Token o‘chirish (logout uchun)
        public static void Remove(long chatId)
        {
            lock (_lock)
            {
                if (_tokens.ContainsKey(chatId))
                {
                    _tokens.Remove(chatId);
                    Save();
                }
            }
        }
        
        // Hamma tokenlarni fayldan yuklash
        private static void Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return;

                var json = File.ReadAllText(FilePath);
                var data = JsonSerializer.Deserialize<Dictionary<long, AuthData>>(json);

                if (data != null)
                    _tokens = data;
            }
            catch
            {
                // agar json buzilgan bo‘lsa — yangisini yaratamiz
                _tokens = new Dictionary<long, AuthData>();
            }
        }

        // Faylga yozish
        private static void Save()
        {
            var json = JsonSerializer.Serialize(_tokens, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(FilePath, json);
        }
    }
}
