﻿using System;
using AtNet.Cms.Infrastructure.KV;

namespace AtNet.Cms.Infrastructure
{
    public static class Kvdb
    {
        private static LevelDb _db;

        public static LevelDb _currentInstance
        {
            get
            {
                ChkDb();
                return _db;
            }
        }

        private static void ChkDb()
        {
            if (_db == null)
            {
                throw new ArgumentNullException("Kvdb未初始化，请使用Kvdb.SetPath()");
            }
        }
        public static void SetPath(string path)
        {
            _db = new LevelDb(path);
        }

        public static string Put(string key, string value)
        {
            return _db.Put(key, value);
        }

        public static void Delete(string key)
        {
            ChkDb();
            _db.Delete(key);
        }

        public static string Get(string key)
        {
            ChkDb();
            return _db.Get(key);
        }

        public static int GetInt(string key)
        {
            return _db.GetInt(key);
        }

        public  static void Clean()
        {
            _db.Clean();
        }
    }
}
