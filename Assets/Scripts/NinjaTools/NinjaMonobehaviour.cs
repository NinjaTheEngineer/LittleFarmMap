using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NinjaTools {
    public class NinjaMonoBehaviour : MonoBehaviour {
        Dictionary<string, string> lastIdMessage = new Dictionary<string, string>();
        Dictionary<string, float> lastMessageTime = new Dictionary<string, float>();
        public void logd(string id, string message, bool ignoreDuplicates = false, float timeDelay = 0f) {
            if (ignoreDuplicates && lastIdMessage.ContainsKey(id) && lastIdMessage[id] == message) {
                return;
            }
            if (lastMessageTime.ContainsKey(id) && Time.realtimeSinceStartup - lastMessageTime[id] < timeDelay) {
                return;
            }
            Debug.Log(name + "::" + id + "->" + message);
            if (lastIdMessage.ContainsKey(id)) {
                lastMessageTime[id] = Time.realtimeSinceStartup;
                lastIdMessage[id] = message;
            } else {
                lastMessageTime.Add(id, Time.realtimeSinceStartup);
                lastIdMessage.Add(id, message);
            }
        }

        public void logw(string id, string message, bool ignoreDuplicates = false, float timeDelay = 0f) {
            if (ignoreDuplicates && lastIdMessage.ContainsKey(id) && lastIdMessage[id] == message) {
                return;
            }
            if(lastMessageTime.ContainsKey(id) && Time.realtimeSinceStartup - lastMessageTime[id] < timeDelay) {
                return;
            }
            Debug.LogWarning(name + "::" + id + "->" + message);
            if (lastIdMessage.ContainsKey(id)) {
                lastMessageTime[id] = Time.realtimeSinceStartup;
                lastIdMessage[id] = message;
            } else {
                lastMessageTime.Add(id, Time.realtimeSinceStartup);
                lastIdMessage.Add(id, message);
            }
        }

        public void loge(string id = null, string message = null, bool ignoreDuplicates = false, float timeDelay = 0f) {
            if (ignoreDuplicates && lastIdMessage.ContainsKey(id) && lastIdMessage[id] == message) {
                return;
            }
            if (lastMessageTime.ContainsKey(id) && Time.realtimeSinceStartup - lastMessageTime[id] < timeDelay) {
                return;
            }
            Debug.LogError(name + "::" + id + "->" + message);

            if (lastIdMessage.ContainsKey(id)) {
                lastMessageTime[id] = Time.realtimeSinceStartup;
                lastIdMessage[id] = message;
            } else {
                lastMessageTime.Add(id, Time.realtimeSinceStartup);
                lastIdMessage.Add(id, message);
            }
        }
        public void logt(string id = null, string message = null, bool ignoreDuplicates = false) {
            return;
        }
    }
}