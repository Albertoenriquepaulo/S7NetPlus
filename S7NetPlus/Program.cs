using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S7NetPlus.XPlc;

namespace S7NetPlus
{
    class Program
    {
        public enum CpuType
        {
            S7200 = 0,
            S7300 = 10,
            S7400 = 20,
            S71200 = 30,
            S71500 = 40
        }



        [Obsolete]
        static void Main(string[] args)
        {
            using (Plc plc = new Plc(S7.Net.CpuType.S71200, "192.168.100.65", 9000, 0, 1))
            {
                plc.Open();

                if (plc.IsConnected)
                {
                    Console.WriteLine("Connected");
                }
                else
                {
                    Console.WriteLine("Not Connected");
                }

                //DB42.DBD10
                XPlc myPLC = new XPlc(plc, 42, 10);
                // myPLC.SetPLCValue(21.60, (int)Tipodato.DWord);
                myPLC.SetStartByteAdr(36);
                myPLC.SetPLCValue(60, (int)PlcDataType.Word);

                //DB42.DBX34.0
                //myPLC.SetStartByteAdr(34);
                //myPLC.SetPLCBit(0, true);

                myPLC.SetStartByteAdr(34);
                myPLC.SetPLCBit(2, true);

                //bool heaterRoom1 = ((bool)plc.Read("A9.2"));
                bool heaterRoom1 = myPLC.ReadBool("A9.2");
                //double tempRoom1 = ((uint)plc.Read("MD104")).ConvertToDouble();
                double tempRoom1 = myPLC.ReadDouble("MD104", 1);
                //double tempSPRoom1 = ((uint)plc.Read("MD200")).ConvertToDouble();
                double tempSPRoom1 = myPLC.ReadDouble("MD200", 1);
                //double humidityRoom1 = ((uint)plc.Read("MD168")).ConvertToDouble();
                double humidityRoom1 = myPLC.ReadDouble("MD168", 1);
                //double DB42_DBD10 = ((uint)plc.Read("DB42.DBD10")).ConvertToDouble();
                double DB42_DBD10 = myPLC.ReadDouble("DB42.DBD10", 1);
                //bool DB42_DBX34_0 = Convert.ToBoolean(plc.ReadBytes(DataType.DataBlock, 42, 34, 1)[0]);
                bool DB42_DBX34_0 = myPLC.ReadBool(DataType.DataBlock, 42, 34);

                int DB42_DBW36 = myPLC.ReadInt("DB42.DBW36");
                int DB42_DBB322 = myPLC.ReadInt("DB42.DBB322");
                myPLC.SetStartByteAdr(318);

                double DB42_DBD318 = myPLC.ReadDouble("DB42.DBD318", 1);

                Console.WriteLine($"Temperatura en habitación 1         =   {tempRoom1}");
                Console.WriteLine($"Temperatura deseada en habitación 1 =   {tempSPRoom1}");
                Console.WriteLine($"Humedad Relativa en habitacion 1    =   {humidityRoom1}");
                Console.WriteLine($"Radiador en habitacion 1            =   {heaterRoom1}");
                Console.WriteLine($"DB42                                =   {DB42_DBD10}");
                Console.WriteLine($"DB42.DBX34.0                        =   {DB42_DBX34_0}");
                Console.WriteLine($"------------------------------------------------------");
                Console.WriteLine($"DB42.DBW36                          =   {DB42_DBW36}");
                Console.WriteLine($"DB42.DBD318                         =   {DB42_DBD318}");
                myPLC.SetPLCValue(15.66, 4);
                DB42_DBD318 = myPLC.ReadDouble("DB42.DBD318", 1);
                Console.WriteLine($"DB42.DBD318                         =   {DB42_DBD318}");
                Console.ReadLine();
            }
        }
    }
}
