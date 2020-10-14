using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public static class Enum
    {
        public enum ChildrensAttractions
        {
            POSSIBLE,
            NOT_INTERESTED,
            NECESSARY
        }
        public enum Garden
        {
            POSSIBLE,
            NOT_INTERESTED,
            NECESSARY
        }
        public enum Jacuzzi
        {
            POSSIBLE,
            NOT_INTERESTED,
            NECESSARY
        }
       public enum Pool
        {
            POSSIBLE,
            NOT_INTERESTED,
            NECESSARY
        }
      public enum UnitType//sort of unit
        {
            ZIMMER,
            UNIT,
            HROOM,
            CAMPING,
            SUBLET
        }

        public enum AreaOfV//area of vacation
        {
            ALL,
            NORTH,
            SOUTH,
            CENTER,
            JERUSALEM
        }
        public enum subArea
        {
            GALIL,
            EMEK_HACHULA,
            HAIFA,
            TEL_AVIV,
            MISHOR_HACHOD,
            JUDDEA_DESERT,
            BEER_SHEVA,
            ARAVA,
            EILAT,
        }
        public enum OrderStatus//status order
        {
            NOT_YET,//TREATED
            EMAIL_SENT,
            CLOSED_NOT_REPLIED,//נסגר מחוסר היענות של 
            CLOSED_REPLIED//נסגר בהיענות הללקוח
        }

        public enum GuestRequestStatus//guest request status
        {
            ACTIVE,
            DEAL_CLOSED_VIA_WEB,
            DEAL_CLOSED_EXPIRED
        }


    }
}
