using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace BanVeMayBay.DesignPattern.Singleton
{
    public sealed class TopicSingleton
    {
        public static TopicSingleton Instance { get; } = new TopicSingleton();
        public List<topic> listTopic { get; } = new List<topic> { };

        private TopicSingleton() { }

        public void Init(BANVEMAYBAYEntities context)
        {
            if(listTopic.Count == 0)
            {
                var topics = context.topics.ToList();

                foreach (var topic in topics)
                {
                    listTopic.Add(topic);
                }

            }
        }

        public void UpdateSingleton(BANVEMAYBAYEntities context)
        {
            Instance.listTopic.Clear();
            Init(context);
        }
    }
}