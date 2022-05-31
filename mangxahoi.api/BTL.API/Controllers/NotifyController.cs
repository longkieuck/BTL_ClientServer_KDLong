using BTL.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotifyController : ControllerBase
    {
        private readonly Social_NetworkContext _db;
        public NotifyController(Social_NetworkContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagingData>> GetNotifyByUser([FromQuery] Guid? user_id, [FromQuery] int? page = 1, [FromQuery] int? record = 20)
        {
            var pagingData = new PagingData();
            List<Notify> records = await _db.Notifies.Where(_ => _.UserId == user_id && _.hasSeen == 0).OrderByDescending(x => x.CreateDate).ToListAsync();
            //Tổng số bản ghi
            pagingData.TotalRecord = records.Count();
            //Tổng số trangalue
            pagingData.TotalPage = Convert.ToInt32(Math.Ceiling((decimal)pagingData.TotalRecord / (decimal)record.Value));
            //Dữ liệu của từng trang
            List<Notify> result = records.Skip((page.Value - 1) * record.Value).Take(record.Value).ToList();
            foreach (var noti in result)
            {
                noti.CreateDateString = this.ChuyenThoiGian((DateTime.Now.Subtract(noti.CreateDate).Hours + DateTime.Now.Subtract(noti.CreateDate).Days * 24), DateTime.Now.Subtract(noti.CreateDate).Minutes, DateTime.Now.Subtract(noti.CreateDate).Seconds);
            }
            pagingData.Data = result;
            return pagingData;
        }
        [HttpGet("mess_notify")]
        public async Task<ActionResult<PagingData>> GetNotifyMessByUser([FromQuery] Guid? user_id, [FromQuery] int? page = 1, [FromQuery] int? record = 20)
        {
            var pagingData = new PagingData();
            List<ChatBox> chatboxes = await _db.ChatBoxes.Where(_ => _.UserId1 == user_id || _.UserId2 == user_id).ToListAsync();
            List<Message> records = new List<Message>();
            
            foreach( var cb in chatboxes)
            {
                var mess = _db.Messages.Where(_ => _.ChatBoxId == cb.Id).OrderByDescending(_ => _.CreateDate).FirstOrDefault();
                if(mess != null)
                    records.Add(mess);
            }


            foreach( var ms in records)
            {
                ms.CreateDateString = this.ChuyenThoiGian((DateTime.Now.Subtract(ms.CreateDate).Hours + DateTime.Now.Subtract(ms.CreateDate).Days * 24), DateTime.Now.Subtract(ms.CreateDate).Minutes, DateTime.Now.Subtract(ms.CreateDate).Seconds);
            }
            ////Tổng số bản ghi
            //pagingData.TotalRecord = records.Count();
            ////Tổng số trangalue
            //pagingData.TotalPage = Convert.ToInt32(Math.Ceiling((decimal)pagingData.TotalRecord / (decimal)record.Value));
            ////Dữ liệu của từng trang
            //List<Notify> result = records.Skip((page.Value - 1) * record.Value).Take(record.Value).ToList();
            //foreach (var noti in result)
            //{
            //    noti.CreateDateString = this.ChuyenThoiGian((DateTime.Now.Subtract(noti.CreateDate).Hours + DateTime.Now.Subtract(noti.CreateDate).Days * 24), DateTime.Now.Subtract(noti.CreateDate).Minutes, DateTime.Now.Subtract(noti.CreateDate).Seconds);
            //}

            var sortedRecords = records.OrderByDescending(_ => _.CreateDate);
            pagingData.Data = sortedRecords;
            return pagingData;
        }
        [HttpPut]
        public async Task<ServiceResponse> PutNotify(Notify notify)
        {
            ServiceResponse res = new ServiceResponse();
            Notify notiDB = await _db.Notifies.FindAsync(notify.Id);
            if (notify == null)
            {
                res.Message = SysMessage.NotFound;
                res.ErrorCode = 404;
                res.Success = false;
            }
            else
            {
                notiDB.hasSeen = 1;
            }
            await _db.SaveChangesAsync();
            res.Success = true;
            res.Message = SysMessage.Success;
            res.Data = notiDB;
            return res;
        }
        string ChuyenThoiGian(int gio, int phut, int giay)
        {
            if (phut < 0)
            {
                return giay + " giây trước";
            }
            if (gio <= 0)
            {
                return phut + " phút trước";
            }
            if (gio < 24)
            {
                return gio + " giờ trước";
            }
            else if (gio >= 24 && gio < 168)
            {
                return (gio / 24) + " ngày trước";
            }
            else if (gio >= 168 && gio < 672)
            {
                return (gio / 168) + " tuần trước";
            }
            else if (gio >= 672 && gio < 8064)
            {
                return (gio / 672) + " tháng trước";
            }
            else
            {
                return (gio / 8064) + " năm trước";
            }
        }
    }
}
