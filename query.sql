select * from post where UserId = 'ef355ba1-86e9-492b-a4b9-748138d81ed2';
select LikeCount as 'số lượng like bài viết',[User].FullName as 'Người đăng bài' from [Post],[User] where UserId = 'ef355ba1-86e9-492b-a4b9-748138d81ed2';

select * from Comment;
select * from Comment where UserId = 'ef355ba1-86e9-492b-a4b9-748138d81ed2';
select count(*) from Comment where UserId = 'ef355ba1-86e9-492b-a4b9-748138d81ed2';

-- Các comment của user trong bài viết
select Comment.*, [User].FullName as 'Người comment' from Comment, [User] where Comment.UserId = [User].Id and PostId = '3A999015-8AA7-4DBE-AE2C-FF8AF1EF3142';

select count(*) as 'Tổng lượng comment' from Comment where UserId = 'ef355ba1-86e9-492b-a4b9-748138d81ed2' and PostId = '3A999015-8AA7-4DBE-AE2C-FF8AF1EF3142';

