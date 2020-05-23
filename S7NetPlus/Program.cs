using S7.Net;
using S7.Net.Types;
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
            using (Plc plc = new Plc(PLCSettings.CPU, PLCSettings.IP, PLCSettings.Port, PLCSettings.Rack, PLCSettings.Slot))
            {
                try
                {
                    XPlc myPLC = new XPlc(plc);

                    myPLC.Close();
                    myPLC.Open();

                    //Test testing = plc.ReadClass<Test>(42, 0);
                    Test testing = new Test();
                    //plc.ReadClass(testing, 1);
                    //System.Net.ServicePointManager.Expect100Continue = false;
                    //var testw = plc.ReadAsync("DB42.DBW36").GetAwaiter().GetResult();

                    //short result = ((ushort)testw).ConvertToShort();

                    //short result = ((ushort) await plc.ReadAsync("DB42.DBW36")).ConvertToShort();

                    if (!myPLC.IsAvailable())
                    {
                        Console.WriteLine("PLC is not Available");
                        Console.ReadLine();
                        return;
                    }

                    if (!myPLC.IsConnected())
                    {
                        Console.WriteLine("PLC is Available but can not connect to it");
                        Console.ReadLine();
                        return;
                    }

                    myPLC.Close();
                    //myPLC.Open();
                    Console.WriteLine("Connected");
                    //plc.Close();
                    //DB42.DBD10
                    myPLC.SetDataBlockAndStartByteAdr(42, 10);
                    ////myPLC.WriteValue(21.60, PlcDataType.DWord);

                    myPLC.SetStartByteAdr(36);
                    ////myPLC.WriteValue(60, PlcDataType.Word);

                    //DB42.DBX34.0
                    //myPLC.SetStartByteAdr(34);
                    //myPLC.SetPLCBit(0, true);

                    myPLC.SetStartByteAdr(34);
                    //myPLC.WriteBit(2, true);

                    bool heaterRoom1 = myPLC.ReadBool("A9.2");
                    double tempRoom1 = myPLC.ReadDouble("MD104", 1);
                    double tempSPRoom1 = myPLC.ReadDouble("MD200", 1);
                    double humidityRoom1 = myPLC.ReadDouble("MD168", 1);
                    double DB42_DBD10 = myPLC.ReadDouble("DB42.DBD10", 1);
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
                    ////myPLC.WriteValue(15.66, 4);
                    DB42_DBD318 = myPLC.ReadDouble("DB42.DBD318", 1);
                    Console.WriteLine($"DB42.DBD318                         =   {DB42_DBD318}");
                    Console.ReadLine();

                    //object objTesting = plc.ReadClass<object>(23, 0);
                    //Test testing = plc.ReadClass<Test>(42, 0);
                    //Test testin1 = myPLC.ReadClass<Test>(23, 0);

                    plc.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PLC is not available | {0}", ex.Message);
                    Console.ReadLine();
                }
            }
        }
    }
}
