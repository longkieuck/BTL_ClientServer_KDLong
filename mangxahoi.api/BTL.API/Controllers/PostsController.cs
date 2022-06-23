﻿using BTL.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PostsController : ControllerBase
    {
        private readonly Social_NetworkContext _db;
        public static IWebHostEnvironment _environment;

        public PostsController(Social_NetworkContext context, IWebHostEnvironment environment)
        {
            _db = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<PagingData>> GetPostByPage([FromQuery] Guid user_id, [FromQuery] Guid group_id, [FromQuery] string search, [FromQuery] int? page = 1, [FromQuery] int? record = 20)
        {
            var pagingData = new PagingData();
            List<Post> records = new List<Post>();
            if (group_id == Guid.Empty)
            {
                records = await _db.Posts.Where(x => x.GroupId == null).OrderByDescending(x => x.CreateDate).ToListAsync();
            }
            else
            {
                records = await _db.Posts.Where(x => x.GroupId == group_id).OrderByDescending(x => x.CreateDate).ToListAsync();
            }
                
            //Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                string sql_search = "select * from post where CHARINDEX(@txtSeach,content) > 0 or CHARINDEX(@txtSeach,full_name) > 0";
                var param = new SqlParameter("@txtSeach", search);
                records = _db.Posts.FromSqlRaw(sql_search, param).OrderByDescending(x => x.CreateDate).ToList();
            }
            //Tổng số bản ghi
            pagingData.TotalRecord = records.Count();
            //Tổng số trangalue
            pagingData.TotalPage = Convert.ToInt32(Math.Ceiling((decimal)pagingData.TotalRecord / (decimal)record.Value));
            //Dữ liệu của từng trang
            List<Post> result = records.Skip((page.Value - 1) * record.Value).Take(record.Value).ToList();
            foreach(var item in result)
            {
                var liked = _db.UserLikes.Where(_ => _.PostId == item.Id && _.UserId == user_id).FirstOrDefault();
                if(liked != null)
                {
                    item.isLike = true;
                }
                else
                {
                    item.isLike = false;
                }
                item.post_image = _db.Images.Where(_ => _.PostId == item.Id).ToList();
                item.CreateDateString = this.ChuyenThoiGian((DateTime.Now.Subtract(item.CreateDate).Hours + DateTime.Now.Subtract(item.CreateDate).Days * 24), DateTime.Now.Subtract(item.CreateDate).Minutes, DateTime.Now.Subtract(item.CreateDate).Seconds);
            }
            pagingData.Data = result;
            return pagingData;
        }
        [HttpGet("post_by_id")]
        public async Task<ActionResult<PagingData>> GetPostById([FromQuery] Guid user_id_current, [FromQuery] Guid user_id_search, [FromQuery] int? page = 1, [FromQuery] int? record = 20)
        {
            var pagingData = new PagingData();
            List<Post> records = await _db.Posts.Where(x=> x.UserId == user_id_search && x.GroupId == null).OrderByDescending(x => x.CreateDate).ToListAsync();
            //Tổng số bản ghi
            pagingData.TotalRecord = records.Count();
            //Tổng số trangalue
            pagingData.TotalPage = Convert.ToInt32(Math.Ceiling((decimal)pagingData.TotalRecord / (decimal)record.Value));
            //Dữ liệu của từng trang
            List<Post> result = records.Skip((page.Value - 1) * record.Value).Take(record.Value).ToList();
            foreach (var item in result)
            {
                var liked = _db.UserLikes.Where(_ => _.PostId == item.Id && _.UserId == user_id_current).FirstOrDefault();
                if (liked != null)
                {
                    item.isLike = true;
                }
                else
                {
                    item.isLike = false;
                }
                item.post_image = _db.Images.Where(_ => _.PostId == item.Id).ToList();
                item.CreateDateString = this.ChuyenThoiGian((DateTime.Now.Subtract(item.CreateDate).Hours + DateTime.Now.Subtract(item.CreateDate).Days * 24), DateTime.Now.Subtract(item.CreateDate).Minutes, DateTime.Now.Subtract(item.CreateDate).Seconds);
            }
            pagingData.Data = result;
            return pagingData;
        }

        [AllowAnonymous]
        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetImage(string fileName)
        {
            string path = _environment.WebRootPath + "\\Upload\\";
            var filePath = path + fileName;
            if (System.IO.File.Exists(filePath))
            {
                byte[] b = System.IO.File.ReadAllBytes(filePath);
                return File(b, "image/jpg");
            }
            else
            {
                byte[] b = System.IO.File.ReadAllBytes(path + "default.png");
                return File(b, "image/jpg");
            }
            return null;
        }

        [HttpGet("detail")]
        public async Task<ServiceResponse> GetPost(Guid? id)
        {
            ServiceResponse res = new ServiceResponse();
            var post = await _db.Posts.FindAsync(id);
            if (post == null)
            {
                res.Message = SysMessage.NotFound;
                res.ErrorCode = 404;
                res.Success = false;
                res.Data = null;
            }
            Dictionary<string, object> result = new Dictionary<string, object>();

            post.post_image = _db.Images.Where(_ => _.PostId == post.Id).ToList();
            post.CreateDateString = this.ChuyenThoiGian((DateTime.Now.Subtract(post.CreateDate).Hours + DateTime.Now.Subtract(post.CreateDate).Days * 24), DateTime.Now.Subtract(post.CreateDate).Minutes, DateTime.Now.Subtract(post.CreateDate).Seconds);
            result.Add("post", post);
            res.Data = result;
            res.Success = true;
            return res;

        }

        [HttpPost]
        public async Task<ServiceResponse> PostPost([FromForm] Post post)
        {
            ServiceResponse res = new ServiceResponse();
            post.Id = Guid.NewGuid();
            post.FullName = post.FullName;
            int i = 0;
            if(post.objFile != null)
            {
                if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                }
                foreach (var objFile in post.objFile)
                {
                    if (objFile.Length > 0)
                    {
                        string fileName = post.Id + "_" + i + System.IO.Path.GetExtension(objFile.FileName);
                        using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + fileName))
                        {
                            objFile.CopyTo(fileStream);
                            fileStream.Flush();
                            //Add vào bảng image
                            Image img = new Image();
                            img.Id = Guid.NewGuid();
                            img.PostId = post.Id;
                            img.Url = fileName;
                            _db.Images.Add(img);
                        }
                    }
                    i++;
                }
            }
            post.CreateDate = DateTime.Now;
            post.ModifiedDate = DateTime.Now;
            post.LikesCount = 0;
            post.CommentCount = 0;
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
            res.Success = true;
            res.Data = post;
            return res;
        }

        [HttpPut("edit")]
        public async Task<ServiceResponse> PutPost(Post post)
        {
            ServiceResponse res = new ServiceResponse();
            Post postDb = await _db.Posts.FindAsync(post.Id);
            if (postDb == null)
            {
                res.Message = SysMessage.NotFound;
                res.ErrorCode = 404;
                res.Success = false;
            }
            postDb.Content = post.Content;
            if (post.post_image != null && post.post_image.Count > 0)
            {
                var listOldImg = await _db.Images.Where(_ => _.PostId == postDb.Id).ToListAsync();
                _db.Images.RemoveRange(listOldImg);
                foreach (var img in post.post_image)
                {
                    img.Id = Guid.NewGuid();
                    img.PostId = postDb.Id;
                    _db.Images.Add(img);
                }
            }
            await _db.SaveChangesAsync();
            res.Success = true;
            res.Message = SysMessage.Success;
            res.Data = post;
            return res;
        }

        [HttpDelete("remove")]
        public async Task<ServiceResponse> DeletePost(Guid? id)
        {
            ServiceResponse res = new ServiceResponse();
            var post = await _db.Posts.FindAsync(id);
            if (post == null)
            {
                res.Message = SysMessage.NotFound;
                res.ErrorCode = 404;
                res.Success = false;
            }
            _db.Posts.Remove(post);
            //delete ở các bảng liên quan 
            var listImg = await _db.Images.Where(_ => _.PostId == post.Id).ToListAsync();
            _db.Images.RemoveRange(listImg);
            var listComment = await _db.Comments.Where(_ => _.PostId == post.Id).ToListAsync();
            _db.Comments.RemoveRange(listComment);
            var listLikes = await _db.UserLikes.Where(_ => _.PostId == post.Id).ToListAsync();
            _db.UserLikes.RemoveRange(listLikes);
            await _db.SaveChangesAsync();
            res.Success = true;
            res.Message = SysMessage.Success;
            res.Data = post;
            return res;
        }


        [HttpPost("add_comment")]
        public async Task<ServiceResponse> AddComment(Comment comment)
        {
            ServiceResponse res = new ServiceResponse();
            comment.Id = Guid.NewGuid();
            comment.CreateDate = DateTime.Now;
            comment.ModifiedDate = DateTime.Now;
            await _db.Comments.AddAsync(comment);
            var post = await _db.Posts.FindAsync(comment.PostId);
            post.CommentCount = post.CommentCount + 1;

            if(comment.UserId != post.UserId)
            {
                Notify newNotify = new Notify();
                newNotify.Id = Guid.NewGuid();
                newNotify.Content = comment.UserName + " đã bình luận bài viết của bạn!";
                newNotify.PostId = comment.PostId;
                newNotify.UserId = post.UserId;
                newNotify.hasSeen = 0;// Chưa xem
                newNotify.UserIdAction = comment.UserId;
                await _db.Notifies.AddAsync(newNotify);
            }

            await _db.SaveChangesAsync();
            res.Success = true;
            res.Data = comment;


            return res;
        }

        [HttpPost("edit_comment")]
        public async Task<ServiceResponse> EditCommentAsync([FromBody] Comment comment)
        {
            ServiceResponse res = new ServiceResponse();
            comment.Id = Guid.NewGuid();
            var commentDb = await _db.Comments.FindAsync(comment.Id);
            commentDb.ModifiedDate = DateTime.Now;
            commentDb.CreateDate = comment.CreateDate;
            commentDb.Content = comment.Content;
            _db.Comments.Update(comment);
            await _db.SaveChangesAsync();
            res.Success = true;
            res.Data = comment;
            return res;
        }

        [HttpPost("like_post")]
        public async Task<ServiceResponse> LikePost(UserLike userLike)
        {
            ServiceResponse res = new ServiceResponse();
            userLike.Id = Guid.NewGuid();
            var liked =  _db.UserLikes.Where(_ => _.PostId == userLike.PostId && _.UserId == userLike.UserId).FirstOrDefault();
            var post = await _db.Posts.FindAsync(userLike.PostId);
            var infoUserLike = await _db.UserTbs.FindAsync(userLike.UserId);
            if (liked != null)
            {
                _db.UserLikes.Remove(liked);
                post.LikesCount = post.LikesCount - 1;
                userLike.isLike = false;
            }
            else
            {
                await _db.UserLikes.AddAsync(userLike);
                post.LikesCount = post.LikesCount + 1;
                userLike.isLike = true;

                if (userLike.UserId != post.UserId)
                {
                    Notify newNotify = new Notify();
                    newNotify.Id = Guid.NewGuid();
                    newNotify.Content = infoUserLike.FullName + " đã thích bài viết của bạn!";
                    newNotify.PostId = post.Id;
                    newNotify.UserId = post.UserId;
                    newNotify.hasSeen = 0;// Chưa xem
                    newNotify.UserIdAction = userLike.UserId;
                    await _db.Notifies.AddAsync(newNotify);
                }

            }
            await _db.SaveChangesAsync();
            res.Success = true;
            res.Data = userLike;
            return res;
        }

        [HttpGet("post_comment")]
        public async Task<ActionResult<PagingData>> GetPostComment(Guid? Id, [FromQuery] int? page = 1, [FromQuery] int? record = 10)
        {
            var pagingData = new PagingData();
            List<Comment> records = await _db.Comments.Where(_=>_.PostId == Id).OrderByDescending(x => x.CreateDate).ToListAsync();
            //Tổng số bản ghi
            pagingData.TotalRecord = records.Count();
            //Tổng số trangalue
            pagingData.TotalPage = Convert.ToInt32(Math.Ceiling((decimal)pagingData.TotalRecord / (decimal)record.Value));
            //Dữ liệu của từng trang
            List<Comment> result = records.Skip((page.Value - 1) * record.Value).Take(record.Value).ToList();
            foreach(var cmt in result)
            {
                cmt.CreateDateString = this.ChuyenThoiGian((DateTime.Now.Subtract(cmt.CreateDate).Hours + DateTime.Now.Subtract(cmt.CreateDate).Days * 24), DateTime.Now.Subtract(cmt.CreateDate).Minutes, DateTime.Now.Subtract(cmt.CreateDate).Seconds);
            }
            pagingData.Data = result;
            return pagingData;
        }



        string ChuyenThoiGian(int gio, int phut, int giay)
        {
            if(phut < 0)
            {
                return giay + " giây trước";
            }
            if(gio <= 0)
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
