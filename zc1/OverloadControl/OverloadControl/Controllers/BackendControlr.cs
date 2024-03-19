using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OverloadControl.Models;

namespace OverloadControl.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BackendControlr : ControllerBase
    {
        private static OverloadControl.DataAccessor.OCDbContext m_OCDbContext;

        public BackendControlr(OverloadControl.DataAccessor.OCDbContext oCDbContext)
        {
            m_OCDbContext = oCDbContext;
        }

        /// <summary>
        /// 查询对应案件处理人员的案件单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        [HttpGet]
        public string GetCase(int PoliceId)
        {
            var cases = from o in m_OCDbContext.Police_Cases
                        join b in m_OCDbContext.Cases on o.CaseId equals b.Id
                        where o.PoliceId == PoliceId
                        select b;
            return JsonConvert.SerializeObject(cases);
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <returns></returns>

        [HttpPut("UpdatePolice/{id}")]
        public IActionResult UpdatePolice(int id, [FromBody] string name, string password, string phone)
        {
            var police = m_OCDbContext.Polices.FirstOrDefault(l => l.Id == id);
            if (police == null)
            {
                return NotFound();
            }

            police.Name = name;
            police.Password = password;
            police.Phone = phone;
            m_OCDbContext.Entry(police).State = EntityState.Modified;
            m_OCDbContext.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// 查询案件对应的案件处理人员
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Police> GetPoliceForCase(int caseId)
        {
            var policeForCase = m_OCDbContext.Polices
                .Include(p => p.Police_Cases)
                .ThenInclude(pc => pc.Case)
                .Where(p => p.Police_Cases.Any(pc => pc.CaseId == caseId))
                .FirstOrDefault();

            if (policeForCase == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                Name = policeForCase.Name,
                Phone = policeForCase.Phone
            });
        }

        /// <summary>
        /// 处理案件
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPut]
        public bool handleCase(string caseId, string lawId, string content, string content1)
        {
            var item = m_OCDbContext.Cases.Where(c => c.Id == int.Parse(caseId)).FirstOrDefault();
            var law = m_OCDbContext.Laws.Where(c => c.Id == int.Parse(lawId)).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            if (AddCaseProgress(item.Id, 5, content) && AddCaseLaw(int.Parse(caseId), int.Parse(lawId)))
            {
                item.State = "已完成";
                item.LegalArticles = content1;
                item.OrderTake = "已完成";
                item.Judgment = content;
                item.LegalArticles = law?.Content;
                m_OCDbContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 根据法律类型查找法律
        /// </summary>
        /// <param name="lawtypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetLaw(int lawtypeId)
        {
            var laws = from o in m_OCDbContext.law_LawTypes
                       join b in m_OCDbContext.Laws on o.LawId equals b.Id
                       where o.LawTypeId == lawtypeId
                       select b;
            return JsonConvert.SerializeObject(laws);
        }

        /// <summary>
        /// 添加案件跟进度表的关系
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="progressId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddCaseProgress(int caseId, int progressId, string content)
        {
            var existingProgressList = m_OCDbContext.Case_Progresses.Where(cp => cp.CaseId == caseId && cp.HistoryState == 0).ToList();

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
            m_OCDbContext.Case_Progresses.Add(caseProgress);
            m_OCDbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 添加案件与法律的关系
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="lawId"></param>
        /// <returns></returns>

        [HttpPost]
        public bool AddCaseLaw(int caseId, int lawId)
        {
            var item = m_OCDbContext.law_Cases.Where(c => c.CaseId == caseId && c.LawId == lawId).FirstOrDefault();
            if (item != null)
            {
                return false;
            }
            Law_Case law_Case = new Law_Case();
            law_Case.CaseId = caseId;
            law_Case.LawId = lawId;
            m_OCDbContext.law_Cases.Add(law_Case);
            m_OCDbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 立案
        /// </summary>
        /// <param name="caseId"></param>

        /// <param name="m_Content"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SaveOrderTake(int caseId, string m_Content)
        {
            var cases = m_OCDbContext.Cases.Where(c => c.Id == caseId).FirstOrDefault();
            if (cases == null)
            {
                return false;
            }

            if (AddCaseProgress(cases.Id, 3, m_Content))
            {
                cases.State = "处理中";
                cases.OrderTake = "处理中";
                cases.Content = m_Content;
                m_OCDbContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 不立案
        /// </summary>
        /// <param name="caseId"></param>

        /// <param name="m_Content"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SaveOrderTake11(int caseId, string m_Content)
        {
            var cases = m_OCDbContext.Cases.Where(c => c.Id == caseId).FirstOrDefault();
            if (cases == null)
            {
                return false;
            }

            if (AddCaseProgress(cases.Id, 9, m_Content))
            {
                cases.State = "不立案";
                cases.OrderTake = "不立案";
                cases.Content = m_Content;
                m_OCDbContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 回退案件
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="m_Content"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SaveOrderTake12(int caseId, string m_Content)
        {
            var cases = m_OCDbContext.Cases.Where(c => c.Id == caseId).FirstOrDefault();
            if (cases == null)
            {
                return false;
            }

            if (AddCaseProgress(cases.Id, 3, m_Content))
            {
                cases.State = "处理中";
                cases.OrderTake = "处理中";
                cases.Content = m_Content;
                m_OCDbContext.SaveChanges();
            }
            return true;
        }
    }
}