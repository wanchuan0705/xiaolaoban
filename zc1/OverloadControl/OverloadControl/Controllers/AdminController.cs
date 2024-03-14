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
        /// 更新工作人员信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedPolice"></param>
        /// <returns></returns>
        [HttpPut]
        public string PutPolice([FromBody] Police updatedPolice)
        {
            using (var context = new OCDbContext())
            {
                if (updatedPolice != null)
                {
                    var existingPolice = GetPoliceById(updatedPolice.Id);
                    if (existingPolice != null)
                    {
                        // 根据传入的 updatedPolice 对象更新现有 Police 对象的信息
                        existingPolice.Account = updatedPolice.Account;
                        existingPolice.Password = updatedPolice.Password;
                        existingPolice.Phone = updatedPolice.Phone;
                        existingPolice.Name = updatedPolice.Name;
                        existingPolice.Age = updatedPolice.Age;
                        existingPolice.Sex = updatedPolice.Sex;

                        // 保存对 Police 对象的更新操作，这里可以是将更新后的信息保存至数据库或内存中
                        // 例如：context.SaveChanges() 或者更新内存中的数据集
                        context.SaveChanges();
                        return "Police 信息更新成功！";
                    }
                    return "未找到对应的 Police 对象，请检查输入的 Id。";
                }
                return "传入的更新对象为空，请检查传入的参数。";
            }
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
        /// 添加案件-进度关系
        /// </summary>
        /// <param name = "bxOrderId" ></ param >
        /// < param name="progressId"></param>
        /// <param name = "content" ></ param >
        /// < returns ></ returns >
        [HttpPost]
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
        public bool AddMaintainer(string caseId, string content, string police, string index)
        {
            //查询工作人员的id

            var query = (from o in m_AdminContext.Police_Roles
                         join r in m_AdminContext.Roles on o.RoleId equals r.Id
                         join u in m_AdminContext.Polices on o.PoliceId equals u.Id
                         where u.Name == police && r.Id == 2
                         select u.Id).FirstOrDefault();
            //查询案件单
            var cases = m_AdminContext.Cases.Where(c => c.Id == int.Parse(caseId)).FirstOrDefault();

            if (cases != null)
            {
                if (query != 0 && cases != null && int.Parse(index) == 1)
                {
                    //安排工作人员
                    Police_Case police_Case = new Police_Case();
                    police_Case.PoliceId = query;
                    police_Case.CaseId = cases.Id;
                    m_AdminContext.Police_Cases.Add(police_Case);

                    //修改案件单状态,添加案件进度
                    cases.State = "已处理";
                    AddCaseProgress(cases.Id, 4, content);
                }

                if (query != 0 && cases != null && int.Parse(index) == 2)
                {
                    //修改案件状态,添加案件进度
                    cases.State = "已驳回";
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
            if (AddCaseProgress(item.Id, 6, content))
            {
                item.State = "已结案";
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

        #endregion 案件管理

        #region 法律管理

        /// <summary>
        /// 增加法律条文
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddLaw(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                Law law = new Law()
                {
                    Content = content
                };
                m_AdminContext.Laws.Add(law);
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
        /// 添加法律
        /// </summary>
        /// <param name="lawTypeId"></param>
        /// <param name="law"></param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult AddLawByLawType(int lawTypeId, [FromBody] Law law)
        {
            // 首先检查指定的 LawType 是否存在
            LawType lawType = m_AdminContext.lawTypes.FirstOrDefault(lt => lt.Id == lawTypeId);
            if (lawType == null)
            {
                return NotFound(); // 返回 404 Not Found
            }

            // 创建 Law 对象并设置相关属性
            Law newLaw = new Law()
            {
                Content = law.Content,
                law_Cases = law.law_Cases,
                law_LawTypes = new List<Law_LawType>()
        {
            new Law_LawType()
            {
                lawType = lawType
            }
        }
            };

            // 保存 Law 对象到数据库
            m_AdminContext.Laws.Add(newLaw);
            m_AdminContext.SaveChanges();

            // 返回成功添加的 Law 对象
            return CreatedAtAction(nameof(GetLawById), new { id = newLaw.Id }, newLaw);
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

        [HttpPut]
        public async Task<IActionResult> PutLaw(Law law)
        {
            //if (id != law.Id)
            //{
            //    return BadRequest();
            //}

            m_AdminContext.Entry(law).State = EntityState.Modified;

            try
            {
                await m_AdminContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LawExists(law.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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