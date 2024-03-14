using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OverloadControl.Models;
using OverloadControl.DataAccessor;
using Microsoft.EntityFrameworkCore;

namespace OverloadControl.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GatherController : ControllerBase
    {
        private static OverloadControl.DataAccessor.OCDbContext m_OCDbContext;

        public GatherController(OverloadControl.DataAccessor.OCDbContext oCDbContext)
        {
            m_OCDbContext = oCDbContext;
        }

        /// <summary>
        /// 保存案件申请单
        /// </summary>
        /// <param name="cases"></param>
        /// <returns></returns>

        [HttpPost("SaveCase")]
        public bool SaveCase([FromBody]Case cases)
        {
            if (cases != null)
            {
                cases.State = "未审核";
                m_OCDbContext.Cases.Add(cases);
                m_OCDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询法律类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetLawType()
        {
            var query = from o in m_OCDbContext.lawTypes
                        select o;
            return JsonConvert.SerializeObject(query);
        }

        /// <summary>
        /// 提交案件申请
        /// </summary>
        /// <param name="cases"></param>
        /// <returns></returns>
        [HttpPost]
        public string UploadFile([FromBody] Case cases)
        {
            if (this.ModelState.IsValid)
            {
                var fileRequest = Request.Form.Files[0];
                if (fileRequest.Length > 0)
                {
                    using (var stream = fileRequest.OpenReadStream())
                    {
                        int len = 0;
                        var buffer = new byte[1024];

                        var fs = new System.IO.FileInfo(fileRequest.FileName);

                        var now = DateTime.Now;
                        var dir = @"D:\file";

                        var filePath = System.IO.Path.Combine(dir, $"{now.Ticks}{fs.Extension}");
                        cases.Image = $"{now.Ticks}{fs.Extension}";
                        if (SaveCase(cases))
                        {
                            if (!System.IO.Directory.Exists(dir))
                            {
                                System.IO.Directory.CreateDirectory(dir);
                            }

                            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
                            {
                                while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fileStream.Write(buffer, 0, len);
                                }
                            }
                            return "ok";
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            return "";
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
        /// 查询案件进度
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>

        [HttpGet]
        public string GetProgress(string caseId)
        {
            var query = from o in m_OCDbContext.Case_Progresses
                        join p in m_OCDbContext.Progresses on o.ProgressId equals p.Id
                        where o.CaseId == int.Parse(caseId)
                        select new
                        {
                            progressName = p.ProgressName,
                            time = o.Time,
                            content = o._Content
                        };
            var progressList = JsonConvert.SerializeObject(query);
            return progressList;
        }

        /// <summary>
        /// 保存案件进度关系
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="progressId"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        [HttpPost]
        private bool AddCaseProgress(int caseId, int progressId, string content)
        {
            var existingProgress = m_OCDbContext.Case_Progresses.FirstOrDefault(cp => cp.CaseId == caseId && cp.ProgressId == progressId);

            if (existingProgress != null)
            {
                // Update existing progress
                existingProgress.Time = DateTime.Now.ToString();
                existingProgress._Content = content;
            }
            else
            {
                // Add new progress
                Case_Progress caseProgress = new Case_Progress();
                caseProgress.CaseId = caseId;
                caseProgress.ProgressId = progressId;
                caseProgress.Time = DateTime.Now.ToString();
                caseProgress._Content = content;
                m_OCDbContext.Case_Progresses.Add(caseProgress);
            }

            m_OCDbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 添加信息采集人员与案件的关系
        /// </summary>
        /// <param name="policeId"></param>
        /// <param name="caseNo"></param>
        /// <returns></returns>

        [HttpPost]
        public bool AddRelationShip(string policeId, string caseNo)
        {
            if (policeId != null && caseNo != null)
            {
                Police_Case item = new Police_Case();
                item.PoliceId = int.Parse(policeId);
                var cases = m_OCDbContext.Cases.Where(c => c.CaseNo == caseNo).FirstOrDefault();
                if (cases != null)
                {
                    item.CaseId = cases.Id;
                    if (!AddCaseProgress(item.CaseId, 1, cases.Details))
                    {
                        return false;
                    }
                    m_OCDbContext.Police_Cases.Add(item);
                    m_OCDbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据案件状态查询案件
        /// </summary>
        /// <param name="PreId"></param>
        /// <returns></returns>

        [HttpGet]
        public string GetCaseByProgress(int PreId)
        {
            var cases = from o in m_OCDbContext.Case_Progresses
                        join b in m_OCDbContext.Cases on o.CaseId equals b.Id
                        where o.ProgressId == PreId
                        select b;
            return JsonConvert.SerializeObject(cases);
        }

        /// <summary>
        /// 撤销案件
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        [HttpPut]
        public bool CancelCase(string caseId, string content)
        {
            var item = m_OCDbContext.Cases.Where(c => c.Id == int.Parse(caseId)).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
          ;
            if (AddCaseProgress(item.Id, 9, content))
            {
                item.State = "已撤销";
                item.Content = content;
                m_OCDbContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 根据PoliceId，CaseId和ProgressId查询Case
        /// </summary>
        /// <param name="policeId"></param>
        /// <param name="caseId"></param>
        /// <param name="progressId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Case>> GetCaseByCriteria(int policeId, int caseId, int progressId)
        {
            var policeCase = await m_OCDbContext.Police_Cases
                .FirstOrDefaultAsync(pc => pc.PoliceId == policeId && pc.CaseId == caseId);

            if (policeCase == null)
            {
                return NotFound("Police does not have access to this case.");
            }

            var caseProgress = await m_OCDbContext.Case_Progresses
                .FirstOrDefaultAsync(cp => cp.CaseId == caseId && cp.ProgressId == progressId);

            if (caseProgress == null)
            {
                return NotFound("Case progress not found.");
            }

            var requestedCase = await m_OCDbContext.Cases
                .Include(c => c.Case_Progresses)
                .ThenInclude(cp => cp.Progress)
                .FirstOrDefaultAsync(c => c.Id == caseId);

            if (requestedCase == null)
            {
                return NotFound("Case not found.");
            }

            return requestedCase;
        }
    }
}