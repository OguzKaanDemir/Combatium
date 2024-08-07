using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;
using Scripts.Helpers;

namespace Scripts.UI
{
    public class JoinByRoomListPanel : PanelBase
    {
        [SerializeField] private RoomListItem m_RoomListItemPrefab;
        [SerializeField] private Transform m_ItemSpawnParent;

        public void ResetUI(List<RoomInfo> rooms)
        {
            foreach (Transform item in m_ItemSpawnParent)
                Destroy(item.gameObject);

            for (int i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i];
                if (!room.CustomProperties.ContainsKey(KeyHelper.PASSWORD_KEY)
                    && (bool)room.CustomProperties[KeyHelper.ISVISIBLE_KEY] == true)
                {
                    var newItem = Instantiate(m_RoomListItemPrefab, m_ItemSpawnParent);
                    newItem.SetItem(room);
                }
            }
        }
    }
}
