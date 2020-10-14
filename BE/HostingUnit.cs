using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
//using Tools;
namespace BE
{
    [Serializable]
    public class HostingUnit
    {
        public long hostingUnitKey = Configuration._HostingUnitKey++;
        public string HostingUnitKey { get { return hostingUnitKey.ToString("00000000"); }  }
      //one implemention of property
        public Host Owner { get; set; }
        public string HostingUnitName { get; set; }


        [XmlIgnore]
        public bool [,] diary=new bool[12, 31];// because we can not serialize it we'll ignore it in the serialization
        [XmlIgnore]
        public bool[,] Diary { get { return diary;}  set { diary = value; } }

        [XmlArray("Diary")] // because we want to call it diary, even if the real name is Arr_Diary- because 'Diary' already exists
        public bool[] Arr_Diary 
        {
            get { return diary.Flatten(); }
            set { Diary = value.Expand(12); }
        }

        public Enum.AreaOfV Area { get; set; }//enum
        public Enum.subArea SubArea { get; set; }
        public Enum.UnitType Type { get; set; }//enum    

        public override string ToString()
        {
            return "Details of hosting unit:\n" +"key of hosting unit: " + HostingUnitKey + " name of hosting unit: " + HostingUnitName +"\n";
        }
        public HostingUnit()//default c-tor
        {
             
        }
        //public HostingUnit(Host Owner, string HostingUnitName)
        //{
        //    this.HostingUnitName = HostingUnitName;
        //    this.Owner = Owner;
        //    hostingUnitKey = Configuration._HostingUnitKey++; //attending to a static variale

        //    Diary = new bool[12, 31];
        //    for (int i = 0; i < 12; i++)
        //    {
        //        for (int j = 0; j < 31; j++)
        //        {
        //            Diary[i, j] = false;
        //        }
        //    }
        //}

    }
}
