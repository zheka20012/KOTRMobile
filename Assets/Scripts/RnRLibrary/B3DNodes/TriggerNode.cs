using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class TriggerNode : BaseNode, IBoundingSphere
    {
        public enum EventType
        {
            unknown = 0,
            NoRain = 4,                 //limit09
            NoSunLins = 5,
            FonMain = 6,
            CheckPoint = 7,
            Restart_010 = 10,
            Restart = 11,               //Restart
            Room,
            BenzoEvent = 14,           //limit09
            STOS,                       //default
            Water,
            Store,                      //default
            InfoStore = 19,             //default
            NoRainNoSun = 22,           //possible limited???? //TODO
            WeatherChange,
            Restart_024,                //Restart
            StartSvetofor = 26,
            NoEntry = 27,               //unknown
            Unknown = 28,               //Unknown, BD.B3D
            SomeEventRoadType = 29,
            RadarEvent,                 //default -> RadarEvent
            Spikes,                     //spikes
            OtherBaza = 4095,           //OtherBaza
        }

        public TriggerNode(NodeHeader header) : base(header)
        {
        }

        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            Event = (EventType) reader.ReadUInt32();

            UNKNOWN = reader.ReadInt32();

            //TODO: Proper triggers handling

            while (reader.ReadUInt32() != 555) ;

            reader.BaseStream.Seek(-4, SeekOrigin.Current);
        }

        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            var _transform = this.CreateObject(parentTransform);

            return _transform;
        }

        public Vector3 Position { get; set; }
        public float Radius { get; set; }

        public EventType Event { get; set; }

        public int UNKNOWN { get; set; }
    }
}