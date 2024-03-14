using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OverloadControl.Models;

namespace OverloadControl2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private static OverloadControl.DataAccessor.OCDbContext m_AdminContext;
        public AdminController(OverloadControl.DataAccessor.OCDbContext oCDbContext)
        {
            m_AdminContext = oCDbContext;
        }



        /// <summary>
        /// 登录
        /// </summary>
        /// <param name = "account" ></ param >
        /// < param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public string Login(string account, string password)
        {
            var item = from o in m_AdminContext.Polices
                       join pr in m_AdminContext.Police_Roles on o.Id equals pr.PoliceId
                       join r in m_AdminContext.Roles on pr.RoleId equals r.Id
                       where o.Account == account && o.Password == password
                       select new
                       {
                           userId = o.Id,
                           account = o.Account,
                           password = o.Password,
                           roleId = r.Id

                       };
            return JsonConvert.SerializeObject(item);
        }
        /// <summary>
        /// 添加信息采集人员
        /// </summary>
        /// <param name = "account" ></ param >
        /// < returns ></ returns >
        [HttpGet]
        public bool AddUserRole(string account)
        {
            if (account != null)
            {
                var item = m_AdminContext.Polices.Where(c => c.Account == account).FirstOrDefault();
                Police_Role policeRole = new Police_Role();
                policeRole.PoliceId = item.Id;
                policeRole.RoleId = 2;
                m_AdminContext.Police_Roles.Add(policeRole);
                m_AdminContext.SaveChanges();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 查看案件
        /// </summary>
        /// <param name = "PoliceId" ></ param >
        /// < returns ></ returns >
        [HttpGet]
        public string GetCase(string PoliceId)
        {
            var item = from uo in m_AdminContext.Police_Cases
                       join ur in m_AdminContext.Police_Roles on uo.PoliceId equals ur.PoliceId
                       join o in m_AdminContext.Cases on uo.CaseId equals o.Id
                       where ur.RoleId == 2 && ur.PoliceId == int.Parse(PoliceId)
                       select o;
            string cases = JsonConvert.SerializeObject(item);
            return cases;
        }


        /// <summary>
        /// 查询所有案件
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public string GetCases()
        {
            var casesList = m_AdminContext.Cases;
            string str = JsonConvert.SerializeObject(casesList);
            return str;
        }
        /// <summary>
        /// 查询所有工作人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetUser()
        {
            var Police = m_AdminContext.Polices;
            string str = JsonConvert.SerializeObject(Police);
            return str;
        }


        /// <summary>
        /// 添加后台案件处理人员
        /// </summary>
        /// <param name = "account" ></ param >
        /// < param name="name"></param>
        /// <param name = "age" ></ param >
        /// < param name="phone"></param>
        /// <param name = "gender" ></ param >
        /// < param name="category"></param>
        /// <returns></returns>


        [HttpGet]
        public bool SetBackend(string account, string name, int age, string phone, string sex, string category)
        {
            Police police = new Police();
            police.Account = account;
            police.Password = (123456).ToString();
            police.Name = name;
            police.Age = age;
            police.Phone = phone;
            police.Sex = sex;
            m_AdminContext.Polices.Add(police);
            m_AdminContext.SaveChanges();

            var user1 = m_AdminContext.Polices.Where(c => c.Account == account).FirstOrDefault();
            if (user1 == null)
            {
                return false;
            }
            Police_Role policeRole = new Police_Role();
            policeRole.PoliceId = user1.Id;
            policeRole.RoleId = 3;
            m_AdminContext.Police_Roles.Add(policeRole);

            var item = m_AdminContext.Categories.Where(c => c.Name == category).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            Police_Category policeCategory = new Police_Category();
            policeCategory.PoliceId = user1.Id;
            policeCategory.CategoryId = item.Id;
            m_AdminContext.police_Categories.Add(policeCategory);
            m_AdminContext.SaveChanges();

            return true;
        }
        /// <summary>
        /// 添加案件-进度关系
        /// </summary>
        /// <param name = "bxOrderId" ></ param >
        /// < param name="progressId"></param>
        /// <param name = "content" ></ param >
        /// < returns ></ returns >
        [HttpGet]
        public bool AddCaseProgress(int caseId, int progressId, string content)
        {
            var item = m_AdminContext.Case_Progresses.Where(c => c.CaseId == caseId && c.ProgressId == progressId).FirstOrDefault();
            if (item != null)
            {
                return false;
            }
            Case_Progress case_Progress = new Case_Progress();
            case_Progress.CaseId = caseId;
            case_Progress.ProgressId = progressId;
            case_Progress.Time = DateTime.Now.ToString();
            case_Progress._Content = content;
            m_AdminContext.Case_Progresses.Add(case_Progress);
            m_AdminContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 查询所有后台案件处理人员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetMaintainerInfo()
        {
            var query = from o in m_AdminContext.Polices
                        join ur in m_AdminContext.Police_Roles on o.Id equals ur.PoliceId
                        where ur.RoleId == 3
                        select o;
            return JsonConvert.SerializeObject(query);
        }
    }
}
