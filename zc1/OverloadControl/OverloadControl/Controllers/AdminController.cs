using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OverloadControl.DataAccessor;
using OverloadControl.Models;

namespace OverloadControl.Controllers
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

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var item = m_AdminContext.Polices
                .Where(o => o.Account == username && o.Password == password)
                .Join(m_AdminContext.Police_Roles, o => o.Id, pr => pr.PoliceId, (o, pr) => new { o, pr })
                .Join(m_AdminContext.Roles, r => r.pr.RoleId, r => r.Id, (r, pr) => new
                {
                    userId = r.o.Id,
                    account = r.o.Account,
                    password = r.o.Password,
                    roleId = r.pr.RoleId
                })
                .FirstOrDefault();

            if (item == null)
            {
                return NotFound("账号或密码不正确");
            }

            return Ok(JsonConvert.SerializeObject(item));
        }

        #region 人员管理

        /// <summary>
        /// 添加后台案件处理人员
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>

        [HttpPost]
        public bool AddUserRole1(string account, string password, string phone, string name, int age, string sex)
        {
            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(name))
            {
                // 创建新的 Police 对象并设置属性
                Police newPolice = new Police
                {
                    Account = account,
                    Password = password,
                    Phone = phone,
                    Name = name,
                    Age = age,
                    Sex = sex
                };

                // 添加新用户信息到 Police 数据表中
                m_AdminContext.Polices.Add(newPolice);
                m_AdminContext.SaveChanges();

                // 获取刚刚添加的用户信息的 ID
                int newPoliceId = newPolice.Id;

                // 创建新的 Police_Role 对象并设置属性
                Police_Role policeRole = new Police_Role
                {
                    PoliceId = newPoliceId,
                    RoleId = 2
                };

                Police_Category police_Category = new Police_Category
                {
                    PoliceId = newPoliceId,
                    CategoryId = 2
                };

                // 将用户角色信息添加到 Police_Roles 数据表中
                m_AdminContext.Police_Roles.Add(policeRole);
                m_AdminContext.police_Categories.Add(police_Category);
                m_AdminContext.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// 添加信息采集处理人员
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>

        [HttpPost]
        public bool AddUserRole2(string account, string password, string phone, string name, int age, string sex)
        {
            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(name))
            {
                // 创建新的 Police 对象并设置属性
                Police newPolice = new Police
                {
                    Account = account,
                    Password = password,
                    Phone = phone,
                    Name = name,
                    Age = age,
                    Sex = sex
                };

                // 添加新用户信息到 Police 数据表中
                m_AdminContext.Polices.Add(newPolice);
                m_AdminContext.SaveChanges();

                // 获取刚刚添加的用户信息的 ID
                int newPoliceId = newPolice.Id;

                // 创建新的 Police_Role 对象并设置属性
                Police_Role policeRole = new Police_Role
                {
                    PoliceId = newPoliceId,
                    RoleId = 2
                };

                Police_Category police_Category = new Police_Category
                {
                    PoliceId = newPoliceId,
                    CategoryId = 1
                };

                // 将用户角色信息添加到 Police_Roles 数据表中
                m_AdminContext.Police_Roles.Add(policeRole);
                m_AdminContext.police_Categories.Add(police_Category);
                m_AdminContext.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// 通过Id查询工作人员
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        public Police GetPoliceById(int Id)
        {
            using (var context = new OCDbContext())
            {
                var query = from o in context.Polices
                            where o.Id == Id
                            select o;
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 修改工作人员信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="age"></param>
        /// <returns></returns>

        [HttpPut("UpdatePolice/{id}")]
        public IActionResult UpdatePolice(int id, [FromBody] string name, string sex, string password, string phone, int age)
        {
            var police = m_AdminContext.Polices.FirstOrDefault(l => l.Id == id);
            if (police == null)
            {
                return NotFound();
            }
            police.Age = age;
            police.Sex = sex;
            police.Name = name;
            police.Password = password;
            police.Phone = phone;
            m_AdminContext.Entry(police).State = EntityState.Modified;
            m_AdminContext.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// 查询所有工作人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetPolice()
        {
            var Police = m_AdminContext.Polices.ToList();
            string str = JsonConvert.SerializeObject(Police);
            return str;
        }

        /// <summary>
        /// 查询所有后台案件处理人员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBackendInfo()
        {
            var query = from o in m_AdminContext.Polices
                        join cp in m_AdminContext.police_Categories on o.Id equals cp.PoliceId

                        where cp.CategoryId == 2
                        select o;
            return JsonConvert.SerializeObject(query);
        }

        /// <summary>
        /// 查询所有工作人员类别
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public string GetPoliceType()
        {
            var query = from o in m_AdminContext.police_Categories
                        join u in m_AdminContext.Polices on o.PoliceId equals u.Id
                        join c in m_AdminContext.Categories on o.CategoryId equals c.Id
                        select new
                        {
                            category = c.Name,
                            maintainer = u.Name
                        };
            var str = JsonConvert.SerializeObject(query);
            return str;
        }

        /// <summary>
        /// 注销后台案件处理人员账号
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        [HttpDelete]
        public bool DeletPolice(int id)
        {
            var item = (from o in m_AdminContext.Polices.Include(c => c.Police_Cases).Include(c => c.police_Categories).Include(c => c.Police_Roles)
                        where o.Id == id
                        select o).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            m_AdminContext.Remove(item);
            m_AdminContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 查询所有工作人员类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetCategory()
        {
            var category = from o in m_AdminContext.Categories
                           select new
                           {
                               categoryName = o.Name
                           };
            string str = JsonConvert.SerializeObject(category);
            return str;
        }

        /// <summary>
        /// 查询所有案件处理人员名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBackend()
        {
            var query = from o in m_AdminContext.Polices
                        join pr in m_AdminContext.Police_Roles on o.Id equals pr.PoliceId
                        join cp in m_AdminContext.police_Categories on o.Id equals cp.Id
                        where pr.RoleId == 2 && cp.CategoryId == 2
                        select o.Name;
            return JsonConvert.SerializeObject(query);
        }

        #endregion 人员管理

        #region 案件管理

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
        /// 根据案件状态查询案件
        /// </summary>
        /// <param name="PreId"></param>
        /// <returns></returns>

        [HttpGet]
        public string GetCaseByProgress(int PreId)
        {
            var cases = from o in m_AdminContext.Case_Progresses
                        join b in m_AdminContext.Cases on o.CaseId equals b.Id
                        where o.ProgressId == PreId && o.HistoryState == 0
                        select b;
            return JsonConvert.SerializeObject(cases);
        }

        /// <summary>
        /// 添加案件-进度关系
        /// </summary>
        /// <param name = "bxOrderId" ></ param >
        /// < param name="progressId"></param>
        /// <param name = "content" ></ param >
        /// < returns ></ returns >
        [HttpPost]
        public bool AddCaseProgress(int caseId, int progressId, string content)
        {
            var existingProgressList = m_AdminContext.Case_Progresses.Where(cp => cp.CaseId == caseId && cp.HistoryState == 0).ToList();

            if (existingProgressList.Any())
            {
                foreach (var item in existingProgressList)
                {
                    item.HistoryState = 1;//改为历史状态
                }
            }
            // Add new progress
            Case_Progress caseProgress = new Case_Progress();
            caseProgress.CaseId = caseId;
            caseProgress.ProgressId = progressId;
            caseProgress.Time = DateTime.Now.ToString();
            caseProgress._Content = content;
            caseProgress.HistoryState = 0;
            m_AdminContext.Case_Progresses.Add(caseProgress);
            m_AdminContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 查看所有案件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetCaseInfo()
        {
            var query = from o in m_AdminContext.Cases
                        select o;
            return JsonConvert.SerializeObject(query);
        }

        /// <summary>
        /// 安排后台案件处理人员处理案件
        /// </summary>
        /// <param name = "bxOrderId" ></ param >
        /// < param name="content"></param>
        /// <param name = "maintainer" ></ param >
        /// < param name="index"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddPoliceCase(int caseId, string content, int policeId)
        {
            //查询案件单
            var cases = m_AdminContext.Cases.Where(c => c.Id == caseId).FirstOrDefault();
            var police = m_AdminContext.Polices.FirstOrDefault(c => c.Id == policeId);

            if (cases != null)
            {
                if (policeId != 0 && cases != null)
                {
                    //安排工作人员
                    Police_Case police_Case = new Police_Case();
                    police_Case.PoliceId = policeId;
                    police_Case.CaseId = cases.Id;
                    m_AdminContext.Police_Cases.Add(police_Case);

                    //修改案件单状态,添加案件进度
                    cases.State = "已受理";
                    cases.OrderTake = "已受理";
                    cases.PolicerName1 = police?.Name;
                    AddCaseProgress(cases.Id, 7, content);
                }

                m_AdminContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 结案
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        [HttpPut]
        public bool handleCase(string caseId, string content)
        {
            var item = m_AdminContext.Cases.Where(c => c.Id == int.Parse(caseId)).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
    ;
            if (AddCaseProgress(item.Id, 5, content))
            {
                item.State = "已结案";
                item.OrderTake = "已结案";
                item.Content = content;
                m_AdminContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 结案不通过
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        [HttpPut]
        public bool handleCase1(string caseId, string content)
        {
            var item = m_AdminContext.Cases.Where(c => c.Id == int.Parse(caseId)).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
         ;
            if (AddCaseProgress(item.Id, 3, content))
            {
                item.State = "处理中";
                item.OrderTake = "处理中";
                item.Content = content;
                m_AdminContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 统计案件处理情况
        /// </summary>
        /// <param name="backendName"></param>
        /// <param name="month"></param>
        /// <returns></returns>

        [HttpGet]
        public int[] TaskNum(string backendName, int month)
        {
            var query = (from o in m_AdminContext.Police_Cases
                         join p in m_AdminContext.Polices on o.PoliceId equals p.Id
                         join pr in m_AdminContext.Police_Roles on o.PoliceId equals pr.PoliceId
                         join cp in m_AdminContext.Case_Progresses on o.CaseId equals cp.CaseId
                         where p.Name == backendName && pr.RoleId == 2 && cp.ProgressId == 6
                         select new
                         {
                             date = cp.Time
                         }).ToList();
            var list = query.Where(c => Convert.ToDateTime(c.date).Month == month).ToList();
            var list1 = query.Where(c => Convert.ToDateTime(c.date).Month == (month - 1)).ToList();
            var taskNum = list.Count();
            var taskNum1 = list1.Count();
            int[] array = { taskNum, taskNum1 };
            return array;
        }

        /// <summary>
        /// 案件审核通过
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        [HttpPut]
        public bool ReviewRases(string caseId, int lawTypeId, string content)
        {
            var item = m_AdminContext.Cases.Where(c => c.Id == int.Parse(caseId)).FirstOrDefault();
            var lawType = m_AdminContext.lawTypes.Where(c => c.Id == lawTypeId).FirstOrDefault();
            if (item == null)
            {
                return false;
            }

            if (AddCaseProgress(item.Id, 2, content))
            {
                item.State = "已审核";
                item.OrderTake = "已审核";
                item.Content = content;
                item.Types = lawType?.Name;
                item.LawTypeId = lawTypeId;
                m_AdminContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 案件审核不通过
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        [HttpPut]
        public bool ReviewRases1(string caseId, string content)
        {
            var item = m_AdminContext.Cases.Where(c => c.Id == int.Parse(caseId)).FirstOrDefault();

            if (item == null)
            {
                return false;
            }

            if (AddCaseProgress(item.Id, 8, content))
            {
                item.State = "已驳回";
                item.OrderTake = "已驳回";
                item.Content = content;

                m_AdminContext.SaveChanges();
            }
            return true;
        }

        #endregion 案件管理

        #region 法律管理

        /// <summary>
        /// 根据法律类型来添加法律条文
        /// </summary>
        /// <param name="LawTypeId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddLaw1(int LawTypeId, string content)
        {
            // 首先检查指定的 LawType 是否存在
            LawType lawType = m_AdminContext.lawTypes.FirstOrDefault(lt => lt.Id == LawTypeId);
            if (lawType == null)
            {
                return NotFound(); // 返回 404 Not Found
            }

            // 创建 Law 对象并设置相关属性
            Law law = new Law()
            {
                Content = content
            };

            // 将 Law 对象保存到数据库
            m_AdminContext.Laws.Add(law);
            m_AdminContext.SaveChanges();

            // 创建 Law_LawType 对象并设置相关属性
            Law_LawType law_LawType = new Law_LawType()
            {
                LawTypeId = LawTypeId,
                LawId = law.Id
            };

            // 将 Law_LawType 对象保存到数据库
            m_AdminContext.law_LawTypes.Add(law_LawType);
            m_AdminContext.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// 增加法律类型
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>

        [HttpPost]
        public bool AddLawType(string Name)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                LawType lawType = new LawType()
                {
                    Name = Name
                };
                m_AdminContext.lawTypes.Add(lawType);
                m_AdminContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 查看所有法律条文
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetLaws()
        {
            var query = from o in m_AdminContext.Laws
                        select o;
            return JsonConvert.SerializeObject(query);
        }

        /// <summary>
        /// 查看所有法律条文类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetLawType()
        {
            var query = from o in m_AdminContext.lawTypes
                        select o;
            return JsonConvert.SerializeObject(query);
        }

        /// <summary>
        /// 通过Id查找法律
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public IActionResult GetLawById(int id)
        {
            // 查询指定 id 的法律
            Law law = m_AdminContext.Laws.FirstOrDefault(l => l.Id == id);

            // 如果找不到对应的法律，则返回 404 Not Found
            if (law == null)
            {
                return NotFound();
            }

            // 找到了法律，则返回该法律对象
            return Ok(law);
        }

        /// <summary>
        /// 修改法律
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newContent"></param>
        /// <returns></returns>

        [HttpPut]
        public IActionResult UpdateLawContent(int id, string newContent)
        {
            // 查找要修改的 Law 对象
            var law = m_AdminContext.Laws.FirstOrDefault(l => l.Id == id);
            if (law == null)
            {
                return NotFound(); // 如果找不到对应的 Law 对象，则返回 404 Not Found
            }

            // 更新 Law 对象的 Content 属性
            law.Content = newContent;

            // 将实体标记为已修改状态
            m_AdminContext.Entry(law).State = EntityState.Modified;

            // 保存更改到数据库
            m_AdminContext.SaveChanges();

            return NoContent(); // 返回 204 No Content 表示成功更新
        }

        /// <summary>
        /// 修改法律类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedLawType"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public IActionResult UpdateLawType(int id, [FromBody] string updatedLawType)
        {
            // 查找要修改的 LawType 对象
            var lawType = m_AdminContext.lawTypes.FirstOrDefault(lt => lt.Id == id);
            if (lawType == null)
            {
                return NotFound(); // 如果找不到对应的 LawType 对象，则返回 404 Not Found
            }

            lawType.Name = updatedLawType;
            m_AdminContext.Entry(lawType).State = EntityState.Modified;

            // 保存更改到数据库
            m_AdminContext.SaveChanges();

            return Ok(lawType); // 返回修改后的 LawType 对象
        }

        /// <summary>
        /// 通过Id查找法律
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool LawExists(int id)
        {
            return m_AdminContext.Laws.Any(e => e.Id == id);
        }

        #endregion 法律管理
    }
}