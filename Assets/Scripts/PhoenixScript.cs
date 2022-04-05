using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phoenix;
using System.Net;
using Newtonsoft.Json.Linq;

namespace PhoenixClinet
{
    public class PhoenixScript : MonoBehaviour
    {
        public GameObject prefab;
        public float minRange = -10.0f;
        public float maxRange = 10.0f;
        
        private Color cube_color = Color.clear;

        // Start is called before the first frame update
        void Start()
        {
            print("hello");
            var socketFactory = new WebsocketSharpFactory();
            var socket = new Socket(socketFactory);

            var onOpenCount = 0;
            Socket.OnOpenDelegate onOpenCallback = () =>
            {
                onOpenCount++;
                print("open");
            };

            List<string> onMessageData = new List<string>();
            Socket.OnMessageDelegate onMessageCallback = m =>
            {
                onMessageData.Add(m);
                print(m);
            };

            socket.OnOpen += onOpenCallback;
            socket.OnMessage += onMessageCallback;

            socket.Connect(string.Format("ws://{0}/socket", "127.0.0.1:4000/"), null);

            Reply? okReply = null;
            Reply? errorReply = null;
            bool closeCalled = false;

            var roomChannel = socket.MakeChannel("cube:current_cube");
            roomChannel.On(Message.InBoundEvent.phx_close, _ => closeCalled = true);
            roomChannel.On("after_join", m => print(m));
            roomChannel.On("new_color", m => { 
                var cube_color_obj = m.payload["cube_color"];
                if (cube_color_obj != null) {
                    string colorStr = cube_color_obj.ToString().Split('#')[1]; 
                
                    this.cube_color = this.convertColorFromHexstring(colorStr);
                }
            });

            roomChannel.Join()
              .Receive(Reply.Status.Ok, r => okReply = r)
              .Receive(Reply.Status.Error, r => errorReply = r);
        }

        // Update is called once per frame
        void Update()
        {
            // {"event":"new_color","payload":{"cube_color":"#f0e593","cube_id":"6"},"ref":null,"topic":"cube:current_cube"}
            if (!this.cube_color.Equals(Color.clear)) {
                // GameObject new_cube = new GameObject("Cube");
                // new_cube.transform.position(getRandomPosition());

                GameObject new_cube = Instantiate(prefab, getRandomPosition(minRange, maxRange), Quaternion.identity);
                Material CubeMaterial = new_cube.GetComponent<Renderer>().material;

                CubeMaterial.SetColor("_Color", this.cube_color);
                
                // print(this.cube_color.ToString());

                this.cube_color = Color.clear;
            }
        }

        private Color convertColorFromHexstring(string hex) {
                byte a = 255; 
                byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
                if(hex.Length == 8) {
                    a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
                }
                Color color = new Color32(r,g,b,a);
                return color;
        }

        private Vector3 getRandomPosition(float min, float max) {
            float x = Random.Range(min, max);
            float y = Random.Range(min, max);
            float z = Random.Range(min, max);

            return new Vector3(x, y, z);
        }
        
    }
}
