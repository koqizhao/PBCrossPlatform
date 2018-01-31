using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using ProtoBuf;
using ProtoBuf.Meta;

namespace PBCrossPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            RuntimeTypeModel.Default.InferTagFromNameDefault = true;
            RuntimeTypeModel.Default.Add(typeof(EnumDemo), true);
            RuntimeTypeModel.Default.Add(typeof(Demo2), true);

            Console.WriteLine(RuntimeTypeModel.Default.GetSchema(null));
            */

            //SerializeDateTimeMaxValue();
            //SerializeDateTimeMinValue();

            //SerializeDateTime();
            //SerializeDecimal();

            // DeserializeListFromJava();

            // SerializeList();

            // SerializeDeserializeNestedList(); // nested list is not supported

            //SerializeDeserializeList();

            //SerializeDeserializeListOfMessage();

            //DeserializeMapOf3IntFromJava();

            SerializeNestedMap();

            //SerializeMapOf3Int();

            // SerializeDeserializeDictionary();

            Console.ReadLine();
        }

        static void SerializeNestedMap()
        {
            Demo demo = new Demo()
            {
                Url = "http://www.ctrip.com",
                Title = "test",
                Snipets = new List<string>() { "t1", "t2" },
                Metadata = new Dictionary<int, Dictionary<string, string>>()
                {
                    {
                        1,
                        new Dictionary<string, string>()
                        {
                            {"k1", "v1"},
                            {"k2", "v2"}
                        }
                    }
                },
                IntValues = new List<int>() { 1, 3, 5 }
            };
            using (FileStream fs = new FileStream("demo.bin", FileMode.OpenOrCreate))
            {
                Serializer.Serialize(fs, demo);
            }
        }

        static void SerializeMapOf3Int()
        {
            Dictionary<int, Dictionary<int, int>> mapOf3Int = new Dictionary<int, Dictionary<int, int>>()
            {
                {
                    11,
                    new Dictionary<int, int>()
                    {
                        { 12, 13 }
                    }
                },
                {
                    21,
                    new Dictionary<int, int>()
                    {
                        { 22, 23 },
                        { 24, 25 }
                    }
                },
            };

            using (FileStream fs = new FileStream("mapOf3Int.bin", FileMode.OpenOrCreate))
            {
                Serializer.Serialize(fs, mapOf3Int);
            }
        }

        static void DeserializeMapOf3IntFromJava()
        {
            using (FileStream fs = new FileStream("D:\\temp\\mapdata.bin", FileMode.Open))
            {
                Dictionary<int, Dictionary<int, int>> mapOf3Int = Serializer.Deserialize<Dictionary<int, Dictionary<int, int>>>(fs);
                foreach (KeyValuePair<int, Dictionary<int, int>> item in mapOf3Int)
                {
                    Console.WriteLine(item.Key);
                    WriteDictionary(item.Value);
                    Console.WriteLine();
                }
            }
        }

        static void SerializeDeserializeDictionary()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                { "1", "v1" },
                { "2", "v2" }
            };
            Console.WriteLine(dict);
            Console.WriteLine();

            WriteDictionary<string, string>(dict);

            Dictionary<string, string> dict2;
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, dict);
                stream.Position = 0;
                dict2 = Serializer.Deserialize<Dictionary<string, string>>(stream);
            }

            WriteDictionary<string, string>(dict2);
        }

        static void SerializeList()
        {
            List<int> list = new List<int>()
            {
                1, 2
            };

            WriteList<int>(list);

            using (FileStream fs = new FileStream("listOfInt.bin", FileMode.OpenOrCreate))
            {
                Serializer.Serialize(fs, list);
            }
        }

        static void DeserializeListFromJava()
        {
            using (FileStream fs = new FileStream("D:\\temp\\listOfInt.bin", FileMode.Open))
            {
                List<int> listOf3Int = Serializer.Deserialize<List<int>>(fs);
                foreach (int item in listOf3Int)
                {
                    Console.WriteLine(item);
                    Console.WriteLine();
                }
            }
        }
 
        static void SerializeDeserializeList()
        {
            List<string> list = new List<string>()
            {
                "1", "2"
            };

            WriteList<string>(list);

            List<string> list2 = null;
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, list);
                stream.Position = 0;
                list2 = Serializer.Deserialize<List<string>>(stream);
            }

            WriteList<string>(list2);
        }

        static void SerializeDeserializeListOfMessage()
        {
            Demo demo = new Demo()
            {
                Url = "http://www.ctrip.com",
                Title = "test",
                Snipets = new List<string>() { "t1", "t2" },
                Metadata = new Dictionary<int, Dictionary<string, string>>()
                {
                    {
                        1,
                        new Dictionary<string, string>()
                        {
                            {"k1", "v1"},
                            {"k2", "v2"}
                        }
                    }
                }
            };

            List<Demo> list = new List<Demo>()
            {
                demo, demo
            };

            WriteList<Demo>(list);

            List<Demo> list2 = null;
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, list);
                stream.Position = 0;
                list2 = Serializer.Deserialize<List<Demo>>(stream);
            }

            WriteList<Demo>(list2);
        }

        static void SerializeDeserializeNestedList()
        {
            List<string> list = new List<string>()
            {
                "1", "2"
            };

            List<List<string>> nestedList = new List<List<string>>()
            {
                list, list
            };

            WriteList<List<string>>(nestedList);
            WriteList<string>(list);

            List<List<string>> nestedList2 = null;
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, list);
                stream.Position = 0;
                nestedList2 = Serializer.Deserialize<List<List<string>>>(stream);
            }

            List<string> list2 = nestedList2[0];
            WriteList<List<string>>(nestedList2);
            WriteList<string>(list2);
        }

        static void WriteDictionary<K, V>(Dictionary<K, V> dictionary)
        {
            Console.WriteLine();
            foreach (KeyValuePair<K, V> item in dictionary)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }

        static void WriteList<V>(List<V> list)
        {
            Console.WriteLine();
            foreach (V item in list)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }

        static void SerializeDateTime()
        {
            DateTimeDemo demo = new DateTimeDemo()
            {
                Title = "111",
                DateTimeValue = new DateTime(2018, 1, 4, 17, 59, 0, 333)
            };
            using (FileStream fs = new FileStream("datetimeOfNow.bin", FileMode.OpenOrCreate))
            {
                Serializer.Serialize(fs, demo);
            }
        }

        static void SerializeDateTimeMinValue()
        {
            DateTimeDemo demo = new DateTimeDemo()
            {
                Title = "111",
                DateTimeValue = DateTime.MinValue
            };
            using (FileStream fs = new FileStream("datetimeOfMinValue.bin", FileMode.OpenOrCreate))
            {
                Serializer.Serialize(fs, demo);
            }
        }

        static void SerializeDateTimeMaxValue()
        {
            DateTimeDemo demo = new DateTimeDemo()
            {
                Title = "111",
                DateTimeValue = DateTime.MaxValue
            };
            using (FileStream fs = new FileStream("datetimeOfMaxValue.bin", FileMode.OpenOrCreate))
            {
                Serializer.Serialize(fs, demo);
            }
        }

        static void SerializeDecimal()
        {
            DecimalDemo demo = new DecimalDemo()
            {
                Title = "111",
                DecimalValue = 0.0000000000000000001M
            };
            using (FileStream fs = new FileStream("decimalOfFloat01.bin", FileMode.OpenOrCreate))
            {
                Serializer.Serialize(fs, demo);
                fs.Position = 0;
                DecimalDemo demo2 = Serializer.Deserialize<DecimalDemo>(fs);
                Console.WriteLine();
            }
        }
    }
}
