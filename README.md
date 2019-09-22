# myblogapi
这是一个我的blog api项目。
# 跟微软的一个官方教程学习的，将项目分为三个，GreenShadow.Blog.Domain主要为model相关的东西，GreenShadow.Blog.DataAccess为数据库操作相关的东西。
# GreenShadow.Blog.Api为api项目，jwt结合identity相关的api和用ef core对数据库进行操作，jwt授权接口，算是把从网友哪里抄的代码都是整合到一起了。
# 记得在GreenShadow.Blog.DataAccess项目执行数据库迁移相关指令如下。

## 创建迁移，此项目 已经创建迁移文件，所以可以不用执行这两个指令。
### add-migration InitialIdentityModel -Context AppIdentityDbContext -o Data/Identity  
### add-migration InitialBlogModel -Context BlogContext -o Data/Blog


## 更新数据库相关的表
### Update-Database InitialIdentityModel -Context AppIdentityDbContext
### Update-Database InitialBlogModel -Context BlogContext
