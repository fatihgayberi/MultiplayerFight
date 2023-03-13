using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Wonnasmith
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public delegate void RoomManagerJoinRoomButtonClick(RoomElementController roomElementController);

        [SerializeField] private Transform roomElementParentTR;
        [SerializeField] private GameObject roomElementControllerPrefab;

        private List<RoomElementController> _roomElementControllerList = new List<RoomElementController>();

        private Dictionary<RoomInfo, RoomElementController> _activeRoomDictionary = new Dictionary<RoomInfo, RoomElementController>();

        private RoomOptions _roomOption;

        private int _masterClientID;

        public override void OnEnable()
        {
            base.OnEnable();

            UIRoomManager.RoomCreateButtonClick += OnRoomCreateButtonClick;

            RoomElementController.JoinRoomButtonClick += OnJoinRoomButtonClick;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            UIRoomManager.RoomCreateButtonClick -= OnRoomCreateButtonClick;

            RoomElementController.JoinRoomButtonClick -= OnJoinRoomButtonClick;
        }


        // dönüp düzeltilecek karmakarışık
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            Debug.Log("OnRoomListUpdate:: ");

            RoomElementController roomElement;

            foreach (RoomInfo newRoomInfo in roomList)
            {
                Debug.Log("newRoomInfo::: ISOPEN" + newRoomInfo.IsVisible);

                if (newRoomInfo.MaxPlayers == 0)
                {
                    if (_activeRoomDictionary.ContainsKey(newRoomInfo))
                    {
                        roomElement = _activeRoomDictionary[newRoomInfo];

                        if (roomElement != null)
                        {
                            roomElement.SetIsAvailableRoomElement(true);

                            _activeRoomDictionary.Remove(newRoomInfo);

                            roomElement.gameObject.SetActiveNullCheck(false);
                        }
                    }
                }
                else
                {
                    if (!_activeRoomDictionary.ContainsKey(newRoomInfo))
                    {
                        roomElement = GetEmptyRoomElement();

                        if (roomElement != null)
                        {
                            roomElement.gameObject.SetActiveNullCheck(true);
                            roomElement.SetIsAvailableRoomElement(false);

                            _activeRoomDictionary.Add(newRoomInfo, roomElement);
                        }
                    }
                }

                if (newRoomInfo.RemovedFromList)
                {
                    foreach (KeyValuePair<RoomInfo, RoomElementController> room in _activeRoomDictionary)
                    {
                        if (string.Equals(room.Key.Name, newRoomInfo.Name))
                        {
                            room.Value.SetIsAvailableRoomElement(true);
                            room.Value.gameObject.SetActiveNullCheck(false);
                        }
                    }
                }
            }

            RoomElementPropertiesUpdate();
        }


        private void RoomElementPropertiesUpdate()
        {
            foreach (KeyValuePair<RoomInfo, RoomElementController> room in _activeRoomDictionary)
            {
                room.Value.SetRoomName(room.Key);
                room.Value.SetRoomPlayerCountTextMessage(room.Key);
            }
        }


        private void OnJoinRoomButtonClick(RoomElementController roomElement)
        {
            if (!GameManager.Instance.GameInLobby())
            {
                return;
            }

            if (PhotonNetwork.CurrentRoom != null)
            {
                return;
            }

            if (roomElement == null)
            {
                return;
            }

            RoomInfo selectRoomInfo = null;

            foreach (KeyValuePair<RoomInfo, RoomElementController> room in _activeRoomDictionary)
            {
                if (room.Value == roomElement)
                {
                    selectRoomInfo = room.Key;

                    break;
                }
            }

            if (selectRoomInfo == null)
            {
                return;
            }

            if (selectRoomInfo.PlayerCount + 1 > selectRoomInfo.MaxPlayers)
            {
                // odada yer yok
                return;
            }

            PhotonNetwork.JoinRoom(selectRoomInfo.Name);
        }


        private void OnRoomCreateButtonClick()
        {
            if (!GameManager.Instance.GameInLobby())
            {
                return;
            }

            if (PhotonNetwork.CurrentRoom != null)
            {
                return;
            }

            string roomName = "Room_" + System.Guid.NewGuid().ToString();

            if (_roomOption == null)
            {
                _roomOption = new RoomOptions();

                _roomOption.MaxPlayers = 2;
                _roomOption.IsOpen = true;
                _roomOption.IsVisible = true;
            }

            PhotonNetwork.CreateRoom(roomName, _roomOption, TypedLobby.Default);
        }


        private RoomElementController GetEmptyRoomElement()
        {
            if (_roomElementControllerList == null)
            {
                _roomElementControllerList = new List<RoomElementController>();
            }

            foreach (RoomElementController roomElement in _roomElementControllerList)
            {
                if (roomElement != null)
                {
                    if (roomElement.GetIsAvailableRoomElement())
                    {
                        return roomElement;
                    }
                }
            }

            RoomElementController roomElement1 = RoomElementGenerator();

            if (roomElement1 != null)
            {
                _roomElementControllerList.Add(roomElement1);

                return roomElement1;
            }

            return null;
        }


        private RoomElementController RoomElementGenerator()
        {
            if (roomElementControllerPrefab == null)
            {
                return null;
            }

            GameObject newRoomElement = GameObject.Instantiate(roomElementControllerPrefab, Vector3.zero, Quaternion.identity, null);

            if (newRoomElement == null)
            {
                return null;
            }

            newRoomElement.transform.SetParent(roomElementParentTR);

            return newRoomElement.GetComponent<RoomElementController>();
        }


        private KeyValuePair<RoomInfo, RoomElementController> GetAvailableRoom()
        {
            if (_activeRoomDictionary == null)
            {
                return default;
            }

            foreach (KeyValuePair<RoomInfo, RoomElementController> room in _activeRoomDictionary)
            {
                if (room.Key.PlayerCount < room.Key.MaxPlayers)
                {
                    return room;
                }
            }

            return default;
        }
    }
}