using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OverloadControl.Models;
using OverloadControl.DataAccessor;
using Microsoft.EntityFrameworkCore;
using OverloadControl.Models.Dto;

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
        /// 查看个人信息
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

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <returns></returns>

        [HttpPut]
        public IActionResult UpdatePolice(int id, string name, string password, string phone)
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
        /// 案件提交
        /// </summary>
        /// <param name="policeId"></param>
        /// <param name="policeName"></param>
        /// <param name="caseNo"></param>
        /// <param name="violatorsName"></param>
        /// <param name="phone"></param>
        /// <param name="address"></param>
        /// <param name="details"></param>
        /// <param name="platenumber"></param>
        /// <param name="date"></param>
        /// <param name="image"></param>
        /// <param name="description"></param>
        /// <param name="model"></param>
        /// <param name="content"></param>
        /// <param name="violatorsPhone"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddCase([FromBody] CaseData caseData)
        {
            if (caseData.CaseNo != 0)
            {
                Case casess = new Case();
                {
                    casess.CaseNo = caseData.CaseNo;
                    casess.ViolatorsName = caseData.ViolatorsName;
                    casess.Address = caseData.Address;
                    casess.Details = caseData.Details;
                    casess.Platenumber = caseData.PlateNumber;
                    casess.Date = caseData.Date;
                    casess.Phone = caseData.Phone;
                    casess.Image = caseData.Image;
                    casess.Description = caseData.Description;
                    casess.Model = caseData.Model;
                    casess.Content = caseData.Content;
                    casess.ViolatorsPhone = caseData.ViolatorsPhone;
                    casess.ApplicationTime = DateTime.Now;
                };

                m_OCDbContext.Cases.Add(casess);
                m_OCDbContext.SaveChanges();
                Police_Case police_Case = new Police_Case();
                {
                    police_Case.PoliceId = caseData.PoliceId;
                    police_Case.CaseId = casess.Id;
                    casess.State = "未审核";
                    casess.PolicerName = caseData.PoliceName;
                }

                Case_Progress case_Progresses = new Case_Progress();
                {
                    case_Progresses.CaseId = casess.Id;
                    case_Progresses.ProgressId = 1;
                    case_Progresses.HistoryState = 1;
                }
                m_OCDbContext.Police_Cases.Add(police_Case);
                m_OCDbContext.Case_Progresses.Add(case_Progresses);
                m_OCDbContext.SaveChanges();
            }
            return true;
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
        public async Task<ActionResult<List<Case>>> GetCaseByCriteria(int policeId, int? caseId, int? progressId)
        {
            var policeCaseList = await m_OCDbContext.Police_Cases
                .Where(pc => pc.PoliceId == policeId).ToListAsync();

            if (!policeCaseList.Any())
            {
                return NotFound("Police does not have access to any cases.");
            }

            List<int> caseIdList = policeCaseList.Select(o => o.CaseId).ToList();
            var a = caseIdList.FirstOrDefault();
            var caseProgressList = m_OCDbContext.Case_Progresses.ToList().Where(cp => caseIdList.Contains(cp.CaseId) && cp.HistoryState == 0).ToList();

            if (progressId.HasValue)
            {
                caseProgressList = caseProgressList.Where(o => o.ProgressId == progressId).ToList();
            }

            List<int> filteredCaseIdList = caseProgressList.Select(o => o.CaseId).ToList();
            var returnCaseList = m_OCDbContext.Cases.ToList().Where(c => caseId.HasValue ? c.Id == caseId : filteredCaseIdList.Contains(c.Id)).ToList();

            if (!returnCaseList.Any())
            {
                return NotFound("Cases not found.");
            }

            return returnCaseList;
        }

        #region 图片上传与显示

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string LOADIMAGE()
        {
            string filePath = null;
            var files = Request.Form.Files;

            // 获取桌面路径
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // 拼接文件保存路径
                    filePath = Path.Combine(desktopPath, "image", file.FileName);

                    // 创建文件夹
                    string directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // 将文件保存到指定路径
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }

            return filePath;
        }

        #endregion 图片上传与显示
    }
}