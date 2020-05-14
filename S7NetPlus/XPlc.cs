using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace S7NetPlus
{
    public class XPlc
    {
        public enum PlcDataType
        {
            Bit = 1,
            Word = 2,
            DWord = 4
        }
        private Plc PLC { get; set; }
        private Single ValueToWrite { get; set; }

        private bool BitValueToWrite { get; set; }
        private int DB { get; set; }
        private int StartByteAdress { get; set; }
        private int ByteSize { get; set; }
        private byte[] ByteValue
        {
            get
            {
                byte[] byteArray = BitConverter.GetBytes(ValueToWrite);
                Array.Reverse(byteArray, 0, ByteSize);
                return byteArray;
            }
        }

        public int GetStartByteAdr()
        {
            return StartByteAdress;
        }

        public void SetStartByteAdr(int value)
        {
            StartByteAdress = value;
        }

        public int GetDataBlock()
        {
            return DB;
        }

        public void SetDataBlock(int value)
        {
            DB = value;
        }

        public XPlc(Plc plc, int dB, int startByteAdr)
        {
            PLC = plc;
            DB = dB;
            this.StartByteAdress = startByteAdr;
        }

        public void SetPLCValue(double value, int byteSize)
        {
            this.ByteSize = byteSize;
            switch (byteSize)
            {
                case (int)PlcDataType.Word:
                    ushort intValueToWrite = (ushort)Convert.ToInt16(value);
                    PLC.Write($"DB{DB}.DBW{StartByteAdress}", intValueToWrite);
                    //ushort ValueToWrite1 = (ushort)Convert.ToInt16(value);
                    //byte[] byteArray1 = BitConverter.GetBytes(ValueToWrite1);
                    //Array.Reverse(byteArray1, 0, ByteSize);
                    //PLC.WriteBytes(DataType.DataBlock, DB, StartByteAdress, byteArray1);
                    break;
                case (int)PlcDataType.DWord:
                    ValueToWrite = Convert.ToSingle(value);
                    PLC.WriteBytes(DataType.DataBlock, DB, StartByteAdress, ByteValue);
                    break;
                default:
                    break;
            }


        }

        public void SetPLCBit(int bitAdr, bool value)
        {
            this.ByteSize = 1;
            BitValueToWrite = value;
            PLC.WriteBit(DataType.DataBlock, DB, StartByteAdress, bitAdr, BitValueToWrite);
        }

        public bool ReadBool(string variable)
        {
            return (bool)PLC.Read(variable);
        }
        public bool ReadBool(DataType dataType, int db, int startByteAdr)
        {
            return Convert.ToBoolean(PLC.ReadBytes(dataType, db, startByteAdr, 1)[0]);
        }

        [Obsolete]
        public double ReadDouble(string variable, int decimalNumbers)
        {
            return Math.Round(((uint)PLC.Read(variable)).ConvertToDouble(), decimalNumbers);
        }
        public int ReadInt(string variable)
        {
            return ((ushort)PLC.Read(variable));
        }

        public int ReadBytes(DataType dataType, int db, int startByteAdr, int count)
        {
            byte[] test = PLC.ReadBytes(dataType, db, startByteAdr, count);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(test);

            int i = BitConverter.ToInt32(test, 0);
            return i;
        }

        public int ReadDInt(string variable)
        {
            return ((ushort)PLC.Read(variable));
        }
    }
}
