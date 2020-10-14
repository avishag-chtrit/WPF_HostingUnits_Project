using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;
using DS;
using DAL;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;
using System.Net.Mail;
using System.Net;

namespace DAL
{   //for config , order- xml
    //for guestRequest, Bank brunch, Hosting unit -XmlSerializer 
    class Dal_XML_imp : Idal
    {
        #region Definitions

        private static Dal_XML_imp instance = null;
        private BackgroundWorker worker;

        XElement GuestRequestRoot;
        string GuestRequestPath = @"XML_Files\GuestRequestXml.xml";

        string HostingUnitPath = @"XML_Files\HostingUnitXml.xml";

        XElement BanKBranchRoot;
        const string BanKBranchPath = @"XML_Files\BanKBranchXml.xml";

        XElement OrderRoot;
        string OrderPath = @"XML_Files\OrderXml.xml";

        XElement ConfigRoot;
        string ConfigPath = @"XML_Files\config.xml";

        //------------------------------
        public static void SaveToXML<T>(List<T> list, string Path)
        {
            FileStream file = new FileStream(Path, FileMode.Create);
            XmlSerializer x = new XmlSerializer(list.GetType());
            x.Serialize(file, list);
            file.Close();
        }

        public static List<T> LoadFromXML<T>(string path)
        {
            List<T> list;
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer x = new XmlSerializer(typeof(List<T>));
            list = (List<T>)x.Deserialize(file);
            file.Close();
            return list;
        }
        //-------------------------

        #region singelton
        public static Dal_XML_imp getDal_XML_Imp()
        {
            if (instance == null)
                instance = new Dal_XML_imp();
            return instance;
        }
        #endregion

        #endregion

        #region create_files 

        private void Create_Order_File()
        {
            OrderRoot = new XElement("Orders");
            OrderRoot.Save(OrderPath);
        }

        private void CreateBankBranchesFile()
        {
            BanKBranchRoot = new XElement("BanKBranches");
            BanKBranchRoot.Save(BanKBranchPath);
        }


        private void Create_Config_File()
        {
            ConfigRoot = new XElement("Configs");
            ConfigRoot.Add(new XElement("_GuestRequestKey", Configuration._GuestRequestKey));
            ConfigRoot.Add(new XElement("_HostKey", Configuration._HostKey));
            ConfigRoot.Add(new XElement("_HostingUnitKey", Configuration._HostingUnitKey));
            ConfigRoot.Add(new XElement("_OrderKey", Configuration._OrderKey));
            ConfigRoot.Add(new XElement("_Fee", Configuration._Fee));
            ConfigRoot.Add(new XElement("MailAddress", Configuration.MailAddress));
            ConfigRoot.Add(new XElement("MailPassWord", Configuration.MailPassWord));
            ConfigRoot.Add(new XElement("DateToday", Configuration.DateToday));
            ConfigRoot.Add(new XElement("IsClearOrdrs", Configuration.IsClearOrdrs));

            ConfigRoot.Save(ConfigPath);
        }
        private void Create_GuestRquest_File()
        {

            GuestRequestRoot = new XElement("GuestRequests");
            GuestRequestRoot.Save(GuestRequestPath);
        }
        private void Create_HostingUnit_File()//using serialize xml
        {
            FileStream HostingUnitFile = new FileStream(HostingUnitPath, FileMode.Create);
            HostingUnitFile.Close();
        }

        private void CreateBankBranchesDetailes()
        {
            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                wc.DownloadFile(xmlServerPath, BanKBranchPath);
            }
            catch (Exception)
            {
                string xmlServerPath =
                  @"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                wc.DownloadFile(xmlServerPath, BanKBranchPath);
            }
            finally
            {
                wc.Dispose();
            }
        }

        #endregion

        #region load_files

        private void LoadData_GuestRequest_File()
        {
            try
            {
                GuestRequestRoot = XElement.Load(GuestRequestPath);
            }
            catch
            {
                throw new CanNotLoadFileException("file problem upload");
            }
        }

        private void LoadData_HostingUnit_File()
        {
            try
            {
                DataSource.hostingUnits = LoadFromXML<HostingUnit>(HostingUnitPath);
                SaveToXML<HostingUnit>(DataSource.hostingUnits, HostingUnitPath); // because we took the list from the file - we need to return it
            }
            catch
            {
                throw new CanNotLoadFileException("file problem upload");
            }
        }

        private void LoadData_BanKBranch_File()
        {
            try
            {
                BanKBranchRoot = XElement.Load(BanKBranchPath);
            }
            catch
            {
                throw new CanNotLoadFileException("file problem upload");
            }
        }

        private void LoadData_Order_File()
        {
            try
            {
                OrderRoot = XElement.Load(OrderPath);
            }
            catch
            {
                throw new CanNotLoadFileException("file problem upload");
            }
        }

        private void LoadData_Config_File()
        {
            try
            {
                ConfigRoot = XElement.Load(ConfigPath);
                Configuration._GuestRequestKey = long.Parse(ConfigRoot.Element("_GuestRequestKey").Value);
                Configuration._HostKey = long.Parse(ConfigRoot.Element("_HostKey").Value);
                Configuration._HostingUnitKey = long.Parse(ConfigRoot.Element("_HostingUnitKey").Value);
                Configuration._OrderKey = long.Parse(ConfigRoot.Element("_OrderKey").Value);
                Configuration._Fee = long.Parse(ConfigRoot.Element("_Fee").Value);
                Configuration.DateToday = (DateTime.Parse(ConfigRoot.Element("DateToday").Value)).ToShortDateString();
                Configuration.IsClearOrdrs = bool.Parse(ConfigRoot.Element("IsClearOrdrs").Value.ToString());

             
            }

            catch
            {
                throw new CanNotLoadFileException("file problem upload");
            }
        }

        #endregion

        #region Convert

        XElement Convert_Order(BE.Order order)//needs fixing
        {
            XElement OrderKey = new XElement("orderkey", order.OrderKey);
            XElement GuestRequestKey = new XElement("GuestRequestKey", order.GuestRequestKey);
            XElement HostingUnitKey = new XElement("HostingUnitKey", order.HostingUnitKey);
            XElement Status = new XElement("Status", order.Status);
            XElement CreateDate = new XElement("CreateDate", order.CreateDate);
            XElement OrderDate = new XElement("OrderDate", order.OrderDate);

            XElement orderElement = new XElement("Order", OrderKey, GuestRequestKey, HostingUnitKey, Status, CreateDate, OrderDate);
            return orderElement;
        }
        BE.Order Convert_Order(XElement element)
        {
            BE.Order order = new BE.Order();
            order.orderKey = long.Parse((element.Element("orderkey").Value));//one of the 2 properties.. capital or low?
            order.guestRequestKey = long.Parse((element.Element("GuestRequestKey").Value));
            order.hostingUnitKey = long.Parse((element.Element("HostingUnitKey").Value));
            order.Status = (BE.Enum.OrderStatus)System.Enum.Parse(typeof(BE.Enum.OrderStatus), element.Element("Status").Value);
            order.OrderDate = Convert.ToDateTime((element.Element("OrderDate").Value));
            order.CreateDate = Convert.ToDateTime((element.Element("CreateDate").Value));

            return order;
        }

        XElement Convert_GuestRequest(BE.GuestRequest gr)
        {
            XElement GuestRequestKey = new XElement("GuestRequestKey", gr.GuestRequestKey);//clerify -ied
            XElement PrivateName = new XElement("PrivateName", gr.PrivateName);
            XElement FamilyName = new XElement("FamilyName", gr.FamilyName);
            XElement MailAddress = new XElement("MailAddress", gr.MailAddress);
            XElement gs_Status = new XElement("Status", gr.Status);
            XElement RegistrationDate = new XElement("RegistrationDate", gr.RegistrationDate);
            XElement EntryDate = new XElement("EntryDate", gr.EntryDate);
            XElement ReleaseDate = new XElement("ReleaseDate", gr.ReleaseDate);
            XElement Area = new XElement("Area", gr.Area);
            XElement SubArea = new XElement("SubArea", gr.SubArea);
            XElement Type = new XElement("Type", gr.Type);
            XElement Adults = new XElement("Adults", gr.Adults);
            XElement Children = new XElement("Children", gr.Children);
            XElement pool = new XElement("pool", gr.pool);
            XElement jacuzzi = new XElement("jacuzzi", gr.jacuzzi);
            XElement garden = new XElement("garden", gr.garden);
            XElement childAt = new XElement("childAt", gr.childAt);

            XElement GuestRequestElement = new XElement("GuestRequest", GuestRequestKey, PrivateName, FamilyName, MailAddress, gs_Status, RegistrationDate,
                                                                   EntryDate, ReleaseDate, Area, SubArea, Type, Adults, Children, pool, jacuzzi, garden, childAt);
            return GuestRequestElement;
        }
        BE.GuestRequest Convert_GuestRequest(XElement element)
        {
            BE.GuestRequest gs = new BE.GuestRequest();

            gs.guestRequestKey = long.Parse((element.Element("GuestRequestKey").Value));

            gs.PrivateName = element.Element("PrivateName").Value;
            gs.FamilyName = element.Element("FamilyName").Value;
            gs.MailAddress = element.Element("MailAddress").Value;
            gs.Status = (BE.Enum.GuestRequestStatus)System.Enum.Parse(typeof(BE.Enum.GuestRequestStatus), element.Element("Status").Value);
            gs.RegistrationDate = Convert.ToDateTime((element.Element("RegistrationDate").Value));
            gs.EntryDate = Convert.ToDateTime((element.Element("EntryDate").Value));
            gs.ReleaseDate = Convert.ToDateTime((element.Element("ReleaseDate").Value));

            gs.Adults = int.Parse((element.Element("Adults").Value));
            gs.Children = int.Parse((element.Element("Children").Value));

            gs.Area = (BE.Enum.AreaOfV)System.Enum.Parse(typeof(BE.Enum.AreaOfV), element.Element("Area").Value);
            gs.SubArea = (BE.Enum.subArea)System.Enum.Parse(typeof(BE.Enum.subArea), element.Element("SubArea").Value);
            gs.Type = (BE.Enum.UnitType)System.Enum.Parse(typeof(BE.Enum.UnitType), element.Element("Type").Value);
            gs.pool = (BE.Enum.Pool)System.Enum.Parse(typeof(BE.Enum.Pool), element.Element("pool").Value);
            gs.jacuzzi = (BE.Enum.Jacuzzi)System.Enum.Parse(typeof(BE.Enum.Jacuzzi), element.Element("jacuzzi").Value);
            gs.garden = (BE.Enum.Garden)System.Enum.Parse(typeof(BE.Enum.Garden), element.Element("garden").Value);
            gs.childAt = (BE.Enum.ChildrensAttractions)System.Enum.Parse(typeof(BE.Enum.ChildrensAttractions), element.Element("childAt").Value);

            return gs;
        }

        XElement Convert_BanKBranchRoot(BE.BankBranch bank)
        {
            XElement bankNumber = new XElement("bankNumber", bank.bankNumber);
            XElement BankName = new XElement("BankName", bank.BankName);
            XElement branchNumber = new XElement("branchNumber", bank.branchNumber);
            XElement BranchAddress = new XElement("BranchAddress", bank.BranchAddress);
            XElement BranchCity = new XElement("BranchCity", bank.BranchCity);

            XElement BankBranchElement = new XElement("BankBranch", bankNumber, BankName, branchNumber, BranchAddress, BranchCity);
            return BankBranchElement;
        }

        BE.BankBranch Convert_BanKBranchRoot(XElement element)
        {
            BankBranch bank = new BankBranch();

            bank.bankNumber = long.Parse((element.Element("קוד_בנק").Value));
            bank.BankName = element.Element("שם_בנק").Value;
            bank.branchNumber = long.Parse((element.Element("קוד_סניף").Value));
            bank.BranchAddress = element.Element("כתובת_ה-ATM").Value;
            bank.BranchCity = element.Element("ישוב").Value;

            return bank;
        }
        #endregion

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!File.Exists(BanKBranchPath))
                CreateBankBranchesFile();

            CreateBankBranchesDetailes();
            LoadData_BanKBranch_File();
            get_All_Bank_Branches();
        }

        private Dal_XML_imp()
        {
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();

            //without backgroundworker it woult take a few seconds to run...
            //if (!File.Exists(BanKBranchPath))
            //    CreateBankBranchesFile();
            //CreateBankBranchesDetailes();
            //LoadData_BanKBranch_File();

            if (!File.Exists(ConfigPath))
                Create_Config_File();
            else
                LoadData_Config_File();

            if (!File.Exists(OrderPath))
                Create_Order_File();
            else
                LoadData_Order_File();

            if (!File.Exists(GuestRequestPath))
                Create_GuestRquest_File();
            else
                LoadData_GuestRequest_File();

            if (!File.Exists(HostingUnitPath))
                Create_HostingUnit_File();
            else
                LoadData_HostingUnit_File();
        }

        #region Order Functions
        public void add_Order(Order order)
        {
            try
            {
                LoadData_Order_File();
                LoadData_GuestRequest_File();
                LoadData_HostingUnit_File();
            }
            catch (CanNotLoadFileException ex)
            {
                throw ex;
            }

            order = order.Clone(); //pretty stupid if you ask me- but we were asked for it
            order.CreateDate = DateTime.Now;

            var it = (from item in OrderRoot.Elements()
                      where item.Element("orderkey").Value == order.OrderKey
                      select item).FirstOrDefault();

            if (it != null)
            {
                throw new DuplicatedKeyException("this order was already made");
            }

            it = (from item in GuestRequestRoot.Elements()
                  where item.Element("GuestRequestKey").Value == order.GuestRequestKey
                  select item).FirstOrDefault();

            if (it == null)
            {
                throw new KeyNotFoundException("Error! There isn't any Guest Request with the current key at the program");
            }

            List<HostingUnit> unitsLists = LoadFromXML<HostingUnit>(HostingUnitPath);

            var unit = (from item in unitsLists
                        where item.HostingUnitKey == order.HostingUnitKey
                        select item).FirstOrDefault();

            if (it == null)
            {
                SaveToXML(unitsLists, HostingUnitPath); // before you throw the exception- return the list to it's place in the file
                throw new KeyNotFoundException("Error! This unit does not exist, therefore you can not order it");
            }

            SaveToXML(unitsLists, HostingUnitPath); // because we took the list out of the file we need to return it

            XElement OrderKey = new XElement("orderkey", order.OrderKey);
            XElement GuestRequestKey = new XElement("GuestRequestKey", order.GuestRequestKey);
            XElement HostingUnitKey = new XElement("HostingUnitKey", order.HostingUnitKey);
            XElement Status = new XElement("Status", order.Status);
            XElement CreateDate = new XElement("CreateDate", order.CreateDate);
            XElement OrderDate = new XElement("OrderDate", order.OrderDate);

            OrderRoot.Add(new XElement("Order", OrderKey, GuestRequestKey, HostingUnitKey, Status, CreateDate, OrderDate));
            OrderRoot.Save(OrderPath);

            ConfigRoot.Element("_OrderKey").Value = (BE.Configuration._OrderKey).ToString();
            ConfigRoot.Save(ConfigPath);
        }

        public void update_Order(Order order)
        {
            XElement it = (from item in OrderRoot.Elements()
                           where item.Element("orderkey").Value == order.OrderKey
                           select item).FirstOrDefault();
            if (it == null)
            {
                throw new KeyDoesNotExistException("this order does not exist, therefore it can not be updated");
            }

            XElement updateOrder = Convert_Order(order.Clone());
            it.ReplaceNodes(updateOrder.Elements());

            OrderRoot.Save(OrderPath);
        }

        public Order Get_Order(Order or)//get an object or orderKey?
        {
            try
            {
                LoadData_Order_File();
            }
            catch (CanNotLoadFileException ex)
            {
                throw ex;
            }
            var order = (from item in OrderRoot.Elements()
                         where item.Element("orderKey").Value == or.OrderKey
                         select item).FirstOrDefault();

            return Convert_Order(order);
        }
        public List<Order> get_All_Orders(Func<Order, bool> predicat = null)
        {
            try
            {
                LoadData_Order_File();
            }
            catch (CanNotLoadFileException ex)
            {
                throw ex;
            }
            if (predicat == null)
            {
                return (from item in OrderRoot.Elements()
                        select Convert_Order(item).Clone()).ToList();
            }

            return (from item in OrderRoot.Elements()
                    let or = Convert_Order(item).Clone()
                    where predicat(or)
                    select or).ToList();
        }
        #endregion

        #region Guest Request Functions
        public void add_GuestRequest(GuestRequest guestRequest)
        {
            try
            {
                LoadData_GuestRequest_File();
            }
            catch (CanNotLoadFileException ex)
            {
                throw ex;
            }

            var it = (from item in GuestRequestRoot.Elements()
                      where item.Element("GuestRequestKey").Value == guestRequest.GuestRequestKey
                      select item).FirstOrDefault();

            if (it != null)
            {
                throw new DuplicatedKeyException("this guest request was already made");
            }

            guestRequest = guestRequest.Clone();// they asked for working on cloned objects

            XElement GuestRequestKey = new XElement("GuestRequestKey", guestRequest.GuestRequestKey);
            XElement PrivateName = new XElement("PrivateName", guestRequest.PrivateName);
            XElement FamilyName = new XElement("FamilyName", guestRequest.FamilyName);
            XElement MailAddress = new XElement("MailAddress", guestRequest.MailAddress);
            XElement Status = new XElement("Status", guestRequest.Status);
            XElement RegistrationDate = new XElement("RegistrationDate", guestRequest.RegistrationDate);
            XElement EntryDate = new XElement("EntryDate", guestRequest.EntryDate);
            XElement ReleaseDate = new XElement("ReleaseDate", guestRequest.ReleaseDate);
            XElement Area = new XElement("Area", guestRequest.Area);
            XElement SubArea = new XElement("SubArea", guestRequest.SubArea);
            XElement Type = new XElement("Type", guestRequest.Type);
            XElement Adults = new XElement("Adults", guestRequest.Adults);
            XElement Children = new XElement("Children", guestRequest.Children);
            XElement pool = new XElement("pool", guestRequest.pool);
            XElement jacuzzi = new XElement("jacuzzi", guestRequest.jacuzzi);
            XElement garden = new XElement("garden", guestRequest.garden);
            XElement childAt = new XElement("childAt", guestRequest.childAt);

            GuestRequestRoot.Add(new XElement("GuestRequest", GuestRequestKey, PrivateName, FamilyName, Status, MailAddress, Status, RegistrationDate, EntryDate, ReleaseDate,
                                                        Area, SubArea, Type, Adults, Children, pool, jacuzzi, garden, childAt));
            GuestRequestRoot.Save(GuestRequestPath);
            ConfigRoot.Element("_GuestRequestKey").Value = (BE.Configuration._GuestRequestKey).ToString();
            ConfigRoot.Save(ConfigPath);
        }

        public void update_GuestRequest(GuestRequest gs)
        {
            gs = gs.Clone(); //pretty stupid if you ask me, but we were asked to work with cloned objects

            XElement it = (from item in GuestRequestRoot.Elements()
                           where item.Element("GuestRequestKey").Value == gs.GuestRequestKey
                           select item).FirstOrDefault();
            if (it == null)
            {
                throw new KeyDoesNotExistException("this request does not exist so it can not be updated");
            }

            if (gs.Adults <= 0)
                throw new NegativeNumberException("there must be at least one adult!");
            if (gs.Children < 0)
                throw new NegativeNumberException("number of children can not be negative!");


            XElement updateGuestRequest = Convert_GuestRequest(gs);
            it.ReplaceNodes(updateGuestRequest.Elements());

            GuestRequestRoot.Save(GuestRequestPath);
        }

        public GuestRequest Get_GuestRequest(GuestRequest gs)
        {
            try
            {
                LoadData_GuestRequest_File();
            }
            catch (CanNotLoadFileException ex)
            {
                throw ex;
            }
            var req = (from item in GuestRequestRoot.Elements()
                       where item.Element("GuestRequestKey").Value == gs.GuestRequestKey
                       select item).FirstOrDefault();

            return Convert_GuestRequest(req);
        }

        public List<GuestRequest> get_All_Guests(Func<GuestRequest, bool> predicat = null)
        {
            if (predicat == null)
            {
                return (from item in GuestRequestRoot.Elements()
                        select Convert_GuestRequest(item)).ToList();
            }

            return (from item in GuestRequestRoot.Elements()
                    let gs = Convert_GuestRequest(item)
                    where predicat(gs)
                    select gs).ToList();
        }
        #endregion

        #region Hosting Unit Functions

        public void add_HostingUnit(HostingUnit unit)
        {
            try
            {
                LoadData_HostingUnit_File();
            }
            catch (CanNotLoadFileException ex)
            {
                throw ex;
            }

            List<HostingUnit> unitsList = LoadFromXML<HostingUnit>(HostingUnitPath); // gets all units from the xml file

            var it = (from item in unitsList
                      where item.HostingUnitKey == unit.HostingUnitKey
                      select item).FirstOrDefault();

            if (it != null)
            {
                SaveToXML(unitsList, HostingUnitPath); // before you throw the exception- return the list to it's place in the file

                throw new DuplicatedKeyException("this unit already exists!");
            }

            unitsList.Add(unit.Clone());
            SaveToXML(unitsList, HostingUnitPath);

            ConfigRoot.Element("_HostingUnitKey").Value = (BE.Configuration._HostingUnitKey).ToString();
            ConfigRoot.Element("_HostKey").Value = (BE.Configuration._HostKey).ToString();
            ConfigRoot.Save(ConfigPath);
        }

        public void update_HostingUnit(HostingUnit updateUnit)
        {
            try
            {
                LoadData_HostingUnit_File();
            }
            catch (CanNotLoadFileException ex)
            {
                throw ex;
            }

            int count = 0;
            List<HostingUnit> unitsList = LoadFromXML<HostingUnit>(HostingUnitPath); // gets all units from the xml file

            for (int i = 0; i < unitsList.Count; i++)
            {
                if (unitsList[i].HostingUnitKey == updateUnit.HostingUnitKey)
                {
                    unitsList[i] = updateUnit.Clone();
                    count++;
                }
            }
            if (count == 0)
            {
                SaveToXML(unitsList, HostingUnitPath);

                throw new KeyDoesNotExistException("This Unit does not exist therefore you can not update it!");
            }

            SaveToXML(unitsList, HostingUnitPath);
        }

        public void delete_HostingUnit(HostingUnit unit)
        {
            try
            {
                LoadData_HostingUnit_File();
            }
            catch (CanNotLoadFileException ex)
            {
                throw ex;
            }

            int count = 0;
            List<HostingUnit> unitsList = LoadFromXML<HostingUnit>(HostingUnitPath); // gets all units from the xml file

            for (int i = 0; i < unitsList.Count; i++)
            {
                if (unitsList[i].HostingUnitKey == unit.HostingUnitKey)
                {
                    unitsList.RemoveAt(i);
                    count++;
                }
            }
            if (count == 0)
            {
                SaveToXML(unitsList, HostingUnitPath);

                throw new KeyDoesNotExistException("Unit does not exist therefore you can not delete it!");
            }

            SaveToXML(unitsList, HostingUnitPath);
        }

        public HostingUnit Get_HostingUnit(HostingUnit unit)
        {
            try
            {
                LoadData_HostingUnit_File();
            }
            catch (CanNotLoadFileException ex)
            {
                throw ex;
            }

            List<HostingUnit> unitsList = LoadFromXML<HostingUnit>(HostingUnitPath); // gets all units from the xml file

            var u = (from item in unitsList
                     where item.HostingUnitKey == unit.HostingUnitKey
                     select item.Clone()).ToList();

            SaveToXML(unitsList, HostingUnitPath);

            return u.FirstOrDefault();
        }

        public List<HostingUnit> get_All_HostingUnits(Func<HostingUnit, bool> predicat = null)
        {
            List<HostingUnit> unitsList = LoadFromXML<HostingUnit>(HostingUnitPath);
            SaveToXML(unitsList, HostingUnitPath);

            if (predicat == null)
            {
                return unitsList;
            }

            return (from item in unitsList
                    where predicat(item)
                    select item).ToList();
        }

        #endregion

        #region Bank Details
        public List<BankBranch> get_All_Bank_Branches(Func<BankBranch, bool> predicat = null)
        {
            if (BanKBranchRoot == null)
                throw new CanNotLoadFileException("list of banks is still loading, sorry for the wait");

            if (predicat == null)
            {
                return (from item in BanKBranchRoot.Elements()
                        select Convert_BanKBranchRoot(item).Clone()).ToList();
            }

            return (from item in BanKBranchRoot.Elements()
                    let bank = Convert_BanKBranchRoot(item).Clone()
                    where predicat(bank)
                    select bank).ToList();
        }
        #endregion

        #region clear orders- thread

        public void UpdateConfigFile()
        {
            ConfigRoot.Element("IsClearOrdrs").Value = false.ToString();//at the file the status remains "false" and at the program it updates
            ConfigRoot.Element("DateToday").Value = (DateTime.Parse(Configuration.DateToday)).AddDays(1).ToString(); //add 1 day at the file - next day after we updated the status


        }



        #endregion
    }
}


